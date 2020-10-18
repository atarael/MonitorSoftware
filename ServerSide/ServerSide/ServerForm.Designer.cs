namespace ServerSide
{
    partial class ServerForm
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
            this.txbChat = new System.Windows.Forms.TextBox();
            this.txbMsg = new System.Windows.Forms.TextBox();
            this.btnSendMsg = new System.Windows.Forms.Button();
            this.checkLstAllClient = new System.Windows.Forms.CheckedListBox();
            this.btnSetSystem = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txbChat
            // 
            this.txbChat.Location = new System.Drawing.Point(23, 45);
            this.txbChat.Multiline = true;
            this.txbChat.Name = "txbChat";
            this.txbChat.Size = new System.Drawing.Size(488, 357);
            this.txbChat.TabIndex = 1;
            // 
            // txbMsg
            // 
            this.txbMsg.Location = new System.Drawing.Point(23, 424);
            this.txbMsg.Multiline = true;
            this.txbMsg.Name = "txbMsg";
            this.txbMsg.Size = new System.Drawing.Size(344, 60);
            this.txbMsg.TabIndex = 1;
            // 
            // btnSendMsg
            // 
            this.btnSendMsg.Location = new System.Drawing.Point(396, 424);
            this.btnSendMsg.Name = "btnSendMsg";
            this.btnSendMsg.Size = new System.Drawing.Size(115, 60);
            this.btnSendMsg.TabIndex = 2;
            this.btnSendMsg.Text = "Send To Client";
            this.btnSendMsg.UseVisualStyleBackColor = true;
            this.btnSendMsg.Click += new System.EventHandler(this.btnSendMsg_Click);
            // 
            // checkLstAllClient
            // 
            this.checkLstAllClient.FormattingEnabled = true;
            this.checkLstAllClient.Location = new System.Drawing.Point(23, 503);
            this.checkLstAllClient.Name = "checkLstAllClient";
            this.checkLstAllClient.Size = new System.Drawing.Size(545, 257);
            this.checkLstAllClient.TabIndex = 4;
            // 
            // btnSetSystem
            // 
            this.btnSetSystem.Location = new System.Drawing.Point(106, 776);
            this.btnSetSystem.Name = "btnSetSystem";
            this.btnSetSystem.Size = new System.Drawing.Size(310, 95);
            this.btnSetSystem.TabIndex = 5;
            this.btnSetSystem.Text = "Set System to Client";
            this.btnSetSystem.UseVisualStyleBackColor = true;
            this.btnSetSystem.Click += new System.EventHandler(this.btnSetSystem_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(996, 962);
            this.Controls.Add(this.btnSetSystem);
            this.Controls.Add(this.checkLstAllClient);
            this.Controls.Add(this.btnSendMsg);
            this.Controls.Add(this.txbMsg);
            this.Controls.Add(this.txbChat);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ServerForm";
            this.Text = "Basic Async Server";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txbChat;
        private System.Windows.Forms.TextBox txbMsg;
        private System.Windows.Forms.Button btnSendMsg;
        private System.Windows.Forms.CheckedListBox checkLstAllClient;
        private System.Windows.Forms.Button btnSetSystem;
    }
}

