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

        const string NOT_CONNECTED = "Not_connected";
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

            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[0].Value.ToString() == "True")
                {

                    int id = int.Parse(row.Cells[3].Value.ToString());
                    playCurrentState handler = Program.startLiveMode;
                    handler(id);
                }
            }

 



        }
       
        public void addClientToCheckBoxLst(string Name, int id, Socket ClientSocket)
        {

            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {
                if (row.Cells[3].Value != null)
                {
                    if (int.Parse(row.Cells[3].Value.ToString()) == id)
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
                        line += "Client Name: " + Name + " ,id: " + id + " ,Socket: " + ClientSocket.RemoteEndPoint;
                        dgvConnectedClients.Rows.Add(false, Name, ClientSocket.RemoteEndPoint, id);
                    }
                    //line += "Client Name: " + Name + " ,id: " + id + " ,Socket: " + ClientSocket.RemoteEndPoint;
                    else
                    {
                        line += "Client Name: " + Name + " ,id: " + id + " ,Socket: " + "not connect";
                        dgvConnectedClients.Rows.Add(false, Name, NOT_CONNECTED, id);
                    }
                    // Check if id exists in checkbox
                    for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                    {

                        string l = (string)checkLstAllClient.Items[i];
                        string stringId = l.Split(',')[1].Split(' ')[1];
                        if (stringId == id.ToString())
                        {
                            //ShowErrorDialog(stringId);
                            Button b = new Button();
                            checkLstAllClient.Items.RemoveAt(i);
                            checkLstAllClient.Items.Insert(i, b);
                            exist = 1;
                            break;
                        }
                    }
                    if (exist == 0)
                        checkLstAllClient.Items.Insert(checkLstAllClient.Items.Count, line);

                });
            }
        }

        public void serverConnect()
        {
            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {
                if (row.Cells[3].Value != null) { 
                    int id = int.Parse(row.Cells[3].Value.ToString());
                    checkClientSocket handler = Program.checckClientConnection;
                    handler(id);
                }
               

            }
        }

        public void serverNotConnect()
        {
            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {
                row.Cells[2].Value = "NO Internet";
                
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

        private void btnRemoveClient_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[0].Value.ToString() == "True")
                {
                    if (row.Cells[2].Value.ToString() == NOT_CONNECTED)
                    {
                        ShowErrorDialog("Client not connected\r\nCannot show last report ");

                    }
                    else
                    {
                        int id = int.Parse(row.Cells[3].Value.ToString());
                        RemoveClient handler = Program.removeClient;
                        handler(id);
                    }
                }
            }

            


        }
 

        private void panel1_Paint(object sender, PaintEventArgs e)
        {


            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Width = 300;
            dataGridView1.Columns.Add("Column", "id");
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.AllowUserToAddRows = false;


            dgvConnectedClients.Columns[0].Width = 20; // checkbox
          //  dgvConnectedClients.Columns[0].
            dgvConnectedClients.Columns[1].Width = 150; // socket number
            dgvConnectedClients.Columns[2].Width = 250; // client name
            dgvConnectedClients.AllowUserToAddRows = false;


        }

        public void addClientToWaitingList(string value, int id)
        {

            Invoke((Action)delegate
            {
                dataGridView1.Rows.Add(value, id);
                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                btn.Text = "Set setting";
                btn.FillWeight = 200;
                btn.UseColumnTextForButtonValue = true;
                btn.Width = 150;
                dataGridView1.Columns.Add(btn);

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

        private void btnLastReport_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[0].Value.ToString() == "True")
                {
                    if (row.Cells[2].Value.ToString() == NOT_CONNECTED)
                    {
                        ShowErrorDialog("Client not connected\r\nCannot show last report ");

                    }
                    else {
                        int id = int.Parse(row.Cells[3].Value.ToString());
                        showLastReport handler = Program.ShowLastReportFromServer;
                        handler(id);
                    }
                }
            }
        }

        internal void setClientNotConnect(EndPoint remoteEndPoint)
        {
            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[2].Value.ToString() == remoteEndPoint.ToString())
                {
                    row.Cells[2].Value = NOT_CONNECTED;
                }
            }
        }

        private void btnSetSystem_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[0].Value.ToString() == "True")
                {
                    int id = int.Parse(row.Cells[3].Value.ToString());
                    MonitorSetting monitorSystem = new MonitorSetting(id);
                    monitorSystem.ShowDialog();
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

