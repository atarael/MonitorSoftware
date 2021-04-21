using System;
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
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Reflection;
 
namespace ServerSide
{

    public delegate void wordFromKeylogger(string word);
    public delegate void playCurrentState(int id);
    public delegate void stopCurrentState(int id);
    public delegate void RemoveClient(int id);
    public delegate void showLastReport(int id);
    public delegate void checkClientSocket();
    
    
    public delegate void clientReconnect(string name, Socket handler, string data);
    public delegate void OpenliveForm(Socket handler);
    public delegate void liveData(Socket handler, string data);
    public delegate void lastReport(Socket handler, string data);

    public delegate void setSetting(int id, string setting);
    public delegate void addExistClient(int id, string name, Socket handler);
    public delegate void addNewClient(int id, string name ); 
    public delegate void removeFromWaitingAdAdd(int id, string name, Socket handler);
    public delegate void clientNotConnect(int id );
    public delegate void noInternet();


    // State object for reading client data asynchronously  

    public class Program
    {
        private List<Client> Allclients;
        private Socket serverSocket;
        private Socket clientSocket; // We will only accept one socket.
        private byte[] buffer;
        public MonitorSetting monitorSystem;
        private List<int> clientIds = new List<int>();

        DBserver DBInstance;

        private int numOfClient;
        private String name;
        private static ServerForm s;
        public static Program program;
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");
        private static object retuen;

        [STAThread]
        static void Main(string[] args)
        {
            CreateShortcut("BSA-Server", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Assembly.GetExecutingAssembly().Location);

            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                connectAtReStartComputer();
                program = new Program();
                List<Client> Allclients;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                s = new ServerForm();
                s.Text = "Monitoring Interface";

               
                Thread listening = new Thread(AsynchronousSocketListener.StartListening);
                listening.Start();
              
                Application.Run(s);
                mutex.ReleaseMutex();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }



        }

         
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            var Report = new Document();
            string projectDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(projectDirectory).Parent.FullName;
            string userName = Environment.UserName;



            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "BE SAFE SERVER SIDE ";   // The description of the shortcut
            shortcut.IconLocation = path + "/besafe-server-icon.ico"; // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }

        private static bool checkInterntConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                {
                    return true;

                }
            }
            catch
            {
                s.serverNotConnect();
                return false;
            }

        }



        //private static void checkAllSocketsConnections()
        //{
        //    for (int i = 0; i < program.Allclients.Count; i++)
        //    {
        //        Socket socket = program.Allclients[i].ClientSocket;
        //        bool conn = checkSocketConnection(socket);
        //        if (conn)
        //        {
        //            s.addClientToCheckBoxLst(program.Allclients[i].Name, program.Allclients[i].id, socket);
        //        }
        //        else
        //        {
        //            s.addClientToCheckBoxLst(program.Allclients[i].Name, program.Allclients[i].id, null);

        //        }

        //    }
        //}


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


                DBInstance = DBserver.Instance;
                Allclients = DBInstance.initialServer();
                numOfClient = Allclients.Count();
                //ShowErrorDialog("num" + numOfClient);

                foreach (Client client in Allclients)
                {
                    //ShowErrorDialog("name " + Allclients[i].Name + "id " + Allclients[i].id);
                    s.addClientToCheckBoxLst(client.Name, client.id, client.ClientSocket);
                    clientIds.Add(client.id);
                     

                }
                serverSocket.BeginAccept(AcceptCallback, serverSocket);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog("StartServer" + ex.Message + " \n\n" + ex);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("StartServer" + ex.Message + " \n\n" + ex);
            }
        }

        private void AcceptCallback(IAsyncResult AR)
        { 
           // serverSocket.BeginReceive(StateObject.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback));
            clientSocket = serverSocket.EndAccept(AR);
            buffer = new byte[clientSocket.ReceiveBufferSize];
 
            // Listen for client data.
            //clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
            // Continue listening for clients.
            serverSocket.BeginAccept(new AsyncCallback( AcceptCallback), serverSocket);


            //try
            //{
            //    Socket handler = serverSocket.EndAccept(AR);
            //    foreach (Client client in Allclients)
            //    {
            //        if (client.ClientSocket == handler)
            //        {
            //            // Listen for client data.
            //            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, client.ClientSocket);
            //            // Continue listening for clients.
            //            clientSocket.BeginAccept(AcceptCallback, null);
            //            return;
            //        }
            //    }

            //    buffer = new Byte[handler.ReceiveBufferSize];
            //    // Listen for client data.
            //    handler.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, handler);
            //    // Continue listening for clients.
            //    serverSocket.BeginAccept(AcceptCallback, null);



            //}
            //catch (SocketException ex)
            //{
            //    ShowErrorDialog("AcceptCallback: " + ex.Message + " \n\n" + ex);
            //}
            //catch (ObjectDisposedException ex)
            //{
            //    ShowErrorDialog("AcceptCallback: " + ex.Message + " \n\n" + ex);
            //}
       
        }

         

        public void SendCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket = AR.AsyncState as Socket;
                foreach (Client client in program.Allclients)
                {
                    if (clientSocket == client.ClientSocket)
                    {
                        client.ClientSocket.EndSend(AR);
                    }
                }
                //ShowErrorDialog("sendCallback to socket: "+ clientSocket.RemoteEndPoint);



            }
            catch (SocketException ex)
            {
                ShowErrorDialog("SendCallback SocketException\n" + ex.Message + " \n\n" + ex);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("SendCallback ObjectDisposedException\n" + ex.Message + " \n\n" + ex);
            }
        }

        //public void ReceiveCallback(IAsyncResult AR)
        //{
        //    try
        //    {
        //        int received = clientSocket.EndReceive(AR); 

        //        if (received == 0)
        //        {
        //            return;
        //        }
        //        // Console.WriteLine("Client buffer: "+Allclients[0].buffer.ToString());
        //        // Console.WriteLine("Server buffer: "+ buffer.ToString());


        //        if (Allclients == null)
        //        {
        //            if (DBInstance == null)
        //            {
        //                DBInstance = DBserver.Instance;
        //            }
        //            Allclients = DBInstance.initialServer();

        //        }
        //        List<byte> encrypted = new List<byte>();
        //        foreach (Byte b in buffer)
        //        {
        //            if (b != 0)
        //            {
        //                encrypted.Add(b);
        //            }
        //        }
        //        Byte[] buf = encrypted.ToArray();
        //        string data = Encoding.UTF8.GetString(buf);
        //        if (data == string.Empty)
        //        {
        //            foreach (Client client in Allclients)
        //            {
        //                if (client.ClientSocket == clientSocket)
        //                {
        //                    data = Encoding.UTF8.GetString(client.buffer);
        //                }
        //            }
        //        }
        //        //string roundtrip = Crypto.Decrypt(data);
        //        string dataFromClient = data.Split('\0')[0];

        //        string decryptionSubject = dataFromClient.Split(new char[] { '\r' }, 2)[0];
        //        string decryptionMessage = dataFromClient.Split(new char[] { '\r' }, 2).Last();

        //        Console.WriteLine("SERVER GET: "+ dataFromClient);
        //        if (Allclients.Count > 0)
        //        {

        //            // client send id to connect or reconnect
        //            if (decryptionSubject == "id")
        //            {
        //                //reConnectSocket(clientSocket, decryptionMessage);

        //            }
        //            else
        //            {
        //                // find the buffer that get data from client
        //                foreach (Client client in Allclients)
        //                {
        //                    if (client.ClientSocket == clientSocket)
        //                    {


        //                        //string data = Encoding.UTF8.GetString(buffer);
        //                        using ( Aes myAes = Aes.Create())
        //                        {

        //                            // client send data in live
        //                            if (decryptionSubject == "current state")
        //                            {
        //                                // the function open form to disply data from client
        //                                client.openCurrentStateForm(decryptionMessage);
        //                            }

        //                            if (decryptionSubject == "open live form")
        //                            {
        //                                // the function open form to disply data from client
        //                                client.openCurrentStateForm("open CurrentState form");
        //                            }

        //                            if (decryptionSubject == "last report")
        //                            {
        //                                if (decryptionMessage != string.Empty)
        //                                {
        //                                    Report.createReportPDFFile(decryptionMessage, client.id);
        //                                }
        //                                else
        //                                {
        //                                    ShowErrorDialog("No last report to show");
        //                                }


        //                            }


        //                        }

        //                        //client.buffer = new byte[client.ClientSocket.ReceiveBufferSize];

        //                        // Start receiving data from this client Socket.
        //                        client.ClientSocket.BeginReceive(client.buffer, 0, client.buffer.Length, SocketFlags.None, this.ReceiveCallback, client.ClientSocket);

        //                    }
        //                }

        //            }

        //        }
        //        else
        //        {
        //            // client send name at first time 
        //            if (dataFromClient == "name")
        //            {
        //                name = dataFromClient;
        //                int newId = createNewId();


        //                Client newClient = new Client(name, newId, clientSocket, buffer);
        //                Allclients.Add(newClient);
        //                clientIds.Add(newId);
        //                numOfClient++;

        //                sendDataToClient(clientSocket, "id\r" + newId);
        //                s.addClientToWaitingList(name, newId);
        //            }
        //        }
        //        buffer = new byte[clientSocket.ReceiveBufferSize];
        //        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
        //    }
        //    // Avoid Pokemon exception handling in cases like these.
        //    catch (SocketException ex)
        //    {
        //        ShowErrorDialog("ReceiveCallback SocketException\n" + ex.Message + "\n" + ex);
        //       // s.setClientNotConnect(clientSocket.RemoteEndPoint);
        //    }
        //    catch (ObjectDisposedException ex)
        //    {
        //        ShowErrorDialog("ReceiveCallback ObjectDisposedException" + ex.Message + "\n" + ex);
        //    }
        //}


        //private int createNewId()
        //{
        //    int newId = 0;
        //    while (true)
        //    {
        //        if (clientIds.Contains(newId))
        //            newId++;
        //        else
        //            return newId;
        //    }
        //}
        //public static void OpenliveForm(Socket handler)
        //{ 
        //    foreach(Client client in program.Allclients)
        //    {
        //        if(client.ClientSocket == handler)
        //        {
        //            client.openCurrentStateForm("open CurrentState form");
        //        }
        //    }
        //}
        //public static void showLiveData(Socket handler,string decryptionMessage)
        //{

        //    foreach (Client client in program.Allclients)
        //    {
        //        if (client.ClientSocket == handler)
        //        {
        //            client.openCurrentStateForm(decryptionMessage);
        //        }
        //    }
        //}

        //public static void openLastReport(Socket handler, string decryptionMessage)
        //{
        //    foreach (Client client in program.Allclients)
        //    {
        //        if (client.ClientSocket == handler)
        //        {
        //            if (decryptionMessage != string.Empty)
        //            {
        //                Report.createReportPDFFile(decryptionMessage, client.id);
        //            }
        //            else
        //            {
        //                ShowErrorDialog("No last report to show");
        //            }  
        //        }
        //    }
        //}
   
        
        public static void reConnectSocket(string name, Socket handler, string id)
        { 
            int CID = Int32.Parse(id);
            s.addClientToCheckBoxLst(name, CID, handler);

           
            //Console.WriteLine(CID + " try reconnect");

            //DBserver DBInstance = DBserver.Instance;
            //if (program.Allclients == null)
            //{
                
            //    program.Allclients = DBInstance.initialServer();

            //}
            //if (CID < program.Allclients.Count)
            //{
            //    program.Allclients[CID].ClientSocket = current;
            //    //program.Allclients[CID].buffer = new byte[program.Allclients[CID].ClientSocket.ReceiveBufferSize];
            //    //program.Allclients[CID].ClientSocket.BeginReceive(program.Allclients[CID].buffer, 0, program.Allclients[CID].buffer.Length, SocketFlags.None, ReceiveCallback, Allclients[CID].ClientSocket);
            //    s.addClientToCheckBoxLst(program.Allclients[CID].Name, CID, program.Allclients[CID].ClientSocket);
            //    //program.sendDataToClient(program.Allclients[CID].ClientSocket, program.Allclients[CID].Name + " reconnected in Socket: " + program.Allclients[CID].ClientSocket.RemoteEndPoint);
                 

            //}
            //else
            //{
            //    Console.WriteLine(CID + " fail reconnect ");
            //}

        }

        //private void openMonitorDialog()
        //{
        //    // sajska 
        //    monitorSystem.ShowDialog();
        //}

        //public void sendDataToClient(Socket ClientSocket, string data)
        //{
        //    try
        //    {
        //        foreach (Client client in program.Allclients)
        //        {
        //            if (client.ClientSocket == ClientSocket && client.ClientSocket != null)
        //            {
        //                client.buffer = Encoding.ASCII.GetBytes(data);
        //                client.ClientSocket.BeginSend(client.buffer, 0, client.buffer.Length, SocketFlags.None, SendCallback, client.ClientSocket);
        //                //client.buffer = new byte[ClientSocket.ReceiveBufferSize];
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // wait to connect from client
        //        ShowErrorDialog("cannot send data to client in socket null\n" + ex.Message + "\n" + ex);
        //    }
        //}




        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Console.WriteLine(message);
        }


        // Create a method for a delegate.
        public static void startLiveMode(int id)
        {
            AsynchronousSocketListener.SendDataToClient(id, "start live mode");
        }
        
        public static void stopCurrentState(int id)
        {
            AsynchronousSocketListener.SendDataToClient(id, "stop current state");
        }
    
        public static void removeClient(int id)
        {           
           
            s.removeClientFromCheckBoxLst(id);
            AsynchronousSocketListener.removeClient(id);
           
            program.DBInstance = DBserver.Instance;
            program.DBInstance.removeClientFromDB(id.ToString());
       
          
        }

        public static void NoInternetConnection() { 
            s.serverNotConnect();
        }
        //public static bool CheckConnection(int id, Socket clientSocket)
        //{

        //    if (internetConnection() && checkSocketConnection(clientSocket))
        //    {
        //        return true;
        //    }

        //    else
        //        return false;
        //}
        //public static bool checkSocketConnection(Socket s)
        //{
        //    if (s == null)
        //    {
        //        return false;
        //    }
        //    try
        //    {
        //        program.sendDataToClient(s, "server check connection");
        //    }
        //    catch (SocketException ex)
        //    {
        //        return false;
        //    }


        //    return true;
        //}

        //public static bool internetConnection()
        //{
        //    try
        //    {
        //        using (var client = new WebClient())
        //        using (client.OpenRead("http://google.com/generate_204"))
        //            return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public static void checkAllConnection() {
            AsynchronousSocketListener.checkAllClientConnection();
        }


        public static void addExistClientToInterface(int id, string name, Socket handler)
        {
            s.addClientToCheckBoxLst(name, id,handler);
        }

        public static void addNewClientToInterface(int id, string name)
        {
            s.addClientToWaitingList(name, id);
        }
     
        public static void moveNewClientToInterface(int id, string name,Socket handler)
        {
            s.addClientToCheckBoxLst(name, id, handler);
            s.removeClientToWaitingList(id);
        }

        public static void setSettingDeleGate(int id, string setting)
        {
            AsynchronousSocketListener.setSetting( id, setting);

        }
        public static void ShowLastReportFromServer(int id)
        {
            AsynchronousSocketListener.SendDataToClient(id, "last report");
            
        }
        public static void setClientNotConnnect(int id) {
            s.setClientNotConnect(id);

        }

       
 
      

    }
}

