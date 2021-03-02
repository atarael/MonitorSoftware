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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerForm));
            this.checkLstAllClient = new System.Windows.Forms.CheckedListBox();
            this.btnSetSystem = new System.Windows.Forms.Button();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnGetCurrentState = new System.Windows.Forms.Button();
            this.btnRemoveClient = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkLstAllClient
            // 
            this.checkLstAllClient.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.checkLstAllClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkLstAllClient.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.checkLstAllClient.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.checkLstAllClient.FormattingEnabled = true;
            this.checkLstAllClient.Location = new System.Drawing.Point(33, 82);
            this.checkLstAllClient.Name = "checkLstAllClient";
            this.checkLstAllClient.Size = new System.Drawing.Size(655, 315);
            this.checkLstAllClient.TabIndex = 4;
            // 
            // btnSetSystem
            // 
            this.btnSetSystem.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnSetSystem.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnSetSystem.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnSetSystem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSetSystem.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnSetSystem.ForeColor = System.Drawing.Color.Lime;
            this.btnSetSystem.Location = new System.Drawing.Point(902, 502);
            this.btnSetSystem.Name = "btnSetSystem";
            this.btnSetSystem.Size = new System.Drawing.Size(274, 72);
            this.btnSetSystem.TabIndex = 5;
            this.btnSetSystem.Text = "Set Setting";
            this.btnSetSystem.UseVisualStyleBackColor = false;
            this.btnSetSystem.Click += new System.EventHandler(this.btnSetSystem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.groupBox1.Controls.Add(this.checkLstAllClient);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei UI Light", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox1.Location = new System.Drawing.Point(146, 318);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(694, 440);
            this.groupBox1.TabIndex = 70;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connected Clients List:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Highlight;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button1.ForeColor = System.Drawing.Color.Lime;
            this.button1.Location = new System.Drawing.Point(902, 318);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(274, 72);
            this.button1.TabIndex = 5;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnSetSystem_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Highlight;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button2.ForeColor = System.Drawing.Color.Lime;
            this.button2.Location = new System.Drawing.Point(902, 592);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(274, 72);
            this.button2.TabIndex = 5;
            this.button2.Text = "Show last report";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btnSetSystem_Click);
            // 
            // btnGetCurrentState
            // 
            this.btnGetCurrentState.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnGetCurrentState.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnGetCurrentState.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGetCurrentState.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGetCurrentState.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnGetCurrentState.ForeColor = System.Drawing.Color.Lime;
            this.btnGetCurrentState.Location = new System.Drawing.Point(902, 412);
            this.btnGetCurrentState.Name = "btnGetCurrentState";
            this.btnGetCurrentState.Size = new System.Drawing.Size(274, 72);
            this.btnGetCurrentState.TabIndex = 5;
            this.btnGetCurrentState.Text = "Get Current State";
            this.btnGetCurrentState.UseVisualStyleBackColor = false;
            this.btnGetCurrentState.Click += new System.EventHandler(this.btnGetCurrentState_Click);
            // 
            // btnRemoveClient
            // 
            this.btnRemoveClient.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnRemoveClient.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnRemoveClient.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnRemoveClient.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRemoveClient.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnRemoveClient.ForeColor = System.Drawing.Color.Lime;
            this.btnRemoveClient.Location = new System.Drawing.Point(902, 686);
            this.btnRemoveClient.Name = "btnRemoveClient";
            this.btnRemoveClient.Size = new System.Drawing.Size(274, 72);
            this.btnRemoveClient.TabIndex = 5;
            this.btnRemoveClient.Text = "Remove Client";
            this.btnRemoveClient.UseVisualStyleBackColor = false;
            this.btnRemoveClient.Click += new System.EventHandler(this.btnRemoveClient_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1357, 914);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetCurrentState);
            this.Controls.Add(this.btnSetSystem);
            this.Controls.Add(this.btnRemoveClient);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ServerForm";
            this.Text = "Basic Async Server";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckedListBox checkLstAllClient;
        private System.Windows.Forms.Button btnSetSystem;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnGetCurrentState;
        private System.Windows.Forms.Button btnRemoveClient;
    }
}

