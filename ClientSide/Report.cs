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
using System.Runtime.Remoting.Messaging;

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
        private static Timer _timer;
         
        public static double frequencySecond;
        public static string frequencyWord;

        public static Setting settingClient;
        public static string mailAddress;

        public DBclient DBInstance = DBclient.Instance;
        private static string stringReport;
        private static bool lastReport;

        public Report()
        {


        }

        public static void sendAlertToMail(string picName, string TriggerDescription, string triggerDetails, string triggerName)
        {
            DBclient DBInstance = DBclient.Instance;
            string date = DateTime.Now.ToString();
            // if no internet - save imadiate alert in DB 
            DBInstance.fillReportImmediateTable(triggerName, TriggerDescription, triggerDetails, date);

          
            string[] args = { picName, TriggerDescription, triggerDetails, triggerName };
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
                Setting settingInstance = Setting.Instance;
                mail.To.Add(settingInstance.email);
                mail.Subject = "Alert " + TriggerDescription;

                switch (trigger)
                {
                    case ("typing"):
                        mail.Body = "The user typing word: " + triggerDetails;
                        break;
                    case ("Site"):
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
                Thread.Sleep(5000);
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
                        Setting settingInstance = Setting.Instance;
                        mail.To.Add(settingInstance.email);
                        if (lastReport)
                        {
                            mail.Subject = "Last Report File ";
                        }
                        else
                        {
                            mail.Subject = "Report File ";
                        }
                        

                        Attachment attachment;
                        attachment = new Attachment(reportPath);
                        mail.Attachments.Add(attachment);

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "rcza voco ctyq ptal");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        if(lastReport)
                        { 
                            // delete fiels folder
                            // this folder contain files such setting file and pichtures to report
                            string userName = Environment.UserName;
                            string fielsfolderDirectory = Environment.CurrentDirectory;
                            fielsfolderDirectory = Directory.GetParent(fielsfolderDirectory).Parent.FullName;
                            fielsfolderDirectory = Path.Combine(fielsfolderDirectory, "files");

                            if (Directory.Exists(fielsfolderDirectory))
                            {
                                try
                                {
                                    Directory.Delete(fielsfolderDirectory, true);
                                }
                                catch (IOException ex)
                                {
                                    ShowErrorDialog("cannot delete files folder\n" + ex);
                                }
                            }
                        }
                        else
                        {
                            // delete report and DB !!  
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

        public static void sendImadiateAlertsToMail(string missingAlerts)
        {

            Thread imadiateAlerts = new Thread(playSendImadiateAlertsThread);
            imadiateAlerts.Start(missingAlerts);

        }

        public static void playSendImadiateAlertsThread(object parameterObj)
        {

            string missingAlerts = (string)parameterObj;
            using (MailMessage mail = new MailMessage())
            using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
            {

                mail.From = new MailAddress("bsafemonitoring@gmail.com", "Bsafe ", Encoding.UTF8);
                Setting settingInstance = Setting.Instance;
                mail.To.Add(settingInstance.email);
                mail.Subject = "Alerts collected from offline mode ";
                mail.Body= missingAlerts;                 
                       
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "rcza voco ctyq ptal");
                SmtpServer.EnableSsl = true;
                try
                {
                    SmtpServer.Send(mail);
                    DBclient dbInstance = DBclient.Instance;
                    dbInstance.RemoveReportImmediateTable();
                }
                catch(Exception ex ) {
                    ShowErrorDialog("fail send missing alerts\n"+ex);
                } 
               
               // ShowErrorDialog("send missingAlerts\n"+ missingAlerts);       
               
            }


               

        }

        public static void createLastReport()
        {
            lastReport = true;
            _timer.Dispose();
            createReportFile(); // send last report

         
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
            if (SettingInstance.reportFrequencyInSecond>0) {
                tickTime = SettingInstance.reportFrequencyInSecond;
            }
            //ShowErrorDialog(timeToReport.ToString());
            //ShowErrorDialog(""+tickTime);

            // Initialization of _timer  
            _timer = new Timer(x => { createReportFile(); }, null, TimeSpan.FromSeconds(tickTime), TimeSpan.FromSeconds(frequencySecond));
            //_timer = new Timer(x => { createReportFile(); }, null, TimeSpan.FromSeconds(600), TimeSpan.FromSeconds(1200));

             

        }

        public static string getReportString()
        {
            string userName = Environment.UserName;
            string projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            //String[] paths = new string[] { @filepath, "files" };
            //filepath = Path.Combine(paths);
            string settingString = "";
            DirectoryInfo d = new DirectoryInfo(filepath);//Assuming Test is your Folder
            //ShowErrorDialog("filepath is: \n" + filepath);
            if (!Directory.Exists(filepath))
            {
                //return;
            }
            string reportStringPath = Path.Combine(filepath, "lastReport.txt");
            if (File.Exists(reportStringPath))
            {
                using (StreamReader sr = System.IO.File.OpenText(reportStringPath))
                {
               
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    settingString += line + "\r\n";
                }
            }
 

            }
            else
            {
                return string.Empty;
            }
            
            return settingString;
        }

        private static void createReportFile()
        {
            //ShowErrorDialog(getReportString());
            var Report = new Document();
            DBclient DBInstance = DBclient.Instance;
            string projectDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(projectDirectory).Parent.FullName;
            string userName = Environment.UserName;

            PdfWriter.GetInstance(Report, new FileStream(path + "/Report.pdf", FileMode.Create));
            //PdfWriter.GetInstance(Report, new FileStream(path + "/logo.JPG", FileMode.Create));

            Report.Open();
            Image jpg = Image.GetInstance(path + "/logo.JPG");
            jpg.ScalePercent(12f);
            jpg.SetAbsolutePosition(Report.PageSize.Width - 410f,
                  Report.PageSize.Height - 130f);

            Report.Add(jpg);
            
            Report.Add(new Paragraph(DateTime.Now.ToString()));
            stringReport += DateTime.Now.ToString() + "\n";
                         
            Report.Add(new Paragraph("\n\n\n\n\n" + frequencyWord + " report for user: " + userName));
            stringReport += frequencyWord + " report for user: " + userName + "\n";
            
            // add bad word trigger
            string wordsTrigger = DBInstance.getTriggerById(1);
            if (wordsTrigger.Length > 0)
            {
                Report.Add(new Paragraph("\nOn the dates listed, the following words were typed:"));
                Report.Add(new Paragraph(wordsTrigger));
                stringReport += "On the dates listed, the following words were typed:\n" + wordsTrigger + "\n";                
            }
            else {
                Report.Add(new Paragraph("\nNO TRIGGER KIND BAD WORDS TO REPORT!"));
                stringReport += "NO TRIGGER KIND BAD WORDS TO REPORT!\n";
            }
            
            // add site trigger
            string siteTrigger = DBInstance.getTriggerById(2);
            if (siteTrigger.Length > 0)
            {
                Report.Add(new Paragraph("\nOn the dates listed the user browsed the following sites:"));
                Report.Add(new Paragraph(siteTrigger));
                stringReport += "On the dates listed the user browsed the following sites:\n" + siteTrigger + "\n";
            }
            else
            {
                Report.Add(new Paragraph("\nNO TRIGGER KIND SITES TO REPORT!"));
                stringReport += "NO TRIGGER KIND SITES TO REPORT!\n";
            }

            // installations trigger
            string installationsTrigger = DBInstance.getTriggerById(3);
            if (installationsTrigger.Length > 0)
            {
                Report.Add(new Paragraph("\nOn the dates listed The user has downloaded the following software:"));
                Report.Add(new Paragraph(installationsTrigger));
                stringReport += "On the dates listed The user has downloaded the following software:" + installationsTrigger + "\n";
            }
            else
            {
                Report.Add(new Paragraph("\nNO TRIGGER KIND INSTALLATIONS TO REPORT!"));
                stringReport += "NO TRIGGER KIND INSTALLATIONS TO REPORT!\n";
            }
                     
            Report.Close();

            ShowErrorDialog("report created, send mail");
            sendReportFileToMail();

            //db.printClientData();  
            string pathLastReport = Path.Combine(path, "lastReport.txt");
            if (System.IO.File.Exists(pathLastReport))
            {
                System.IO.File.Delete(pathLastReport);

            }
            try
            {
                using (StreamWriter sw = System.IO.File.CreateText(pathLastReport)) ;
                System.IO.File.WriteAllText(pathLastReport, stringReport);

            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex + "hh");
            }
            ShowErrorDialog(stringReport);
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

       



    }
}
