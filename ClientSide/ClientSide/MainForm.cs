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
        Client client;

        delegate void ProcessFormFilling();
        Dictionary<int, AllDialogsMessages> chatDialogsInfo;
        List<ClientsInfo> clientsInfo;
        int CurrentDialog = CHATDIALOG;
        int selectedReceiverIndex = 0;

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

        public void ProcessReceivedMessages(CommonInformation.Message message)
        {
            int i = CommonInfo.RetrieveMessageType(message.messageType);
            switch (i)
            {
                case 1:
                    if (message.IPAdress == "")
                        chatDialogsInfo[CHATDIALOG].AddMessage(DateTime.Now.ToShortTimeString()
                        + " - " + message.messageName + " : " + message.messageContent);
                    else
                        chatDialogsInfo[CHATDIALOG].AddMessage(message.messageTime.ToString() + " - " + message.IPAdress
                            + " - " + message.messageName + " : " + message.messageContent);
                    break;
                case 2:
                    chatDialogsInfo[message.messageSenderID].AddMessage(message.messageTime.ToString() + " : " + message.messageContent);
                    labelNewMessage.Text = "Новое сообщение от " + message.messageName;
                    break;
                case 3:
                    chatDialogsInfo[CurrentDialog].Messages = message.messageHistory;
                    break;
                case 5:
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
                case 7:
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
                default:
                    return;
            }
            if (i != 7)
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
            };
            if (InvokeRequired)
                Invoke(FormUpdate);
            else
                FormUpdate();
        }

        private void buttonFindServer_Click(object sender, EventArgs e)
        {
            client.SetClientSocketForUDPListening();
            buttonConnectToServer.Enabled = true;
            buttonFindServer.Enabled = false;
        }

        private void buttonConnectToServer_Click(object sender, EventArgs e)
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

        private void buttonDisconnect_Click(object sender, EventArgs e)
        {
            if (client.isClientConnected)
            {
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

        private void buttonSendMessage_Click(object sender, EventArgs e)
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
                        { messageContent = messagecontent, messageType = CommonInformation.Message.MessageType[1] };
                    }
                    client.SendMessage(message);
                    richTextBoxMessageContent.Clear();

                    UpdateView();
                }
            }
            else
                MessageBox.Show("Введите текст сообщения!", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void buttonShowHistory_Click(object sender, EventArgs e)
        {
            comboBoxParticipants.SelectedIndex = 0;
            CurrentDialog = CHATDIALOG;
            selectedReceiverIndex = comboBoxParticipants.SelectedIndex;
            labelCurrentClientDialog.Text = chatDialogsInfo[CHATDIALOG].Name;
            var message = new CommonInformation.Message() { messageType = CommonInformation.Message.MessageType[3] };
            client.SendMessage(message);
            UpdateView();
        }

        private void comboBoxParticipants_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxParticipants.SelectedIndex != -1)
            {
                selectedReceiverIndex = comboBoxParticipants.SelectedIndex;
                CurrentDialog = clientsInfo[selectedReceiverIndex].clientID;
                UpdateView();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.CloseAllThreads();
        }
    }
}
