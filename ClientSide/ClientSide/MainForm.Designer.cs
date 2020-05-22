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
            this.labelQuestion = new System.Windows.Forms.Label();
            this.labelOpponentPointsNumber = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelYourPointsNumber = new System.Windows.Forms.Label();
            this.labelOpponent = new System.Windows.Forms.Label();
            this.labelYou = new System.Windows.Forms.Label();
            this.labelGameStatus = new System.Windows.Forms.Label();
            this.buttonAcceptGame = new System.Windows.Forms.Button();
            this.buttonRejectGame = new System.Windows.Forms.Button();
            this.panelGame.SuspendLayout();
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
            this.buttonConnectToServer.Location = new System.Drawing.Point(12, 145);
            this.buttonConnectToServer.Name = "buttonConnectToServer";
            this.buttonConnectToServer.Size = new System.Drawing.Size(118, 25);
            this.buttonConnectToServer.TabIndex = 7;
            this.buttonConnectToServer.Text = "Подключиться";
            this.buttonConnectToServer.UseVisualStyleBackColor = true;
            this.buttonConnectToServer.Click += new System.EventHandler(this.buttonConnectToServer_Click_1);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(140, 145);
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
            this.labelDisplayConnection.Location = new System.Drawing.Point(88, 185);
            this.labelDisplayConnection.Name = "labelDisplayConnection";
            this.labelDisplayConnection.Size = new System.Drawing.Size(23, 17);
            this.labelDisplayConnection.TabIndex = 9;
            this.labelDisplayConnection.Text = "---";
            // 
            // buttonShowHistory
            // 
            this.buttonShowHistory.Location = new System.Drawing.Point(15, 219);
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
            this.label4.Location = new System.Drawing.Point(12, 260);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Участники";
            // 
            // comboBoxParticipants
            // 
            this.comboBoxParticipants.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxParticipants.FormattingEnabled = true;
            this.comboBoxParticipants.Location = new System.Drawing.Point(96, 260);
            this.comboBoxParticipants.Name = "comboBoxParticipants";
            this.comboBoxParticipants.Size = new System.Drawing.Size(159, 24);
            this.comboBoxParticipants.TabIndex = 12;
            this.comboBoxParticipants.SelectedIndexChanged += new System.EventHandler(this.comboBoxParticipants_SelectedIndexChanged_1);
            // 
            // labelNewMessage
            // 
            this.labelNewMessage.AutoSize = true;
            this.labelNewMessage.Location = new System.Drawing.Point(12, 297);
            this.labelNewMessage.Name = "labelNewMessage";
            this.labelNewMessage.Size = new System.Drawing.Size(23, 17);
            this.labelNewMessage.TabIndex = 13;
            this.labelNewMessage.Text = "---";
            // 
            // richTextBoxChatContent
            // 
            this.richTextBoxChatContent.Location = new System.Drawing.Point(291, 60);
            this.richTextBoxChatContent.Name = "richTextBoxChatContent";
            this.richTextBoxChatContent.Size = new System.Drawing.Size(385, 234);
            this.richTextBoxChatContent.TabIndex = 14;
            this.richTextBoxChatContent.Text = "";
            // 
            // labelCurrentClientDialog
            // 
            this.labelCurrentClientDialog.AutoSize = true;
            this.labelCurrentClientDialog.Location = new System.Drawing.Point(288, 23);
            this.labelCurrentClientDialog.Name = "labelCurrentClientDialog";
            this.labelCurrentClientDialog.Size = new System.Drawing.Size(23, 17);
            this.labelCurrentClientDialog.TabIndex = 15;
            this.labelCurrentClientDialog.Text = "---";
            // 
            // richTextBoxMessageContent
            // 
            this.richTextBoxMessageContent.Location = new System.Drawing.Point(291, 300);
            this.richTextBoxMessageContent.Name = "richTextBoxMessageContent";
            this.richTextBoxMessageContent.Size = new System.Drawing.Size(248, 26);
            this.richTextBoxMessageContent.TabIndex = 16;
            this.richTextBoxMessageContent.Text = "";
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(565, 23);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(111, 27);
            this.buttonPlay.TabIndex = 17;
            this.buttonPlay.Text = "Играть";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Visible = false;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonSendMessage
            // 
            this.buttonSendMessage.Location = new System.Drawing.Point(550, 300);
            this.buttonSendMessage.Name = "buttonSendMessage";
            this.buttonSendMessage.Size = new System.Drawing.Size(126, 26);
            this.buttonSendMessage.TabIndex = 18;
            this.buttonSendMessage.Text = "Отправить";
            this.buttonSendMessage.UseVisualStyleBackColor = true;
            this.buttonSendMessage.Click += new System.EventHandler(this.buttonSendMessage_Click_1);
            // 
            // panelGame
            // 
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
            this.panelGame.Controls.Add(this.labelQuestion);
            this.panelGame.Controls.Add(this.labelOpponentPointsNumber);
            this.panelGame.Controls.Add(this.label8);
            this.panelGame.Controls.Add(this.labelYourPointsNumber);
            this.panelGame.Controls.Add(this.labelOpponent);
            this.panelGame.Controls.Add(this.labelYou);
            this.panelGame.Location = new System.Drawing.Point(698, 4);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(283, 322);
            this.panelGame.TabIndex = 19;
            this.panelGame.Visible = false;
            // 
            // labelAnswerStatus
            // 
            this.labelAnswerStatus.AutoSize = true;
            this.labelAnswerStatus.Location = new System.Drawing.Point(3, 299);
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
            // labelQuestion
            // 
            this.labelQuestion.AutoSize = true;
            this.labelQuestion.Location = new System.Drawing.Point(3, 57);
            this.labelQuestion.Name = "labelQuestion";
            this.labelQuestion.Size = new System.Drawing.Size(23, 17);
            this.labelQuestion.TabIndex = 5;
            this.labelQuestion.Text = "---";
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
            // labelGameStatus
            // 
            this.labelGameStatus.AutoSize = true;
            this.labelGameStatus.Location = new System.Drawing.Point(288, 343);
            this.labelGameStatus.Name = "labelGameStatus";
            this.labelGameStatus.Size = new System.Drawing.Size(23, 17);
            this.labelGameStatus.TabIndex = 20;
            this.labelGameStatus.Text = "---";
            // 
            // buttonAcceptGame
            // 
            this.buttonAcceptGame.Location = new System.Drawing.Point(291, 373);
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
            this.buttonRejectGame.Location = new System.Drawing.Point(427, 373);
            this.buttonRejectGame.Name = "buttonRejectGame";
            this.buttonRejectGame.Size = new System.Drawing.Size(97, 27);
            this.buttonRejectGame.TabIndex = 22;
            this.buttonRejectGame.Text = "Отклонить";
            this.buttonRejectGame.UseVisualStyleBackColor = true;
            this.buttonRejectGame.Visible = false;
            this.buttonRejectGame.Click += new System.EventHandler(this.buttonRejectGame_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 506);
            this.Controls.Add(this.buttonRejectGame);
            this.Controls.Add(this.buttonAcceptGame);
            this.Controls.Add(this.labelGameStatus);
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
        private System.Windows.Forms.Label labelQuestion;
        private System.Windows.Forms.Label labelAnswerStatus;
        private System.Windows.Forms.Label labelGameStatus;
        private System.Windows.Forms.Button buttonAcceptGame;
        private System.Windows.Forms.Button buttonRejectGame;
    }
}

