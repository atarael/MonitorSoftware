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
            this.dgvConnectedClients = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.btnLastReport = new System.Windows.Forms.Button();
            this.btnGetCurrentState = new System.Windows.Forms.Button();
            this.btnRemoveClient = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.setting = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConnectedClients)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkLstAllClient
            // 
            this.checkLstAllClient.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.checkLstAllClient.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkLstAllClient.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.checkLstAllClient.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.checkLstAllClient.FormattingEnabled = true;
            this.checkLstAllClient.Location = new System.Drawing.Point(22, 61);
            this.checkLstAllClient.Name = "checkLstAllClient";
            this.checkLstAllClient.Size = new System.Drawing.Size(655, 35);
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
            this.btnSetSystem.Location = new System.Drawing.Point(849, 225);
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
            this.groupBox1.Controls.Add(this.dgvConnectedClients);
            this.groupBox1.Controls.Add(this.checkLstAllClient);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei UI Light", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.groupBox1.Location = new System.Drawing.Point(91, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(694, 256);
            this.groupBox1.TabIndex = 70;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connected Clients List:";
            // 
            // dgvConnectedClients
            // 
            this.dgvConnectedClients.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dgvConnectedClients.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvConnectedClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConnectedClients.ColumnHeadersVisible = false;
            this.dgvConnectedClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column4,
            this.Column2});
            this.dgvConnectedClients.GridColor = System.Drawing.Color.FloralWhite;
            this.dgvConnectedClients.Location = new System.Drawing.Point(22, 99);
            this.dgvConnectedClients.MaximumSize = new System.Drawing.Size(600, 200);
            this.dgvConnectedClients.MinimumSize = new System.Drawing.Size(600, 100);
            this.dgvConnectedClients.Name = "dgvConnectedClients";
            this.dgvConnectedClients.RowHeadersVisible = false;
            this.dgvConnectedClients.RowHeadersWidth = 62;
            this.dgvConnectedClients.RowTemplate.Height = 28;
            this.dgvConnectedClients.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dgvConnectedClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvConnectedClients.Size = new System.Drawing.Size(600, 138);
            this.dgvConnectedClients.TabIndex = 72;
            this.dgvConnectedClients.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvConnectedClients_CellClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.MinimumWidth = 8;
            this.Column1.Name = "Column1";
            this.Column1.Width = 20;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "socket";
            this.Column3.MinimumWidth = 70;
            this.Column3.Name = "Column3";
            this.Column3.Width = 70;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "name";
            this.Column4.MinimumWidth = 70;
            this.Column4.Name = "Column4";
            this.Column4.Width = 70;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "id";
            this.Column2.MinimumWidth = 8;
            this.Column2.Name = "Column2";
            this.Column2.Visible = false;
            this.Column2.Width = 8;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Highlight;
            this.button1.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.button1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.button1.ForeColor = System.Drawing.Color.Lime;
            this.button1.Location = new System.Drawing.Point(849, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(274, 72);
            this.button1.TabIndex = 5;
            this.button1.Text = "?";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // btnLastReport
            // 
            this.btnLastReport.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnLastReport.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnLastReport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnLastReport.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLastReport.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnLastReport.ForeColor = System.Drawing.Color.Lime;
            this.btnLastReport.Location = new System.Drawing.Point(849, 315);
            this.btnLastReport.Name = "btnLastReport";
            this.btnLastReport.Size = new System.Drawing.Size(274, 72);
            this.btnLastReport.TabIndex = 5;
            this.btnLastReport.Text = "Show last report";
            this.btnLastReport.UseVisualStyleBackColor = false;
            this.btnLastReport.Click += new System.EventHandler(this.btnLastReport_Click);
            // 
            // btnGetCurrentState
            // 
            this.btnGetCurrentState.BackColor = System.Drawing.SystemColors.Highlight;
            this.btnGetCurrentState.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnGetCurrentState.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnGetCurrentState.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnGetCurrentState.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnGetCurrentState.ForeColor = System.Drawing.Color.Lime;
            this.btnGetCurrentState.Location = new System.Drawing.Point(849, 135);
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
            this.btnRemoveClient.Location = new System.Drawing.Point(849, 409);
            this.btnRemoveClient.Name = "btnRemoveClient";
            this.btnRemoveClient.Size = new System.Drawing.Size(274, 72);
            this.btnRemoveClient.TabIndex = 5;
            this.btnRemoveClient.Text = "Remove Client";
            this.btnRemoveClient.UseVisualStyleBackColor = false;
            this.btnRemoveClient.Click += new System.EventHandler(this.btnRemoveClient_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.btnGetCurrentState);
            this.panel1.Controls.Add(this.btnSetSystem);
            this.panel1.Controls.Add(this.btnRemoveClient);
            this.panel1.Controls.Add(this.btnLastReport);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(30, 241);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 532);
            this.panel1.TabIndex = 72;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.name,
            this.id,
            this.setting});
            this.dataGridView1.Location = new System.Drawing.Point(91, 330);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 28;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(694, 151);
            this.dataGridView1.TabIndex = 72;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.setSttingToNewClient);
            // 
            // name
            // 
            this.name.HeaderText = "Column5";
            this.name.MinimumWidth = 150;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 150;
            // 
            // id
            // 
            this.id.HeaderText = "Column5";
            this.id.MinimumWidth = 8;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 150;
            // 
            // setting
            // 
            this.setting.HeaderText = "Column5";
            this.setting.MinimumWidth = 100;
            this.setting.Name = "setting";
            this.setting.ReadOnly = true;
            this.setting.Width = 150;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1297, 977);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(1600, 1600);
            this.Name = "ServerForm";
            this.Text = "Basic Async Server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvConnectedClients)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckedListBox checkLstAllClient;
        private System.Windows.Forms.Button btnSetSystem;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLastReport;
        private System.Windows.Forms.Button btnGetCurrentState;
        private System.Windows.Forms.Button btnRemoveClient;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dgvConnectedClients;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewButtonColumn setting;
    }
}

