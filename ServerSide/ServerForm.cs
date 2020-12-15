using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class ServerForm : Form
    {
        private List<Client> Allclients;
        private Socket serverSocket;
        private Socket clientSocket; // We will only accept one socket.
        private byte[] buffer;
        public MonitorSetting monitorSystem;
        public DBserver dbs;

        private int numOfClient;
        private String name;
        private bool getNameFromServer;

        public ServerForm()
        {
            InitializeComponent();
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Construct server socket and bind socket to all local network interfaces, then listen for connections
        /// with a backlog of 10. Which means there can only be 10 pending connections lined up in the TCP stack
        /// at a time. This does not mean the server can handle only 10 connections. The we begin accepting connections.
        /// Meaning if there are connections queued, then we should process them.
        /// </summary>
     
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

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
        private List<Socket> getCheckedClients()
        {
            List<Socket> selectedClient = new List<Socket>();
            Invoke((Action)delegate
            {
                for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                    if (checkLstAllClient.GetItemChecked(i))
                    {
                        selectedClient.Add(Allclients[i].ClientSocket);
                    }

            });
            return selectedClient;
        }

        private void btnSetSystem_Click(object sender, EventArgs e)
        {

            List<Socket> selectedClient = getCheckedClients();

            try
            {
                String Setting = "";
                
                
                foreach (Socket s in selectedClient)
                {
                    // set system 
                    Invoke((Action)delegate
                    {
                        monitorSystem = new MonitorSetting(name);
                        // open GUI to set Setting 
                        monitorSystem.ShowDialog();
                      
                        ShowErrorDialog(Setting);
                       // sendSettingToClient(s, Setting);
                    });
                   
                }

                

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
                      
            playCurrentState handler = Program.startCurrentState;
            handler(0);
        }
        public void addClientToCheckBoxLst(String Name, int id , Socket ClientSocket)
        {
            if (Name != null && id >=0   )
            {
                 Invoke((Action)delegate
                {
                    String line = "";
                    if( ClientSocket != null)
                        line += "Client Name: " + Name + " ,id: " + id + " ,Socket: " + ClientSocket.RemoteEndPoint;
                    else
                        line += "Client Name: " + Name + " ,id: " + id + " ,Socket: " + "not connect";
                    // Check if id exists in checkbox 

                    for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                         if (i == id) {
                            checkLstAllClient.Items.RemoveAt(id);
                        }
 
                     checkLstAllClient.Items.Insert(id, line);
                  });

            }

        }

        internal void enabledbtnGetCurrentState(bool enable)
        {
            Invoke((Action)delegate
            {
                btnGetCurrentState.Enabled = enable;
            });
        
        }
    }

}