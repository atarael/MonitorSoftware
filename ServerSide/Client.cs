using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    class Client
    {
        public String Name
        {
            get;
            set;
        }
        public int id
        {
            get;
            set;
        }
        public Socket ClientSocket
        {
            get;
            set;
        }
        public byte[] buffer
        {
            get;
            set;
        }
        public MonitorSystem monitorSystem;

        public Client(String name, int id, Socket clientSocket, byte[] buffer)
        {
            Name = name;
            this.id = id;
            ClientSocket = clientSocket;
            this.buffer = buffer;
        }





    }
}
