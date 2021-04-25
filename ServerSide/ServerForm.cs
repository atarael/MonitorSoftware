using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Documents;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class ServerForm : Form
    {
        const string NO_INTERNET = "NO Internet";
        const string NOT_CONNECTED = "Not Connected";
        public MonitorSetting monitorSystem;
        public DBserver dbs;

        public ServerForm()
        {
            InitializeComponent();
        }
   
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SHOWME)
            {
                ShowMe();
            }
            base.WndProc(ref m);
        }
      
        private void ShowMe()
        {
            this.Show();
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            // get our current "TopMost" value (ours will always be false though)
            bool top = TopMost;
            // make our form jump to the top of everything
            TopMost = true;
            // set it back to whatever it was
            TopMost = top;
        }

        
        public void btnGetCurrentState_Click(object sender, EventArgs e)
        {
            bool SelectClient = false;

            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[0].Value.ToString() == "True")
                {
                    SelectClient = true;
                    string socketState = row.Cells[2].Value.ToString();
                    if (socketState == NOT_CONNECTED || socketState == NO_INTERNET)
                    {
                        ShowErrorDialog("Client not connected\nLive mode not available ");

                    }
                    else
                    {                        
                        int id = int.Parse(row.Cells[3].Value.ToString());
                        playCurrentState handler = Program.startLiveMode;
                         
                        handler(id);
                    }
                    
                }
            }
            if (!SelectClient)
            {
                ShowErrorDialog("Please select computer!");

            }




        }

        private void btnLastReport_Click(object sender, EventArgs e)
        {


            bool SelectClient = false;

            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[0].Value.ToString() == "True")
                {
                    SelectClient = true;
                    string socketState = row.Cells[2].Value.ToString();
                    if (socketState == NOT_CONNECTED || socketState == NO_INTERNET)
                    {
                        ShowErrorDialog("Client not connected\r\nCannot show last report ");

                    }
                    else
                    {
                        int id = int.Parse(row.Cells[3].Value.ToString());
                        showLastReport handler = Program.ShowLastReportFromServer;
                        handler(id);

                    }
                }
            }
            if (!SelectClient)
            {
                ShowErrorDialog("Please select computer!");

            }

        }

        private void btnRemoveClient_Click(object sender, EventArgs e)
        {

            bool SelectClient = false;

            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[0].Value.ToString() == "True")
                {
                    SelectClient = true;
                    string socketState = row.Cells[2].Value.ToString();
                    if (socketState == NOT_CONNECTED || socketState == NO_INTERNET)
                    {
                        ShowErrorDialog("Client not connected\r\nCannot remove computer ");

                    }
                    else
                    {
                        int id = int.Parse(row.Cells[3].Value.ToString());
                        RemoveClient handler = Program.removeClient;
                        handler(id);
                    }
                }
            }
            if (!SelectClient)
            {
                ShowErrorDialog("Please select computer!");


            }


        }

        private void btnSetSystem_Click(object sender, EventArgs e)
        {
            bool SelectClient = false;

            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[0].Value.ToString() == "True")
                {
                    SelectClient = true;
                    string socketState = row.Cells[2].Value.ToString();
                    if (socketState == NOT_CONNECTED || socketState == NO_INTERNET)
                    {
                        ShowErrorDialog("Client not connected\nLive mode not available ");

                    }
                    else
                    {
                        int id = int.Parse(row.Cells[3].Value.ToString());
                        MonitorSetting monitorSystem = new MonitorSetting(id);
                        monitorSystem.ShowDialog();

                    }
                }
            }
            if (!SelectClient)
            {
                ShowErrorDialog("Please select computer!");

            }





        }



        public void addClientToCheckBoxLst(string Name, int id, Socket ClientSocket)
        {
            // Check if settings have been set for this client
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value != null)
                {
                    if (row.Cells[1].Value.Equals(id))
                    {
                        return; 
                    
                    }
                }
               
            }



            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {
                if (row.Cells[3].Value != null)
                {
                    int currentID = int.Parse(row.Cells[3].Value.ToString());
                    if (currentID == id)
                    {
                        if (ClientSocket == null)
                        {
                            row.Cells[2].Value = NOT_CONNECTED;
                        }
                        else
                        {
                            row.Cells[2].Value = ClientSocket.RemoteEndPoint;
                        }

                        // ShowErrorDialog("id exist in datagrid");
                        return;
                    }
                }


            }

            int exist = 0;
            if (Name != null && id >= 0)
            {

                if (this.Visible != true)
                {
                    this.Show();
                }
                Invoke((Action)delegate
                {

                    String line = "";
                    if (ClientSocket != null)
                    {
                          dgvConnectedClients.Rows.Add(false, Name, ClientSocket.RemoteEndPoint, id);
                    }
                    //line += "Client Name: " + Name + " ,id: " + id + " ,Socket: " + ClientSocket.RemoteEndPoint;
                    else
                    {
                         dgvConnectedClients.Rows.Add(false, Name, NOT_CONNECTED, id);
                    }
                    
                  
                });
         
            }
        }

        public void serverConnect()
        {
            
                    checkClientSocket handler = Program.checkAllConnection;
                    handler();
               
        }

        public void serverNotConnect()
        {
            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {
                row.Cells[2].Value = NO_INTERNET;
                
            }
        }

        internal void enabledbtnGetCurrentState(bool enable)
        {
            Invoke((Action)delegate
            {
                //btnGetCurrentState.Enabled = enable;
            });

        }

        public void removeClientFromCheckBoxLst(int id)
        {
            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[3].Value.ToString() == id.ToString())
                {
                    dgvConnectedClients.Rows.Remove(row);
                }
            }
 

        }

        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {


            dataGridView1.AllowUserToAddRows = false;

           
            dgvConnectedClients.Columns[0].Width = 20; // checkbox
            // dgvConnectedClients.Columns[0].
            dgvConnectedClients.Columns[1].Width = 150; // socket number
            dgvConnectedClients.Columns[2].Width = 250; // client name
            dgvConnectedClients.AllowUserToAddRows = false;
            

        }

        public void addClientToWaitingList(string value, int id)
        {
             
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value.Equals(id))
                {
                    return;

                }
            }

            Invoke((Action)delegate
            {
                dataGridView1.Rows.Add(value, id, "Confirm");
              
            });





        }

        public void removeClientToWaitingList(int id)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[1].Value.Equals(id))
                {
                    Invoke((Action)delegate
                    {
                        dataGridView1.Rows.Remove(row);
                    });
                    break;
                }
            }






        }
       
        public bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }
      
        private void setSttingToNewClient(object sender, DataGridViewCellEventArgs e)
        {
            //ShowErrorDialog("set setting");
            if (e.RowIndex >= 0)
            {

                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                string s = row.Cells[0].Value.ToString();
                int id = int.Parse(row.Cells[1].Value.ToString());

                MonitorSetting monitorSystem = new MonitorSetting(id);
                monitorSystem.ShowDialog();


            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }

        }

       
        internal void setClientNotConnect(int id)
        {
            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {
                int clientId = int.Parse(row.Cells[3].Value.ToString());
                if (clientId == id)
                {
                    row.Cells[2].Value = NOT_CONNECTED;
                }
            }
        }

        private void dgvConnectedClients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {                
                int id = int.Parse(row.Cells[3].Value.ToString());
               // showLastReport handler = Program.ShowLastReportFromServer;
               // handler(id);

            }
        }
    }

}

