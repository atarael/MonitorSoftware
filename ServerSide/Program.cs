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
    public delegate void checkClientSocket(int id);

    public delegate void setSetting(int id, string setting);
    class Program
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
                program.StartServer();
                Thread interntAvilable = new Thread(checkConnections);
                interntAvilable.Start();
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

        private static void checkConnections()
        {
            while (true)
            {

                if (checkInterntConnection())
                {
                    checkAllSocketsConnections();
                 
                }

                Thread.Sleep(5000);


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

     

        private static void checkAllSocketsConnections()
        {
            for (int i=0; i < program.Allclients.Count;i++)
            {
                Socket socket = program.Allclients[i].ClientSocket;
                bool conn = checkSocketConnection(socket);
                if (conn)
                {
                    s.addClientToCheckBoxLst(program.Allclients[i].Name, program.Allclients[i].id,socket);
                }
                else{
                    s.addClientToCheckBoxLst(program.Allclients[i].Name, program.Allclients[i].id, null);

                }

            }
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
                DBInstance = DBserver.Instance;
                Allclients = DBInstance.initialServer();
                numOfClient = Allclients.Count();
                //ShowErrorDialog("num" + numOfClient);

                for (int i = 0; i < numOfClient; i++)
                {
                    //ShowErrorDialog("name " + Allclients[i].Name + "id " + Allclients[i].id);
                    s.addClientToCheckBoxLst(Allclients[i].Name, Allclients[i].id, Allclients[i].ClientSocket);
                    clientIds.Add(Allclients[i].id);
                }

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
                ShowErrorDialog("AcceptCallback: " + ex.Message + " \n\n" + ex);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("AcceptCallback: " + ex.Message + " \n\n" + ex);
            }
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

                if (Allclients==null) {
                    if(DBInstance == null)
                    {
                        DBInstance = DBserver.Instance;
                    }
                    Allclients = DBInstance.initialServer();
                   
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

                //string data = Encoding.UTF8.GetString(buffer);
                using (Aes myAes = Aes.Create())
                {

                    // Encrypt the string to an array of bytes.
                    //byte[] encrypted = Crypto.EncryptStringToBytes_Aes(data, myAes.Key, myAes.IV);

                    List<byte> encrypted = new List<byte>();
                    foreach (Byte b in buffer) {
                        if (b != 0)
                        {
                            encrypted.Add(b);
                        }
                    }
                    Byte[] buf = encrypted.ToArray();
                    string data = Encoding.UTF8.GetString(buf);
                    // Decrypt the bytes to a string.
                    string roundtrip = Crypto.Decrypt(data);
                    string dataFromClient = roundtrip.Split('\0')[0];
                     

                                    
                string decryptionSubject = dataFromClient.Split(new char[]{ '\r' },2)[0];
                 string decryptionMessage = dataFromClient.Split(new char[] { '\r' }, 2).Last();

                 
                // client send name at first time 
                if (decryptionSubject == "name")
                {
                    name = decryptionMessage;
                    int newId = createNewId();


                    Client newClient = new Client(name, newId, CurrentClientSocket, buffer);
                    Allclients.Add(newClient);
                    clientIds.Add(newId);
                    numOfClient++;

                    sendDataToClient(CurrentClientSocket, "id\r" + newId);
                    s.addClientToWaitingList(name, newId);
                }

                // client send id to connect or reconnect
                if (decryptionSubject == "id")
                {
                    reConnectSocket(CurrentClientSocket, decryptionMessage);

                }

                // client send data in live
                if (decryptionSubject == "current state")
                {
                    
                    foreach (Client client in Allclients)
                    {
                        if (client.ClientSocket == CurrentClientSocket)
                        {
                            // the function open form to disply data from client
                            client.openCurrentStateForm(decryptionMessage);

                        }
                    }


                }
           
                if (decryptionSubject == "open live form")
                {

                    foreach (Client client in Allclients)
                    {
                        if (client.ClientSocket == CurrentClientSocket)
                        {
                            // the function open form to disply data from client
                            client.openCurrentStateForm("open CurrentState form");

                        } 
                    }


                }
             
                if (decryptionSubject == "last report")
                {
                    // find the buffer that get data from client
                    foreach (Client client in Allclients)
                    {
                        if (client.ClientSocket == CurrentClientSocket)
                        {
                                if (decryptionMessage!= string.Empty) {
                                    Report.createReportPDFFile(decryptionMessage, client.id);
                                }
                                else
                                {
                                    ShowErrorDialog("No last report to show");
                                }
                                
                        }
                    }
                   
                     
                }
            
                
                }
            }
            // Avoid Pokemon exception handling in cases like these.
            catch (SocketException ex)
            {
                ShowErrorDialog("ReceiveCallback SocketException\n" + ex.Message + "\n" + ex);
                s.setClientNotConnect(clientSocket.RemoteEndPoint);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("ReceiveCallback ObjectDisposedException" + ex.Message +"\n" +ex);
            }//dfrg bdhdh ghfgds 
        }

       
        private int createNewId()
        {
            int newId = 0;
            while (true)
            {
                if (clientIds.Contains(newId))
                    newId++;
                else
                    return newId;
            }
        }

        private void reConnectSocket(Socket current, string id)
        {

            int CID = Int32.Parse(id);
            Console.WriteLine(CID + " try reconnect");
          
             if (  CID < Allclients.Count)
             {
                Allclients[CID].ClientSocket = current;
                Allclients[CID].buffer = new byte[Allclients[CID].ClientSocket.ReceiveBufferSize];
                Allclients[CID].ClientSocket.BeginReceive(Allclients[CID].buffer, 0, Allclients[CID].buffer.Length, SocketFlags.None, ReceiveCallback, Allclients[CID].ClientSocket);
                s.addClientToCheckBoxLst(Allclients[CID].Name, CID, Allclients[CID].ClientSocket);
                sendDataToClient(Allclients[CID].ClientSocket, Allclients[CID].Name + " reconnected in Socket: " + Allclients[CID].ClientSocket.RemoteEndPoint);
            }
            else
            {
                Console.WriteLine(CID + " fail reconnect " + Allclients.Count);
            }

        }

        private void openMonitorDialog()
        {
            // sajska 
            monitorSystem.ShowDialog();
        }

        public void sendDataToClient(Socket ClientSocket, string data)
        {
            try
            {
                foreach (Client client in program.Allclients)
                {
                    if (client.ClientSocket == ClientSocket && client.ClientSocket != null)
                    {
                        client.buffer = Encoding.ASCII.GetBytes(data);
                        client.ClientSocket.BeginSend(client.buffer, 0, client.buffer.Length, SocketFlags.None, SendCallback, client.ClientSocket);
                        //client.buffer = new byte[ClientSocket.ReceiveBufferSize];
                    }
                }
            }
            catch (Exception ex)
            {
                // wait to connect from client
                ShowErrorDialog("cannot send data to client in socket null\n" + ex.Message+"\n"+ex);
            }
        }




        public static void ShowErrorDialog(string message)
        {
             MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Console.WriteLine(message);
        }
            

        // Create a method for a delegate.
        public static void startLiveMode(int id)
        {

            if (id < program.Allclients.Count)
            {

                Socket clientSocket = program.Allclients[id].ClientSocket;


                try
                {
         
                    //ShowErrorDialog("DelegateMethod play, id is: " + id + ", in Socket: " + clientSocket.RemoteEndPoint);
                    if (!CheckConnection(id,clientSocket))
                    {
                        ShowErrorDialog("Client Not connect");
                        s.addClientToCheckBoxLst(program.Allclients[id].Name, id, null);
                    }
                    else { 
                        program.sendDataToClient(program.Allclients[id].ClientSocket, "start live mode"); 
                    }

                }
                catch (Exception ex)
                {
                    ShowErrorDialog("The monitored computer has not yet connected");
                }


                //s.enabledbtnGetCurrentState(false);
            }

        }
        public static void stopCurrentState(int id)
        {
            if (id < program.Allclients.Count)
            {
                Socket clientSocket = program.Allclients[id].ClientSocket;
                program.sendDataToClient(program.Allclients[id].ClientSocket, "stop current state");
                //  ShowErrorDialog("in stopCurrentState: " + id);
                //s.enabledbtnGetCurrentState(true);
            }


        }
        public static void removeClient(int id)
        {
            for (int i = 0; i < program.Allclients.Count(); i++)
            {
                if (program.Allclients[i].id == id)
                {
                    
                    program.sendDataToClient(program.Allclients[i].ClientSocket, "remove client");
                    
                }
            }

            program.clientIds.Remove(id);
            s.removeClientFromCheckBoxLst(id);
            program.DBInstance = DBserver.Instance;
            program.DBInstance.removeClient(id.ToString());
            program.removeClientfromMemory(id.ToString());



        }
        public static bool CheckConnection(int id, Socket clientSocket) {

            if (internetConnection() && checkSocketConnection(clientSocket))
            {
                return true;
            }
                
            else
                return false;
        }
        public static bool checkSocketConnection(Socket s)
        {
            if (s == null)
            {
                return false;
            }
            try
            {
                program.sendDataToClient(s, "check connection");
            }
            catch (SocketException ex) {
                return false;
            }


            return true;
        }
       
        public static bool internetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
       
        public static void setSettingDeleGate(int id, string setting)
        {
           
            int index = 0;
            // Start receiving data from this client Socket.
            for (int i = 0; i < program.Allclients.Count; i++)
            {
                if (program.Allclients[i].id == id)
                {
                    // save all client data in DB
                    program.DBInstance = DBserver.Instance;
                    program.DBInstance.fillClientsTable(id, program.Allclients[i].Name, setting);
                    
                    //if (CheckConnection(id,program.Allclients[i].ClientSocket))
                    if(program.Allclients[i].ClientSocket != null)
                    {
                        program.sendDataToClient(program.Allclients[i].ClientSocket, "setting\r\n" + setting);
                        index = i;
                    }
                         
                  

                }

            }
            if(program.Allclients[index].ClientSocket!= null)
            {
                program.Allclients[index].ClientSocket.BeginReceive(program.Allclients[index].buffer, 0, program.Allclients[index].buffer.Length, SocketFlags.None, program.ReceiveCallback, program.Allclients[index].ClientSocket);

            }
            bool connect = checkSocketConnection(program.Allclients[index].ClientSocket);
            if (connect) {
                s.addClientToCheckBoxLst(program.Allclients[index].Name, program.Allclients[index].id, program.Allclients[index].ClientSocket);
                s.removeClientToWaitingList(id);
            }
           
        }


        private void removeClientfromMemory(string id)
        {
            for (int i = 0; i < Allclients.Count(); i++)
            {
                if (Allclients[i].id.ToString() == id)
                {
                    Allclients.Remove(Allclients[i]);
                }
            }
        }

        public static void ShowLastReportFromServer(int id)
        {
            for (int i = 0; i < program.Allclients.Count(); i++)
            {
                if (program.Allclients[i].id  == id)
                {
                    program.sendDataToClient(program.Allclients[i].ClientSocket, "send last report");
                    //program.Allclients[i].buffer = new byte[program.Allclients[i].ClientSocket.ReceiveBufferSize];
                }
            }
        }

        public static void checkClientConnection(int id)
        {
            for (int i = 0; i < program.Allclients.Count(); i++)
            {
                if (program.Allclients[i].id == id)
                {
                    if (!CheckConnection(id, program.Allclients[i].ClientSocket))
                    {

                        if (program.Allclients[i].ClientSocket != null)
                        {
                            s.setClientNotConnect(program.Allclients[i].ClientSocket.RemoteEndPoint);
                        }
                        else
                        {
                            s.addClientToCheckBoxLst(program.Allclients[i].Name, program.Allclients[i].id, null);
                        }

                    }
                    else
                    {
                        s.addClientToCheckBoxLst(program.Allclients[i].Name, id, program.Allclients[i].ClientSocket);

                    }
                }
            }
        }

    }
}
