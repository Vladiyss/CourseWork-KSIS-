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
            if (message.messageReceiverID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
            {
                var responseMessage = new Message() { messageType = Message.MessageType.StartGameResponse,
                gameStartDetails = "Самому с собой играть нельзя!", mayStartGame = false };
                Server.SendMessage(responseMessage, Server.clientSockets[message.messageReceiverID]);
            }
            else
            {
                if (isOpponentPlaying(message.messageReceiverID))
                {
                    var responseMessage = new Message() { messageType = Message.MessageType.StartGameResponse,
                    gameStartDetails = "Оппонент в текущий момент уже играет!", mayStartGame = false };
                    Server.SendMessage(responseMessage, Server.clientSockets[message.messageReceiverID]);
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

        void ProcessStartGameResponseMessage(Message message)
        {
            if (message.mayStartGame)
            {
                var game = new Game(message.messageSenderID, message.messageReceiverID, message.messageName,
                Server.clientNames[listeningClientMessagesSocket.RemoteEndPoint.GetHashCode()]);
                Server.GamesList.Add(game);
                Console.WriteLine("game is set");

                message.questionsToSend = new string[Server.NumberOfQuestions];
                message.answersToSend = new string[40];
                for(int i = 0; i < 10; i++)
                    message.questionsToSend[i] = Server.questions[i].Title;
                int k = 0;
                for (int i = 0; i < 10; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        message.answersToSend[k] = Server.answers[i, j].Title;
                        k++;
                    }

                Console.WriteLine(message.messageSenderID);
                Console.WriteLine(message.messageReceiverID);
                Server.SendMessage(message, Server.clientSockets[message.messageSenderID]);
                Server.SendMessage(message, Server.clientSockets[message.messageReceiverID]);
                Console.WriteLine("messages is sended");
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

                        if (Server.answers[message.answeredQuestionNumber, message.answerNumber].IsRight)
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
                            if (Server.NumberOfQuestions == game.firstNumberAnsweredQuestions)
                            {
                                game.isFirstFinished = true;

                                if (game.isSecondFinished)
                                {
                                    var responseResultMessage = new Message() { messageType = Message.MessageType.GameResults };
                                    game.gameResults = game.firstPlayerName + "   " + game.firstPlayerPoints + " : "
                                    + game.secondPlayerPoints + "   " + game.secondPlayerName;
                                    responseResultMessage.gameStatus = game.gameResults;

                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);
                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);

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
                            if (Server.NumberOfQuestions == game.secondNumberAnsweredQuestions)
                            {
                                game.isSecondFinished = true;

                                if (game.isFirstFinished)
                                {
                                    var responseResultMessage = new Message() { messageType = Message.MessageType.GameResults };
                                    game.gameResults = game.firstPlayerName + "   " + game.firstPlayerPoints + " : "
                                    + game.secondPlayerPoints + "   " + game.secondPlayerName;
                                    responseResultMessage.gameStatus = game.gameResults;

                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);
                                    Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);
                                    
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
                    var responseResultMessage = new Message() { messageType = Message.MessageType.GameResults };

                    game.isFirstFinished = true;
                    game.isSecondFinished = true;
                    if (game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                        game.firstPlayerPoints = 0;
                    else
                        game.secondPlayerPoints = 0;

                    game.gameResults = game.firstPlayerName + "   " + game.firstPlayerPoints + " : "
                    + game.secondPlayerPoints + "   " + game.secondPlayerName + " (тех.поражение)";
                    responseResultMessage.gameStatus = game.gameResults;

                    if (game.firstPlayerID == listeningClientMessagesSocket.RemoteEndPoint.GetHashCode())
                        Server.SendMessage(responseResultMessage, Server.clientSockets[game.secondPlayerID]);
                    else
                        Server.SendMessage(responseResultMessage, Server.clientSockets[game.firstPlayerID]);

                    game.isPlayedNow = false;

                    NotifyAboutEndedGame(game.gameResults);

                }
            }
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

