namespace ClientSide
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                clientSocket?.Close();
            }
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSend = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.txbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbMsg = new System.Windows.Forms.TextBox();
            this.txbChat = new System.Windows.Forms.TextBox();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.txbDB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sendDB = new System.Windows.Forms.Button();
            this.txbShowAllApp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonSend
            // 
            this.buttonSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSend.Enabled = false;
            this.buttonSend.Location = new System.Drawing.Point(397, 443);
            this.buttonSend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(123, 60);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnect.Location = new System.Drawing.Point(408, 61);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(112, 35);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // txbName
            // 
            this.txbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbName.Location = new System.Drawing.Point(77, 14);
            this.txbName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(660, 26);
            this.txbName.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 19);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name";
            // 
            // txbMsg
            // 
            this.txbMsg.Location = new System.Drawing.Point(46, 440);
            this.txbMsg.Multiline = true;
            this.txbMsg.Name = "txbMsg";
            this.txbMsg.Size = new System.Drawing.Size(344, 60);
            this.txbMsg.TabIndex = 10;
            // 
            // txbChat
            // 
            this.txbChat.Location = new System.Drawing.Point(37, 115);
            this.txbChat.Multiline = true;
            this.txbChat.Name = "txbChat";
            this.txbChat.Size = new System.Drawing.Size(488, 305);
            this.txbChat.TabIndex = 11;
            // 
            // labelIP
            // 
            this.labelIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(20, 70);
            this.labelIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(24, 20);
            this.labelIP.TabIndex = 3;
            this.labelIP.Text = "IP";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxAddress.Location = new System.Drawing.Point(54, 65);
            this.textBoxAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(276, 26);
            this.textBoxAddress.TabIndex = 4;
            this.textBoxAddress.Text = "127.0.0.1";
            // 
            // txbDB
            // 
            this.txbDB.Location = new System.Drawing.Point(28, 813);
            this.txbDB.Multiline = true;
            this.txbDB.Name = "txbDB";
            this.txbDB.Size = new System.Drawing.Size(492, 171);
            this.txbDB.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 772);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Data Base";
            // 
            // sendDB
            // 
            this.sendDB.Location = new System.Drawing.Point(322, 766);
            this.sendDB.Name = "sendDB";
            this.sendDB.Size = new System.Drawing.Size(167, 32);
            this.sendDB.TabIndex = 13;
            this.sendDB.Text = "Send DB";
            this.sendDB.UseVisualStyleBackColor = true;
            this.sendDB.Click += new System.EventHandler(this.sendDB_Click);
            // 
            // txbShowAllApp
            // 
            this.txbShowAllApp.Location = new System.Drawing.Point(28, 553);
            this.txbShowAllApp.Multiline = true;
            this.txbShowAllApp.Name = "txbShowAllApp";
            this.txbShowAllApp.Size = new System.Drawing.Size(539, 207);
            this.txbShowAllApp.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 521);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Show All APP";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 1178);
            this.Controls.Add(this.txbChat);
            this.Controls.Add(this.sendDB);
            this.Controls.Add(this.txbDB);
            this.Controls.Add(this.txbMsg);
            this.Controls.Add(this.txbShowAllApp);
            this.Controls.Add(this.txbName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "ClientForm";
            this.Text = "Basic Async Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox txbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbMsg;
        private System.Windows.Forms.TextBox txbChat;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.TextBox txbDB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button sendDB;
        private System.Windows.Forms.TextBox txbShowAllApp;
        private System.Windows.Forms.Label label3;
    }
}

