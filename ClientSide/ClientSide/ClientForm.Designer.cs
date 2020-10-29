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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.txbDB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txbName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.labelIP = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.lblConnectedSeccesful = new System.Windows.Forms.Label();
            this.lblStartMonitoring = new System.Windows.Forms.Label();
            this.SuspendLayout();
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
            this.label2.Location = new System.Drawing.Point(261, 237);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 6;
            // 
            // txbName
            // 
            this.txbName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbName.Location = new System.Drawing.Point(315, 283);
            this.txbName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbName.Name = "txbName";
            this.txbName.Size = new System.Drawing.Size(384, 26);
            this.txbName.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(254, 288);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 20);
            this.label1.TabIndex = 16;
            this.label1.Text = "Name";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxAddress.Location = new System.Drawing.Point(292, 334);
            this.textBoxAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(276, 26);
            this.textBoxAddress.TabIndex = 15;
            this.textBoxAddress.Text = "127.0.0.1";
            // 
            // labelIP
            // 
            this.labelIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelIP.AutoSize = true;
            this.labelIP.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelIP.Location = new System.Drawing.Point(258, 339);
            this.labelIP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(24, 20);
            this.labelIP.TabIndex = 14;
            this.labelIP.Text = "IP";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnect.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonConnect.BackgroundImage")));
            this.buttonConnect.Location = new System.Drawing.Point(597, 330);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(112, 35);
            this.buttonConnect.TabIndex = 13;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // lblConnectedSeccesful
            // 
            this.lblConnectedSeccesful.AutoSize = true;
            this.lblConnectedSeccesful.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblConnectedSeccesful.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblConnectedSeccesful.ForeColor = System.Drawing.Color.LawnGreen;
            this.lblConnectedSeccesful.Location = new System.Drawing.Point(288, 393);
            this.lblConnectedSeccesful.Name = "lblConnectedSeccesful";
            this.lblConnectedSeccesful.Size = new System.Drawing.Size(398, 46);
            this.lblConnectedSeccesful.TabIndex = 18;
            this.lblConnectedSeccesful.Text = "Connected Seccesfull!";
            this.lblConnectedSeccesful.Visible = false;
            // 
            // lblStartMonitoring
            // 
            this.lblStartMonitoring.AutoSize = true;
            this.lblStartMonitoring.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblStartMonitoring.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblStartMonitoring.ForeColor = System.Drawing.Color.LawnGreen;
            this.lblStartMonitoring.Location = new System.Drawing.Point(316, 439);
            this.lblStartMonitoring.Name = "lblStartMonitoring";
            this.lblStartMonitoring.Size = new System.Drawing.Size(334, 46);
            this.lblStartMonitoring.TabIndex = 18;
            this.lblStartMonitoring.Text = "Start Monitoring...";
            this.lblStartMonitoring.Visible = false;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1106, 515);
            this.Controls.Add(this.lblStartMonitoring);
            this.Controls.Add(this.lblConnectedSeccesful);
            this.Controls.Add(this.txbName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.txbDB);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "ClientForm";
            this.Text = "Basic Async Client";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txbDB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label lblConnectedSeccesful;
        private System.Windows.Forms.Label lblStartMonitoring;
    }
}

