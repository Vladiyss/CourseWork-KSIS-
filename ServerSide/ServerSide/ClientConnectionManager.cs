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
            byte[] data = new byte[1024];
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

        void DefineMessageType(Message message)
        {
            int i = CommonInfo.RetrieveMessageType(message.messageType);

            message.messageTime = DateTime.Now;
            message.IPAdress = ((IPEndPoint)listeningClientMessagesSocket.RemoteEndPoint).Address.ToString();
            switch (i)
            {
                case 1:
                    ProcessCommonMessage(message);
                    break;
                case 2:
                    ProcessPrivateMessage(message);
                    break;
                case 3:
                    ProcessHistoryRequestMessage(message);
                    break;
                case 4:
                    ProcessJoinToChatMessage(message);
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
                Console.WriteLine("Failed to send message");
            }
        }

        void ProcessHistoryRequestMessage(Message message)
        {
            Message responseMessage = new Message() { messageHistory = Server.MessageHistory, messageType = Message.MessageType[3] };
            Server.SendMessage(responseMessage, listeningClientMessagesSocket);
        }

        void ProcessJoinToChatMessage(Message message)
        {
            Server.clientNames.Add(listeningClientMessagesSocket.RemoteEndPoint.GetHashCode(), message.messageName);
            Console.WriteLine(message.messageName + " присоединился к беседе");
            List<ClientsInfo> info = GetClientsList();
            Server.SendToAll(new Message(info));
            Server.SendToAll(new Message(message.messageName, " присоединился к беседе", Message.MessageType[1]));
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
                listeningClientMessagesSocket.Send(messageSerializer.Serialize(new Message()));
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
            Server.SendToAll(new Message(leftClientName, " покинул беседу", Message.MessageType[1]));
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

