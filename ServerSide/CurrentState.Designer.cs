namespace ServerSide
{
    partial class CurrentState
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
            this.txbProcesses = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.txbTyped = new System.Windows.Forms.TextBox();
            this.txbSites = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txbProcesses
            // 
            this.txbProcesses.Location = new System.Drawing.Point(58, 84);
            this.txbProcesses.Multiline = true;
            this.txbProcesses.Name = "txbProcesses";
            this.txbProcesses.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbProcesses.Size = new System.Drawing.Size(431, 597);
            this.txbProcesses.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(305, 704);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(466, 66);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txbTyped
            // 
            this.txbTyped.Location = new System.Drawing.Point(512, 451);
            this.txbTyped.Multiline = true;
            this.txbTyped.Name = "txbTyped";
            this.txbTyped.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbTyped.Size = new System.Drawing.Size(582, 230);
            this.txbTyped.TabIndex = 0;
            // 
            // txbSites
            // 
            this.txbSites.Location = new System.Drawing.Point(512, 84);
            this.txbSites.Multiline = true;
            this.txbSites.Name = "txbSites";
            this.txbSites.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txbSites.Size = new System.Drawing.Size(582, 318);
            this.txbSites.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(508, 428);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(141, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Typed in keyboard:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(508, 61);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Browes in Sites:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(58, 61);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current Procces:";
            // 
            // CurrentState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 962);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.txbSites);
            this.Controls.Add(this.txbTyped);
            this.Controls.Add(this.txbProcesses);
            this.MaximumSize = new System.Drawing.Size(1200, 1218);
            this.MinimumSize = new System.Drawing.Size(1200, 1018);
            this.Name = "CurrentState";
            this.Text = "CurrentState";
            this.Load += new System.EventHandler(this.CurrentState_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbProcesses;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txbTyped;
        private System.Windows.Forms.TextBox txbSites;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}