using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BasicAsyncServer
{
    class Client
    {
        public string Name {
            get;
            set;
        }
        public int id {
            get ;
            set;
        }
        public Socket ClientSocket {
            get ;
            set;
        }

        public Client(String name, int id, Socket clientSocket)
        {
            Name = name;
            this.id = id;
            ClientSocket = clientSocket;
        }



    }
}
