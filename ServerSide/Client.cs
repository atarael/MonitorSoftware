using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public delegate void getID();
    
    public class Client
    {
        public string Name
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
        static private int ID;
        
        CurrentState currentStateForm;
        private bool isOpen;
        public string lastReportByParts = "";
        public string procceccByParts = "";


        public Client(string name, int id, Socket clientSocket, byte[] buffer)
        {
            Name = name;
            this.id = id;
            ClientSocket = clientSocket;
            this.buffer = buffer;
            currentStateForm = new CurrentState(id,name);
            //currentStateForm.Text = "Current State from client: " + Name + ", ID:" + id;
            ID = this.id;
             
        }

        public void openCurrentStateForm(string data) {
            try
            {
                // ShowErrorDialog("client send  live:\n  |" + data + "|");
                if (data.Split('\0')[0] == "open CurrentState form")
                {
                    isOpen = true;
                    // currentStateForm = new CurrentState();
                    // currentStateForm.Text = "Current State from client: " + Name + ", ID:" + id;
                    Thread liveForm = new Thread(openLiveForm);
                    liveForm.Start();

                }
                else
                {
                    FormCollection fc = Application.OpenForms;

                    foreach (Form frm in fc)
                    {
                        //iterate through
                        if (frm.Name == "CurrentState")
                        {
                            isOpen = true;
                            currentStateForm.addText(data);
                        }
                    }
                    if (!isOpen)
                    {
                        currentStateForm.ShowDialog();

                        //sara ayash atara 
                    }
                }


            }
            catch (Exception ex) {
                Console.WriteLine("openCurrentStateForm fail: \n"+ex.Message);
                return;
            }




        }

        private void openLiveForm()
        {
            currentStateForm.ShowDialog();
            isOpen = false;

        }

        public static void sendID()
        {
            stopCurrentState handler = Program.stopCurrentState;
             
            handler(ID);
             
        }
        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



    }
}
