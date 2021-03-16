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


        /// <summary>
        /// Construct server socket and bind socket to all local network interfaces, then listen for connections
        /// with a backlog of 10. Which means there can only be 10 pending connections lined up in the TCP stack
        /// at a time. This does not mean the server can handle only 10 connections. The we begin accepting connections.
        /// Meaning if there are connections queued, then we should process them.
        /// </summary>


        /*   private void btnSendMsg_Click(object sender, EventArgs e)
           {
               List<Socket> selectedClient = getCheckedClients();

               try
               {
                   String msg = txbMsg.Text;
                   txbMsg.Text = "";
                   var sendData = Encoding.ASCII.GetBytes(msg);
                   foreach (Socket s in selectedClient)
                   {
                       clientSocket = s;
                       clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);

                   }

                   // write on txtBox
                   txbChat.AppendText("ME: " + msg);
                   txbChat.AppendText(Environment.NewLine);

               }
               catch (SocketException ex)
               {
                   ShowErrorDialog(ex.Message);

               }
               catch (ObjectDisposedException ex)
               {
                   ShowErrorDialog(ex.Message);

               }
           }
           */


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

            Invoke((Action)delegate
            {
                for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                    if (checkLstAllClient.GetItemChecked(i))
                    {
                        // MonitorSetting monitorSystem = new MonitorSetting(i);
                        // monitorSystem.ShowDialog();


                    }

            });


        }
        public void btnGetCurrentState_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dgvConnectedClients.Rows)
            {

                if (row.Cells[0].Value.ToString() == "True")
                {
                    int id = int.Parse(row.Cells[3].Value.ToString());
                    playCurrentState handler = Program.startCurrentState;
                    handler(id);
                }
            }


            //for (int i = 0; i < checkLstAllClient.Items.Count; i++)
            //  if (checkLstAllClient.GetItemChecked(i))
            //  {

            //playCurrentState handler = Program.startCurrentState;
            // handler(i);
            //}



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
                            row.Cells[2].Value = "Not Connnected";
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
                        dgvConnectedClients.Rows.Add(false, Name, "not connect", id);
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

        internal void enabledbtnGetCurrentState(bool enable)
        {
            Invoke((Action)delegate
            {
                //btnGetCurrentState.Enabled = enable;
            });

        }

        public void removeClientFromCheckBoxLst(int id)
        {

            for (int i = 0; i < checkLstAllClient.Items.Count; i++)
            {

                string l = (string)checkLstAllClient.Items[i];
                string stringId = l.Split(',')[1].Split(' ')[1];
                if (stringId == id.ToString())
                {
                    // ShowErrorDialog("remove" + stringId);
                    checkLstAllClient.Items.RemoveAt(i);
                    break;
                }
            }

        }

        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnRemoveClient_Click(object sender, EventArgs e)
        {
            List<int> selectedClient = new List<int>();

            Invoke((Action)delegate
            {
                for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                    if (checkLstAllClient.GetItemChecked(i))
                    {
                        string l = (string)checkLstAllClient.Items[i];
                        string stringId = l.Split(',')[1].Split(' ')[1];
                        RemoveClient handler = Program.removeClient;
                        handler(Int32.Parse(stringId));
                    }

            });


        }

        private void checkLstAllClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {


            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Width = 300;
            dataGridView1.Columns.Add("Column", "id");
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.AllowUserToAddRows = false;


            dgvConnectedClients.Columns[0].Width = 20; // checkbox
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


        public void checkConnections()
        {



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
    }

}

