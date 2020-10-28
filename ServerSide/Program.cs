using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace ServerSide
{
    class Program
    {
        private List<Client> Allclients;
        private Socket serverSocket;
        private Socket clientSocket; // We will only accept one socket.
        private byte[] buffer;
        public MonitorSetting monitorSystem;
        public DBserver dbs;

        private int numOfClient;
        private String name;
        
        private static ServerForm s;


        [STAThread]
        static void Main(string[] args)
        {
            Program p = new Program(); 
            p.StartServer();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            s = new ServerForm();
            s.Text = "Server";
            Application.Run(s);
          //  System.Threading.Thread.CurrentThread.ApartmentState = System.Threading.ApartmentState.STA;

        }
        public void StartServer()
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
                dbs.createClientsTable();
                dbs.createTriggersTable();
                

            }
            catch (SocketException ex)
            {
                ShowErrorDialog("StartServer" + ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("StartServer" + ex.Message);
            }
        }

        public void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket = serverSocket.EndAccept(AR);
                buffer = new byte[clientSocket.ReceiveBufferSize];
               
                // Listen for client data.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, FirstReceiveCallback, clientSocket);
                // Continue listening for clients.
                serverSocket.BeginAccept(AcceptCallback, null);

            }
            catch (SocketException ex)
            {
                ShowErrorDialog("AcceptCallback: " + ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("AcceptCallback: " + ex.Message);
            }
        }

        public void addClientToCheckBoxLst(Client newClient)
        {
            if (newClient != null)
            {
                String line = "";
                line += "Client Name: " + newClient.Name + " ,id: " + newClient.id + " ,Socket: " + newClient.ClientSocket.RemoteEndPoint;
                //checkLstAllClient.Items.Insert(newClient.id, line);

            }


        }

        public void SendCallback(IAsyncResult AR)
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

        public void ReceiveCallback(IAsyncResult AR)
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

        public void FirstReceiveCallback(IAsyncResult AR)
        {
            try
            {

                Socket NewClientSocket = AR.AsyncState as Socket;
                int received = NewClientSocket.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }

                name = Encoding.ASCII.GetString(buffer);
                String[] SplitedName = name.Split('\0');
                name = SplitedName[0];

                String Setting = "";
                // set system 
                monitorSystem = new MonitorSetting(name);
                // open GUI to set Setting 
                //monitorSystem.ShowDialog();

                Thread openMonitorSetting = new Thread(openMonitorDialog);
                openMonitorSetting.SetApartmentState(ApartmentState.STA);
                openMonitorSetting.Start();

                while (openMonitorSetting.IsAlive); 
                Setting = monitorSystem.sendSystem(); // get Setting from monitorSystem form
               // ShowErrorDialog("Setting: \n"+Setting);
               
                // create new client 
                Client newClient = new Client(name, numOfClient, NewClientSocket, buffer);

                Allclients.Add(newClient);

                // write new client to Data base 
                dbs.fillClientsTable(numOfClient, name, Setting);
                 // send Setting to new Client
                sendSettingToClient(NewClientSocket, Setting);

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

        private void openMonitorDialog()
        {

            monitorSystem.ShowDialog();
        }

        public void sendSettingToClient(Socket ClientSocket, string Setting)
        {
            var sendSetting = Encoding.ASCII.GetBytes(Setting);
            clientSocket.BeginSend(sendSetting, 0, sendSetting.Length, SocketFlags.None, SendCallback, clientSocket);

        }
        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


    }
}
