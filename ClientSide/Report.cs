using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using System.Net.Mail;
using System.Net.Mime;
using System.IO.Compression;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using System;
using Microsoft.Win32.TaskScheduler;
using static System.Net.Mime.MediaTypeNames;

namespace ClientSide
{

    // 1. get frequency from server 
    // 2. difine time to create report 
    // 3. create report from DB
    // 4. send mail 

    class Report
    {
        private static bool send;
        private static string screenPic;
        private static string cameraPic;
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        static int alarmCounter = 1;
        static bool exitFlag = false;
        private static Timer _timer;
        private static DateTime timetoReport;
        private static int count = 1;

        public static void sendAlertToMail(String picName)
        {
            Debug.WriteLine("insert to sendAlertToMail");
            // get directory to pictures
            string projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
           
            // get Camera picture 
            string[] paths = new string[] { @filepath, "files", picName };
            cameraPic = Path.Combine(paths);
            
            // get screenshot picture 
            paths = new string[] { @filepath, "files","snapshot_" + picName };
            screenPic = Path.Combine(paths);
            // send mail only if the files exist
            
            bool exists = File.Exists(screenPic);
            
                 if (exists)
                 {
                     if (!File.Exists(cameraPic))
                     {
                         Thread.Sleep(9000);
                         if (!File.Exists(cameraPic))
                         {
                             send = false;
                             Debug.WriteLine("files not exist");
                         }
                         else
                         {
                             send = true;

                         }

                     }
                     else
                     {
                         send = true;
                     }
                    Debug.WriteLine("send is: "+send);
                    Thread sendMail = new Thread(SendMail);
                    sendMail.Start();
                 }
                 else {
                     Debug.WriteLine("notttttttttttttttttttt");
                 }
             
           


        }

        private static void SendMail()
        {
           
            if (send) {
                try
                {
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("bsafemonitoring@gmail.com", "Bsafe ", Encoding.UTF8);
                    mail.To.Add("sara05485@gmail.com");
                    mail.Subject = "This mail is Alert of browse in forbidden site ";
 

                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(cameraPic);
                    mail.Attachments.Add(attachment);
                    Debug.WriteLine("add camera pic");
                    
                    System.Net.Mail.Attachment attachment2;
                    attachment2 = new System.Net.Mail.Attachment(screenPic);
                    mail.Attachments.Add(attachment2);
                    
                    Debug.WriteLine("add screen pic");

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "atara1998");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(mail);
                    Debug.WriteLine("send email seccess");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("fail send mail: \n" +ex);
                }
            }
            send = false;
           
        }
       
        public void setReportFrequency(string reportTime, double frequency)
        {
            DateTime timeToReport = DateTime.Parse(reportTime);
            double tickTime = (double)(timeToReport - DateTime.Now).TotalSeconds;
            //Initialization of _timer   
            _timer = new Timer(x => { callTimerMethode(); }, null, TimeSpan.FromSeconds(tickTime), TimeSpan.FromSeconds(5));
             
        }

        private static void callTimerMethode()
        {
            Console.WriteLine(string.Format("Timer Executed {0}   times.", count));
            count = count + 1;
            // save new time to report

        }



    }
}