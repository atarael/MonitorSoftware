namespace ServerSide
{
    partial class MonitorSystem
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chbUpdateReportLimitApp = new System.Windows.Forms.CheckBox();
            this.chbReportImmediatelyLimitApp = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txbNumOfLimitHours = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpFrom1 = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chbEachDay = new System.Windows.Forms.CheckBox();
            this.chbEachWeek = new System.Windows.Forms.CheckBox();
            this.chbEachMonth = new System.Windows.Forms.CheckBox();
            this.chbEach2onceWeek = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.dtgCategorySites = new System.Windows.Forms.DataGridView();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReportImmediately = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UpdateReport = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Blocked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chbBlockLimitApp = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.dtpFrom2 = new System.Windows.Forms.DateTimePicker();
            this.dtpFrom3 = new System.Windows.Forms.DateTimePicker();
            this.dtpTo3 = new System.Windows.Forms.DateTimePicker();
            this.dtpTo2 = new System.Windows.Forms.DateTimePicker();
            this.dtpTo1 = new System.Windows.Forms.DateTimePicker();
            this.txbUnblockedSites = new System.Windows.Forms.TextBox();
            this.txbBlockedSites = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dtgCategorySites)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(190, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Set Trigger to new Client: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(95, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Blocking Sites  By Category:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(95, 706);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(201, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Limit application installation";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // chbUpdateReportLimitApp
            // 
            this.chbUpdateReportLimitApp.AutoSize = true;
            this.chbUpdateReportLimitApp.Location = new System.Drawing.Point(121, 739);
            this.chbUpdateReportLimitApp.Name = "chbUpdateReportLimitApp";
            this.chbUpdateReportLimitApp.Size = new System.Drawing.Size(134, 24);
            this.chbUpdateReportLimitApp.TabIndex = 0;
            this.chbUpdateReportLimitApp.Text = "Update report";
            this.chbUpdateReportLimitApp.UseVisualStyleBackColor = true;
            // 
            // chbReportImmediatelyLimitApp
            // 
            this.chbReportImmediatelyLimitApp.AutoSize = true;
            this.chbReportImmediatelyLimitApp.Location = new System.Drawing.Point(121, 769);
            this.chbReportImmediatelyLimitApp.Name = "chbReportImmediatelyLimitApp";
            this.chbReportImmediatelyLimitApp.Size = new System.Drawing.Size(171, 24);
            this.chbReportImmediatelyLimitApp.TabIndex = 0;
            this.chbReportImmediatelyLimitApp.Text = "Report immediately";
            this.chbReportImmediatelyLimitApp.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(96, 858);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Limit Computer time";
            // 
            // txbNumOfLimitHours
            // 
            this.txbNumOfLimitHours.Location = new System.Drawing.Point(371, 886);
            this.txbNumOfLimitHours.Name = "txbNumOfLimitHours";
            this.txbNumOfLimitHours.Size = new System.Drawing.Size(50, 26);
            this.txbNumOfLimitHours.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(126, 889);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(239, 20);
            this.label7.TabIndex = 2;
            this.label7.Text = "Enter number of hours to limited:";
            // 
            // dtpFrom1
            // 
            this.dtpFrom1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpFrom1.Location = new System.Drawing.Point(388, 921);
            this.dtpFrom1.Name = "dtpFrom1";
            this.dtpFrom1.ShowUpDown = true;
            this.dtpFrom1.Size = new System.Drawing.Size(130, 26);
            this.dtpFrom1.TabIndex = 6;
            this.dtpFrom1.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(530, 926);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 20);
            this.label8.TabIndex = 2;
            this.label8.Text = "to";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(341, 920);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 20);
            this.label9.TabIndex = 2;
            this.label9.Text = "from";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(87, 1057);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(201, 20);
            this.label10.TabIndex = 1;
            this.label10.Text = "Set frequncy to get Report:";
            // 
            // chbEachDay
            // 
            this.chbEachDay.AutoSize = true;
            this.chbEachDay.Location = new System.Drawing.Point(130, 1095);
            this.chbEachDay.Name = "chbEachDay";
            this.chbEachDay.Size = new System.Drawing.Size(99, 24);
            this.chbEachDay.TabIndex = 0;
            this.chbEachDay.Text = "each day";
            this.chbEachDay.UseVisualStyleBackColor = true;
            // 
            // chbEachWeek
            // 
            this.chbEachWeek.AutoSize = true;
            this.chbEachWeek.Location = new System.Drawing.Point(241, 1096);
            this.chbEachWeek.Name = "chbEachWeek";
            this.chbEachWeek.Size = new System.Drawing.Size(111, 24);
            this.chbEachWeek.TabIndex = 0;
            this.chbEachWeek.Text = "each week";
            this.chbEachWeek.UseVisualStyleBackColor = true;
            // 
            // chbEachMonth
            // 
            this.chbEachMonth.AutoSize = true;
            this.chbEachMonth.Location = new System.Drawing.Point(543, 1094);
            this.chbEachMonth.Name = "chbEachMonth";
            this.chbEachMonth.Size = new System.Drawing.Size(119, 24);
            this.chbEachMonth.TabIndex = 0;
            this.chbEachMonth.Text = "each month";
            this.chbEachMonth.UseVisualStyleBackColor = true;
            // 
            // chbEach2onceWeek
            // 
            this.chbEach2onceWeek.AutoSize = true;
            this.chbEach2onceWeek.Location = new System.Drawing.Point(358, 1094);
            this.chbEach2onceWeek.Name = "chbEach2onceWeek";
            this.chbEach2onceWeek.Size = new System.Drawing.Size(179, 24);
            this.chbEach2onceWeek.TabIndex = 0;
            this.chbEach2onceWeek.Text = "two once each week";
            this.chbEach2onceWeek.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(744, 22);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(312, 54);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dtgCategorySites
            // 
            this.dtgCategorySites.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dtgCategorySites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgCategorySites.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Category,
            this.ReportImmediately,
            this.UpdateReport,
            this.Blocked});
            this.dtgCategorySites.Location = new System.Drawing.Point(91, 132);
            this.dtgCategorySites.Name = "dtgCategorySites";
            this.dtgCategorySites.RowHeadersVisible = false;
            this.dtgCategorySites.RowHeadersWidth = 200;
            this.dtgCategorySites.RowTemplate.Height = 28;
            this.dtgCategorySites.Size = new System.Drawing.Size(949, 270);
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
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(530, 958);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(23, 20);
            this.label11.TabIndex = 2;
            this.label11.Text = "to";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(530, 991);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 20);
            this.label12.TabIndex = 2;
            this.label12.Text = "to";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(92, 568);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Please fill Site to unblock:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(92, 425);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 20);
            this.label3.TabIndex = 10;
            this.label3.Text = "Please add site to blocking:";
            // 
            // chbBlockLimitApp
            // 
            this.chbBlockLimitApp.AutoSize = true;
            this.chbBlockLimitApp.Location = new System.Drawing.Point(122, 799);
            this.chbBlockLimitApp.Name = "chbBlockLimitApp";
            this.chbBlockLimitApp.Size = new System.Drawing.Size(92, 24);
            this.chbBlockLimitApp.TabIndex = 0;
            this.chbBlockLimitApp.Text = "Blocked";
            this.chbBlockLimitApp.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(126, 920);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(208, 20);
            this.label13.TabIndex = 2;
            this.label13.Text = "Limit to a certain time range:";
            // 
            // dtpFrom2
            // 
            this.dtpFrom2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpFrom2.Location = new System.Drawing.Point(388, 956);
            this.dtpFrom2.Name = "dtpFrom2";
            this.dtpFrom2.ShowUpDown = true;
            this.dtpFrom2.Size = new System.Drawing.Size(130, 26);
            this.dtpFrom2.TabIndex = 6;
            this.dtpFrom2.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // dtpFrom3
            // 
            this.dtpFrom3.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpFrom3.Location = new System.Drawing.Point(388, 990);
            this.dtpFrom3.Name = "dtpFrom3";
            this.dtpFrom3.ShowUpDown = true;
            this.dtpFrom3.Size = new System.Drawing.Size(130, 26);
            this.dtpFrom3.TabIndex = 6;
            this.dtpFrom3.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // dtpTo3
            // 
            this.dtpTo3.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTo3.Location = new System.Drawing.Point(559, 990);
            this.dtpTo3.Name = "dtpTo3";
            this.dtpTo3.ShowUpDown = true;
            this.dtpTo3.Size = new System.Drawing.Size(130, 26);
            this.dtpTo3.TabIndex = 6;
            this.dtpTo3.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // dtpTo2
            // 
            this.dtpTo2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTo2.Location = new System.Drawing.Point(559, 956);
            this.dtpTo2.Name = "dtpTo2";
            this.dtpTo2.ShowUpDown = true;
            this.dtpTo2.Size = new System.Drawing.Size(130, 26);
            this.dtpTo2.TabIndex = 6;
            this.dtpTo2.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // dtpTo1
            // 
            this.dtpTo1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTo1.Location = new System.Drawing.Point(559, 920);
            this.dtpTo1.Name = "dtpTo1";
            this.dtpTo1.ShowUpDown = true;
            this.dtpTo1.Size = new System.Drawing.Size(130, 26);
            this.dtpTo1.TabIndex = 6;
            this.dtpTo1.Value = new System.DateTime(2020, 9, 2, 12, 17, 0, 0);
            // 
            // txbUnblockedSites
            // 
            this.txbUnblockedSites.Location = new System.Drawing.Point(91, 591);
            this.txbUnblockedSites.Multiline = true;
            this.txbUnblockedSites.Name = "txbUnblockedSites";
            this.txbUnblockedSites.Size = new System.Drawing.Size(507, 92);
            this.txbUnblockedSites.TabIndex = 13;
            // 
            // txbBlockedSites
            // 
            this.txbBlockedSites.Location = new System.Drawing.Point(91, 453);
            this.txbBlockedSites.Multiline = true;
            this.txbBlockedSites.Name = "txbBlockedSites";
            this.txbBlockedSites.Size = new System.Drawing.Size(507, 92);
            this.txbBlockedSites.TabIndex = 13;
            // 
            // MonitorSystem
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 1168);
            this.Controls.Add(this.txbBlockedSites);
            this.Controls.Add(this.txbUnblockedSites);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtgCategorySites);
            this.Controls.Add(this.btnOK);
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
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chbBlockLimitApp);
            this.Controls.Add(this.chbReportImmediatelyLimitApp);
            this.Controls.Add(this.chbEachMonth);
            this.Controls.Add(this.chbEach2onceWeek);
            this.Controls.Add(this.chbEachWeek);
            this.Controls.Add(this.chbEachDay);
            this.Controls.Add(this.chbUpdateReportLimitApp);
            this.HelpButton = true;
            this.Name = "MonitorSystem";
            this.Text = "MonitorSystem";
            this.Load += new System.EventHandler(this.MonitorSystem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dtgCategorySites)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chbUpdateReportLimitApp;
        private System.Windows.Forms.CheckBox chbReportImmediatelyLimitApp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txbNumOfLimitHours;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpFrom1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chbEachDay;
        private System.Windows.Forms.CheckBox chbEachWeek;
        private System.Windows.Forms.CheckBox chbEachMonth;
        private System.Windows.Forms.CheckBox chbEach2onceWeek;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridView dtgCategorySites;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ReportImmediately;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UpdateReport;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Blocked;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbBlockLimitApp;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dtpFrom2;
        private System.Windows.Forms.DateTimePicker dtpFrom3;
        private System.Windows.Forms.DateTimePicker dtpTo3;
        private System.Windows.Forms.DateTimePicker dtpTo2;
        private System.Windows.Forms.DateTimePicker dtpTo1;
        private System.Windows.Forms.TextBox txbUnblockedSites;
        private System.Windows.Forms.TextBox txbBlockedSites;
    }
}