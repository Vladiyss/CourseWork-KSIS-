using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using CommonInformation;

namespace ClientSide
{
    public partial class MainForm : Form
    {
        private const int ChatDialog = 0;
        private const int NumberOfQuestions = 10;
        private const int NumberOfAnswers = 4;
        private const int StartNumber = 0;
        private const int buttonAnswerBClicked = 1;
        private const int buttonAnswerCClicked = 2;
        private const int buttonAnswerDClicked = 3;
        private const int InvalidValue = -1;
        private readonly int[] BoundaryProbabilityOfAnswerCorrectness =  new int[] { 10, 20, 30, 40, 50, 60 }; 

        private const string Nothing = "---";
        private const string NotConnected = "Нет соединения...";
        private const string Connected = "ПОДКЛЮЧЕНО";
        private const string ChatName = "Чат";
        private const string StringWithNull = "0";
        private const string NullString = "";
        private const string Percent = "%";

        private Client client;

        private delegate void ProcessFormFilling();
        private Dictionary<int, AllDialogsMessages> chatDialogsInfo;
        private List<ClientsInfo> clientsInfo;
        private int currentDialog = ChatDialog;
        private int selectedReceiverIndex = 0;
        private int currentGameTopic = -1;

        private string[] questions = new string[NumberOfQuestions];
        private string[] answers = new string[NumberOfQuestions*NumberOfAnswers];
        private int currentQuestionNumber;
        private int currentAnswerNumber;
        private int messageSenderID;
        private int messageReceiverID;
        private int gametopic;
        private string messageSenderName;
        private bool isPromptUsed = false;

        public MainForm()
        {
            InitializeComponent();
            clientsInfo = new List<ClientsInfo>();
            clientsInfo.Add(new ClientsInfo() { clientID = ChatDialog, clientName = ChatName });
            chatDialogsInfo = new Dictionary<int, AllDialogsMessages>();
            chatDialogsInfo.Add(ChatDialog, new AllDialogsMessages(ChatName));

            client = new Client();
            client.ProcessReceivedMessagesEvent += ProcessReceivedMessages;
        }

        private void ChangeNamesOfAnswerButtons()
        {
            buttonAnswerA.Text = "A";
            buttonAnswerB.Text = "B";
            buttonAnswerC.Text = "C";
            buttonAnswerD.Text = "D";
        }

        private void ChangeAnswerButtonsColor()
        {
            buttonAnswerA.BackColor = SystemColors.Control;
            buttonAnswerB.BackColor = SystemColors.Control;
            buttonAnswerC.BackColor = SystemColors.Control;
            buttonAnswerD.BackColor = SystemColors.Control;
        }

        private void SetAnswerButtonsVisible()
        {
            buttonAnswerA.Visible = true;
            buttonAnswerB.Visible = true;
            buttonAnswerC.Visible = true;
            buttonAnswerD.Visible = true;
        }

        private void SetPromptsButtonsVisible()
        {
            button5050Prompt.Visible = true;
            buttonHallPrompt.Visible = true;
        }

        private void SetPromptsButtonsEnable(bool isEnabled)
        {
            button5050Prompt.Enabled = isEnabled;
            buttonHallPrompt.Enabled = isEnabled;
        }

        public void ShowQuestion()
        {
            if (currentQuestionNumber != NumberOfQuestions)
            {
                richTextBoxQuestion.Text = questions[currentQuestionNumber];
                labelAnswerA.Text = "A:  " + answers[currentAnswerNumber];
                labelAnswerB.Text = "B:  " + answers[currentAnswerNumber + 1];
                labelAnswerC.Text = "C:  " + answers[currentAnswerNumber + 2];
                labelAnswerD.Text = "D:  " + answers[currentAnswerNumber + 3];
                currentAnswerNumber += NumberOfAnswers;
                labelCurrentQuestionNumber.Text = (currentQuestionNumber + 1).ToString();

                if (isPromptUsed)
                {
                    SetPromptsButtonsVisible();
                    ChangeNamesOfAnswerButtons();
                    ChangeAnswerButtonsColor();
                    SetAnswerButtonsVisible();
                    isPromptUsed = false;
                }
            }
        }

        private void ClearGameField()
        {
            richTextBoxQuestion.Text = Nothing;
            labelAnswerA.Text = Nothing;
            labelAnswerB.Text = Nothing;
            labelAnswerC.Text = Nothing;
            labelAnswerD.Text = Nothing;
            if (isPromptUsed)
            {
                ChangeAnswerButtonsColor();
                ChangeNamesOfAnswerButtons();
                SetAnswerButtonsVisible();
                SetPromptsButtonsVisible();
                isPromptUsed = false;
            }
            
            labelAnswerStatus.Text = Nothing;
            labelCurrentQuestionNumber.Text = Nothing;

            buttonAnswerA.Enabled = false;
            buttonAnswerB.Enabled = false;
            buttonAnswerC.Enabled = false;
            buttonAnswerD.Enabled = false;
            SetPromptsButtonsEnable(false);
        }

        public void ProcessReceivedMessages(CommonInformation.Message message)
        {
            switch (message.messageType)
            {
                case CommonInformation.Message.MessageType.Common:
                    ProcessCommonMessage(message);
                    break;
                case CommonInformation.Message.MessageType.Private:
                    ProcessPrivateMessage(message);
                    break;
                case CommonInformation.Message.MessageType.History:
                    ProcessHistoryRequestMessage(message);
                    break;
                case CommonInformation.Message.MessageType.SendGameTopics:
                    ProcessSendGameTopicsMessage(message);
                    break;
                case CommonInformation.Message.MessageType.ClientsList:
                    ProcessClientsListMessage(message);
                    break;
                case CommonInformation.Message.MessageType.SearchResponce:
                    ProcessServerResponseMessage(message);
                    break;
                case CommonInformation.Message.MessageType.StartGameRequest:
                    ProcessStartGameRequestMessage(message);
                    break;
                case CommonInformation.Message.MessageType.StartGameResponse:
                    ProcessStartGameResponseMessage(message);
                    break;
                case CommonInformation.Message.MessageType.YourAnswerStatus:
                    ProcessYourAnswerStatusMessage(message);
                    break;
                case CommonInformation.Message.MessageType.OpponentRightAnswer:
                    ProcessOpponentRightAnswerMessage(message);
                    break;
                case CommonInformation.Message.MessageType.GameStatus:
                    ProcessGameStatusMessage(message);
                    break;
                case CommonInformation.Message.MessageType.GameResults:
                    ProcessGameResultsMessage(message);
                    break;
                case CommonInformation.Message.MessageType.GetStatistics:
                    ProcessGetStatisticsMessage(message);
                    break;
                case CommonInformation.Message.MessageType.PromptResponse:
                    ProcessPromptResponseMessage(message);
                    break;
                default:
                    return;
            }
            if ((message.messageType != CommonInformation.Message.MessageType.SearchResponce)
            && (message.messageType != CommonInformation.Message.MessageType.StartGameRequest)
            && (message.messageType != CommonInformation.Message.MessageType.StartGameResponse)
            && (message.messageType != CommonInformation.Message.MessageType.YourAnswerStatus)
            && (message.messageType != CommonInformation.Message.MessageType.OpponentRightAnswer)
            && (message.messageType != CommonInformation.Message.MessageType.GameStatus)
            && (message.messageType != CommonInformation.Message.MessageType.GameResults)
            && (message.messageType != CommonInformation.Message.MessageType.GetStatistics)
            && (message.messageType != CommonInformation.Message.MessageType.PromptResponse))
                UpdateView();
        }

        public void UpdateView()
        {
            ProcessFormFilling FormUpdate = delegate
            {
                richTextBoxChatContent.Clear();
                if (chatDialogsInfo != null)
                {
                    string[] messages = new string[chatDialogsInfo[currentDialog].messages.Count];
                    chatDialogsInfo[currentDialog].messages.CopyTo(messages);
                    foreach (string messageContent in messages)
                    {
                        richTextBoxChatContent.Text += messageContent + "\r\n";
                    }
                }
                comboBoxParticipants.Items.Clear();
                foreach (ClientsInfo clientInfo in clientsInfo)
                {
                    comboBoxParticipants.Items.Add(clientInfo.clientName);
                }

                labelCurrentClientDialog.Text = chatDialogsInfo[currentDialog].dialogName;

                if (currentDialog != ChatDialog)
                {
                    buttonPlay.Visible = true;
                    labelSelectTopic.Visible = true;
                    comboBoxGameTopics.Visible = true;
                }
                else
                {
                    buttonPlay.Visible = false;
                    labelSelectTopic.Visible = false;
                    comboBoxGameTopics.Visible = false;
                }
            };
            if (InvokeRequired)
                Invoke(FormUpdate);
            else
                FormUpdate();
        }

        private void ProcessCommonMessage(CommonInformation.Message message)
        {
            if (String.IsNullOrEmpty(message.IPAdress))
            {
                chatDialogsInfo[ChatDialog].AddMessage(DateTime.Now.ToShortTimeString()
                + " - " + message.messageName + " : " + message.messageContent);
            }
            else
            {
                chatDialogsInfo[ChatDialog].AddMessage(message.messageTime.ToString() + " - " + message.IPAdress
                + " - " + message.messageName + " : " + message.messageContent);
            }     
        }

        private void ProcessPrivateMessage(CommonInformation.Message message)
        {
            chatDialogsInfo[message.messageSenderID].AddMessage(message.messageTime.ToString() + " : " + message.messageContent);
            labelNewMessage.Text = "Новое сообщение от " + message.messageName;
        }

        private void ProcessHistoryRequestMessage(CommonInformation.Message message)
        {
            chatDialogsInfo[currentDialog].messages = message.messageHistory;
        }

        private void ProcessSendGameTopicsMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormFillingGameTopics = delegate
            {
                foreach (string topic in message.gameTopics)
                {
                    comboBoxGameTopics.Items.Add(topic);
                }
            };
            if (InvokeRequired)
                Invoke(FormFillingGameTopics);
            else
                FormFillingGameTopics();
        }

        private void ProcessClientsListMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormFillingNewClient = delegate
            {
                clientsInfo.Clear();
                clientsInfo.Add(new ClientsInfo() { clientID = ChatDialog, clientName = ChatName });
                foreach (ClientsInfo nameClient in message.clientsInfo)
                {
                    clientsInfo.Add(nameClient);
                    if (!chatDialogsInfo.ContainsKey(nameClient.clientID))
                    {
                        chatDialogsInfo.Add(nameClient.clientID, new AllDialogsMessages(nameClient.clientName));
                    }
                }
            };
            if (InvokeRequired)
                Invoke(FormFillingNewClient);
            else
                FormFillingNewClient();
        }

        private void ProcessServerResponseMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormFillingServerResponse = delegate
            {
                textBoxServerIPAddress.Text = message.IPAdress;
                textBoxServerPort.Text = message.serverPort.ToString();
                textBoxServerIPAddress.Enabled = false;
                textBoxServerPort.Enabled = false;
            };
            if (InvokeRequired)
                Invoke(FormFillingServerResponse);
            else
                FormFillingServerResponse();
        }

        private void ProcessStartGameRequestMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormAddGameRequest = delegate
            {
                richTextBoxGameStatus.Text = message.gameStartDetails;
                buttonAcceptGame.Visible = true;
                buttonRejectGame.Visible = true;
                messageSenderID = message.messageSenderID;
                messageReceiverID = message.messageReceiverID;
                messageSenderName = message.messageName;
                gametopic = message.gameTopic;
            };
            if (InvokeRequired)
                Invoke(FormAddGameRequest);
            else
                FormAddGameRequest();
        }

        private void ProcessStartGameResponseMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormAddGameResponse = delegate
            {
                if (message.mayStartGame == false)
                {
                    richTextBoxGameStatus.Text = message.gameStartDetails;
                }
                else
                {
                    questions = message.questionsToSend;
                    answers = message.answersToSend;
                    currentQuestionNumber = StartNumber;
                    currentAnswerNumber = StartNumber;
                    HideStatisticsTables();
                    ShowDetailsOfTheBeginningOfTheGameWithRandomPlayer(false);
                    GiveAccessToPlayButtons(false);
                    panelGame.Visible = true;
                    labelOpponent.Text = message.opponentForGameName;
                    richTextBoxGameStatus.Text = NullString;
                    labelAnswerStatus.Text = Nothing;
                    buttonAnswerA.Enabled = true;
                    buttonAnswerB.Enabled = true;
                    buttonAnswerC.Enabled = true;
                    buttonAnswerD.Enabled = true;
                    SetPromptsButtonsEnable(true);
                    ShowQuestion();
                    labelYourPointsNumber.Text = StringWithNull;
                    labelOpponentPointsNumber.Text = StringWithNull;
                }
            };
            if (InvokeRequired)
                Invoke(FormAddGameResponse);
            else
                FormAddGameResponse();
        }

        private void ProcessYourAnswerStatusMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormAnswerStatus = delegate
            {
                if (message.isCorrectAnswer)
                {
                    labelAnswerStatus.Text = "Верно!";
                    int points = int.Parse(labelYourPointsNumber.Text);
                    points++;
                    labelYourPointsNumber.Text = points.ToString();
                }
                else
                {
                    labelAnswerStatus.Text = "Неверно!";
                }
            };
            if (InvokeRequired)
                Invoke(FormAnswerStatus);
            else
                FormAnswerStatus();
        }

        private void ProcessOpponentRightAnswerMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormOpponentAnswerStatus = delegate
            {
                int opponentPoints = int.Parse(labelOpponentPointsNumber.Text);
                opponentPoints++;
                labelOpponentPointsNumber.Text = opponentPoints.ToString();

            };
            if (InvokeRequired)
                Invoke(FormOpponentAnswerStatus);
            else
                FormOpponentAnswerStatus();
        }

        private void ProcessGameStatusMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormGameStatus = delegate
            {
                ClearGameField();
                richTextBoxGameStatus.Text = message.gameStatus;
            };
            if (InvokeRequired)
                Invoke(FormGameStatus);
            else
                FormGameStatus();
        }

        private void ProcessGameResultsMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormGameResult = delegate
            {
                ClearGameField();
                labelOpponent.Text = Nothing;
                panelGame.Visible = false;
                richTextBoxGameStatus.Text = message.gameStatus;
                GiveAccessToPlayButtons(true);
            };
            if (InvokeRequired)
                Invoke(FormGameResult);
            else
                FormGameResult();
        }

        private void ProcessGetStatisticsMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormGetStatisticsResult = delegate
            {
                if (message.messageReceiverID != 0)
                {
                    richTextBoxStatistics.Visible = true;
                    richTextBoxStatistics.Clear();
                    if (message.isfailedToGetStatistics)
                        richTextBoxStatistics.Text = "Данный участник чата ещё ни разу не играл!";
                    else
                    {
                        richTextBoxStatistics.Text = message.playerInfo.playerName + "(" + message.playerInfo.playerStatus
                        + ")" + "\r\n";
                        richTextBoxStatistics.Text += "Игры                     " + message.playerInfo.numberOfPlayedGames + "\r\n";
                        richTextBoxStatistics.Text += "Победы/ничьи/поражения   " + message.playerInfo.winsNumber
                        + "/" + message.playerInfo.drawsNumber + "/" + message.playerInfo.losesNumber + "\r\n";
                        richTextBoxStatistics.Text += "Верные/неверные ответы   " + message.playerInfo.rightAnswersNumber
                        + "/" + message.playerInfo.wrongAnswersNumber + "\r\n";
                        richTextBoxStatistics.Text += "Количество очков         " + message.playerInfo.pointsNumber + "\r\n";
                    }
                }
                else
                {
                    dataGridViewStatistics.Rows.Clear();
                    dataGridViewStatistics.Visible = true;
                    foreach (PlayerInfo playerInfo in message.playerInfoList)
                    {
                        dataGridViewStatistics.Rows.Add(playerInfo.playerName, playerInfo.numberOfPlayedGames,
                        playerInfo.winsNumber, playerInfo.drawsNumber, playerInfo.losesNumber, playerInfo.rightAnswersNumber,
                        playerInfo.wrongAnswersNumber, playerInfo.pointsNumber);
                    }
                }
            };
            if (InvokeRequired)
                Invoke(FormGetStatisticsResult);
            else
                FormGetStatisticsResult();
        }

        private void ProcessPromptResponseMessage(CommonInformation.Message message)
        {
            ProcessFormFilling FormPromptResponse = delegate
            {
                if (message.is5050Prompt)
                {
                    RemoveOneIncorrectAnswer(message.twoWrongAnswersFor5050Prompt[0]);
                    RemoveOneIncorrectAnswer(message.twoWrongAnswersFor5050Prompt[1]);
                }
                else
                {
                    buttonAnswerA.Text = "A   " + message.probabilityOfAnswersCorrectness[0].ToString() + Percent;
                    buttonAnswerB.Text = "B   " + message.probabilityOfAnswersCorrectness[1].ToString() + Percent;
                    buttonAnswerC.Text = "C   " + message.probabilityOfAnswersCorrectness[2].ToString() + Percent;
                    buttonAnswerD.Text = "D   " + message.probabilityOfAnswersCorrectness[3].ToString() + Percent;

                    ChangeButtonColorAccordingToProbabilityOfVarian(buttonAnswerA, message.probabilityOfAnswersCorrectness[0]);
                    ChangeButtonColorAccordingToProbabilityOfVarian(buttonAnswerB, message.probabilityOfAnswersCorrectness[1]);
                    ChangeButtonColorAccordingToProbabilityOfVarian(buttonAnswerC, message.probabilityOfAnswersCorrectness[2]);
                    ChangeButtonColorAccordingToProbabilityOfVarian(buttonAnswerD, message.probabilityOfAnswersCorrectness[3]);
                }
            };
            if (InvokeRequired)
                Invoke(FormPromptResponse);
            else
                FormPromptResponse();
        }


        private void GiveAccessToPlayButtons(bool isEnabled)
        {
            buttonPlay.Enabled = isEnabled;
            buttonPlayWithRandomPlayer.Enabled = isEnabled;
        }

        private void ChangeButtonColorAccordingToProbabilityOfVarian(Button button, int probability)
        {
            if (probability < BoundaryProbabilityOfAnswerCorrectness[0])
            {
                button.BackColor = Color.LightCyan;
            }
            else if (probability < BoundaryProbabilityOfAnswerCorrectness[1])
            {
                button.BackColor = Color.PaleGreen;
            }
            else if (probability < BoundaryProbabilityOfAnswerCorrectness[2])
            {
                button.BackColor = Color.LightGreen;
            }
            else if (probability < BoundaryProbabilityOfAnswerCorrectness[3])
            {
                button.BackColor = Color.GreenYellow;
            }
            else if (probability < BoundaryProbabilityOfAnswerCorrectness[4])
            {
                button.BackColor = Color.LawnGreen;
            }
            else if (probability < BoundaryProbabilityOfAnswerCorrectness[5])
            {
                button.BackColor = Color.Lime;
            }
            else
            {
                button.BackColor = Color.LimeGreen;
            }
        }

        private void HideStatisticsTables()
        {
            richTextBoxStatistics.Visible = false;
            dataGridViewStatistics.Visible = false;
        }

        private void RemoveOneIncorrectAnswer(int incorrectAnswerNumber)
        {
            switch (incorrectAnswerNumber)
            {
                case StartNumber:
                    buttonAnswerA.Visible = false;
                    break;
                case buttonAnswerBClicked:
                    buttonAnswerB.Visible = false;
                    break;
                case buttonAnswerCClicked:
                    buttonAnswerC.Visible = false;
                    break;
                case buttonAnswerDClicked:
                    buttonAnswerD.Visible = false;
                    break;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.CloseAllThreads();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (currentGameTopic != InvalidValue)
            {
                labelTopicIsNotSelected.Visible = false;
                var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.StartGameRequest,
                messageReceiverID = currentDialog, gameTopic = currentGameTopic, isSelectedOpponentForGame = true };
                client.SendMessage(message);
            }
            else
            {
                labelTopicIsNotSelected.Visible = true;
            }     
        }

        private void FormAnswerTheQuestionMessage(int numberOfAnswer)
        {
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.PlayerAnswer,
            answeredQuestionNumber = currentQuestionNumber, answerNumber = numberOfAnswer };
            currentQuestionNumber++;
            ShowQuestion();
            client.SendMessage(message);
        }

        private void buttonAnswerA_Click(object sender, EventArgs e)
        {
            FormAnswerTheQuestionMessage(StartNumber);
        }

        private void buttonAnswerB_Click(object sender, EventArgs e)
        {
            FormAnswerTheQuestionMessage(buttonAnswerBClicked);
        }

        private void buttonAnswerC_Click(object sender, EventArgs e)
        {
            FormAnswerTheQuestionMessage(buttonAnswerCClicked);
        }

        private void buttonAnswerD_Click(object sender, EventArgs e)
        {
            FormAnswerTheQuestionMessage(buttonAnswerDClicked);
        }

        private void buttonLeftGame_Click(object sender, EventArgs e)
        {
            ClearGameField();
            panelGame.Visible = false;
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.LeftGame };
            client.SendMessage(message);
        }

        private void buttonFindServer_Click_1(object sender, EventArgs e)
        {
            client.SetClientSocketForUDPListening();
            buttonConnectToServer.Enabled = true;
            buttonFindServer.Enabled = false;
        }

        private void buttonConnectToServer_Click_1(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBoxName.Text))
            {
                IPEndPoint IPendPoint = new IPEndPoint(IPAddress.Parse(textBoxServerIPAddress.Text), int.Parse(textBoxServerPort.Text));
                if (client.ConnectToServer(IPendPoint, textBoxName.Text))
                {
                    labelDisplayConnection.Text = Connected;
                    buttonConnectToServer.Enabled = false;
                    buttonDisconnect.Enabled = true;
                    buttonSendMessage.Enabled = true;
                    buttonShowHistory.Enabled = true;
                }
                else
                {
                    labelDisplayConnection.Text = NotConnected;
                }   
                labelPutYourName.Visible = false;
            }
            else
            {
                labelPutYourName.Visible = true;
            }   
        }

        private void buttonDisconnect_Click_1(object sender, EventArgs e)
        {
            if (client.isClientConnected)
            {
                if (panelGame.Visible)
                {
                    ClearGameField();
                    panelGame.Visible = false;
                    var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.LeftGame };
                    client.SendMessage(message);
                }
                client.Disconnect();
                buttonFindServer.Enabled = true;
                buttonDisconnect.Enabled = false;
                buttonSendMessage.Enabled = false;
                buttonShowHistory.Enabled = false;
                labelDisplayConnection.Text = NotConnected;
                textBoxServerIPAddress.Enabled = true;
                textBoxServerIPAddress.Text = NullString;
                textBoxServerPort.Enabled = true;
                textBoxServerPort.Text = NullString;
                textBoxName.Enabled = true;
                textBoxName.Text = NullString;
                comboBoxParticipants.Items.Clear();
                richTextBoxChatContent.Clear();
                labelCurrentClientDialog.Text = Nothing;
                richTextBoxMessageContent.Clear();
                labelNewMessage.Text = Nothing;
            }
            else
            {
                labelDisplayConnection.Text = Nothing;
            }       
        }

        private void buttonShowHistory_Click_1(object sender, EventArgs e)
        {
            comboBoxParticipants.SelectedIndex = StartNumber;
            currentDialog = ChatDialog;
            selectedReceiverIndex = comboBoxParticipants.SelectedIndex;
            labelCurrentClientDialog.Text = chatDialogsInfo[ChatDialog].dialogName;
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.History };
            client.SendMessage(message);
            UpdateView();
        }

        private void buttonSendMessage_Click_1(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(richTextBoxMessageContent.Text))
            {
                if (client.isClientConnected)
                {
                    string messagecontent = richTextBoxMessageContent.Text;
                    CommonInformation.Message message;
                    if (currentDialog != ChatDialog)
                    {
                        message = new CommonInformation.Message(currentDialog, messagecontent);
                        chatDialogsInfo[message.messageReceiverID].messages.Add("Я : " + DateTime.Now.ToString()
                        + " - " + messagecontent);
                    }
                    else
                    {
                        message = new CommonInformation.Message()
                        { messageContent = messagecontent, messageType = CommonInformation.Message.MessageType.Common };
                    }
                    client.SendMessage(message);
                    richTextBoxMessageContent.Clear();
                    labelPutMessage.Visible = false;
                    UpdateView();
                }
            }
            else
            {
                labelPutMessage.Visible = true;
            }      
        }

        private void comboBoxParticipants_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBoxParticipants.SelectedIndex != InvalidValue)
            {
                selectedReceiverIndex = comboBoxParticipants.SelectedIndex;
                currentDialog = clientsInfo[selectedReceiverIndex].clientID;
                UpdateView();
            }
        }

        private void ProcessGameContinuationMessage(CommonInformation.Message message)
        {
            message.messageTime = DateTime.Now;
            message.messageType = CommonInformation.Message.MessageType.StartGameResponse;
            message.messageName = messageSenderName;
            message.messageSenderID = messageSenderID;
            message.messageReceiverID = messageReceiverID;
            message.gameTopic = gametopic;
            message.isSelectedOpponentForGame = true;
        }

        private void HideButtons()
        {
            buttonAcceptGame.Visible = false;
            buttonRejectGame.Visible = false;
        }

        private void FormStartGameResponseMesssage(bool isGameContinue)
        {
            var message = new CommonInformation.Message();
            ProcessGameContinuationMessage(message);
            message.mayStartGame = isGameContinue;
            HideButtons();
            client.SendMessage(message);
        }

        private void buttonAcceptGame_Click(object sender, EventArgs e)
        {
            FormStartGameResponseMesssage(true);
        }

        private void buttonRejectGame_Click(object sender, EventArgs e)
        {
            FormStartGameResponseMesssage(false);
        }

        private void buttonShowStatistics_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.GetStatistics,
            messageReceiverID = currentDialog};
            client.SendMessage(message);
        }

        private void comboBoxGameTopics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxGameTopics.SelectedIndex != InvalidValue)
            {
                currentGameTopic = comboBoxGameTopics.SelectedIndex;
            }             
        }

        private void SendPromptRequestMessage(Button buttonToDisable, Button buttonToHide, bool is5050prompt)
        {
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.PromptRequest,
            is5050Prompt = is5050prompt, answeredQuestionNumber = currentQuestionNumber };
            buttonToDisable.Enabled = false;
            buttonToHide.Visible = false;
            isPromptUsed = true;
            client.SendMessage(message);
        }

        private void button5050Prompt_Click(object sender, EventArgs e)
        {
            SendPromptRequestMessage(button5050Prompt, buttonHallPrompt, true);
        }

        private void buttonHallPrompt_Click(object sender, EventArgs e)
        {
            SendPromptRequestMessage(buttonHallPrompt, button5050Prompt, false);
        }

        private void ShowDetailsOfTheBeginningOfTheGameWithRandomPlayer(bool isPlayButtonClick)
        {
            buttonPlayWithRandomPlayer.Visible = !isPlayButtonClick;
            buttonInterruptWaitingForOpponent.Visible = isPlayButtonClick;
            labelWaitingForOpponent.Visible = isPlayButtonClick;
        }

        private void buttonPlayWithRandomPlayer_Click(object sender, EventArgs e)
        {
            ShowDetailsOfTheBeginningOfTheGameWithRandomPlayer(true);
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.StartGameRequest,
            isSelectedOpponentForGame = false};
            client.SendMessage(message);
        }

        private void buttonInterruptWaitingForOpponent_Click(object sender, EventArgs e)
        {
            ShowDetailsOfTheBeginningOfTheGameWithRandomPlayer(false);
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.InterruptSearchingForOpponent };
            client.SendMessage(message);
        }

        private void buttonHideStatistics_Click(object sender, EventArgs e)
        {
            HideStatisticsTables();
        }
    }
}
