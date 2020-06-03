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
        private const int Timeout = 700;
        private const int TopicsNumber = 3;
        private const int StartNumber = 0;
        private const int BoundaryOfLightQuestions = 5;
        private const int BoundaryOfMediumQuestions = 8;
        private const int MaxProbability = 100;
        private readonly int[] Probabilities = new int[6] { 20, 25, 30, 40, 50, 70 };

        private const string JoinChat = " присоединился к беседе";
        private const string LeftChat = " покинул беседу";
        private const string WaitingForOpponent = "Ожидайте оппонента...";

        private Socket listeningClientMessagesSocket;
        private Thread handleClientThread;
        private MessageSerializer messageSerializer;
        public bool isConnected;
        
        private int[] randomNumbersOfQuestions = new int[Server.NumberOfQuestionsInOneGame]; 

        public ClientConnectionManager(Socket listeningClientMessagesSocket)
        {
            messageSerializer = new MessageSerializer();
            isConnected = true;
            this.listeningClientMessagesSocket = listeningClientMessagesSocket;
            listeningClientMessagesSocket.ReceiveTimeout = Timeout;
            listeningClientMessagesSocket.SendTimeout = Timeout;
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
            byte[] data = new byte[Server.MessageCapacity];
            int amount;
            do
            {
                MemoryStream messageContainer = new MemoryStream();
                try
                {
                    do
                    {
                        amount = listeningClientMessagesSocket.Receive(data);
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

        private void DefineMessageType(Message message)
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

        private void ProcessCommonMessage(Message message)
        {
            message.messageName = Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()];
            Console.WriteLine(message.messageName + " : " + message.messageContent);
            Server.SendToAll(message);
            Server.messageHistory.Add(message.messageTime.ToString() + " - " + message.IPAdress + " - "
            + message.messageName + " : " + message.messageContent);
        }

        private void ProcessPrivateMessage(Message message)
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

        private void ProcessHistoryRequestMessage(Message message)
        {
            Message responseMessage = new Message() { messageHistory = Server.messageHistory, messageType = Message.MessageType.History };
            Server.SendMessage(responseMessage, listeningClientMessagesSocket);
        }

        private void ProcessJoinToChatMessage(Message message)
        {
            Server.clientNames.Add(listeningClientMessagesSocket.RemoteEndPoint.GetHashCode(), message.messageName);
            Console.WriteLine(message.messageName + JoinChat);

            var responseTopicsMessage = new Message()
            { messageType = Message.MessageType.SendGameTopics, gameTopics = GetGameTopics() };
            Server.SendMessage(responseTopicsMessage, listeningClientMessagesSocket);

            List<ClientsInfo> info = GetClientsList();
            Server.SendToAll(new Message(info));
            Server.SendToAll(new Message(message.messageName, JoinChat, Message.MessageType.Common));
        }

        private bool isOpponentPlaying(int opponentID)
        {
            bool flag = false;
            foreach(Game game in Server.gamesList)
            {
                if (((game.firstPlayerID == opponentID) || (game.secondPlayerID == opponentID)) && game.isPlayedNow)
                {
                    flag = true;
                }      
            }
            return flag;
        }

        private void ProcessStartGameRequestMessage(Message message)
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
                if (Server.waitingForRandomGamePlayers.Count == StartNumber)
                {
                    Server.waitingForRandomGamePlayers.Add(listeningClientMessagesSocket.RemoteEndPoint.GetHashCode());
                }
                else
                {
                    var random = new Random();
                    int randomValue = random.Next(StartNumber, TopicsNumber);

                    var responseGameMessage = new CommonInformation.Message()
                    {
                        messageType = Message.MessageType.StartGameResponse,
                        messageSenderID = Server.waitingForRandomGamePlayers[StartNumber],
                        messageReceiverID = listeningClientMessagesSocket.RemoteEndPoint.GetHashCode(),
                        gameTopic = randomValue,
                        messageName = Server.clientNames[Server.waitingForRandomGamePlayers[StartNumber]],
                        mayStartGame = true
                    };
                    Server.waitingForRandomGamePlayers.RemoveAt(StartNumber);
                    OrganizeGame(responseGameMessage);
                }
            }
            
        }

        private GameTopic DefineGameTopic(int gameTopic)
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
            {
                return GameTopic.TechnicalProgress;
            }       
        }

        private void GenerateRandomQuestions()
        {
            Random random = new Random();

            int currentAttempt = StartNumber;
            int randomVariable;
            while (currentAttempt < randomNumbersOfQuestions.Length)
            {
                randomVariable = random.Next(StartNumber, Server.AllQuestionsInTopic);
                bool isNOTExistsThisNumber = true;
                int j = StartNumber;
                while ((j < currentAttempt) && isNOTExistsThisNumber)
                {
                    if (randomVariable == randomNumbersOfQuestions[j])
                        isNOTExistsThisNumber = false;
                    j++;
                }
                if (isNOTExistsThisNumber)
                {
                    randomNumbersOfQuestions[currentAttempt] = randomVariable;
                    currentAttempt++;
                }
            }
        }

        private void OrganizeGame(Message message)
        {
            GameTopic gameTopic = DefineGameTopic(message.gameTopic);
            GenerateRandomQuestions();

            var game = new Game(message.messageSenderID, message.messageReceiverID, message.messageName,
            Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()]);

            for (int i = 0; i < Server.NumberOfQuestionsInOneGame; i++)
            {
                game.rigthAnswersOnQuestions[i] = randomNumbersOfQuestions[i] % Server.NumberOfAnswers;
            }

            Server.gamesList.Add(game);
            Console.WriteLine("Game is set");

            message.questionsToSend = new string[Server.NumberOfQuestionsInOneGame];
            message.answersToSend = new string[Server.NumberOfAnswers * Server.NumberOfQuestionsInOneGame];
            for (int i = 0; i < Server.NumberOfQuestionsInOneGame; i++)
            {
                message.questionsToSend[i] = Server.questionsDictionary[gameTopic].questions[randomNumbersOfQuestions[i]];
            }    
            int k = StartNumber;
            for (int i = 0; i < Server.NumberOfQuestionsInOneGame; i++)
            {
                for (int j = 0; j < Server.NumberOfAnswers; j++)
                {
                    message.answersToSend[k] = Server.questionsDictionary[gameTopic].answers[randomNumbersOfQuestions[i], j].Title;
                    k++;
                }
            }

            message.opponentForGameName = Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()];
            Server.SendMessage(message, Server.clientSockets[message.messageSenderID]);

            message.opponentForGameName = message.messageName;
            Server.SendMessage(message, Server.clientSockets[message.messageReceiverID]);
        }

        private void ProcessStartGameResponseMessage(Message message)
        {
            if (message.mayStartGame)
            {
                OrganizeGame(message);
            }
            else
            {
                message.gameStartDetails = Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()] + " не хочет с Вами играть";
                Server.SendMessage(message, Server.clientSockets[message.messageSenderID]);
            }
        }

        private void NotifyAboutEndedGame(string gameResults)
        {
            var messageGameResults = new Message() { messageName = "Сервер", messageContent = gameResults,
            messageTime = DateTime.Now, messageType = Message.MessageType.Common, IPAdress = "" };
            Console.WriteLine(messageGameResults.messageName + " : " + messageGameResults.messageContent);
            Server.SendToAll(messageGameResults);
            Server.messageHistory.Add(messageGameResults.messageTime.ToString() + "  " + messageGameResults.messageName
            + " : " + messageGameResults.messageContent);
        }

        private string DefineGameResults(int yourPoints, int opponentPoints, string opponentName)
        {
            string gameResults;
            if (yourPoints > opponentPoints)
            {
                gameResults = "Поздравляем, вы выиграли у " + opponentName + " со счётом " + yourPoints.ToString() + ":"
                + opponentPoints.ToString() + ". ";
            }
            else if (yourPoints < opponentPoints)
            {
                gameResults = "Увы, вы потерпели поражение от " + opponentName + " со счётом " + opponentPoints.ToString() + ":"
                + yourPoints.ToString() + ". ";
            }
            else
            {
                gameResults = "Эхх, ничья с " + opponentName + " - " + yourPoints.ToString() + ":" + opponentPoints.ToString() + ". ";
            }
            return gameResults;
        }

        private string UpdatePlayerInformation(int playerID, string playername, int yourPoints, int opponentPoints, bool isLeftGame)
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
                    playerInformation.pointsNumber = StartNumber;
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

        private void ProcessPlayerAnswerMessage(Message message)
        {
            bool firstPlayerFlag = false;
            foreach (Game game in Server.gamesList)
            {
                if ( (game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                    || (game.secondPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()) )
                {
                    if (game.isPlayedNow)
                    {
                        if (game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                        {
                            firstPlayerFlag = true;
                        }
                            
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

                                    responseResultMessage.gameStatus = DefineGameResults(game.secondPlayerPoints, game.firstPlayerPoints,
                                    game.firstPlayerName);
                                    responseResultMessage.gameStatus = responseResultMessage.gameStatus
                                    + UpdatePlayerInformation(game.secondPlayerID, Server.clientNames[game.secondPlayerID],
                                    game.secondPlayerPoints, game.firstPlayerPoints, false);
                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);

                                    Thread.Sleep(250);

                                    responseResultMessage.gameStatus = DefineGameResults(game.firstPlayerPoints, game.secondPlayerPoints,
                                    game.secondPlayerName);
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
                                    responseStatusMessage.gameStatus = WaitingForOpponent;
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

                                    responseResultMessage.gameStatus = DefineGameResults(game.firstPlayerPoints, game.secondPlayerPoints,
                                    game.secondPlayerName);
                                    responseResultMessage.gameStatus = responseResultMessage.gameStatus
                                    + UpdatePlayerInformation(game.firstPlayerID, Server.clientNames[game.firstPlayerID],
                                    game.firstPlayerPoints, game.secondPlayerPoints, false);
                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);

                                    Thread.Sleep(250);

                                    responseResultMessage.gameStatus = DefineGameResults(game.secondPlayerPoints, game.firstPlayerPoints,
                                    game.firstPlayerName);
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
                                    responseStatusMessage.gameStatus = WaitingForOpponent;
                                    Server.SendMessage(responseStatusMessage, Server.clientSockets[game.secondPlayerID]);
                                }
                            }
                        }

                    }
                }
            }
        }

        private void ProcessLeftGameMesasage(Message message)
        {
            foreach (Game game in Server.gamesList)
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
                        game.firstPlayerPoints = StartNumber;
                        if (game.secondPlayerPoints == StartNumber)
                        {
                            game.secondPlayerPoints = 3;
                        }
                        isFirstPlayerLeft = true;
                    }     
                    else
                    {
                        game.secondPlayerPoints = StartNumber;
                        if (game.firstPlayerPoints == StartNumber)
                        {
                            game.firstPlayerPoints = 3;
                        }
                        isFirstPlayerLeft = false;
                    }
                           
                    game.gameResults = game.firstPlayerName + "   " + game.firstPlayerPoints + " : "
                    + game.secondPlayerPoints + "   " + game.secondPlayerName + " (тех.поражение)";

                    if (isFirstPlayerLeft)
                    {
                        responseResultMessage.gameStatus = DefineGameResults(game.secondPlayerPoints, game.firstPlayerPoints, game.firstPlayerName);
                        string updatedResultOfFirstPlayer = UpdatePlayerInformation(game.firstPlayerID, Server.clientNames[game.firstPlayerID],
                        game.firstPlayerPoints, game.secondPlayerPoints, true);
                        string updatedResultOfSecondPlayer = UpdatePlayerInformation(game.secondPlayerID, Server.clientNames[game.secondPlayerID],
                        game.secondPlayerPoints, game.firstPlayerPoints, false);
                        responseResultMessage.gameStatus += updatedResultOfSecondPlayer;
                        Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);
                        Thread.Sleep(100);

                        if (Server.clientSockets.ContainsKey(game.firstPlayerID))
                        {
                            responseResultMessage.gameStatus = DefineGameResults(game.firstPlayerPoints, game.secondPlayerPoints,
                            game.secondPlayerName);
                            responseResultMessage.gameStatus += updatedResultOfFirstPlayer;
                            Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);
                        }
                    }
                    else
                    {
                        responseResultMessage.gameStatus = DefineGameResults(game.firstPlayerPoints, game.secondPlayerPoints, game.secondPlayerName);
                        string updatedResultOfFirstPlayer = UpdatePlayerInformation(game.firstPlayerID, Server.clientNames[game.firstPlayerID],
                        game.firstPlayerPoints, game.secondPlayerPoints, false);
                        string updatedResultOfSecondPlayer = UpdatePlayerInformation(game.secondPlayerID, Server.clientNames[game.secondPlayerID],
                        game.secondPlayerPoints, game.firstPlayerPoints, true);
                        responseResultMessage.gameStatus += updatedResultOfFirstPlayer;
                        Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);
                        Thread.Sleep(100);

                        if (Server.clientSockets.ContainsKey(game.secondPlayerID))
                        {
                            responseResultMessage.gameStatus = DefineGameResults(game.secondPlayerPoints, game.firstPlayerPoints,
                            game.firstPlayerName);
                            responseResultMessage.gameStatus += updatedResultOfSecondPlayer;
                            Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);
                        }
                    }
                    game.isPlayedNow = false;
                    NotifyAboutEndedGame(game.gameResults);
                }
            }
        }

        private void ProcessGetStatisticsMesasage(Message message)
        {
            if (message.messageReceiverID != StartNumber)
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

        private int[] SelectionOfTwoRandomWrongAnswers(int numberOfRightAnswer)
        {
            const int WrongAnswersNumber = 2;
            var random = new Random();
            int[] resultTwoRandomValues = new int[WrongAnswersNumber];

            int randomValue;
            while ((randomValue = random.Next(StartNumber, Server.NumberOfAnswers)) == numberOfRightAnswer);
            resultTwoRandomValues[0] = randomValue;

            randomValue = random.Next(StartNumber, Server.NumberOfAnswers);
            while ((randomValue == numberOfRightAnswer) || (randomValue == resultTwoRandomValues[0]))
            {
                randomValue = random.Next(StartNumber, Server.NumberOfAnswers);
            }
            resultTwoRandomValues[1] = randomValue;

            return resultTwoRandomValues;
        }

        private int[] DefineProbabilityOfAnswers(int numberOfRightAnswer, int questionNumber)
        {
            var random = new Random();
            int[] resultFourRandomPercentagesOfAnswers = new int[Server.NumberOfAnswers];

            if (questionNumber < BoundaryOfLightQuestions)
            {
                resultFourRandomPercentagesOfAnswers[numberOfRightAnswer] = random.Next(Probabilities[4], Probabilities[5]);
                int leftProbability = MaxProbability - resultFourRandomPercentagesOfAnswers[numberOfRightAnswer];
                int[] leftProbabilities = new int[Server.NumberOfAnswers - 1];
                leftProbabilities[0] = random.Next(0, leftProbability / (Server.NumberOfAnswers - 1));
                leftProbabilities[1] = random.Next(0, leftProbability / (Server.NumberOfAnswers - 1));
                leftProbabilities[2] = leftProbability - leftProbabilities[0] - leftProbabilities[1];

                int i = 0;
                int j = 0;
                while (i < (Server.NumberOfAnswers - 1))
                {
                    if (numberOfRightAnswer != j)
                    {
                        resultFourRandomPercentagesOfAnswers[j] = leftProbabilities[i];
                        i++;
                    }
                    j++;
                }
            }
            else if (questionNumber < BoundaryOfMediumQuestions)
            {
                resultFourRandomPercentagesOfAnswers[numberOfRightAnswer] = random.Next(Probabilities[2], Probabilities[4]);
                int[] leftProbabilities = new int[Server.NumberOfAnswers - 1];
                leftProbabilities[0] = random.Next(Probabilities[1], Probabilities[3]);
                int leftProbability = MaxProbability - resultFourRandomPercentagesOfAnswers[numberOfRightAnswer] - leftProbabilities[0];
                leftProbabilities[1] = random.Next(0, leftProbability / 2);
                leftProbabilities[2] = leftProbability - leftProbabilities[1];

                int i = 0;
                int j = 0;
                while (i < (Server.NumberOfAnswers - 1))
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
                    resultFourRandomPercentagesOfAnswers[i] = random.Next(Probabilities[0], Probabilities[2]);
                }
                resultFourRandomPercentagesOfAnswers[Server.NumberOfAnswers - 1] = MaxProbability - resultFourRandomPercentagesOfAnswers[0] -
                resultFourRandomPercentagesOfAnswers[1] - resultFourRandomPercentagesOfAnswers[2];
            }
            return resultFourRandomPercentagesOfAnswers;
        }

        private void ProcessPromptRequestMesasage(Message message)
        {
            foreach (Game game in Server.gamesList)
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

        private void ProcessInterruptSearchingForOpponentMesasage(Message message)
        {
            Server.waitingForRandomGamePlayers.Remove(listeningClientMessagesSocket.RemoteEndPoint.GetHashCode());
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
            foreach (KeyValuePair<GameTopic, string> keyValuePair in QuestionsForTopic.topicsDictionary)
            {
                topics.Add(keyValuePair.Value);
            }
            return topics;
        }

        private bool IsClientConnected()
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
            Console.WriteLine(leftClientName + LeftChat);
            Server.RemoveClient(listeningClientMessagesSocket.RemoteEndPoint.GetHashCode());
            listeningClientMessagesSocket.Close();
            listeningClientMessagesSocket = null;
            return leftClientName;
        }

        public void NotifyClientLeft(string leftClientName)
        {
            Server.SendToAll(new Message(leftClientName, LeftChat, Message.MessageType.Common));
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

