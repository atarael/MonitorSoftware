using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
namespace Client
{
    public partial class ClientForm : Form
    {
        Socket ClientSocket;
        ListBox chat;
        public ClientForm()
        {
            InitializeComponent();
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // convert msg to byte[]
            ASCIIEncoding aEncoding = new ASCIIEncoding();
            byte[] sendMsg = new byte[1500];
            sendMsg = aEncoding.GetBytes(txbMsg.Text);
            // send msg
            ClientSocket.Send(sendMsg);
            // add msg to list box
            chat.Items.Add("ME: "+ txbMsg.Text);
            txbMsg.Text = "";
            

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            btnSendMsg.Enabled = true;
            String Ipadress = txbIP.Text;
            String clientName = txbClientName.Text;
            
            try
            {

                int port = 13000;
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
              
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse(Ipadress), port);
                ClientSocket.Connect(ep);
                txbChat.Text = clientName + " connected!";
                txbIP.Text = "sara";
                //Console.WriteLine();
                while (true)
                {

                    string messageFromClient = null;
                    Console.WriteLine("Enter the message");
                    messageFromClient = Console.ReadLine();
                    ClientSocket.Send(System.Text.Encoding.ASCII.GetBytes(messageFromClient), 0, messageFromClient.Length, SocketFlags.None);
                    byte[] MsgFromServer = new byte[1024];
                    int size = ClientSocket.Receive(MsgFromServer);
                    Console.WriteLine("Server: " + System.Text.Encoding.ASCII.GetString(MsgFromServer, 0, size));

                }
            }
            catch (SocketException ex)
            {
                //ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
               // ShowErrorDialog(ex.Message);
            }
        }
    }
}
