using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonInformation
{
    public struct ClientsInfo
    {
        public int clientID;
        public string clientName;
    }

    public class Message
    {
        public static Dictionary<int, string> MessageType = new Dictionary<int, string>()
        {
            [1] = "Common",
            [2] = "Private",
            [3] = "History",
            [4] = "JoinToChat",
            [5] = "ClientsList",
            [6] = "SearchRequest",
            [7] = "SearchResponce"
        };

        public int clientPort;
        public int serverPort;
        public int messageSenderID;
        public int messageReceiverID;
        public string IPAdress;

        public string messageType;
        public string messageContent;
        public string messageName;

        public List<ClientsInfo> clientsInfo;
        public List<string> messageHistory;

        public DateTime messageTime;

        public Message(string iPAdress, int port, string messageType)
        {
            IPAdress = iPAdress;
            clientPort = port;
            this.messageType = messageType;
        }

        public Message(string name, string content, string type)
        {
            messageName = name;
            messageContent = content;
            messageType = type;
            IPAdress = "";
        }

        public Message(int receiver, string content)
        {
            messageReceiverID = receiver;
            messageContent = content;
            messageType = MessageType[2];
        }

        public Message(List<ClientsInfo> clientsInfo)
        {
            this.clientsInfo = clientsInfo;
            messageType = MessageType[5];
        }

        public Message() { }
    }
}

