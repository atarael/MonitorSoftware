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
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace ClientSide
{

    // 1. get frequency from server 
    // 2. difine time to create report 
    // 3. create report from DB
    // 4. send mail 

    class Report
    {
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
       
        public static void sendAlertToMail(string picName, string TriggerDescription)
        {
            
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
            
            try{
                Thread.Sleep(6000);
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("bsafemonitoring@gmail.com", "Bsafe ", Encoding.UTF8);
                mail.To.Add("sara05485@gmail.com");
                mail.Subject = "Alert " + TriggerDescription;


                Attachment attachment;
                attachment = new Attachment(cameraPic);
                mail.Attachments.Add(attachment);
                Debug.WriteLine("add camera pic");

                Attachment attachment2;
                attachment2 = new Attachment(screenPic);
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
                Debug.WriteLine("fail send mail: \n" + ex);
                ShowErrorDialog("fail send mail: \n" + ex);
            }

            
           
             
           


        }
       
        public static void sendReportFileToMail()
        {
            Debug.WriteLine("insert to sendAlertToMail");
            // get directory to report
            string projectDirectory = Environment.CurrentDirectory;
            string reportPath = Directory.GetParent(projectDirectory).Parent.FullName;
               
            // get report file
            string[] paths = new string[] { @reportPath,  "Report.pdf"};
            reportPath = Path.Combine(paths);


            bool exists = File.Exists(reportPath);

            if (exists)
            {
                try
                {
                    using (MailMessage mail = new MailMessage())
                    using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
                    {

                        mail.From = new MailAddress("bsafemonitoring@gmail.com", "Bsafe ", Encoding.UTF8);
                        mail.To.Add("sara05485@gmail.com");
                        mail.Subject = "Report File ";

                        Attachment attachment;
                        attachment = new Attachment(reportPath);
                        mail.Attachments.Add(attachment);

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "atara1998");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        Debug.WriteLine("send email seccess");

                        if (File.Exists(reportPath))
                        {
                            try {
                                File.Delete(reportPath);
                            }
                            catch (Exception ex) {
                                ShowErrorDialog("fail delete report adter send:\n"+ex);
                            }

                       

                        }
                }

                   

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("fail send mail: \n" + ex);
                    ShowErrorDialog("fail send Report to mail: \n" + ex);
                }
            }
            else
            {
                Debug.WriteLine("report file not exist");
            }




        }
 
        public static void setReportFrequency(string reportTime, double frequencySecond1, string frequencyWord1 , DBclient db)
        {
            frequencySecond = frequencySecond1;
            frequencyWord = frequencyWord1; // dayly or weekly.. 
            //createReportFile(db);
            DateTime timeToReport = DateTime.Parse(reportTime);
            double tickTime = (double)(timeToReport - DateTime.Now).TotalSeconds;
            //ShowErrorDialog(timeToReport.ToString());
            //ShowErrorDialog(""+tickTime);

            // Initialization of _timer   
            //_timer = new Timer(x => { createReportFile(db); }, null, TimeSpan.FromSeconds(tickTime), TimeSpan.FromSeconds(frequencySecond));
            _timer = new Timer(x => { createReportFile(db); }, null, TimeSpan.FromSeconds(120), TimeSpan.FromSeconds(120));




        }

        private static void createReportFile(DBclient db) 
        {
            var Report = new Document();

            String projectDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(projectDirectory).Parent.FullName;
            
            PdfWriter.GetInstance(Report, new FileStream(path + "/Report.pdf", FileMode.Create));
            //PdfWriter.GetInstance(Report, new FileStream(path + "/logo.JPG", FileMode.Create));

            Report.Open();
            Image jpg = Image.GetInstance(path + "/logo.JPG");
            jpg.ScalePercent(12f);
            jpg.SetAbsolutePosition(Report.PageSize.Width - 410f,
                  Report.PageSize.Height - 130f );

            Report.Add(jpg);
            Report.Add(new Paragraph(DateTime.Now.ToString()));
            string userName = Environment.UserName;
            Report.Add(new Paragraph("\n\n\n\n\n"+frequencyWord + " report for user: "+userName));
            Report.Add(new Paragraph("\nOn the dates listed, the following words were typed:"));
            Report.Add(new Paragraph(db.getTriggerById(1)));
            Report.Add(new Paragraph("\nOn the dates listed the user browsed the following sites:"));
            Report.Add(new Paragraph(db.getTriggerById(2)));
            Report.Add(new Paragraph("\nOn the dates listed The user has downloaded the following software:"));
            Report.Add(new Paragraph(db.getTriggerById(3)));
            Report.Close();

            ShowErrorDialog("report created, send mail");
            sendReportFileToMail();

            //db.printClientData();
          
                
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

 
    }
}