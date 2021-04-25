 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerSide
{

    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Size of receive buffer.  
        public const int BufferSize = 1024;

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.
        public StringBuilder sb = new StringBuilder();

        // Client socket.
        public Socket workSocket = null;
    }

    public class AsynchronousSocketListener
    {
         
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private static Socket listener;
        private static List<Client> Allclients;
          
        public AsynchronousSocketListener()
        { 
        }
        
        // this function start listenin for all connections requests
        public static void StartListening()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer                
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            listener.Bind(new IPEndPoint(IPAddress.Any, 3333));
            listener.Listen(10);
            listener.BeginAccept(
                       new AsyncCallback(AcceptCallback),
                       listener);
           
            DBserver DBInstance = DBserver.Instance;
            Allclients = DBInstance.initialServer();
            
            foreach (Client client in Allclients)
            {
                 addExistClient dg = Program.addExistClientToInterface;
                dg(client.id, client.Name,  null);
 

            }
            Thread interntAvilable = new Thread(checkConnectionThread);
            interntAvilable.Start();

            try
            {
                

                while (true)
                {
                    // Set the event to nonsignaled state.  
                     allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        private static void checkConnectionThread()
        {
            while (true)
            {   
                checkAllClientConnection();
                Thread.Sleep(3000);
            }
            
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            try
            {
                string content = String.Empty;

                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;
                
                int bytesRead =0 ;

                // Read data from the client socket.
                try
                {
                    bytesRead = handler.EndReceive(ar);
                }
                catch(SocketException ex) {
                    Console.WriteLine("Fail EndReceive");
                }

                if (bytesRead > 0)
                {
                    state.sb = new StringBuilder();
                    
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.UTF8.GetString(
                        state.buffer, 0, bytesRead));

                    state.buffer = new byte[StateObject.BufferSize];

                    content = state.sb.ToString();

                    // string dataFromClient = content.Split('\0')[0];
                    string dataFromClient = Crypto.Decrypt(content);

                    string decryptionSubject = dataFromClient.Split(new char[] { '\r' }, 2)[0];
                    string decryptionMessage = dataFromClient.Split(new char[] { '\r' }, 2).Last();
                    Console.WriteLine("ServerForm GET: |"+ dataFromClient + "|\n|" + content + "|\n|" + content.Length);
                    // client send name at first time 
                    if (decryptionSubject == "name")
                    {
                        Console.WriteLine("New client name: "+ decryptionMessage );
                        int newId = createNewId();


                        Client newClient = new Client(decryptionMessage, newId, handler, null);
                        Allclients.Add(newClient);
                       
                        Send(handler, "id\r" + newId);
                        addNewClient dg = Program.addNewClientToInterface;
                        dg(newId, decryptionMessage);

                    }

                    // client send id to connect or reconnect
                    if (decryptionSubject == "id")
                    {
                        int CID;
                        try {

                            CID = Int32.Parse(decryptionMessage);

                        }
                        catch {
                            Console.WriteLine("fail convert id: "+ decryptionMessage);
                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                            new AsyncCallback(ReadCallback), state);
                            return;
                        }

                        Console.WriteLine(CID + " try reconnect");

                        DBserver DBInstance = DBserver.Instance;
                        if (Allclients == null)
                        {

                            Allclients = DBInstance.initialServer();

                        }
                        if (CID < Allclients.Count)
                        {
                            Allclients[CID].ClientSocket = handler;

                            clientReconnect dg = Program.reConnectSocket;
                            dg(Allclients[CID].Name, handler, decryptionMessage);

                        }

                    }

                    if (decryptionSubject == "current state")
                    {
                        foreach (Client client in Allclients)
                        {
                            if (client.ClientSocket == handler)
                            {
                                Console.WriteLine("Live mode for client : " + client.Name);
                                client.openCurrentStateForm(decryptionMessage);
                            }
                        }
                        // the function open form to disply data from client 

                        //liveData dg = Program.showLiveData;
                        //dg(handler, decryptionMessage);

                    }

                    if (decryptionSubject == "last report")
                    {
                        recieveLastRepoert(handler, decryptionMessage);
                        
                    }

                    if (decryptionSubject == "parts")
                    {

                        string[] partData = decryptionMessage.Split(new char[] { '\r' }, 4);
                        if (partData.Length>=3) {
                            string id = partData[0];
                            string status = partData[1]; 
                            string subject = partData[2];
                            string massege = partData[3];
                            massege = massege.Remove(massege.Length - 1);

                            foreach (Client client in Allclients)
                            {
                                if(client.id.ToString() == id)
                                {
                                    
                                         
                                    switch (status)
                                    {

                                        case "first":
                                            if (subject == "last report")
                                                client.lastReportByParts = massege;
                                            else if (subject == "current state")
                                                client.procceccByParts = massege;
                                            break;

                                        case "continue":
                                            if (subject == "last report")
                                                client.lastReportByParts += massege;
                                            else if (subject == "current state")
                                                client.procceccByParts += massege;
                                            break;

                                        case "final":
                                            if (subject == "last report")
                                            {
                                                client.lastReportByParts += massege;
                                                recieveLastRepoert(handler, client.lastReportByParts);

                                            }
                                            else if (subject == "current state")
                                            {
                                                client.procceccByParts += massege;
                                                client.openCurrentStateForm(client.procceccByParts);

                                            }                                                
                                            break;

                                           
                                             

                                    }

                                     
                                    


                                }
                            }
                          

                        }
                        

                         

                    }


                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
               
                }
                  
            }
            catch (SocketException ex)
            {
                 
                ShowErrorDialog("ReadCallback SocketException\n" + ex.Message + "\n" + ex);
                checkAllClientConnection();


            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog("ReceiveCallback ObjectDisposedException" + ex.Message + "\n" + ex);
            }
        }

        private static void recieveLastRepoert(Socket handler, string data)
        {
           // ShowErrorDialog("show last report");
            foreach (Client client in Allclients)
            {
                if (client.ClientSocket == handler)
                {
                    Console.WriteLine("last report for client : " + client.Name);
                    if (data != string.Empty)
                    {
                        Report.createReportPDFFile(data, client.id);
                    }
                    else
                    {
                        ShowErrorDialog("No last report to show");
                    }
                }
            }
        }

        public static void checkAllClientConnection( )
        {
            // check internet connection
            if (internetConnection())
            {
                foreach (Client client in Allclients)
            {
               
                    if (!CheckSocketConnection(client.ClientSocket))
                    {
                        // client not connect
                        clientNotConnect dg = Program.setClientNotConnnect;
                        dg(client.id);
                        
                    }
                    else
                    {
                        addExistClient dg = Program.addExistClientToInterface;
                        dg(client.id, client.Name, client.ClientSocket);

                    }


                }
            }
            else
            {
                noInternet dg = Program.NoInternetConnection;
                dg();

            }
        }
       
        public static bool CheckSocketConnection(Socket s)
        {
            if (s == null)
            {
                return false;
            }
            try
            {
                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.ASCII.GetBytes("server check connection");

                // Begin sending the data to the remote device.  
                s.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), s);
                return true;
            }
            
            catch (SocketException ex)
            {
                Console.WriteLine("client not connect");
                return false;
            }


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

        private static int createNewId()
        {

            int newId = 0;
            List<int> ids = new List<int>();
            
            foreach (Client client in Allclients)
            {
                ids.Add(client.id);
            }
            
            while (true)
            {
            if (ids.Contains(newId))
                newId++;
            else
                return newId;
            }




        }

        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //Console.WriteLine(message);
        }

        private static void Send(Socket handler, String data)
        {
            try {
                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = Encoding.ASCII.GetBytes(data);

                // Begin sending the data to the remote device.  
                handler.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, new AsyncCallback(SendCallback), handler);
            }
            catch (SocketException ex) {
                Console.WriteLine("Fail send data to client\ndata is: "+data);
                
                 

            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                allDone.Set();
                 

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void SendDataToClient( int id, string subject) {
            foreach(Client client in Allclients)
            {
                if(client.id == id)
                { 
                    Send(client.ClientSocket, subject);

                }
            }
        }

        public static void removeClient(int id)
        {
            SendDataToClient(id, "remove client");
            foreach(Client client in Allclients)
            {
                if (client.id == id)
                {
                    Allclients.Remove(client);
                    return;
                }
            }

        }
     
        public static void setSetting(int id, string setting)
        {
            foreach (Client client in Allclients)
            {
                if (client.id == id)
                {
                    // save all client data in DB
                    DBserver DBInstance = DBserver.Instance;
                    DBInstance.fillClientsTable(id, client.Name, setting);

                    //if (CheckConnection(id,program.Allclients[i].ClientSocket))
                    if (client.ClientSocket != null)
                    {
                        Send(client.ClientSocket, "setting\r\n" + setting);
                        removeFromWaitingAdAdd dg = Program.moveNewClientToInterface;
                        dg(client.id, client.Name, client.ClientSocket);
                    }

                                  
                }

            }
        }
      
        public static bool checkAllSocketConnection(Socket s)
        {
            if (s == null)
            {
                return false;
            }
             
                return CheckSocketConnection(s);
              
        }
    }
}