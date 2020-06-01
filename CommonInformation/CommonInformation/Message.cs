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

    public struct PlayerInfo
    {
        public string playerName;
        public string playerStatus;

        public int numberOfPlayedGames;
        public int pointsNumber;

        public int winsNumber;
        public int drawsNumber;
        public int losesNumber;
        public int rightAnswersNumber;
        public int wrongAnswersNumber;
    }
    
    public class Message
    {
        public enum MessageType { Common, Private, History, JoinToChat, SendGameTopics, ClientsList, SearchRequest, SearchResponce,
        CheckConnection, StartGameRequest, StartGameResponse, PromptRequest, PromptResponse, PlayerAnswer, OpponentRightAnswer,
        YourAnswerStatus, GameStatus, GameResults, LeftGame, GetStatistics, InterruptSearchingForOpponent };

        public int clientPort;
        public int serverPort;
        public int messageSenderID;
        public int messageReceiverID;
        public string IPAdress;

        public MessageType messageType;
        public string messageContent;
        public string messageName;

        public bool isSelectedOpponentForGame;
        public string gameStartDetails;
        public bool mayStartGame;
        public string[] questionsToSend;
        public string[] answersToSend;
        public int answeredQuestionNumber;
        public int answerNumber;
        public bool isCorrectAnswer;
        public string gameStatus;
        public int gameTopic;

        public bool is5050Prompt;
        public int[] twoWrongAnswersFor5050Prompt;
        public int[] probabilityOfAnswersCorrectness;

        public PlayerInfo playerInfo;
        public bool isfailedToGetStatistics;

        public List<ClientsInfo> clientsInfo;
        public List<PlayerInfo> playerInfoList;
        public List<string> GameTopics;
        public List<string> messageHistory;

        public DateTime messageTime;

        public Message(string iPAdress, int port, MessageType messageType)
        {
            IPAdress = iPAdress;
            clientPort = port;
            this.messageType = messageType;
        }

        public Message(string name, string content, MessageType type)
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
            messageType = MessageType.Private;
        }

        public Message(List<ClientsInfo> clientsInfo)
        {
            this.clientsInfo = clientsInfo;
            messageType = MessageType.ClientsList;
        }

        public Message() { }
    }
}

