using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

            try
            {
                

                

            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);

            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);

            }

            Invoke((Action)delegate
            {
                for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                    if (checkLstAllClient.GetItemChecked(i))
                    {

                    }

            });

        }
        public void btnGetCurrentState_Click(object sender, EventArgs e)
        {
            List<int> selectedClient = new List<int>();

            Invoke((Action)delegate
            {
                for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                    if (checkLstAllClient.GetItemChecked(i))
                    { 
                      
                        playCurrentState handler = Program.startCurrentState;
                        handler(i);
                    }

            });

        }
        public void addClientToCheckBoxLst(String Name, int id, Socket ClientSocket)
        {
            int exist = 0;
            if (Name != null && id >= 0)
            {
                _ = Invoke((Action)delegate
                {
                    String line = "";
                    if (ClientSocket != null)
                        line += "Client Name: " + Name + " ,id: " + id + " ,Socket: " + ClientSocket.RemoteEndPoint;
                    else
                        line += "Client Name: " + Name + " ,id: " + id + " ,Socket: " + "not connect";
                    // Check if id exists in checkbox
                    for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                    {

                        string l = (string)checkLstAllClient.Items[i];
                        string stringId = l.Split(',')[1].Split(' ')[1];
                        if (stringId == id.ToString())
                        {
                            ShowErrorDialog(stringId);
                            checkLstAllClient.Items.RemoveAt(i);
                            checkLstAllClient.Items.Insert(i, line);
                            exist = 1;
                            break;
                        }
                    }
                    if (exist == 0)
                        checkLstAllClient.Items.Insert(checkLstAllClient.Items.Count, line);
                    /*  for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                           if (i == id) {
                              checkLstAllClient.Items.RemoveAt(id);
                          }
                       checkLstAllClient.Items.Insert(checkLstAllClient.Items.Count, line);*/
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
                    ShowErrorDialog("remove" + stringId);
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
                        handler( Int32.Parse(stringId));
                    }

            });


        }
        

        
       


    }

}