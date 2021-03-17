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
using VisioForge.MediaFramework.ONVIF;
using DateTime = System.DateTime;

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
         
        public static Setting settingClient;
        public static string mailAddress; 
        
        public DBclient DBInstance = DBclient.Instance;
        private Setting SettingInstance = Setting.Instance;
      
        public Report()
        {
           
             
        }

        public static void sendAlertToMail(string picName, string TriggerDescription, string triggerDetails, string trigger)
        {
            string[] args = { picName, TriggerDescription, triggerDetails, trigger };
            Thread alertTread = new Thread(playSendAlertThread);
            alertTread.Start(args);


        }
       
        private static void playSendAlertThread(object parameterObj)
        {
            string[] args = (string[])parameterObj;
            string picName = args[0];
            string TriggerDescription = args[1];
            string triggerDetails = args[2];
            string trigger = args[3];

            // get directory to pictures
            string projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            

            // get Camera picture
            string[] paths = new string[] { @filepath, "files", picName };
            cameraPic = Path.Combine(paths);


            // get screenshot picture
            paths = new string[] { @filepath, "files", "snapshot_" + picName };
            screenPic = Path.Combine(paths);

            Thread.Sleep(60000);

            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("bsafemonitoring@gmail.com", "Bsafe ", Encoding.UTF8);
                mail.To.Add("ataraelmal@gmail.com");
                mail.Subject = "Alert " + TriggerDescription;
               
                switch (trigger)
                {
                    case ("typing"):
                        mail.Body = "The user typing word: " + triggerDetails;
                        break;
                    case ("siteTrigger"):
                        mail.Body = "The user browse in site: " + triggerDetails;
                        break;
                    case ("Insta"):
                        mail.Body = "The user try install app " + triggerDetails;
                        break;
                }

                Attachment attachment;
                attachment = new Attachment(cameraPic);
                mail.Attachments.Add(attachment);

                Debug.WriteLine("add camera pic");

                Attachment attachment2;
                attachment2 = new Attachment(screenPic);//sara 
                mail.Attachments.Add(attachment2);

                Debug.WriteLine("add screen pic");

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "rcza voco ctyq ptal");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                mail.Attachments.Dispose();
                Thread.Sleep(120000);
                if (File.Exists(cameraPic))
                    File.Delete(cameraPic);
                if (File.Exists(screenPic))
                    File.Delete(screenPic);
                Debug.WriteLine("send email seccess");

            }

            catch (SmtpException ex)
            {
                ShowErrorDialog("fail send mailllll: \n" + ex);
                Thread alertThread = new Thread(playSendAlertThread);
                alertThread.Start(args);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("fail send mail: \n" + ex);
                ShowErrorDialog("fail send mail: \n" + ex);
            }






            Thread reportThread = new Thread(playSendReportThread);
            reportThread.Start();

        }

        public static void sendReportFileToMail()
        {

            Thread reportThread = new Thread(playSendReportThread);
            reportThread.Start();
          
        }

        public static void playSendReportThread()
        {
            Debug.WriteLine("insert to sendAlertToMail");
            // get directory to report
            string projectDirectory = Environment.CurrentDirectory;
            string reportPath = Directory.GetParent(projectDirectory).Parent.FullName;

            // get report file
            string[] paths = new string[] { @reportPath, "Report.pdf" };
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
                        mail.To.Add("ataraelmal@gmail.com");
                        mail.Subject = "Report File ";

                        Attachment attachment;
                        attachment = new Attachment(reportPath);
                        mail.Attachments.Add(attachment);

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "rcza voco ctyq ptal");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        // delete report and DB !!  
                        ShowErrorDialog("jjj");

                        removeTriggers();

                        if (File.Exists(reportPath))
                        {
                            try
                            {
                                File.Delete(reportPath);
                            }
                            catch (Exception ex)
                            {
                                //ShowErrorDialog("fail delete report adter send:\n"+ex);
                            }



                        }
                    }


                }
                catch (SmtpException ex)
                {
                    ShowErrorDialog("fail send mailllll: \n" + ex);
                    Thread reportThread = new Thread(playSendReportThread);
                    reportThread.Start();

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

        private static void removeTriggers()
        {
            DBclient DBInstance = DBclient.Instance;
            DBInstance.RemoveTriggersTable();
        }
        public static void setReportFrequency()
        {
            Setting SettingInstance = Setting.Instance;
            frequencySecond = SettingInstance.reportFrequencyInSecond;
            frequencyWord = SettingInstance.reportFrequencyInWord; // dayly or weekly..
            //createReportFile(db);
            DateTime timeToReport = DateTime.Parse(SettingInstance.futureDateToReport);
            double tickTime = (double)(timeToReport - DateTime.Now).TotalSeconds;
            //ShowErrorDialog(timeToReport.ToString());
            //ShowErrorDialog(""+tickTime);

            // Initialization of _timer  
            //_timer = new Timer(x => { createReportFile(db); }, null, TimeSpan.FromSeconds(tickTime), TimeSpan.FromSeconds(frequencySecond));
            _timer = new Timer(x => { createReportFile(); }, null, TimeSpan.FromSeconds(120), TimeSpan.FromSeconds(120));

    //sara atara 


        }

        private static void createReportFile()
        {
            var Report = new Document();
            DBclient DBInstance = DBclient.Instance;
            string projectDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(projectDirectory).Parent.FullName;

            PdfWriter.GetInstance(Report, new FileStream(path + "/Report.pdf", FileMode.Create));
            //PdfWriter.GetInstance(Report, new FileStream(path + "/logo.JPG", FileMode.Create));

            Report.Open();
            Image jpg = Image.GetInstance(path + "/logo.JPG");
            jpg.ScalePercent(12f);
            jpg.SetAbsolutePosition(Report.PageSize.Width - 410f,
                  Report.PageSize.Height - 130f);

            Report.Add(jpg);
            Report.Add(new Paragraph(DateTime.Now.ToString()));
            string userName = Environment.UserName;
            Report.Add(new Paragraph("\n\n\n\n\n" + frequencyWord + " report for user: " + userName));
            Report.Add(new Paragraph("\nOn the dates listed, the following words were typed:"));
            Report.Add(new Paragraph(DBInstance.getTriggerById(1)));
            Report.Add(new Paragraph("\nOn the dates listed the user browsed the following sites:"));
            Report.Add(new Paragraph(DBInstance.getTriggerById(2)));
            Report.Add(new Paragraph("\nOn the dates listed The user has downloaded the following software:"));
            Report.Add(new Paragraph(DBInstance.getTriggerById(3)));
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
