
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
        public string clientName = "";
        public string ip = "";
        private Socket clientSocket;
        private byte[] buffer;// Holds our connection with the database
        
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
                    ShowErrorDialog("Must fill Client Name!");
                }
                else
                {
                    clientName = txbName.Text;
                    ip = tbxIPAddress.Text;
                    this.Close();
                }

            }
            catch (SocketException ex)
            {
                ShowErrorDialog("buttonConnect_Click: " + ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("buttonConnect_Click: " + ex.Message);
            }
        }
    }  
}

