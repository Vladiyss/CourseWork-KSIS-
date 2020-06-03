namespace ClientSide
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonFindServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxServerIPAddress = new System.Windows.Forms.TextBox();
            this.textBoxServerPort = new System.Windows.Forms.TextBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.buttonConnectToServer = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.labelDisplayConnection = new System.Windows.Forms.Label();
            this.buttonShowHistory = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxParticipants = new System.Windows.Forms.ComboBox();
            this.labelNewMessage = new System.Windows.Forms.Label();
            this.richTextBoxChatContent = new System.Windows.Forms.RichTextBox();
            this.labelCurrentClientDialog = new System.Windows.Forms.Label();
            this.richTextBoxMessageContent = new System.Windows.Forms.RichTextBox();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonSendMessage = new System.Windows.Forms.Button();
            this.panelGame = new System.Windows.Forms.Panel();
            this.labelAllNumberOfQuestions = new System.Windows.Forms.Label();
            this.labelCurrentQuestionNumber = new System.Windows.Forms.Label();
            this.labelQues = new System.Windows.Forms.Label();
            this.richTextBoxQuestion = new System.Windows.Forms.RichTextBox();
            this.buttonHallPrompt = new System.Windows.Forms.Button();
            this.button5050Prompt = new System.Windows.Forms.Button();
            this.labelAnswerStatus = new System.Windows.Forms.Label();
            this.buttonLeftGame = new System.Windows.Forms.Button();
            this.buttonAnswerD = new System.Windows.Forms.Button();
            this.buttonAnswerC = new System.Windows.Forms.Button();
            this.buttonAnswerB = new System.Windows.Forms.Button();
            this.buttonAnswerA = new System.Windows.Forms.Button();
            this.labelAnswerD = new System.Windows.Forms.Label();
            this.labelAnswerC = new System.Windows.Forms.Label();
            this.labelAnswerB = new System.Windows.Forms.Label();
            this.labelAnswerA = new System.Windows.Forms.Label();
            this.labelOpponentPointsNumber = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelYourPointsNumber = new System.Windows.Forms.Label();
            this.labelOpponent = new System.Windows.Forms.Label();
            this.labelYou = new System.Windows.Forms.Label();
            this.buttonAcceptGame = new System.Windows.Forms.Button();
            this.buttonRejectGame = new System.Windows.Forms.Button();
            this.buttonShowStatistics = new System.Windows.Forms.Button();
            this.richTextBoxStatistics = new System.Windows.Forms.RichTextBox();
            this.dataGridViewStatistics = new System.Windows.Forms.DataGridView();
            this.playerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Games = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Wins = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Draws = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Loses = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RightAnswer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WrongAnswer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Points = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comboBoxGameTopics = new System.Windows.Forms.ComboBox();
            this.labelSelectTopic = new System.Windows.Forms.Label();
            this.labelTopicIsNotSelected = new System.Windows.Forms.Label();
            this.buttonPlayWithRandomPlayer = new System.Windows.Forms.Button();
            this.labelWaitingForOpponent = new System.Windows.Forms.Label();
            this.buttonInterruptWaitingForOpponent = new System.Windows.Forms.Button();
            this.buttonHideStatistics = new System.Windows.Forms.Button();
            this.richTextBoxGameStatus = new System.Windows.Forms.RichTextBox();
            this.labelPutYourName = new System.Windows.Forms.Label();
            this.labelPutMessage = new System.Windows.Forms.Label();
            this.panelGame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStatistics)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonFindServer
            // 
            this.buttonFindServer.Location = new System.Drawing.Point(12, 12);
            this.buttonFindServer.Name = "buttonFindServer";
            this.buttonFindServer.Size = new System.Drawing.Size(243, 38);
            this.buttonFindServer.TabIndex = 0;
            this.buttonFindServer.Text = "Найти сервер";
            this.buttonFindServer.UseVisualStyleBackColor = true;
            this.buttonFindServer.Click += new System.EventHandler(this.buttonFindServer_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP-адрес сервера";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Порт сервера";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Ваше имя";
            // 
            // textBoxServerIPAddress
            // 
            this.textBoxServerIPAddress.Location = new System.Drawing.Point(140, 63);
            this.textBoxServerIPAddress.Name = "textBoxServerIPAddress";
            this.textBoxServerIPAddress.Size = new System.Drawing.Size(115, 22);
            this.textBoxServerIPAddress.TabIndex = 4;
            // 
            // textBoxServerPort
            // 
            this.textBoxServerPort.Location = new System.Drawing.Point(140, 89);
            this.textBoxServerPort.Name = "textBoxServerPort";
            this.textBoxServerPort.Size = new System.Drawing.Size(115, 22);
            this.textBoxServerPort.TabIndex = 5;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(140, 116);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(115, 22);
            this.textBoxName.TabIndex = 6;
            // 
            // buttonConnectToServer
            // 
            this.buttonConnectToServer.Location = new System.Drawing.Point(12, 170);
            this.buttonConnectToServer.Name = "buttonConnectToServer";
            this.buttonConnectToServer.Size = new System.Drawing.Size(118, 25);
            this.buttonConnectToServer.TabIndex = 7;
            this.buttonConnectToServer.Text = "Подключиться";
            this.buttonConnectToServer.UseVisualStyleBackColor = true;
            this.buttonConnectToServer.Click += new System.EventHandler(this.buttonConnectToServer_Click_1);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(140, 170);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(115, 25);
            this.buttonDisconnect.TabIndex = 8;
            this.buttonDisconnect.Text = "Отключиться";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click_1);
            // 
            // labelDisplayConnection
            // 
            this.labelDisplayConnection.AutoSize = true;
            this.labelDisplayConnection.Location = new System.Drawing.Point(88, 200);
            this.labelDisplayConnection.Name = "labelDisplayConnection";
            this.labelDisplayConnection.Size = new System.Drawing.Size(23, 17);
            this.labelDisplayConnection.TabIndex = 9;
            this.labelDisplayConnection.Text = "---";
            // 
            // buttonShowHistory
            // 
            this.buttonShowHistory.Location = new System.Drawing.Point(15, 224);
            this.buttonShowHistory.Name = "buttonShowHistory";
            this.buttonShowHistory.Size = new System.Drawing.Size(240, 23);
            this.buttonShowHistory.TabIndex = 10;
            this.buttonShowHistory.Text = "Показать историю сообщений";
            this.buttonShowHistory.UseVisualStyleBackColor = true;
            this.buttonShowHistory.Click += new System.EventHandler(this.buttonShowHistory_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 277);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Участники";
            // 
            // comboBoxParticipants
            // 
            this.comboBoxParticipants.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParticipants.FormattingEnabled = true;
            this.comboBoxParticipants.Location = new System.Drawing.Point(96, 277);
            this.comboBoxParticipants.Name = "comboBoxParticipants";
            this.comboBoxParticipants.Size = new System.Drawing.Size(159, 24);
            this.comboBoxParticipants.TabIndex = 12;
            this.comboBoxParticipants.SelectedIndexChanged += new System.EventHandler(this.comboBoxParticipants_SelectedIndexChanged_1);
            // 
            // labelNewMessage
            // 
            this.labelNewMessage.AutoSize = true;
            this.labelNewMessage.Location = new System.Drawing.Point(15, 250);
            this.labelNewMessage.Name = "labelNewMessage";
            this.labelNewMessage.Size = new System.Drawing.Size(23, 17);
            this.labelNewMessage.TabIndex = 13;
            this.labelNewMessage.Text = "---";
            // 
            // richTextBoxChatContent
            // 
            this.richTextBoxChatContent.Location = new System.Drawing.Point(291, 149);
            this.richTextBoxChatContent.Name = "richTextBoxChatContent";
            this.richTextBoxChatContent.Size = new System.Drawing.Size(385, 194);
            this.richTextBoxChatContent.TabIndex = 14;
            this.richTextBoxChatContent.Text = "";
            // 
            // labelCurrentClientDialog
            // 
            this.labelCurrentClientDialog.AutoSize = true;
            this.labelCurrentClientDialog.Location = new System.Drawing.Point(288, 68);
            this.labelCurrentClientDialog.Name = "labelCurrentClientDialog";
            this.labelCurrentClientDialog.Size = new System.Drawing.Size(23, 17);
            this.labelCurrentClientDialog.TabIndex = 15;
            this.labelCurrentClientDialog.Text = "---";
            // 
            // richTextBoxMessageContent
            // 
            this.richTextBoxMessageContent.BackColor = System.Drawing.SystemColors.Window;
            this.richTextBoxMessageContent.Location = new System.Drawing.Point(291, 354);
            this.richTextBoxMessageContent.Name = "richTextBoxMessageContent";
            this.richTextBoxMessageContent.Size = new System.Drawing.Size(248, 26);
            this.richTextBoxMessageContent.TabIndex = 16;
            this.richTextBoxMessageContent.Text = "";
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(586, 53);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(86, 27);
            this.buttonPlay.TabIndex = 17;
            this.buttonPlay.Text = "Играть";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Visible = false;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(550, 352);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(126, 26);
            this.buttonSendMessage.TabIndex = 18;
            this.buttonSendMessage.Text = "Отправить";
            this.buttonSendMessage.UseVisualStyleBackColor = true;
            this.buttonSendMessage.Click += new System.EventHandler(this.buttonSendMessage_Click_1);
            // 
            // panelGame
            // 
            this.panelGame.Controls.Add(this.labelAllNumberOfQuestions);
            this.panelGame.Controls.Add(this.labelCurrentQuestionNumber);
            this.panelGame.Controls.Add(this.labelQues);
            this.panelGame.Controls.Add(this.richTextBoxQuestion);
            this.panelGame.Controls.Add(this.buttonHallPrompt);
            this.panelGame.Controls.Add(this.button5050Prompt);
            this.panelGame.Controls.Add(this.labelAnswerStatus);
            this.panelGame.Controls.Add(this.buttonLeftGame);
            this.panelGame.Controls.Add(this.buttonAnswerD);
            this.panelGame.Controls.Add(this.buttonAnswerC);
            this.panelGame.Controls.Add(this.buttonAnswerB);
            this.panelGame.Controls.Add(this.buttonAnswerA);
            this.panelGame.Controls.Add(this.labelAnswerD);
            this.panelGame.Controls.Add(this.labelAnswerC);
            this.panelGame.Controls.Add(this.labelAnswerB);
            this.panelGame.Controls.Add(this.labelAnswerA);
            this.panelGame.Controls.Add(this.labelOpponentPointsNumber);
            this.panelGame.Controls.Add(this.label8);
            this.panelGame.Controls.Add(this.labelYourPointsNumber);
            this.panelGame.Controls.Add(this.labelOpponent);
            this.panelGame.Controls.Add(this.labelYou);
            this.panelGame.Location = new System.Drawing.Point(698, 4);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(353, 351);
            this.panelGame.TabIndex = 19;
            this.panelGame.Visible = false;
            // 
            // labelAllNumberOfQuestions
            // 
            this.labelAllNumberOfQuestions.AutoSize = true;
            this.labelAllNumberOfQuestions.Location = new System.Drawing.Point(95, 323);
            this.labelAllNumberOfQuestions.Name = "labelAllNumberOfQuestions";
            this.labelAllNumberOfQuestions.Size = new System.Drawing.Size(28, 17);
            this.labelAllNumberOfQuestions.TabIndex = 22;
            this.labelAllNumberOfQuestions.Text = "/10";
            // 
            // labelCurrentQuestionNumber
            // 
            this.labelCurrentQuestionNumber.AutoSize = true;
            this.labelCurrentQuestionNumber.Location = new System.Drawing.Point(74, 323);
            this.labelCurrentQuestionNumber.Name = "labelCurrentQuestionNumber";
            this.labelCurrentQuestionNumber.Size = new System.Drawing.Size(23, 17);
            this.labelCurrentQuestionNumber.TabIndex = 21;
            this.labelCurrentQuestionNumber.Text = "---";
            // 
            // labelQues
            // 
            this.labelQues.AutoSize = true;
            this.labelQues.Location = new System.Drawing.Point(5, 323);
            this.labelQues.Name = "labelQues";
            this.labelQues.Size = new System.Drawing.Size(66, 17);
            this.labelQues.TabIndex = 20;
            this.labelQues.Text = "Вопросы";
            // 
            // richTextBoxQuestion
            // 
            this.richTextBoxQuestion.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxQuestion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxQuestion.Location = new System.Drawing.Point(3, 49);
            this.richTextBoxQuestion.Name = "richTextBoxQuestion";
            this.richTextBoxQuestion.Size = new System.Drawing.Size(332, 35);
            this.richTextBoxQuestion.TabIndex = 19;
            this.richTextBoxQuestion.Text = "";
            // 
            // buttonHallPrompt
            // 
            this.buttonHallPrompt.Location = new System.Drawing.Point(274, 225);
            this.buttonHallPrompt.Name = "buttonHallPrompt";
            this.buttonHallPrompt.Size = new System.Drawing.Size(61, 30);
            this.buttonHallPrompt.TabIndex = 17;
            this.buttonHallPrompt.Text = "Зал";
            this.buttonHallPrompt.UseVisualStyleBackColor = true;
            this.buttonHallPrompt.Click += new System.EventHandler(this.buttonHallPrompt_Click);
            // 
            // button5050Prompt
            // 
            this.button5050Prompt.Location = new System.Drawing.Point(274, 189);
            this.button5050Prompt.Name = "button5050Prompt";
            this.button5050Prompt.Size = new System.Drawing.Size(61, 30);
            this.button5050Prompt.TabIndex = 16;
            this.button5050Prompt.Text = "50/50";
            this.button5050Prompt.UseVisualStyleBackColor = true;
            this.button5050Prompt.Click += new System.EventHandler(this.button5050Prompt_Click);
            // 
            // labelAnswerStatus
            // 
            this.labelAnswerStatus.AutoSize = true;
            this.labelAnswerStatus.Location = new System.Drawing.Point(3, 300);
            this.labelAnswerStatus.Name = "labelAnswerStatus";
            this.labelAnswerStatus.Size = new System.Drawing.Size(23, 17);
            this.labelAnswerStatus.TabIndex = 15;
            this.labelAnswerStatus.Text = "---";
            // 
            // buttonLeftGame
            // 
            this.buttonLeftGame.Location = new System.Drawing.Point(6, 260);
            this.buttonLeftGame.Name = "buttonLeftGame";
            this.buttonLeftGame.Size = new System.Drawing.Size(252, 31);
            this.buttonLeftGame.TabIndex = 14;
            this.buttonLeftGame.Text = "Покинуть игру";
            this.buttonLeftGame.UseVisualStyleBackColor = true;
            this.buttonLeftGame.Click += new System.EventHandler(this.buttonLeftGame_Click);
            // 
            // buttonAnswerD
            // 
            this.buttonAnswerD.Location = new System.Drawing.Point(145, 224);
            this.buttonAnswerD.Name = "buttonAnswerD";
            this.buttonAnswerD.Size = new System.Drawing.Size(113, 30);
            this.buttonAnswerD.TabIndex = 13;
            this.buttonAnswerD.Text = "D";
            this.buttonAnswerD.UseVisualStyleBackColor = true;
            this.buttonAnswerD.Click += new System.EventHandler(this.buttonAnswerD_Click);
            // 
            // buttonAnswerC
            // 
            this.buttonAnswerC.Location = new System.Drawing.Point(6, 225);
            this.buttonAnswerC.Name = "buttonAnswerC";
            this.buttonAnswerC.Size = new System.Drawing.Size(120, 29);
            this.buttonAnswerC.TabIndex = 12;
            this.buttonAnswerC.Text = "C";
            this.buttonAnswerC.UseVisualStyleBackColor = true;
            this.buttonAnswerC.Click += new System.EventHandler(this.buttonAnswerC_Click);
            // 
            // buttonAnswerB
            // 
            this.buttonAnswerB.Location = new System.Drawing.Point(145, 189);
            this.buttonAnswerB.Name = "buttonAnswerB";
            this.buttonAnswerB.Size = new System.Drawing.Size(113, 30);
            this.buttonAnswerB.TabIndex = 11;
            this.buttonAnswerB.Text = "B";
            this.buttonAnswerB.UseVisualStyleBackColor = true;
            this.buttonAnswerB.Click += new System.EventHandler(this.buttonAnswerB_Click);
            // 
            // buttonAnswerA
            // 
            this.buttonAnswerA.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.buttonAnswerA.Location = new System.Drawing.Point(6, 189);
            this.buttonAnswerA.Name = "buttonAnswerA";
            this.buttonAnswerA.Size = new System.Drawing.Size(120, 30);
            this.buttonAnswerA.TabIndex = 10;
            this.buttonAnswerA.Text = "A";
            this.buttonAnswerA.UseVisualStyleBackColor = true;
            this.buttonAnswerA.Click += new System.EventHandler(this.buttonAnswerA_Click);
            // 
            // labelAnswerD
            // 
            this.labelAnswerD.AutoSize = true;
            this.labelAnswerD.Location = new System.Drawing.Point(3, 162);
            this.labelAnswerD.Name = "labelAnswerD";
            this.labelAnswerD.Size = new System.Drawing.Size(23, 17);
            this.labelAnswerD.TabIndex = 9;
            this.labelAnswerD.Text = "---";
            // 
            // labelAnswerC
            // 
            this.labelAnswerC.AutoSize = true;
            this.labelAnswerC.Location = new System.Drawing.Point(3, 140);
            this.labelAnswerC.Name = "labelAnswerC";
            this.labelAnswerC.Size = new System.Drawing.Size(23, 17);
            this.labelAnswerC.TabIndex = 8;
            this.labelAnswerC.Text = "---";
            // 
            // labelAnswerB
            // 
            this.labelAnswerB.AutoSize = true;
            this.labelAnswerB.Location = new System.Drawing.Point(3, 118);
            this.labelAnswerB.Name = "labelAnswerB";
            this.labelAnswerB.Size = new System.Drawing.Size(23, 17);
            this.labelAnswerB.TabIndex = 7;
            this.labelAnswerB.Text = "---";
            // 
            // labelAnswerA
            // 
            this.labelAnswerA.AutoSize = true;
            this.labelAnswerA.Location = new System.Drawing.Point(3, 96);
            this.labelAnswerA.Name = "labelAnswerA";
            this.labelAnswerA.Size = new System.Drawing.Size(23, 17);
            this.labelAnswerA.TabIndex = 6;
            this.labelAnswerA.Text = "---";
            // 
            // labelOpponentPointsNumber
            // 
            this.labelOpponentPointsNumber.AutoSize = true;
            this.labelOpponentPointsNumber.Location = new System.Drawing.Point(154, 21);
            this.labelOpponentPointsNumber.Name = "labelOpponentPointsNumber";
            this.labelOpponentPointsNumber.Size = new System.Drawing.Size(23, 17);
            this.labelOpponentPointsNumber.TabIndex = 4;
            this.labelOpponentPointsNumber.Text = "---";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(132, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 17);
            this.label8.TabIndex = 3;
            this.label8.Text = "::";
            // 
            // labelYourPointsNumber
            // 
            this.labelYourPointsNumber.AutoSize = true;
            this.labelYourPointsNumber.Location = new System.Drawing.Point(103, 21);
            this.labelYourPointsNumber.Name = "labelYourPointsNumber";
            this.labelYourPointsNumber.Size = new System.Drawing.Size(23, 17);
            this.labelYourPointsNumber.TabIndex = 2;
            this.labelYourPointsNumber.Text = "---";
            // 
            // labelOpponent
            // 
            this.labelOpponent.AutoSize = true;
            this.labelOpponent.Location = new System.Drawing.Point(197, 21);
            this.labelOpponent.Name = "labelOpponent";
            this.labelOpponent.Size = new System.Drawing.Size(74, 17);
            this.labelOpponent.TabIndex = 1;
            this.labelOpponent.Text = "Оппонент";
            // 
            // labelYou
            // 
            this.labelYou.AutoSize = true;
            this.labelYou.Location = new System.Drawing.Point(36, 21);
            this.labelYou.Name = "labelYou";
            this.labelYou.Size = new System.Drawing.Size(29, 17);
            this.labelYou.TabIndex = 0;
            this.labelYou.Text = "ВЫ";
            // 
            // buttonAcceptGame
            // 
            this.buttonAcceptGame.Location = new System.Drawing.Point(289, 432);
            this.buttonAcceptGame.Name = "buttonAcceptGame";
            this.buttonAcceptGame.Size = new System.Drawing.Size(104, 27);
            this.buttonAcceptGame.TabIndex = 21;
            this.buttonAcceptGame.Text = "Принять";
            this.buttonAcceptGame.UseVisualStyleBackColor = true;
            this.buttonAcceptGame.Visible = false;
            this.buttonAcceptGame.Click += new System.EventHandler(this.buttonAcceptGame_Click);
            // 
            // buttonRejectGame
            // 
            this.buttonRejectGame.Location = new System.Drawing.Point(405, 432);
            this.buttonRejectGame.Name = "buttonRejectGame";
            this.buttonRejectGame.Size = new System.Drawing.Size(97, 27);
            this.buttonRejectGame.TabIndex = 22;
            this.buttonRejectGame.Text = "Отклонить";
            this.buttonRejectGame.UseVisualStyleBackColor = true;
            this.buttonRejectGame.Visible = false;
            this.buttonRejectGame.Click += new System.EventHandler(this.buttonRejectGame_Click);
            // 
            // buttonShowStatistics
            // 
            this.buttonShowStatistics.Location = new System.Drawing.Point(15, 318);
            this.buttonShowStatistics.Name = "buttonShowStatistics";
            this.buttonShowStatistics.Size = new System.Drawing.Size(240, 25);
            this.buttonShowStatistics.TabIndex = 23;
            this.buttonShowStatistics.Text = "Показать статистику";
            this.buttonShowStatistics.UseVisualStyleBackColor = true;
            this.buttonShowStatistics.Click += new System.EventHandler(this.buttonShowStatistics_Click);
            // 
            // richTextBoxStatistics
            // 
            this.richTextBoxStatistics.Location = new System.Drawing.Point(10, 389);
            this.richTextBoxStatistics.Name = "richTextBoxStatistics";
            this.richTextBoxStatistics.Size = new System.Drawing.Size(260, 95);
            this.richTextBoxStatistics.TabIndex = 24;
            this.richTextBoxStatistics.Text = "";
            this.richTextBoxStatistics.Visible = false;
            // 
            // dataGridViewStatistics
            // 
            this.dataGridViewStatistics.AllowUserToAddRows = false;
            this.dataGridViewStatistics.AllowUserToDeleteRows = false;
            this.dataGridViewStatistics.AllowUserToResizeRows = false;
            this.dataGridViewStatistics.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewStatistics.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStatistics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.playerName,
            this.Games,
            this.Wins,
            this.Draws,
            this.Loses,
            this.RightAnswer,
            this.WrongAnswer,
            this.Points});
            this.dataGridViewStatistics.Location = new System.Drawing.Point(508, 432);
            this.dataGridViewStatistics.Name = "dataGridViewStatistics";
            this.dataGridViewStatistics.ReadOnly = true;
            this.dataGridViewStatistics.RowHeadersVisible = false;
            this.dataGridViewStatistics.RowTemplate.Height = 24;
            this.dataGridViewStatistics.Size = new System.Drawing.Size(653, 132);
            this.dataGridViewStatistics.TabIndex = 25;
            this.dataGridViewStatistics.Visible = false;
            // 
            // playerName
            // 
            this.playerName.HeaderText = "Имя игрока";
            this.playerName.Name = "playerName";
            this.playerName.ReadOnly = true;
            this.playerName.Width = 150;
            // 
            // Games
            // 
            this.Games.HeaderText = "Игры";
            this.Games.Name = "Games";
            this.Games.ReadOnly = true;
            this.Games.Width = 50;
            // 
            // Wins
            // 
            this.Wins.HeaderText = "П";
            this.Wins.Name = "Wins";
            this.Wins.ReadOnly = true;
            this.Wins.Width = 25;
            // 
            // Draws
            // 
            this.Draws.HeaderText = "Н";
            this.Draws.Name = "Draws";
            this.Draws.ReadOnly = true;
            this.Draws.Width = 25;
            // 
            // Loses
            // 
            this.Loses.HeaderText = "П";
            this.Loses.Name = "Loses";
            this.Loses.ReadOnly = true;
            this.Loses.Width = 25;
            // 
            // RightAnswer
            // 
            this.RightAnswer.HeaderText = "Верно";
            this.RightAnswer.Name = "RightAnswer";
            this.RightAnswer.ReadOnly = true;
            this.RightAnswer.Width = 75;
            // 
            // WrongAnswer
            // 
            this.WrongAnswer.HeaderText = "Неверно";
            this.WrongAnswer.Name = "WrongAnswer";
            this.WrongAnswer.ReadOnly = true;
            this.WrongAnswer.Width = 75;
            // 
            // Points
            // 
            this.Points.HeaderText = "Очки";
            this.Points.Name = "Points";
            this.Points.ReadOnly = true;
            this.Points.Width = 50;
            // 
            // comboBoxGameTopics
            // 
            this.comboBoxGameTopics.FormattingEnabled = true;
            this.comboBoxGameTopics.Location = new System.Drawing.Point(485, 26);
            this.comboBoxGameTopics.Name = "comboBoxGameTopics";
            this.comboBoxGameTopics.Size = new System.Drawing.Size(186, 24);
            this.comboBoxGameTopics.TabIndex = 26;
            this.comboBoxGameTopics.Visible = false;
            this.comboBoxGameTopics.SelectedIndexChanged += new System.EventHandler(this.comboBoxGameTopics_SelectedIndexChanged);
            // 
            // labelSelectTopic
            // 
            this.labelSelectTopic.AutoSize = true;
            this.labelSelectTopic.Location = new System.Drawing.Point(286, 29);
            this.labelSelectTopic.Name = "labelSelectTopic";
            this.labelSelectTopic.Size = new System.Drawing.Size(109, 17);
            this.labelSelectTopic.TabIndex = 27;
            this.labelSelectTopic.Text = "Выберите тему";
            this.labelSelectTopic.Visible = false;
            // 
            // labelTopicIsNotSelected
            // 
            this.labelTopicIsNotSelected.AutoSize = true;
            this.labelTopicIsNotSelected.Location = new System.Drawing.Point(550, 80);
            this.labelTopicIsNotSelected.Name = "labelTopicIsNotSelected";
            this.labelTopicIsNotSelected.Size = new System.Drawing.Size(126, 17);
            this.labelTopicIsNotSelected.TabIndex = 28;
            this.labelTopicIsNotSelected.Text = "Тема не выбрана!";
            this.labelTopicIsNotSelected.Visible = false;
            // 
            // buttonPlayWithRandomPlayer
            // 
            this.buttonPlayWithRandomPlayer.Location = new System.Drawing.Point(392, 113);
            this.buttonPlayWithRandomPlayer.Name = "buttonPlayWithRandomPlayer";
            this.buttonPlayWithRandomPlayer.Size = new System.Drawing.Size(162, 25);
            this.buttonPlayWithRandomPlayer.TabIndex = 29;
            this.buttonPlayWithRandomPlayer.Text = "Случайный игрок";
            this.buttonPlayWithRandomPlayer.UseVisualStyleBackColor = true;
            this.buttonPlayWithRandomPlayer.Click += new System.EventHandler(this.buttonPlayWithRandomPlayer_Click);
            // 
            // labelWaitingForOpponent
            // 
            this.labelWaitingForOpponent.AutoSize = true;
            this.labelWaitingForOpponent.Location = new System.Drawing.Point(288, 117);
            this.labelWaitingForOpponent.Name = "labelWaitingForOpponent";
            this.labelWaitingForOpponent.Size = new System.Drawing.Size(162, 17);
            this.labelWaitingForOpponent.TabIndex = 30;
            this.labelWaitingForOpponent.Text = "Ожидайте оппонента...";
            this.labelWaitingForOpponent.Visible = false;
            // 
            // buttonInterruptWaitingForOpponent
            // 
            this.buttonInterruptWaitingForOpponent.Location = new System.Drawing.Point(582, 113);
            this.buttonInterruptWaitingForOpponent.Name = "buttonInterruptWaitingForOpponent";
            this.buttonInterruptWaitingForOpponent.Size = new System.Drawing.Size(89, 25);
            this.buttonInterruptWaitingForOpponent.TabIndex = 31;
            this.buttonInterruptWaitingForOpponent.Text = "Прервать";
            this.buttonInterruptWaitingForOpponent.UseVisualStyleBackColor = true;
            this.buttonInterruptWaitingForOpponent.Visible = false;
            this.buttonInterruptWaitingForOpponent.Click += new System.EventHandler(this.buttonInterruptWaitingForOpponent_Click);
            // 
            // buttonHideStatistics
            // 
            this.buttonHideStatistics.Location = new System.Drawing.Point(15, 351);
            this.buttonHideStatistics.Name = "buttonHideStatistics";
            this.buttonHideStatistics.Size = new System.Drawing.Size(240, 25);
            this.buttonHideStatistics.TabIndex = 32;
            this.buttonHideStatistics.Text = "Скрыть статистику";
            this.buttonHideStatistics.UseVisualStyleBackColor = true;
            this.buttonHideStatistics.Click += new System.EventHandler(this.buttonHideStatistics_Click);
            // 
            // richTextBoxGameStatus
            // 
            this.richTextBoxGameStatus.BackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxGameStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxGameStatus.Location = new System.Drawing.Point(291, 386);
            this.richTextBoxGameStatus.Name = "richTextBoxGameStatus";
            this.richTextBoxGameStatus.Size = new System.Drawing.Size(344, 40);
            this.richTextBoxGameStatus.TabIndex = 33;
            this.richTextBoxGameStatus.Text = "";
            // 
            // labelPutYourName
            // 
            this.labelPutYourName.AutoSize = true;
            this.labelPutYourName.Location = new System.Drawing.Point(156, 144);
            this.labelPutYourName.Name = "labelPutYourName";
            this.labelPutYourName.Size = new System.Drawing.Size(95, 17);
            this.labelPutYourName.TabIndex = 34;
            this.labelPutYourName.Text = "Введите имя!";
            this.labelPutYourName.Visible = false;
            // 
            // labelPutMessage
            // 
            this.labelPutMessage.AutoSize = true;
            this.labelPutMessage.Location = new System.Drawing.Point(681, 358);
            this.labelPutMessage.Name = "labelPutMessage";
            this.labelPutMessage.Size = new System.Drawing.Size(144, 17);
            this.labelPutMessage.TabIndex = 35;
            this.labelPutMessage.Text = "Введите сообщение!";
            this.labelPutMessage.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 587);
            this.Controls.Add(this.labelPutMessage);
            this.Controls.Add(this.labelPutYourName);
            this.Controls.Add(this.richTextBoxGameStatus);
            this.Controls.Add(this.buttonHideStatistics);
            this.Controls.Add(this.buttonInterruptWaitingForOpponent);
            this.Controls.Add(this.labelWaitingForOpponent);
            this.Controls.Add(this.buttonPlayWithRandomPlayer);
            this.Controls.Add(this.labelTopicIsNotSelected);
            this.Controls.Add(this.labelSelectTopic);
            this.Controls.Add(this.comboBoxGameTopics);
            this.Controls.Add(this.dataGridViewStatistics);
            this.Controls.Add(this.richTextBoxStatistics);
            this.Controls.Add(this.buttonShowStatistics);
            this.Controls.Add(this.buttonRejectGame);
            this.Controls.Add(this.buttonAcceptGame);
            this.Controls.Add(this.panelGame);
            this.Controls.Add(this.buttonSendMessage);
            this.Controls.Add(this.buttonPlay);
            this.Controls.Add(this.richTextBoxMessageContent);
            this.Controls.Add(this.labelCurrentClientDialog);
            this.Controls.Add(this.richTextBoxChatContent);
            this.Controls.Add(this.labelNewMessage);
            this.Controls.Add(this.comboBoxParticipants);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonShowHistory);
            this.Controls.Add(this.labelDisplayConnection);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnectToServer);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.textBoxServerPort);
            this.Controls.Add(this.textBoxServerIPAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonFindServer);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.panelGame.ResumeLayout(false);
            this.panelGame.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStatistics)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonFindServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxServerIPAddress;
        private System.Windows.Forms.TextBox textBoxServerPort;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonConnectToServer;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Label labelDisplayConnection;
        private System.Windows.Forms.Button buttonShowHistory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxParticipants;
        private System.Windows.Forms.Label labelNewMessage;
        private System.Windows.Forms.RichTextBox richTextBoxChatContent;
        private System.Windows.Forms.Label labelCurrentClientDialog;
        private System.Windows.Forms.RichTextBox richTextBoxMessageContent;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonSendMessage;
        private System.Windows.Forms.Panel panelGame;
        private System.Windows.Forms.Label labelOpponentPointsNumber;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelYourPointsNumber;
        private System.Windows.Forms.Label labelOpponent;
        private System.Windows.Forms.Label labelYou;
        private System.Windows.Forms.Button buttonLeftGame;
        private System.Windows.Forms.Button buttonAnswerD;
        private System.Windows.Forms.Button buttonAnswerC;
        private System.Windows.Forms.Button buttonAnswerB;
        private System.Windows.Forms.Button buttonAnswerA;
        private System.Windows.Forms.Label labelAnswerD;
        private System.Windows.Forms.Label labelAnswerC;
        private System.Windows.Forms.Label labelAnswerB;
        private System.Windows.Forms.Label labelAnswerA;
        private System.Windows.Forms.Label labelAnswerStatus;
        private System.Windows.Forms.Button buttonAcceptGame;
        private System.Windows.Forms.Button buttonRejectGame;
        private System.Windows.Forms.Button buttonShowStatistics;
        private System.Windows.Forms.RichTextBox richTextBoxStatistics;
        private System.Windows.Forms.DataGridView dataGridViewStatistics;
        private System.Windows.Forms.DataGridViewTextBoxColumn playerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Games;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wins;
        private System.Windows.Forms.DataGridViewTextBoxColumn Draws;
        private System.Windows.Forms.DataGridViewTextBoxColumn Loses;
        private System.Windows.Forms.DataGridViewTextBoxColumn RightAnswer;
        private System.Windows.Forms.DataGridViewTextBoxColumn WrongAnswer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Points;
        private System.Windows.Forms.ComboBox comboBoxGameTopics;
        private System.Windows.Forms.Label labelSelectTopic;
        private System.Windows.Forms.Label labelTopicIsNotSelected;
        private System.Windows.Forms.Button buttonHallPrompt;
        private System.Windows.Forms.Button button5050Prompt;
        private System.Windows.Forms.Button buttonPlayWithRandomPlayer;
        private System.Windows.Forms.Label labelWaitingForOpponent;
        private System.Windows.Forms.Button buttonInterruptWaitingForOpponent;
        private System.Windows.Forms.RichTextBox richTextBoxQuestion;
        private System.Windows.Forms.Button buttonHideStatistics;
        private System.Windows.Forms.RichTextBox richTextBoxGameStatus;
        private System.Windows.Forms.Label labelPutYourName;
        private System.Windows.Forms.Label labelPutMessage;
        private System.Windows.Forms.Label labelAllNumberOfQuestions;
        private System.Windows.Forms.Label labelCurrentQuestionNumber;
        private System.Windows.Forms.Label labelQues;
    }
}

