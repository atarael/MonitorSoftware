namespace ServerSide
{
    partial class MonitorSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorSetting));
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chbUpdateReportLimitApp = new System.Windows.Forms.CheckBox();
            this.chbReportImmediatelyLimitApp = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.dtgCategorySites = new System.Windows.Forms.DataGridView();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReportImmediately = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UpdateReport = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Blocked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chbBlockLimitApp = new System.Windows.Forms.CheckBox();
            this.txbUnblockedSites = new System.Windows.Forms.TextBox();
            this.chbUpdateReportIinappropriateWords = new System.Windows.Forms.CheckBox();
            this.chbReportImamediatelyIinappropriateWords = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.chblFrequency = new System.Windows.Forms.CheckedListBox();
            this.dtpTo1 = new System.Windows.Forms.DateTimePicker();
            this.dtpTo2 = new System.Windows.Forms.DateTimePicker();
            this.dtpTo3 = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom3 = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom2 = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom1 = new System.Windows.Forms.DateTimePicker();
            this.txbNumOfLimitHours = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txbBlockedSites = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnAddSiteToCancelMonitoring = new System.Windows.Forms.Button();
            this.btnAddSiteToMonitoring = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dtgCategorySites)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(100, 200);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Blocking Sites  By Category:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label5.Location = new System.Drawing.Point(88, 572);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(201, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Limit application installation";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // chbUpdateReportLimitApp
            // 
            this.chbUpdateReportLimitApp.AutoSize = true;
            this.chbUpdateReportLimitApp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chbUpdateReportLimitApp.Location = new System.Drawing.Point(115, 605);
            this.chbUpdateReportLimitApp.Name = "chbUpdateReportLimitApp";
            this.chbUpdateReportLimitApp.Size = new System.Drawing.Size(134, 24);
            this.chbUpdateReportLimitApp.TabIndex = 0;
            this.chbUpdateReportLimitApp.Text = "Update report";
            this.chbUpdateReportLimitApp.UseVisualStyleBackColor = false;
            // 
            // chbReportImmediatelyLimitApp
            // 
            this.chbReportImmediatelyLimitApp.AutoSize = true;
            this.chbReportImmediatelyLimitApp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chbReportImmediatelyLimitApp.Location = new System.Drawing.Point(269, 605);
            this.chbReportImmediatelyLimitApp.Name = "chbReportImmediatelyLimitApp";
            this.chbReportImmediatelyLimitApp.Size = new System.Drawing.Size(171, 24);
            this.chbReportImmediatelyLimitApp.TabIndex = 0;
            this.chbReportImmediatelyLimitApp.Text = "Report immediately";
            this.chbReportImmediatelyLimitApp.UseVisualStyleBackColor = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(84, 735);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(210, 20);
            this.label10.TabIndex = 1;
            this.label10.Text = "Set frequency to get Report:";
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnOK.Font = new System.Drawing.Font("Microsoft JhengHei UI Light", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnOK.ForeColor = System.Drawing.Color.Lime;
            this.btnOK.Location = new System.Drawing.Point(406, 872);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(312, 54);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "Set Setting";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dtgCategorySites
            // 
            this.dtgCategorySites.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgCategorySites.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dtgCategorySites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgCategorySites.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Category,
            this.ReportImmediately,
            this.UpdateReport,
            this.Blocked});
            this.dtgCategorySites.Location = new System.Drawing.Point(92, 233);
            this.dtgCategorySites.Name = "dtgCategorySites";
            this.dtgCategorySites.RowHeadersVisible = false;
            this.dtgCategorySites.RowHeadersWidth = 200;
            this.dtgCategorySites.RowTemplate.Height = 28;
            this.dtgCategorySites.Size = new System.Drawing.Size(1049, 168);
            this.dtgCategorySites.TabIndex = 8;
            // 
            // Category
            // 
            this.Category.DataPropertyName = "News";
            this.Category.HeaderText = "Category";
            this.Category.MinimumWidth = 8;
            this.Category.Name = "Category";
            // 
            // ReportImmediately
            // 
            this.ReportImmediately.HeaderText = "Report immediately";
            this.ReportImmediately.MinimumWidth = 8;
            this.ReportImmediately.Name = "ReportImmediately";
            // 
            // UpdateReport
            // 
            this.UpdateReport.HeaderText = "Update report";
            this.UpdateReport.MinimumWidth = 8;
            this.UpdateReport.Name = "UpdateReport";
            // 
            // Blocked
            // 
            this.Blocked.HeaderText = "Blocked";
            this.Blocked.MinimumWidth = 8;
            this.Blocked.Name = "Blocked";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(89, 508);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(283, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Please Add Sites To Cancel Monitoring";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(93, 429);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(345, 30);
            this.label3.TabIndex = 10;
            this.label3.Text = "Please Add Sites To Monitoring";
            // 
            // chbBlockLimitApp
            // 
            this.chbBlockLimitApp.AutoSize = true;
            this.chbBlockLimitApp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chbBlockLimitApp.Location = new System.Drawing.Point(467, 605);
            this.chbBlockLimitApp.Name = "chbBlockLimitApp";
            this.chbBlockLimitApp.Size = new System.Drawing.Size(92, 24);
            this.chbBlockLimitApp.TabIndex = 0;
            this.chbBlockLimitApp.Text = "Blocked";
            this.chbBlockLimitApp.UseVisualStyleBackColor = false;
            // 
            // txbUnblockedSites
            // 
            this.txbUnblockedSites.Location = new System.Drawing.Point(95, 532);
            this.txbUnblockedSites.Name = "txbUnblockedSites";
            this.txbUnblockedSites.Size = new System.Drawing.Size(507, 26);
            this.txbUnblockedSites.TabIndex = 13;
            // 
            // chbUpdateReportIinappropriateWords
            // 
            this.chbUpdateReportIinappropriateWords.AutoSize = true;
            this.chbUpdateReportIinappropriateWords.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chbUpdateReportIinappropriateWords.Location = new System.Drawing.Point(115, 687);
            this.chbUpdateReportIinappropriateWords.Name = "chbUpdateReportIinappropriateWords";
            this.chbUpdateReportIinappropriateWords.Size = new System.Drawing.Size(134, 24);
            this.chbUpdateReportIinappropriateWords.TabIndex = 0;
            this.chbUpdateReportIinappropriateWords.Text = "Update report";
            this.chbUpdateReportIinappropriateWords.UseVisualStyleBackColor = false;
            // 
            // chbReportImamediatelyIinappropriateWords
            // 
            this.chbReportImamediatelyIinappropriateWords.AutoSize = true;
            this.chbReportImamediatelyIinappropriateWords.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.chbReportImamediatelyIinappropriateWords.Location = new System.Drawing.Point(269, 687);
            this.chbReportImamediatelyIinappropriateWords.Name = "chbReportImamediatelyIinappropriateWords";
            this.chbReportImamediatelyIinappropriateWords.Size = new System.Drawing.Size(171, 24);
            this.chbReportImamediatelyIinappropriateWords.TabIndex = 0;
            this.chbReportImamediatelyIinappropriateWords.Text = "Report immediately";
            this.chbReportImamediatelyIinappropriateWords.UseVisualStyleBackColor = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label14.Location = new System.Drawing.Point(88, 654);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(198, 20);
            this.label14.TabIndex = 2;
            this.label14.Text = "Typing inappropriate words";
            this.label14.Click += new System.EventHandler(this.label5_Click);
            // 
            // chblFrequency
            // 
            this.chblFrequency.CheckOnClick = true;
            this.chblFrequency.FormattingEnabled = true;
            this.chblFrequency.Items.AddRange(new object[] {
            "Each Day",
            "Each Week",
            "Two once each week",
            "Each Month"});
            this.chblFrequency.Location = new System.Drawing.Point(124, 766);
            this.chblFrequency.Name = "chblFrequency";
            this.chblFrequency.Size = new System.Drawing.Size(200, 96);
            this.chblFrequency.TabIndex = 14;
            this.chblFrequency.SelectedIndexChanged += new System.EventHandler(this.chblFrequency_SelectedIndexChanged);
            // 
            // dtpTo1
            // 
            this.dtpTo1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTo1.Location = new System.Drawing.Point(549, 1018);
            this.dtpTo1.Name = "dtpTo1";
            this.dtpTo1.ShowUpDown = true;
            this.dtpTo1.Size = new System.Drawing.Size(130, 26);
            this.dtpTo1.TabIndex = 23;
            this.dtpTo1.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // dtpTo2
            // 
            this.dtpTo2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTo2.Location = new System.Drawing.Point(549, 1054);
            this.dtpTo2.Name = "dtpTo2";
            this.dtpTo2.ShowUpDown = true;
            this.dtpTo2.Size = new System.Drawing.Size(130, 26);
            this.dtpTo2.TabIndex = 24;
            this.dtpTo2.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // dtpTo3
            // 
            this.dtpTo3.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTo3.Location = new System.Drawing.Point(549, 1088);
            this.dtpTo3.Name = "dtpTo3";
            this.dtpTo3.ShowUpDown = true;
            this.dtpTo3.Size = new System.Drawing.Size(130, 26);
            this.dtpTo3.TabIndex = 25;
            this.dtpTo3.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // dtpFrom3
            // 
            this.dtpFrom3.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpFrom3.Location = new System.Drawing.Point(378, 1088);
            this.dtpFrom3.Name = "dtpFrom3";
            this.dtpFrom3.ShowUpDown = true;
            this.dtpFrom3.Size = new System.Drawing.Size(130, 26);
            this.dtpFrom3.TabIndex = 26;
            this.dtpFrom3.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // dtpFrom2
            // 
            this.dtpFrom2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpFrom2.Location = new System.Drawing.Point(378, 1054);
            this.dtpFrom2.Name = "dtpFrom2";
            this.dtpFrom2.ShowUpDown = true;
            this.dtpFrom2.Size = new System.Drawing.Size(130, 26);
            this.dtpFrom2.TabIndex = 27;
            this.dtpFrom2.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // dtpFrom1
            // 
            this.dtpFrom1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpFrom1.Location = new System.Drawing.Point(378, 1019);
            this.dtpFrom1.Name = "dtpFrom1";
            this.dtpFrom1.ShowUpDown = true;
            this.dtpFrom1.Size = new System.Drawing.Size(130, 26);
            this.dtpFrom1.TabIndex = 28;
            this.dtpFrom1.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // txbNumOfLimitHours
            // 
            this.txbNumOfLimitHours.Location = new System.Drawing.Point(361, 984);
            this.txbNumOfLimitHours.Name = "txbNumOfLimitHours";
            this.txbNumOfLimitHours.Size = new System.Drawing.Size(50, 26);
            this.txbNumOfLimitHours.TabIndex = 22;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label12.Location = new System.Drawing.Point(520, 1089);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 20);
            this.label12.TabIndex = 15;
            this.label12.Text = "to";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label13.Location = new System.Drawing.Point(116, 1018);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(208, 20);
            this.label13.TabIndex = 16;
            this.label13.Text = "Limit to a certain time range:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label7.Location = new System.Drawing.Point(116, 987);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(239, 20);
            this.label7.TabIndex = 17;
            this.label7.Text = "Enter number of hours to limited:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label11.Location = new System.Drawing.Point(520, 1056);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(23, 20);
            this.label11.TabIndex = 18;
            this.label11.Text = "to";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label9.Location = new System.Drawing.Point(331, 1018);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 20);
            this.label9.TabIndex = 19;
            this.label9.Text = "from";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label8.Location = new System.Drawing.Point(520, 1024);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 20);
            this.label8.TabIndex = 20;
            this.label8.Text = "to";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(86, 956);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 20);
            this.label6.TabIndex = 21;
            this.label6.Text = "Limit Computer time";
            // 
            // txbBlockedSites
            // 
            this.txbBlockedSites.Location = new System.Drawing.Point(92, 458);
            this.txbBlockedSites.Name = "txbBlockedSites";
            this.txbBlockedSites.Size = new System.Drawing.Size(507, 26);
            this.txbBlockedSites.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 29;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnAddSiteToCancelMonitoring
            // 
            this.btnAddSiteToCancelMonitoring.Location = new System.Drawing.Point(617, 532);
            this.btnAddSiteToCancelMonitoring.Name = "btnAddSiteToCancelMonitoring";
            this.btnAddSiteToCancelMonitoring.Size = new System.Drawing.Size(143, 30);
            this.btnAddSiteToCancelMonitoring.TabIndex = 30;
            this.btnAddSiteToCancelMonitoring.Text = "Add Site";
            this.btnAddSiteToCancelMonitoring.UseVisualStyleBackColor = true;
            this.btnAddSiteToCancelMonitoring.Click += new System.EventHandler(this.btnAddSiteToCancelMonitoring_Click);
            // 
            // btnAddSiteToMonitoring
            // 
            this.btnAddSiteToMonitoring.Location = new System.Drawing.Point(617, 454);
            this.btnAddSiteToMonitoring.Name = "btnAddSiteToMonitoring";
            this.btnAddSiteToMonitoring.Size = new System.Drawing.Size(143, 30);
            this.btnAddSiteToMonitoring.TabIndex = 30;
            this.btnAddSiteToMonitoring.Text = "Add Site";
            this.btnAddSiteToMonitoring.UseVisualStyleBackColor = true;
            this.btnAddSiteToMonitoring.Click += new System.EventHandler(this.btnAddSiteToMonitoring_Click);
            // 
            // MonitorSetting
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1332, 1195);
            this.Controls.Add(this.btnAddSiteToMonitoring);
            this.Controls.Add(this.btnAddSiteToCancelMonitoring);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dtpTo1);
            this.Controls.Add(this.dtpTo2);
            this.Controls.Add(this.dtpTo3);
            this.Controls.Add(this.dtpFrom3);
            this.Controls.Add(this.dtpFrom2);
            this.Controls.Add(this.dtpFrom1);
            this.Controls.Add(this.txbNumOfLimitHours);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chblFrequency);
            this.Controls.Add(this.txbBlockedSites);
            this.Controls.Add(this.txbUnblockedSites);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtgCategorySites);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.chbReportImamediatelyIinappropriateWords);
            this.Controls.Add(this.chbBlockLimitApp);
            this.Controls.Add(this.chbReportImmediatelyLimitApp);
            this.Controls.Add(this.chbUpdateReportIinappropriateWords);
            this.Controls.Add(this.chbUpdateReportLimitApp);
            this.HelpButton = true;
            this.Name = "MonitorSetting";
            this.Text = "MonitorSystem";
            this.Load += new System.EventHandler(this.MonitorSystem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgCategorySites)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chbUpdateReportLimitApp;
        private System.Windows.Forms.CheckBox chbReportImmediatelyLimitApp;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridView dtgCategorySites;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ReportImmediately;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UpdateReport;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Blocked;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbBlockLimitApp;
        private System.Windows.Forms.TextBox txbUnblockedSites;
        private System.Windows.Forms.CheckBox chbUpdateReportIinappropriateWords;
        private System.Windows.Forms.CheckBox chbReportImamediatelyIinappropriateWords;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckedListBox chblFrequency;
        private System.Windows.Forms.DateTimePicker dtpTo1;
        private System.Windows.Forms.DateTimePicker dtpTo2;
        private System.Windows.Forms.DateTimePicker dtpTo3;
        private System.Windows.Forms.DateTimePicker dtpFrom3;
        private System.Windows.Forms.DateTimePicker dtpFrom2;
        private System.Windows.Forms.DateTimePicker dtpFrom1;
        private System.Windows.Forms.TextBox txbNumOfLimitHours;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbBlockedSites;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnAddSiteToCancelMonitoring;
        private System.Windows.Forms.Button btnAddSiteToMonitoring;
    }
}