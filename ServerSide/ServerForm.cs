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
        public MonitorSystem monitorSystem;
        public DBserver dbs;

        private int numOfClient;
        private String name;
        private bool getNameFromServer;

        public ServerForm()
        {
            InitializeComponent();
            StartServer();
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
        private void StartServer()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, 3333));
                serverSocket.Listen(10);
                serverSocket.BeginAccept(AcceptCallback, null);
                Allclients = new List<Client>();
                numOfClient = 0;
                dbs = new DBserver("TableOfClient");
                dbs.createNewDatabase();
                dbs.connectToDatabase();
                dbs.createTable();


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

        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket = serverSocket.EndAccept(AR);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                getNameFromServer = false;

                // Send a message to the newly connected client.
                var sendData = Encoding.ASCII.GetBytes("Hello client im server");
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);
                // Listen for client data.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, FirstReceiveCallback, clientSocket);
                // Continue listening for clients.
                serverSocket.BeginAccept(AcceptCallback, null);

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

        private void addClientToCheckBoxLst(Client newClient)
        {
            if (newClient != null)
            {
                Invoke((Action)delegate
                {
                    String line = "";
                    line += "Client Name: " + newClient.Name + " ,id: " + newClient.id + " ,Socket: " + newClient.ClientSocket.RemoteEndPoint;
                    checkLstAllClient.Items.Insert(newClient.id, line);

                });
            }


        }

        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket = AR.AsyncState as Socket;
                clientSocket.EndSend(AR);


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

        private void ReceiveCallback(IAsyncResult AR)
        {

            try
            {
                if (AR.AsyncState as Socket != null)
                {


                    int id = -1;
                    String clientName = "";
                    for (int i = 0; i < Allclients.Count; i++)
                    {
                        if (Allclients[i].ClientSocket == AR.AsyncState as Socket)
                        {
                            clientName = Allclients[i].Name;
                            id = i;
                            break;
                        }
                    }

                    if (id != -1)
                    {
                        int received = Allclients[id].ClientSocket.EndReceive(AR);

                        if (received == 0)
                        {
                            return;
                        }

                        string message = Encoding.ASCII.GetString(Allclients[id].buffer);

                        String[] SplitedMessage = message.Split('\0');
                        message = SplitedMessage[0];
                        var msg1 = Allclients[id].buffer;

                        Invoke((Action)delegate
                        {
                            // write on txtBox

                            txbChat.AppendText("CLIENT(" + clientName + "): |" + message + "|");
                            txbChat.AppendText(Environment.NewLine);
                            Text = "client says: ";
                        });

                        Allclients[id].buffer = new byte[Allclients[id].ClientSocket.ReceiveBufferSize];
                        Allclients[id].ClientSocket.BeginReceive(Allclients[id].buffer, 0, Allclients[id].buffer.Length, SocketFlags.None, ReceiveCallback, Allclients[id].ClientSocket);


                    }


                }


            }


            // Avoid Pokemon exception handling in cases like these.
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private void FirstReceiveCallback(IAsyncResult AR)
        {
            try
            {

                Socket client = AR.AsyncState as Socket;
                int received = client.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }

                name = Encoding.ASCII.GetString(buffer);
                String[] SplitedName = name.Split('\0');
                name = SplitedName[0];

                getNameFromServer = true;

                String System = "";
                // set system 
                Invoke((Action)delegate
                {
                    monitorSystem = new MonitorSystem(name);
                    // open gui to set system 
                    monitorSystem.ShowDialog();
                    System = monitorSystem.sendSystem(); // get system from monitorSystem form
                    ShowErrorDialog(name);

                });

                // create new client 
                Client newClient = new Client(name, numOfClient, client, buffer);
                Allclients.Add(newClient);
                dbs.fillTable(numOfClient, name, System);
                // Start receiving data from this client Socket.
                Allclients[numOfClient].ClientSocket.BeginReceive(Allclients[numOfClient].buffer, 0, Allclients[numOfClient].buffer.Length, SocketFlags.None, this.ReceiveCallback, Allclients[numOfClient].ClientSocket);


                addClientToCheckBoxLst(newClient);

                numOfClient++;

            }
            // Avoid Pokemon exception handling in cases like these.
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }



        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSendMsg_Click(object sender, EventArgs e)
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
            Invoke((Action)delegate
            {
                for (int i = 0; i < checkLstAllClient.Items.Count; i++)
                    if (checkLstAllClient.GetItemChecked(i))
                    {

                    }

            });

        }
    }

}