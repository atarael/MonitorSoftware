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
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using GemBox.Document;
using PdfSharp.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Paragraph = iTextSharp.text.Paragraph;
using Image = iTextSharp.text.Image;
using Rectangle = iTextSharp.text.Rectangle;
using Color = iTextSharp.text.Color;
using Element = iTextSharp.text.Element;
using Spire.Pdf.Exporting.XPS.Schema;
using Path = System.IO.Path;
using Font = iTextSharp.text.Font;
using BaseLib.Graphic;
using System.Runtime.CompilerServices;

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
        public static double frequencySecond;
        public static string frequencyWord;
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
       
        public  static void setReportFrequency(string reportTime, double frequencySecond1, string frequencyWord1 , DBclient db)
        {
            frequencySecond = frequencySecond1;
            frequencyWord = frequencyWord1;
            createReportFile(db);
           // DateTime timeToReport = DateTime.Parse(reportTime);
           // double tickTime = (double)(timeToReport - DateTime.Now).TotalSeconds;
            //Initialization of _timer   
            //_timer = new Timer(x => { createReportFile(); }, null, TimeSpan.FromSeconds(tickTime), TimeSpan.FromSeconds(5));
             
        }

        private static void createReportFile(DBclient db) 
        {
            var doc1 = new Document();

            String projectDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(projectDirectory).Parent.FullName;
            
            PdfWriter.GetInstance(doc1, new FileStream(path + "/Doc1.pdf", FileMode.Create));
            //PdfWriter.GetInstance(doc1, new FileStream(path + "/logo.JPG", FileMode.Create));

            doc1.Open();
            Image jpg = Image.GetInstance(path + "/logo.JPG");
            jpg.ScalePercent(12f);
            jpg.SetAbsolutePosition(doc1.PageSize.Width - 410f,
                  doc1.PageSize.Height - 130f );

            doc1.Add(jpg);
            doc1.Add(new Paragraph(DateTime.Now.ToString()));
            string userName = Environment.UserName;
            doc1.Add(new Paragraph("\n\n\n\n\n"+frequencyWord + " report for user: "+userName));
            doc1.Add(new Paragraph("\nOn the dates listed, the following words were typed:"));
            doc1.Add(new Paragraph(db.getTriggerById(1)));
            doc1.Add(new Paragraph("\nOn the dates listed the user browsed the following sites:"));
            doc1.Add(new Paragraph(db.getTriggerById(2)));
            doc1.Add(new Paragraph("\nOn the dates listed The user has downloaded the following software:"));
            doc1.Add(new Paragraph(db.getTriggerById(3)));
            doc1.Close();


            //db.printClientData();

        }

     /*   private static string editTriggerById(int id, DBclient db)
        {
            string editTableTrigger = db.getTriggerById(id);
            return editTableTrigger;
        }*/
    }
}