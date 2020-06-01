using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using CommonInformation;

namespace ServerSide
{
    class ClientConnectionManager
    {
        Socket listeningClientMessagesSocket;
        Thread handleClientThread;

        public bool isConnected;
        MessageSerializer messageSerializer;

        int[] randomNumbersOfQuestions = new int[Server.NumberOfQuestionsInOneGame]; 

        public ClientConnectionManager(Socket listeningClientMessagesSocket)
        {
            messageSerializer = new MessageSerializer();
            isConnected = true;
            this.listeningClientMessagesSocket = listeningClientMessagesSocket;
            listeningClientMessagesSocket.ReceiveTimeout = 700;
            listeningClientMessagesSocket.SendTimeout = 700;
            Server.clientSockets.Add(listeningClientMessagesSocket.RemoteEndPoint.GetHashCode(), listeningClientMessagesSocket);
            handleClientThread = new Thread(ListeningClient);
            handleClientThread.Start();
        }

        public void ListeningClient()
        {
            ReceiveClientMessages();
            DisconnectClient();
        }

        public void ReceiveClientMessages()
        {
            byte[] data = new byte[8192];
            int amount;
            do
            {
                MemoryStream messageContainer = new MemoryStream();
                try
                {
                    do
                    {
                        amount = listeningClientMessagesSocket.Receive(data);
                        Console.WriteLine(amount);
                        messageContainer.Write(data, 0, amount);
                    } while (listeningClientMessagesSocket.Available > 0);
                    Message recievedMessage = messageSerializer.Deserialize(messageContainer.GetBuffer(),
                        messageContainer.GetBuffer().Length);
                    DefineMessageType(recievedMessage);
                }
                catch
                {
                    if (!IsClientConnected())
                    {
                        isConnected = false;
                    }
                }
            } while (isConnected);
        }

        void DefineMessageType(Message message)
        {
            message.messageTime = DateTime.Now;
            message.IPAdress = ((IPEndPoint)listeningClientMessagesSocket.RemoteEndPoint).Address.ToString();
            switch (message.messageType)
            {
                case Message.MessageType.Common:
                    ProcessCommonMessage(message);
                    break;
                case Message.MessageType.Private:
                    ProcessPrivateMessage(message);
                    break;
                case Message.MessageType.History:
                    ProcessHistoryRequestMessage(message);
                    break;
                case Message.MessageType.JoinToChat:
                    ProcessJoinToChatMessage(message);
                    break;
                case Message.MessageType.StartGameRequest:
                    ProcessStartGameRequestMessage(message);
                    break;
                case Message.MessageType.StartGameResponse:
                    ProcessStartGameResponseMessage(message);
                    break;
                case Message.MessageType.PlayerAnswer:
                    ProcessPlayerAnswerMessage(message);
                    break;
                case Message.MessageType.LeftGame:
                    ProcessLeftGameMesasage(message);
                    break;
                case Message.MessageType.GetStatistics:
                    ProcessGetStatisticsMesasage(message);
                    break;
                case Message.MessageType.PromptRequest:
                    ProcessPromptRequestMesasage(message);
                    break;
                case Message.MessageType.InterruptSearchingForOpponent:
                    ProcessInterruptSearchingForOpponentMesasage(message);
                    break;
            }
        }

        void ProcessCommonMessage(Message message)
        {
            message.messageName = Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()];
            Console.WriteLine(message.messageName + " : " + message.messageContent);
            Server.SendToAll(message);
            Server.MessageHistory.Add(message.messageTime.ToString() + " - " + message.IPAdress + " - " + message.messageName + " : " + message.messageContent);
        }

        void ProcessPrivateMessage(Message message)
        {
            message.messageName = Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()];
            if (Server.clientSockets.ContainsKey(message.messageReceiverID))
            {
                message.messageSenderID = listeningClientMessagesSocket.RemoteEndPoint.GetHashCode();
                Server.SendMessage(message, Server.clientSockets[message.messageReceiverID]);
            }
            else
            {
                Console.WriteLine("Failed to send private message");
            }
        }

        void ProcessHistoryRequestMessage(Message message)
        {
            Message responseMessage = new Message() { messageHistory = Server.MessageHistory, messageType = Message.MessageType.History };
            Server.SendMessage(responseMessage, listeningClientMessagesSocket);
        }

        void ProcessJoinToChatMessage(Message message)
        {
            Server.clientNames.Add(listeningClientMessagesSocket.RemoteEndPoint.GetHashCode(), message.messageName);
            Console.WriteLine(message.messageName + " присоединился к беседе");

            var responseTopicsMessage = new Message()
            { messageType = Message.MessageType.SendGameTopics, GameTopics = GetGameTopics() };
            Server.SendMessage(responseTopicsMessage, listeningClientMessagesSocket);

            List<ClientsInfo> info = GetClientsList();
            Server.SendToAll(new Message(info));
            Server.SendToAll(new Message(message.messageName, " присоединился к беседе", Message.MessageType.Common));
        }

        bool isOpponentPlaying(int opponentID)
        {
            bool flag = false;
            foreach(Game game in Server.GamesList)
            {
                if (((game.firstPlayerID == opponentID) || (game.secondPlayerID == opponentID)) && game.isPlayedNow)
                    flag = true;
            }
            return flag;
        } 

        void ProcessStartGameRequestMessage(Message message)
        {
            if (message.isSelectedOpponentForGame)
            {
                if (message.messageReceiverID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                {
                    var responseMessage = new Message() { messageType = Message.MessageType.StartGameResponse,
                    gameStartDetails = "Самому с собой играть нельзя!", mayStartGame = false };
                    Server.SendMessage(responseMessage, listeningClientMessagesSocket);
                }
                else
                {
                    if (isOpponentPlaying(message.messageReceiverID))
                    {
                        var responseMessage = new Message() { messageType = Message.MessageType.StartGameResponse,
                        gameStartDetails = "Оппонент в текущий момент уже играет!", mayStartGame = false };
                        Server.SendMessage(responseMessage, listeningClientMessagesSocket);
                    }
                    else
                    {
                        message.messageName = Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()];
                        message.gameStartDetails = message.messageName + " хочет играть с Вами. Принять?";
                        if (Server.clientSockets.ContainsKey(message.messageReceiverID))
                        {
                            message.messageSenderID = listeningClientMessagesSocket.RemoteEndPoint.GetHashCode();
                            Console.WriteLine(message.messageSenderID);
                            Console.WriteLine(message.messageReceiverID);
                            Server.SendMessage(message, Server.clientSockets[message.messageReceiverID]);
                        }
                        else
                        {
                            Console.WriteLine("Failed to send start game message");
                        }

                    }
                }
            }
            else
            {
                if (Server.waitingForRandomGamePlayers.Count == 0)
                {
                    Server.waitingForRandomGamePlayers.Add(listeningClientMessagesSocket.RemoteEndPoint.GetHashCode());
                }
                else
                {
                    var random = new Random();
                    int randomValue = random.Next(0, 3);

                    var responseGameMessage = new CommonInformation.Message()
                    {
                        messageType = Message.MessageType.StartGameResponse,
                        messageSenderID = Server.waitingForRandomGamePlayers[0],
                        messageReceiverID = listeningClientMessagesSocket.RemoteEndPoint.GetHashCode(),
                        gameTopic = randomValue,
                        messageName = Server.clientNames[Server.waitingForRandomGamePlayers[0]],
                        mayStartGame = true
                    };
                    Server.waitingForRandomGamePlayers.Clear();
                    OrganizeGame(responseGameMessage);
                }
            }
            
        }

        GameTopic DefineGameTopic(int gameTopic)
        {
            if (gameTopic == (int)GameTopic.AroundTheWorld)
            {
                return GameTopic.AroundTheWorld;
            }
            else if (gameTopic == (int)GameTopic.ScienceGranite)
            {
                return GameTopic.ScienceGranite;
            }
            else
                return GameTopic.TechnicalProgress;
        }

        void GenerateRandomQuestions()
        {
            Random random = new Random();

            int currentAttempt = 0;
            int randomVariable;
            while (currentAttempt < randomNumbersOfQuestions.Length)
            {
                randomVariable = random.Next(0, Server.AllQuestionsInTopic);
                bool isNOTExistsThisNumber = true;
                int j = 0;
                while ((j < currentAttempt) && isNOTExistsThisNumber)
                {
                    if (randomVariable == randomNumbersOfQuestions[j])
                        isNOTExistsThisNumber = false;
                }
                if (isNOTExistsThisNumber)
                {
                    randomNumbersOfQuestions[currentAttempt] = randomVariable;
                    currentAttempt++;
                }
            }
            /*for (int i = 0; i < randomNumbersOfQuestions.Length; i++)
            {
                randomNumbersOfQuestions[i] = random.Next(0, Server.AllQuestionsInTopic);
            }*/
        }

        void OrganizeGame(Message message)
        {
            GameTopic gameTopic = DefineGameTopic(message.gameTopic);
            GenerateRandomQuestions();

            var game = new Game(message.messageSenderID, message.messageReceiverID, message.messageName,
            Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()]);

            for (int i = 0; i < Server.NumberOfQuestionsInOneGame; i++)
            {
                game.rigthAnswersOnQuestions[i] = randomNumbersOfQuestions[i] % Server.NumberOfAnswers;
            }

            Server.GamesList.Add(game);
            Console.WriteLine("game is set");

            message.questionsToSend = new string[Server.NumberOfQuestionsInOneGame];
            message.answersToSend = new string[Server.NumberOfAnswers * Server.NumberOfQuestionsInOneGame];
            for (int i = 0; i < Server.NumberOfQuestionsInOneGame; i++)
                message.questionsToSend[i] = Server.QuestionsDictionary[gameTopic].questions[randomNumbersOfQuestions[i]];
            int k = 0;
            for (int i = 0; i < Server.NumberOfQuestionsInOneGame; i++)
                for (int j = 0; j < Server.NumberOfAnswers; j++)
                {
                    message.answersToSend[k] = Server.QuestionsDictionary[gameTopic].answers[randomNumbersOfQuestions[i], j].Title;
                    k++;
                }

            Console.WriteLine(message.messageSenderID);
            Console.WriteLine(message.messageReceiverID);
            Server.SendMessage(message, Server.clientSockets[message.messageSenderID]);
            Server.SendMessage(message, Server.clientSockets[message.messageReceiverID]);
            Console.WriteLine("messages is sended");
        }

        void ProcessStartGameResponseMessage(Message message)
        {
            if (message.mayStartGame)
            {
                /*GameTopic gameTopic = DefineGameTopic(message.gameTopic);
                GenerateRandomQuestions();
          
                var game = new Game(message.messageSenderID, message.messageReceiverID, message.messageName,
                Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()]);

                for (int i = 0; i < Server.NumberOfQuestionsInOneGame; i++)
                {
                    game.rigthAnswersOnQuestions[i] = randomNumbersOfQuestions[i] % Server.NumberOfAnswers;
                }

                Server.GamesList.Add(game);
                Console.WriteLine("game is set");

                message.questionsToSend = new string[Server.NumberOfQuestionsInOneGame];
                message.answersToSend = new string[Server.NumberOfAnswers * Server.NumberOfQuestionsInOneGame];
                for(int i = 0; i < Server.NumberOfQuestionsInOneGame; i++)
                    message.questionsToSend[i] = Server.QuestionsDictionary[gameTopic].questions[randomNumbersOfQuestions[i]];
                int k = 0;
                for (int i = 0; i < Server.NumberOfQuestionsInOneGame; i++)
                    for (int j = 0; j < Server.NumberOfAnswers; j++)
                    {
                        message.answersToSend[k] = Server.QuestionsDictionary[gameTopic].answers[randomNumbersOfQuestions[i], j].Title;
                        k++;
                    }

                Console.WriteLine(message.messageSenderID);
                Console.WriteLine(message.messageReceiverID);
                Server.SendMessage(message, Server.clientSockets[message.messageSenderID]);
                Server.SendMessage(message, Server.clientSockets[message.messageReceiverID]);
                Console.WriteLine("messages is sended");*/
                OrganizeGame(message);
            }
            else
            {
                message.gameStartDetails = Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()] + " не хочет с Вами играть";
                Server.SendMessage(message, Server.clientSockets[message.messageSenderID]);
            }
        }

        void NotifyAboutEndedGame(string gameResults)
        {
            var messageGameResults = new Message() { messageName = "Сервер", messageContent = gameResults,
            messageTime = DateTime.Now, messageType = Message.MessageType.Common, IPAdress = "" };
            Console.WriteLine(messageGameResults.messageName + " : " + messageGameResults.messageContent);
            Server.SendToAll(messageGameResults);
            Server.MessageHistory.Add(messageGameResults.messageTime.ToString() + "  " + messageGameResults.messageName
            + " : " + messageGameResults.messageContent);
        }

        string UpdatePlayerInformation(int playerID, string playername, int yourPoints, int opponentPoints, bool isLeftGame)
        {
            string resultStatus = "";
            if (Server.playerInformationDictionary.ContainsKey(playerID))
            {
                Server.playerInformationDictionary[playerID].numberOfPlayedGames++;
                Server.playerInformationDictionary[playerID].rightAnswersNumber += yourPoints;
                Server.playerInformationDictionary[playerID].wrongAnswersNumber += Server.NumberOfQuestionsInOneGame - yourPoints;
                if (isLeftGame)
                {
                    Server.playerInformationDictionary[playerID].losesNumber++;
                }
                else
                {
                    if (yourPoints > opponentPoints)
                    {
                        Server.playerInformationDictionary[playerID].winsNumber++;
                        Server.playerInformationDictionary[playerID].pointsNumber += 3;
                    }
                    else if (yourPoints < opponentPoints)
                    {
                        Server.playerInformationDictionary[playerID].losesNumber++;
                    }
                    else
                    {
                        Server.playerInformationDictionary[playerID].drawsNumber++;
                        Server.playerInformationDictionary[playerID].pointsNumber++;
                    }
                }
                
                PlayerInformation.StatusType statusType = Server.playerInformationDictionary[playerID].DefineStatus();
                if (statusType != Server.playerInformationDictionary[playerID].playerStatus)
                {
                    if ((Server.playerInformationDictionary[playerID].playerStatus == PlayerInformation.StatusType.Beginner) &&
                    (statusType == PlayerInformation.StatusType.Specialist))
                    {
                        resultStatus = "Поздравляем, ваше новое звание - специалист!";
                    }
                    else if ((Server.playerInformationDictionary[playerID].playerStatus == PlayerInformation.StatusType.Specialist) &&
                    (statusType == PlayerInformation.StatusType.Master))
                    {
                        resultStatus = "Поздравляем, ваше новое звание - мастер!";
                    }
                    else if ((Server.playerInformationDictionary[playerID].playerStatus == PlayerInformation.StatusType.Specialist) &&
                    (statusType == PlayerInformation.StatusType.Beginner))
                    {
                        resultStatus = "Увы, вы потеряли звание специалист, теперь - новичок!";
                    }
                    else if ((Server.playerInformationDictionary[playerID].playerStatus == PlayerInformation.StatusType.Master) &&
                    (statusType == PlayerInformation.StatusType.Specialist))
                    {
                        resultStatus = "Увы, вы потеряли звание мастер, теперь - специалист!";
                    }
                    Server.playerInformationDictionary[playerID].playerStatus = statusType;
                    return resultStatus;
                }
            }
            else
            {
                var playerInformation = new PlayerInformation(playername, yourPoints, Server.NumberOfQuestionsInOneGame - yourPoints);
                if (yourPoints > opponentPoints)
                {
                    playerInformation.winsNumber = 1;
                    playerInformation.pointsNumber = 3;
                }
                else if (yourPoints < opponentPoints)
                {
                    playerInformation.losesNumber = 1;
                    playerInformation.pointsNumber = 0;
                }
                else
                {
                    playerInformation.drawsNumber = 1;
                    playerInformation.pointsNumber = 1;
                }      
                Server.playerInformationDictionary.Add(playerID, playerInformation);
            }
            return resultStatus;
        }

        void ProcessPlayerAnswerMessage(Message message)
        {
            bool firstPlayerFlag = false;
            bool secondPlayerFlag = false;
            foreach (Game game in Server.GamesList)
            {
                if ( (game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                    || (game.secondPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()) )
                {
                    if (game.isPlayedNow)
                    {
                        if (game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                            firstPlayerFlag = true;
                        else
                            secondPlayerFlag = true;

                        var responseYourselfMessage = new Message() { messageType = Message.MessageType.YourAnswerStatus };

                        if (game.rigthAnswersOnQuestions[message.answeredQuestionNumber] == message.answerNumber)
                        {
                            responseYourselfMessage.isCorrectAnswer = true;

                            var responseForOpponentMessage = new Message() { messageType = Message.MessageType.OpponentRightAnswer };
                            if (firstPlayerFlag)
                            {
                                game.firstPlayerPoints++;
                                responseForOpponentMessage.messageReceiverID = game.secondPlayerID;
                                responseYourselfMessage.messageReceiverID = game.firstPlayerID;
                            }
                            else
                            {
                                game.secondPlayerPoints++;
                                responseForOpponentMessage.messageReceiverID = game.firstPlayerID;
                                responseYourselfMessage.messageReceiverID = game.secondPlayerID;
                            }

                            Server.SendMessage(responseForOpponentMessage, Server.clientSockets[responseForOpponentMessage.messageReceiverID]);    
                        }
                        else
                        {
                            responseYourselfMessage.isCorrectAnswer = false;

                            if (firstPlayerFlag)
                            {
                                responseYourselfMessage.messageReceiverID = game.firstPlayerID;
                            }
                            else
                            {
                                responseYourselfMessage.messageReceiverID = game.secondPlayerID;
                            }
                        }

                        Server.SendMessage(responseYourselfMessage, Server.clientSockets[responseYourselfMessage.messageReceiverID]);

                        if (firstPlayerFlag)
                        {
                            game.firstNumberAnsweredQuestions++;
                            if (Server.NumberOfQuestionsInOneGame == game.firstNumberAnsweredQuestions)
                            {
                                game.isFirstFinished = true;

                                if (game.isSecondFinished)
                                {
                                    var responseResultMessage = new Message() { messageType = Message.MessageType.GameResults };
                                    game.gameResults = game.firstPlayerName + "   " + game.firstPlayerPoints + " : "
                                    + game.secondPlayerPoints + "   " + game.secondPlayerName;

                                    responseResultMessage.gameStatus = game.gameResults;
                                    responseResultMessage.gameStatus = responseResultMessage.gameStatus
                                    + UpdatePlayerInformation(game.secondPlayerID, Server.clientNames[game.secondPlayerID],
                                    game.secondPlayerPoints, game.firstPlayerPoints, false);
                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);

                                    Thread.Sleep(250);

                                    responseResultMessage.gameStatus = game.gameResults;
                                    responseResultMessage.gameStatus = responseResultMessage.gameStatus
                                    + UpdatePlayerInformation(game.firstPlayerID, Server.clientNames[game.firstPlayerID],
                                    game.firstPlayerPoints, game.secondPlayerPoints, false);
                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);

                                    game.isPlayedNow = false;
                                    NotifyAboutEndedGame(game.gameResults);
                                }
                                else
                                {
                                    var responseStatusMessage = new Message() { messageType = Message.MessageType.GameStatus };
                                    responseStatusMessage.gameStatus = "Ожидайте оппонента...";
                                    Server.SendMessage(responseStatusMessage, Server.clientSockets[game.firstPlayerID]);
                                }
                            }
                        }
                        else
                        {
                            game.secondNumberAnsweredQuestions++;
                            if (Server.NumberOfQuestionsInOneGame == game.secondNumberAnsweredQuestions)
                            {
                                game.isSecondFinished = true;

                                if (game.isFirstFinished)
                                {
                                    var responseResultMessage = new Message() { messageType = Message.MessageType.GameResults };
                                    game.gameResults = game.firstPlayerName + "   " + game.firstPlayerPoints + " : "
                                    + game.secondPlayerPoints + "   " + game.secondPlayerName;

                                    responseResultMessage.gameStatus = game.gameResults;
                                    responseResultMessage.gameStatus = responseResultMessage.gameStatus
                                    + UpdatePlayerInformation(game.firstPlayerID, Server.clientNames[game.firstPlayerID],
                                    game.firstPlayerPoints, game.secondPlayerPoints, false);
                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);

                                    Thread.Sleep(250);

                                    responseResultMessage.gameStatus = game.gameResults;
                                    responseResultMessage.gameStatus = responseResultMessage.gameStatus
                                    + UpdatePlayerInformation(game.secondPlayerID, Server.clientNames[game.secondPlayerID],
                                    game.secondPlayerPoints, game.firstPlayerPoints, false);
                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);

                                    game.isPlayedNow = false;
                                    NotifyAboutEndedGame(game.gameResults);
                                }
                                else
                                {
                                    var responseStatusMessage = new Message() { messageType = Message.MessageType.GameStatus };
                                    responseStatusMessage.gameStatus = "Ожидайте оппонента...";
                                    Server.SendMessage(responseStatusMessage, Server.clientSockets[game.secondPlayerID]);
                                }
                            }
                        }

                    }
                }
            }
        }

        void ProcessLeftGameMesasage(Message message)
        {
            foreach (Game game in Server.GamesList)
            {
                if ( ((game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                || (game.secondPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())) && game.isPlayedNow )
                {
                    bool isFirstPlayerLeft; 
                    var responseResultMessage = new Message() { messageType = Message.MessageType.GameResults };

                    game.isFirstFinished = true;
                    game.isSecondFinished = true;
                    if (game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                    {
                        game.firstPlayerPoints = 0;
                        isFirstPlayerLeft = true;
                    }     
                    else
                    {
                        game.secondPlayerPoints = 0;
                        isFirstPlayerLeft = false;
                    }
                           
                    game.gameResults = game.firstPlayerName + "   " + game.firstPlayerPoints + " : "
                    + game.secondPlayerPoints + "   " + game.secondPlayerName + " (тех.поражение)";
                    responseResultMessage.gameStatus = game.gameResults;

                    if (isFirstPlayerLeft)
                    {
                        string updatedResultOfFirstPlayer = UpdatePlayerInformation(game.firstPlayerID, Server.clientNames[game.firstPlayerID],
                        game.firstPlayerPoints, game.secondPlayerPoints, true);
                        string updatedResultOfSecondPlayer = UpdatePlayerInformation(game.secondPlayerID, Server.clientNames[game.secondPlayerID],
                        game.secondPlayerPoints, game.firstPlayerPoints, false);
                        responseResultMessage.gameStatus += updatedResultOfSecondPlayer;
                        Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);
                        Thread.Sleep(100);

                        if (Server.clientSockets.ContainsKey(game.firstPlayerID))
                        {
                            responseResultMessage.gameStatus = game.gameResults;
                            responseResultMessage.gameStatus += updatedResultOfFirstPlayer;
                            Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);
                        }
                    }
                    else
                    {
                        string updatedResultOfFirstPlayer = UpdatePlayerInformation(game.firstPlayerID, Server.clientNames[game.firstPlayerID],
                        game.firstPlayerPoints, game.secondPlayerPoints, false);
                        string updatedResultOfSecondPlayer = UpdatePlayerInformation(game.secondPlayerID, Server.clientNames[game.secondPlayerID],
                        game.secondPlayerPoints, game.firstPlayerPoints, true);
                        responseResultMessage.gameStatus += updatedResultOfFirstPlayer;
                        Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);
                        Thread.Sleep(100);

                        if (Server.clientSockets.ContainsKey(game.secondPlayerID))
                        {
                            responseResultMessage.gameStatus = game.gameResults;
                            responseResultMessage.gameStatus += updatedResultOfSecondPlayer;
                            Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);
                        }
                    }
                        

                    /*if (game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                        Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);
                    else
                        Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);*/

                    game.isPlayedNow = false;

                    NotifyAboutEndedGame(game.gameResults);

                }
            }
        }

        void ProcessGetStatisticsMesasage(Message message)
        {
            if (message.messageReceiverID != 0)
            {
                if (Server.playerInformationDictionary.ContainsKey(message.messageReceiverID))
                {
                    message.isfailedToGetStatistics = false;
                    var playerInformation = new PlayerInfo()
                    {
                        playerName = Server.playerInformationDictionary[message.messageReceiverID].playerName,
                        playerStatus = Server.playerInformationDictionary[message.messageReceiverID].playerStatus.ToString(),
                        pointsNumber = Server.playerInformationDictionary[message.messageReceiverID].pointsNumber,
                        numberOfPlayedGames = Server.playerInformationDictionary[message.messageReceiverID].numberOfPlayedGames,
                        winsNumber = Server.playerInformationDictionary[message.messageReceiverID].winsNumber,
                        drawsNumber = Server.playerInformationDictionary[message.messageReceiverID].drawsNumber,
                        losesNumber = Server.playerInformationDictionary[message.messageReceiverID].losesNumber,
                        rightAnswersNumber = Server.playerInformationDictionary[message.messageReceiverID].rightAnswersNumber,
                        wrongAnswersNumber = Server.playerInformationDictionary[message.messageReceiverID].wrongAnswersNumber
                    };
                    message.playerInfo = playerInformation;
                }
                else
                {
                    message.isfailedToGetStatistics = true;
                }
            }
            else
            {
                List<PlayerInfo> playerInfo = new List<PlayerInfo>();
                foreach (KeyValuePair<int, PlayerInformation> keyValuePair in Server.playerInformationDictionary)
                {
                    playerInfo.Add(new PlayerInfo() { playerName = keyValuePair.Value.playerName,
                    playerStatus = keyValuePair.Value.playerStatus.ToString(), pointsNumber = keyValuePair.Value.pointsNumber,
                    numberOfPlayedGames = keyValuePair.Value.numberOfPlayedGames, winsNumber = keyValuePair.Value.winsNumber,
                    drawsNumber = keyValuePair.Value.drawsNumber, losesNumber = keyValuePair.Value.losesNumber, 
                    rightAnswersNumber = keyValuePair.Value.rightAnswersNumber, wrongAnswersNumber = keyValuePair.Value.wrongAnswersNumber
                    });
                }
                message.isfailedToGetStatistics = false;
                message.playerInfoList = playerInfo;
            }
            Server.SendMessage(message, listeningClientMessagesSocket);
        }

        int[] SelectionOfTwoRandomWrongAnswers(int numberOfRightAnswer)
        {
            var random = new Random();
            int[] resultTwoRandomValues = new int[2];

            int randomValue;
            while ((randomValue = random.Next(0, Server.NumberOfAnswers)) == numberOfRightAnswer);
            resultTwoRandomValues[0] = randomValue;

            randomValue = random.Next(0, Server.NumberOfAnswers);
            while ((randomValue == numberOfRightAnswer) || (randomValue == resultTwoRandomValues[0]))
            {
                randomValue = random.Next(0, Server.NumberOfAnswers);
            }
            resultTwoRandomValues[1] = randomValue;

            return resultTwoRandomValues;
        }

        int[] DefineProbabilityOfAnswers(int numberOfRightAnswer, int questionNumber)
        {
            var random = new Random();
            int[] resultFourRandomPercentagesOfAnswers = new int[Server.NumberOfAnswers];

            if (questionNumber < 5)
            {
                resultFourRandomPercentagesOfAnswers[numberOfRightAnswer] = random.Next(50, 70);
                int leftProbability = 100 - resultFourRandomPercentagesOfAnswers[numberOfRightAnswer];
                int[] leftProbabilities = new int[Server.NumberOfAnswers - 1];
                leftProbabilities[0] = random.Next(0, leftProbability / 3);
                leftProbabilities[1] = random.Next(0, leftProbability / 3);
                leftProbabilities[2] = leftProbability - leftProbabilities[0] - leftProbabilities[1];

                int i = 0;
                int j = 0;
                while (i < 3)
                {
                    if (numberOfRightAnswer != j)
                    {
                        resultFourRandomPercentagesOfAnswers[j] = leftProbabilities[i];
                        i++;
                    }
                    j++;
                }
            }
            else if (questionNumber < 8)
            {
                resultFourRandomPercentagesOfAnswers[numberOfRightAnswer] = random.Next(30, 50);
                int[] leftProbabilities = new int[Server.NumberOfAnswers - 1];
                leftProbabilities[0] = random.Next(25, 40);
                int leftProbability = 100 - resultFourRandomPercentagesOfAnswers[numberOfRightAnswer] - leftProbabilities[0];
                leftProbabilities[1] = random.Next(0, leftProbability / 2);
                leftProbabilities[2] = leftProbability - leftProbabilities[1];

                int i = 0;
                int j = 0;
                while (i < 3)
                {
                    if (numberOfRightAnswer != j)
                    {
                        resultFourRandomPercentagesOfAnswers[j] = leftProbabilities[i];
                        i++;
                    }
                    j++;
                }
            }
            else
            {
                for (int i = 0; i < Server.NumberOfAnswers - 1; i++)
                {
                    resultFourRandomPercentagesOfAnswers[i] = random.Next(20, 30);
                }
                resultFourRandomPercentagesOfAnswers[Server.NumberOfAnswers - 1] = 100 - resultFourRandomPercentagesOfAnswers[0] -
                resultFourRandomPercentagesOfAnswers[1] - resultFourRandomPercentagesOfAnswers[2];
            }
            return resultFourRandomPercentagesOfAnswers;
        }

        void ProcessPromptRequestMesasage(Message message)
        {
            foreach (Game game in Server.GamesList)
            {
                if (((game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                || (game.secondPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())) && game.isPlayedNow)
                {
                    if (message.is5050Prompt)
                    {
                        int[] randomWrongAnwers = SelectionOfTwoRandomWrongAnswers(game.rigthAnswersOnQuestions[message.answeredQuestionNumber]);
                        message.twoWrongAnswersFor5050Prompt = randomWrongAnwers;
                    }
                    else
                    {
                        int[] probabilityOfAnswers = DefineProbabilityOfAnswers(game.rigthAnswersOnQuestions[message.answeredQuestionNumber],
                        message.answeredQuestionNumber);
                        message.probabilityOfAnswersCorrectness = probabilityOfAnswers;
                    }
                    message.messageType = Message.MessageType.PromptResponse;
                    Server.SendMessage(message, listeningClientMessagesSocket);
                }
            }
        }

        void ProcessInterruptSearchingForOpponentMesasage(Message message)
        {
            Server.waitingForRandomGamePlayers.Clear();
        }

        public List<ClientsInfo> GetClientsList()
        {
            List<ClientsInfo> info = new List<ClientsInfo>();
            foreach (KeyValuePair<int, string> keyValuePair in Server.clientNames)
            {
                info.Add(new ClientsInfo() { clientID = keyValuePair.Key, clientName = keyValuePair.Value });
            }
            return info;
        }

        public List<string> GetGameTopics()
        {
            List<string> topics = new List<string>();
            foreach (KeyValuePair<GameTopic, string> keyValuePair in QuestionsForTopic.TopicsDictionary)
            {
                topics.Add(keyValuePair.Value);
            }
            return topics;
        }

        bool IsClientConnected()
        {
            bool IsConnected = true;
            try
            {
                listeningClientMessagesSocket.Send(messageSerializer.Serialize(new Message() { messageType = Message.MessageType.CheckConnection}));
                if (!listeningClientMessagesSocket.Connected)
                    IsConnected = false;
            }
            catch
            {
                IsConnected = false;
            }
            return IsConnected;
        }

        public string RemoveClient()
        {
            string leftClientName = Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()];
            Console.WriteLine(leftClientName + " покинул беседу");
            Server.RemoveClient(listeningClientMessagesSocket.RemoteEndPoint.GetHashCode());
            listeningClientMessagesSocket.Close();
            listeningClientMessagesSocket = null;
            return leftClientName;
        }

        public void NotifyClientLeft(string leftClientName)
        {
            Server.SendToAll(new Message(leftClientName, " покинул беседу", Message.MessageType.Common));
            List<ClientsInfo> info = GetClientsList();
            Server.SendToAll(new Message(info));
        }

        public void DisconnectClient()
        {
            string leftCLientName = RemoveClient();
            NotifyClientLeft(leftCLientName);
        }
    }
}

