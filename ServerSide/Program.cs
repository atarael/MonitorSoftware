using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using IWshRuntimeLibrary;

namespace ServerSide
{
    
    public delegate void playCurrentState(int id);
    public delegate void stopCurrentState(int id);
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
        public static Program program;

        [STAThread]
        static void Main(string[] args)
        {
            connectAtReStartComputer();
            program = new Program();
            List<Client> Allclients;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            s = new ServerForm();
            s.Text = "Server";
            s.Show();
            program.StartServer();
            Application.Run();
            //System.Threading.Thread.CurrentThread.ApartmentState = System.Threading.ApartmentState.STA;
            
        }
        private static void connectAtReStartComputer()
        {
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            WshShell shell = new WshShell();
            string shortcutAddress = startupFolder + @"\MyStartupShortcut.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "A startup shortcut. If you delete this shortcut from your computer, LaunchOnStartup.exe will not launch on Windows Startup"; // set the description of the shortcut
            shortcut.WorkingDirectory = Application.StartupPath; /* working directory */
            shortcut.TargetPath = Application.ExecutablePath; /* path of the executable */
            shortcut.Save(); // save the shortcut 
            shortcut.Arguments = "/a /c";
        }

        public void StartServer()
        {
            try
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, 3333));
                serverSocket.Listen(10);
                serverSocket.BeginAccept(AcceptCallback, null);
                dbs = new DBserver();
                Allclients = dbs.initialServer();
                numOfClient = Allclients.Count();
                //ShowErrorDialog("num" + numOfClient);
               
                for (int i = 0; i < numOfClient; i++)
                {
                    //ShowErrorDialog("name " + Allclients[i].Name + "id " + Allclients[i].id);
                    s.addClientToCheckBoxLst(Allclients[i].Name, Allclients[i].id, Allclients[i].ClientSocket);

                }



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
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
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
        /*
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
                        ShowErrorDialog(message);
                       


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
        */
        public void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                Socket CurrentClientSocket = AR.AsyncState as Socket;
                int received = CurrentClientSocket.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }
                // find the buffer that get data from client
                foreach (Client client in Allclients)
                {
                    if (client.ClientSocket == CurrentClientSocket)
                    {
                         buffer = client.buffer;
                         client.buffer = new byte[client.ClientSocket.ReceiveBufferSize];

                        // Start receiving data from this client Socket.
                        client.ClientSocket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None, this.ReceiveCallback, client.ClientSocket);

                    }
                }
                string data = Encoding.UTF8.GetString(buffer);
                ShowErrorDialog("get data in sokcet: \n" + CurrentClientSocket.RemoteEndPoint + "\nData from client is:\n"+ data);
                var dataFromClient = data.Split(new[] { '\r','\0', '\n' }, 2);
                if (dataFromClient[0] == "name")
                {
                    name = dataFromClient[1];
                    String[] SplitedMessage = name.Split('\0');
                    name = SplitedMessage[0];

                    ShowErrorDialog("|" + name + "|");
                    String Setting = "";
                    // set system 
                    monitorSystem = new MonitorSetting(name);
                    // open GUI to set Setting 
                    //monitorSystem.ShowDialog();

                    Thread openMonitorSetting = new Thread(openMonitorDialog);
                    openMonitorSetting.SetApartmentState(ApartmentState.STA);
                    openMonitorSetting.Start();

                    while (openMonitorSetting.IsAlive) ;
                    Setting = monitorSystem.setting; // get Setting from monitorSystem form
                                                     // ShowErrorDialog("Setting: \n"+Setting);

                    // create new client 
                    Client newClient = new Client(name, numOfClient, CurrentClientSocket, buffer);

                    Allclients.Add(newClient);

                    // write new client to Data base 
                    dbs.fillClientsTable(numOfClient, name, Setting);
                    // Send Id to Client
                    sendDataToClient(CurrentClientSocket, "id\r" + numOfClient);

                    // send Setting to new Client
                    String set = "setting\r\n" + Setting;
                    sendDataToClient(CurrentClientSocket, set);

                    // Start receiving data from this client Socket.
                    Allclients[numOfClient].ClientSocket.BeginReceive(Allclients[numOfClient].buffer, 0, Allclients[numOfClient].buffer.Length, SocketFlags.None, this.ReceiveCallback, Allclients[numOfClient].ClientSocket);

                    s.addClientToCheckBoxLst(newClient.Name, newClient.id, newClient.ClientSocket);
                    numOfClient++;

                }

                if (dataFromClient[0] == "id")
                {
                    reConnectSocket(CurrentClientSocket, dataFromClient[1]);

                }
                if (dataFromClient[0] == "current state") {
                    //int CID = Int32.Parse(dataFromClient[0].Split('\0')[0]);
                    // dataFromClient[1].Split('\0')[0]
                    foreach (Client client in Allclients)
                    {
                        if (client.ClientSocket == CurrentClientSocket)
                        {
                           client.openCurrentStateForm(dataFromClient[1].Split('\0')[0]);
                        }
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

        private void reConnectSocket(Socket current,string id)
        {
            int CID = Int32.Parse(id.Split('\0')[0]);
            if (CID < Allclients.Count)
            {
                Allclients[CID].ClientSocket = current;
                Allclients[CID].buffer = new byte[Allclients[CID].ClientSocket.ReceiveBufferSize];
                Allclients[CID].ClientSocket.BeginReceive(Allclients[CID].buffer, 0, Allclients[CID].buffer.Length, SocketFlags.None, ReceiveCallback, Allclients[CID].ClientSocket);
                s.addClientToCheckBoxLst(Allclients[CID].Name, CID, Allclients[CID].ClientSocket);
                sendDataToClient(Allclients[CID].ClientSocket, Allclients[CID].Name + " reconnected in Socket: " + Allclients[CID].ClientSocket.RemoteEndPoint);
            }

        }

        private void openMonitorDialog()
        {

            monitorSystem.ShowDialog();
        }

        public void sendDataToClient(Socket ClientSocket, string data)
        {
            if (ClientSocket != null)
            {
                var sendSetting = Encoding.ASCII.GetBytes(data);
                ClientSocket.BeginSend(sendSetting, 0, sendSetting.Length, SocketFlags.None, SendCallback, clientSocket);
            }
            else {
                // wait to connect from client
                ShowErrorDialog("cannot send data to client in socket null");
            }
            
        }
        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

      
        // Create a method for a delegate.
        public static void startCurrentState(int id)
        {
            if (id < program.Allclients.Count) { 
                
                Socket clientSocket = program.Allclients[id].ClientSocket;
                program.sendDataToClient(clientSocket, "get current state");
                ShowErrorDialog("DelegateMethod play, id is: "+ id );
                s.enabledbtnGetCurrentState(false);
            }
         
        }
        public static void stopCurrentState(int id)
        {
            if (id < program.Allclients.Count)
            {
               
                Socket clientSocket = program.Allclients[id].ClientSocket;
                program.sendDataToClient(clientSocket, "stop current state");
                ShowErrorDialog("in stopCurrentState: " + id); 
                s.enabledbtnGetCurrentState(true);
            }
           

        }
    }
}
