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
        const int CHATDIALOG = 0;
        const int NumberOfQuestions = 10;
        const int NumberOfAnswers = 4;

        Client client;

        delegate void ProcessFormFilling();
        Dictionary<int, AllDialogsMessages> chatDialogsInfo;
        List<ClientsInfo> clientsInfo;
        int CurrentDialog = CHATDIALOG;
        int selectedReceiverIndex = 0;
        int currentGameTopic = -1;

        string[] questions = new string[NumberOfQuestions];
        string[] answers = new string[NumberOfQuestions*NumberOfAnswers];
        int currentQuestionNumber;
        int currentAnswerNumber;
        int messageSenderID;
        int messageReceiverID;
        int gametopic;
        string messageSenderName;

        public MainForm()
        {
            InitializeComponent();
            clientsInfo = new List<ClientsInfo>();
            clientsInfo.Add(new ClientsInfo() { clientID = CHATDIALOG, clientName = "Чат" });
            chatDialogsInfo = new Dictionary<int, AllDialogsMessages>();
            chatDialogsInfo.Add(CHATDIALOG, new AllDialogsMessages("Чат"));

            client = new Client();
            client.ProcessReceivedMessagesEvent += ProcessReceivedMessages;
        }

        public void ShowQuestion()
        {
            if (currentQuestionNumber != NumberOfQuestions)
            {
                labelQuestion.Text = questions[currentQuestionNumber];
                labelAnswerA.Text = "A:  " + answers[currentAnswerNumber];
                labelAnswerB.Text = "B:  " + answers[currentAnswerNumber + 1];
                labelAnswerC.Text = "C:  " + answers[currentAnswerNumber + 2];
                labelAnswerD.Text = "D:  " + answers[currentAnswerNumber + 3];
                currentAnswerNumber += 4;

                buttonAnswerA.Visible = true;
                buttonAnswerB.Visible = true;
                buttonAnswerC.Visible = true;
                buttonAnswerD.Visible = true;
                panelHallPrompt.Visible = false;
            }
        }

        void ClearGameField()
        {
            labelQuestion.Text = "---";
            labelAnswerA.Text = "---";
            labelAnswerB.Text = "---";
            labelAnswerC.Text = "---";
            labelAnswerD.Text = "---";
            labelAnswerStatus.Text = "---";

            buttonAnswerA.Enabled = false;
            buttonAnswerB.Enabled = false;
            buttonAnswerC.Enabled = false;
            buttonAnswerD.Enabled = false;

            panelHallPrompt.Visible = false;
        }

        public void ProcessReceivedMessages(CommonInformation.Message message)
        {
            switch (message.messageType)
            {
                case CommonInformation.Message.MessageType.Common:
                    if (message.IPAdress == "")
                        chatDialogsInfo[CHATDIALOG].AddMessage(DateTime.Now.ToShortTimeString()
                        + " - " + message.messageName + " : " + message.messageContent);
                    else
                        chatDialogsInfo[CHATDIALOG].AddMessage(message.messageTime.ToString() + " - " + message.IPAdress
                            + " - " + message.messageName + " : " + message.messageContent);
                    break;
                case CommonInformation.Message.MessageType.Private:
                    chatDialogsInfo[message.messageSenderID].AddMessage(message.messageTime.ToString() + " : " + message.messageContent);
                    labelNewMessage.Text = "Новое сообщение от " + message.messageName;
                    break;
                case CommonInformation.Message.MessageType.History:
                    chatDialogsInfo[CurrentDialog].Messages = message.messageHistory;
                    break;
                case CommonInformation.Message.MessageType.SendGameTopics:
                    {
                        ProcessFormFilling FormFillingGameTopics = delegate
                        {
                            foreach (string topic in message.GameTopics)
                            {
                                comboBoxGameTopics.Items.Add(topic);
                            }
                        };
                        if (InvokeRequired)
                            Invoke(FormFillingGameTopics);
                        else
                            FormFillingGameTopics();
                    }
                    break;
                case CommonInformation.Message.MessageType.ClientsList:
                    {
                        ProcessFormFilling FormFillingNewClient = delegate
                        {
                            clientsInfo.Clear();
                            clientsInfo.Add(new ClientsInfo() { clientID = CHATDIALOG, clientName = "Чат" });
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
                    break;
                case CommonInformation.Message.MessageType.SearchResponce:
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
                    break;
                case CommonInformation.Message.MessageType.StartGameRequest:
                    {
                        ProcessFormFilling FormAddGameRequest = delegate
                        {
                            labelGameStatus.Text = message.gameStartDetails;
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
                    break;
                case CommonInformation.Message.MessageType.StartGameResponse:
                    {
                        ProcessFormFilling FormAddGameResponse = delegate
                        {
                            if (message.mayStartGame == false)
                            {
                                labelGameStatus.Text = message.gameStartDetails;
                            }
                            else
                            {
                                questions = message.questionsToSend;
                                answers = message.answersToSend;
                                currentQuestionNumber = 0;
                                currentAnswerNumber = 0;
                                panelGame.Visible = true;
                                labelAnswerStatus.Text = "---";
                                buttonAnswerA.Enabled = true;
                                buttonAnswerB.Enabled = true;
                                buttonAnswerC.Enabled = true;
                                buttonAnswerD.Enabled = true;
                                button5050Prompt.Enabled = true;
                                buttonHallPrompt.Enabled = true;
                                ShowQuestion();
                                labelYourPointsNumber.Text = "0";
                                labelOpponentPointsNumber.Text = "0";
                            }

                        };
                        if (InvokeRequired)
                            Invoke(FormAddGameResponse);
                        else
                            FormAddGameResponse();
                    }
                    break;
                case CommonInformation.Message.MessageType.YourAnswerStatus:
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
                    break;
                case CommonInformation.Message.MessageType.OpponentRightAnswer:
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
                    break;
                case CommonInformation.Message.MessageType.GameStatus:
                    {
                        ProcessFormFilling FormGameStatus = delegate
                        {
                            ClearGameField();
                            labelGameStatus.Text = message.gameStatus;
                        };
                        if (InvokeRequired)
                            Invoke(FormGameStatus);
                        else
                            FormGameStatus();
                    }
                    break;
                case CommonInformation.Message.MessageType.GameResults:
                    {
                        ProcessFormFilling FormGameResult = delegate
                        {
                            ClearGameField();
                            panelGame.Visible = false;
                            labelGameStatus.Text = message.gameStatus;
                        };
                        if (InvokeRequired)
                            Invoke(FormGameResult);
                        else
                            FormGameResult();
                    }
                    break;
                case CommonInformation.Message.MessageType.GetStatistics:
                    {
                        ProcessFormFilling FormGetStatisticsResult = delegate
                        {
                            if (message.messageReceiverID != 0)
                            {
                                richTextBoxStatistics.Visible = true;
                                richTextBoxStatistics.Clear();
                                dataGridViewStatistics.Visible = false;
                                if (message.isfailedToGetStatistics)
                                    richTextBoxStatistics.Text = "Данный участник чата ещё ни разу не играл!";
                                else
                                {
                                    richTextBoxStatistics.Text = message.playerInfo.playerName + "(" + message.playerInfo.playerStatus
                                    + ")" + "\r\n";
                                    richTextBoxStatistics.Text += "Игры                     " + message.playerInfo.numberOfPlayedGames + "\r\n";
                                    richTextBoxStatistics.Text += "Победы/ничьи/поражения   " + message.playerInfo.winsNumber
                                    + "/" + message.playerInfo.drawsNumber + "/" + message.playerInfo.losesNumber +"\r\n";
                                    richTextBoxStatistics.Text += "Верные/неверные ответы   " + message.playerInfo.rightAnswersNumber
                                    + "/" + message.playerInfo.wrongAnswersNumber + "\r\n";
                                    richTextBoxStatistics.Text += "Количество очков         " + message.playerInfo.pointsNumber + "\r\n";
                                }
                            }
                            else
                            {
                                dataGridViewStatistics.Rows.Clear();
                                dataGridViewStatistics.Visible = true;
                                richTextBoxStatistics.Visible = false;
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
                    break;
                case CommonInformation.Message.MessageType.PromptResponse:
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
                                panelHallPrompt.Visible = true;
                                labelProbabilityAAnswer.Text = message.probabilityOfAnswersCorrectness[0].ToString();
                                labelProbabilityBAnswer.Text = message.probabilityOfAnswersCorrectness[1].ToString();
                                labelProbabilityCAnswer.Text = message.probabilityOfAnswersCorrectness[2].ToString();
                                labelProbabilityDAnswer.Text = message.probabilityOfAnswersCorrectness[3].ToString();
                            }
                        };
                        if (InvokeRequired)
                            Invoke(FormPromptResponse);
                        else
                            FormPromptResponse();
                    }
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
                    string[] messages = new string[chatDialogsInfo[CurrentDialog].Messages.Count];
                    chatDialogsInfo[CurrentDialog].Messages.CopyTo(messages);
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

                labelCurrentClientDialog.Text = chatDialogsInfo[CurrentDialog].Name;

                if (CurrentDialog != CHATDIALOG)
                {
                    buttonPlay.Visible = true;
                    labelSelectTopic.Visible = true;
                    comboBoxGameTopics.Visible = true;
                }
                    
            };
            if (InvokeRequired)
                Invoke(FormUpdate);
            else
                FormUpdate();
        }

        void RemoveOneIncorrectAnswer(int incorrectAnswerNumber)
        {
            switch (incorrectAnswerNumber)
            {
                case 0:
                    buttonAnswerA.Visible = false;
                    break;
                case 1:
                    buttonAnswerB.Visible = false;
                    break;
                case 2:
                    buttonAnswerC.Visible = false;
                    break;
                case 3:
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
            if (currentGameTopic != -1)
            {
                labelTopicIsNotSelected.Visible = false;
                var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.StartGameRequest,
                messageReceiverID = CurrentDialog, gameTopic = currentGameTopic, isSelectedOpponentForGame = true };
                client.SendMessage(message);
            }
            else
                labelTopicIsNotSelected.Visible = true;
        }

        private void buttonAnswerA_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.PlayerAnswer,
            answeredQuestionNumber = currentQuestionNumber, answerNumber = 0 };
            currentQuestionNumber++;
            ShowQuestion();
            client.SendMessage(message);
        }

        private void buttonAnswerB_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.PlayerAnswer,
            answeredQuestionNumber = currentQuestionNumber, answerNumber = 1 };
            currentQuestionNumber++;
            ShowQuestion();
            client.SendMessage(message);
        }

        private void buttonAnswerC_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.PlayerAnswer,
            answeredQuestionNumber = currentQuestionNumber, answerNumber = 2 };
            currentQuestionNumber++;
            ShowQuestion();
            client.SendMessage(message);
        }

        private void buttonAnswerD_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.PlayerAnswer,
            answeredQuestionNumber = currentQuestionNumber, answerNumber = 3 };
            currentQuestionNumber++;
            ShowQuestion();
            client.SendMessage(message);
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
            if (textBoxName.Text != "")
            {
                IPEndPoint IPendPoint = new IPEndPoint(IPAddress.Parse(textBoxServerIPAddress.Text), int.Parse(textBoxServerPort.Text));
                if (client.ConnectToServer(IPendPoint, textBoxName.Text))
                {
                    labelDisplayConnection.Text = "ПОДКЛЮЧЕНО";
                    buttonConnectToServer.Enabled = false;
                    buttonDisconnect.Enabled = true;
                    buttonSendMessage.Enabled = true;
                    buttonShowHistory.Enabled = true;
                }
                else
                    labelDisplayConnection.Text = "Нет соединения...";
            }
            else
                MessageBox.Show("Введите свой ник!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                labelDisplayConnection.Text = "Нет соединения...";
                textBoxServerIPAddress.Enabled = true;
                textBoxServerIPAddress.Text = "";
                textBoxServerPort.Enabled = true;
                textBoxServerPort.Text = "";
                textBoxName.Enabled = true;
                textBoxName.Text = "";
                comboBoxParticipants.Items.Clear();
                richTextBoxChatContent.Clear();
                labelCurrentClientDialog.Text = "-";
                richTextBoxMessageContent.Clear();
                labelNewMessage.Text = "-";
            }
            else
                labelDisplayConnection.Text = "...";
        }

        private void buttonShowHistory_Click_1(object sender, EventArgs e)
        {
            comboBoxParticipants.SelectedIndex = 0;
            CurrentDialog = CHATDIALOG;
            selectedReceiverIndex = comboBoxParticipants.SelectedIndex;
            labelCurrentClientDialog.Text = chatDialogsInfo[CHATDIALOG].Name;
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.History };
            client.SendMessage(message);
            UpdateView();
        }

        private void buttonSendMessage_Click_1(object sender, EventArgs e)
        {
            if (richTextBoxMessageContent.Text != "")
            {
                if (client.isClientConnected)
                {
                    string messagecontent = richTextBoxMessageContent.Text;
                    CommonInformation.Message message;
                    if (CurrentDialog != CHATDIALOG)
                    {
                        message = new CommonInformation.Message(CurrentDialog, messagecontent);
                        chatDialogsInfo[message.messageReceiverID].Messages.Add("Я : " + messagecontent);
                    }
                    else
                    {
                        message = new CommonInformation.Message()
                        { messageContent = messagecontent, messageType = CommonInformation.Message.MessageType.Common };
                    }
                    client.SendMessage(message);
                    richTextBoxMessageContent.Clear();

                    UpdateView();
                }
            }
            else
                MessageBox.Show("Введите текст сообщения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void comboBoxParticipants_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBoxParticipants.SelectedIndex != -1)
            {
                selectedReceiverIndex = comboBoxParticipants.SelectedIndex;
                CurrentDialog = clientsInfo[selectedReceiverIndex].clientID;
                UpdateView();
            }
        }

        void ProcessGameContinuationMessage(CommonInformation.Message message)
        {
            message.messageTime = DateTime.Now;
            message.messageType = CommonInformation.Message.MessageType.StartGameResponse;
            message.messageName = messageSenderName;
            message.messageSenderID = messageSenderID;
            message.messageReceiverID = messageReceiverID;
            message.gameTopic = gametopic;
        }

        void HideButtons()
        {
            buttonAcceptGame.Visible = false;
            buttonRejectGame.Visible = false;
        }

        private void buttonAcceptGame_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message();
            ProcessGameContinuationMessage(message);
            message.mayStartGame = true;
            HideButtons();
            client.SendMessage(message);
        }

        private void buttonRejectGame_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message();
            ProcessGameContinuationMessage(message);
            message.mayStartGame = false;
            HideButtons();
            client.SendMessage(message);
        }

        private void buttonShowStatistics_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.GetStatistics,
            messageReceiverID = CurrentDialog};
            client.SendMessage(message);
        }

        private void comboBoxGameTopics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxGameTopics.SelectedIndex != -1)
                currentGameTopic = comboBoxGameTopics.SelectedIndex;
        }

        private void button5050Prompt_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message() { is5050Prompt = true, answeredQuestionNumber = currentQuestionNumber};
            button5050Prompt.Enabled = false;
            client.SendMessage(message);
        }

        private void buttonHallPrompt_Click(object sender, EventArgs e)
        {
            var message = new CommonInformation.Message() { is5050Prompt = false, answeredQuestionNumber = currentQuestionNumber };
            buttonHallPrompt.Enabled = false;
            client.SendMessage(message);
        }

        private void buttonPlayWithRandomPlayer_Click(object sender, EventArgs e)
        {
            buttonPlayWithRandomPlayer.Visible = false;
            buttonInterruptWaitingForOpponent.Visible = true;
            labelWaitingForOpponent.Visible = true;
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.StartGameRequest,
            isSelectedOpponentForGame = false};
            client.SendMessage(message);
        }

        private void buttonInterruptWaitingForOpponent_Click(object sender, EventArgs e)
        {
            buttonPlayWithRandomPlayer.Visible = true;
            buttonInterruptWaitingForOpponent.Visible = false;
            labelWaitingForOpponent.Visible = false;
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType.InterruptSearchingForOpponent };
            client.SendMessage(message);
        }
    }
}
