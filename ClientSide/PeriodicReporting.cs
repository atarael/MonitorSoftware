using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using DateTime = System.DateTime;
using Image = iTextSharp.text.Image;
using Paragraph = iTextSharp.text.Paragraph;
using Path = System.IO.Path;
using Timer = System.Threading.Timer;
namespace ClientSide
{
    class PeriodicReporting
    {

        private static Timer periodicTimer;
        private static Timer dailyTimer;
        public static double frequencySecond;
        public static string frequencyWord;

        public DBclient DBInstance = DBclient.Instance;
        private static string stringReport;
        private static bool lastReport;

        public PeriodicReporting()
        {


        }



        public static void sendReportFileToMail(string subject, string fileName)
        {
            string[] args = { subject, fileName };
             
            Thread reportThread = new Thread(playSendReportThread);
            reportThread.Start(args);

        }

        public static void playSendReportThread(object parameterObj)
        {
            string[] args = (string[])parameterObj;
            if (args != null) {
                string subject = args[0];
                string fileName = args[1];

                // get directory to report
                string projectDirectory = Environment.CurrentDirectory;
                string reportPath = Directory.GetParent(projectDirectory).Parent.FullName;

                // get report file
                string[] paths = new string[] { @reportPath, fileName };
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
                            mail.Subject = subject;

                            Attachment attachment;
                            attachment = new Attachment(reportPath);
                            mail.Attachments.Add(attachment);

                            SmtpServer.Port = 587;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "rcza voco ctyq ptal");
                            SmtpServer.EnableSsl = true;

                            SmtpServer.Send(mail);
                            if (lastReport)
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
         

        }

        public static void createLastReport()
        {
            lastReport = true;
            periodicTimer.Dispose();
            createReportFile(); // send last report


        }

        private static void removeTriggers()
        {
            DBclient DBInstance = DBclient.Instance;
            DBInstance.RemoveTriggersTable();
        }

        public static string getReportString()
        {
            DBclient DBInstance = DBclient.Instance;
            string settingString = DBInstance.getGeneralDetailsTable("lastReport");
            //string userName = Environment.UserName;
            //string projectDirectory = Environment.CurrentDirectory;
            //string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            ////String[] paths = new string[] { @filepath, "files" };
            ////filepath = Path.Combine(paths);
            //string settingString = "";
            //DirectoryInfo d = new DirectoryInfo(filepath);//Assuming Test is your Folder
            ////ShowErrorDialog("filepath is: \n" + filepath);
            //if (!Directory.Exists(filepath))
            //{
            //    //return;
            //}
            //string reportStringPath = Path.Combine(filepath, "lastReport.txt");
            //if (File.Exists(reportStringPath))
            //{
            //    using (StreamReader sr = System.IO.File.OpenText(reportStringPath))
            //    {

            //        string line = "";
            //        while ((line = sr.ReadLine()) != null)
            //        {
            //            settingString += line + "\r\n";
            //        }
            //    }


            //}
            //else
            //{
            //    return string.Empty;
            //}

            return settingString;
        }

        public static void setReportPeriodic()
        {
            Setting SettingInstance = Setting.Instance;
            frequencySecond = SettingInstance.reportFrequencyInSecond;
            frequencyWord = SettingInstance.reportFrequencyInWord; // dayly or weekly..
            //createReportFile(db);
            DateTime timeToReport = DateTime.Parse(SettingInstance.futureDateToReport);
            double tickTime = (double)(timeToReport - DateTime.Now).TotalSeconds;
            if (SettingInstance.reportFrequencyInSecond > 0)
            {
                tickTime = SettingInstance.reportFrequencyInSecond;
            }
           
            // Initialization of periodicTimer  
            periodicTimer = new Timer(x => { createReportFile(); }, null, TimeSpan.FromSeconds(tickTime), TimeSpan.FromSeconds(frequencySecond));
            //periodicTimer = new Timer(x => { createReportFile(); }, null, TimeSpan.FromSeconds(600), TimeSpan.FromSeconds(1200));



        }

        private static void createReportFile()
        {
            stringReport = "";
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
            else
            {
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
            sendReportFileToMail("Report File", "Report.pdf");

            //db.printClientData();  
            string pathLastReport = Path.Combine(path, "lastReport.txt");
            DBInstance.fillGeneralDetailsTable("lastReport", stringReport);
            //    {
            //    using (StreamWriter sw = System.IO.File.CreateText(pathLastReport)) ;
            //    System.IO.File.WriteAllText(pathLastReport, stringReport);

            //}
            //    catch (Exception ex)
            //{
            //    ShowErrorDialog(ex + "hh");
            //}


            //if (System.IO.File.Exists(pathLastReport))
            //{
            //    System.IO.File.Delete(pathLastReport);
                
            //}
            //try
            //{
            //    using (StreamWriter sw = System.IO.File.CreateText(pathLastReport)) ;
            //    System.IO.File.WriteAllText(pathLastReport, stringReport);

            //}
            //catch (Exception ex)
            //{
            //    ShowErrorDialog(ex + "hh");
            //}
            //ShowErrorDialog(stringReport);
        }

        public static void ShowErrorDialog(string message)
        {
            // MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Console.WriteLine(message);
        }

        public static void sendMissingReportsToMail(string missingAlerts)
        {

            Thread imadiateAlerts = new Thread(playSendMissingReportsThread);
            imadiateAlerts.Start(missingAlerts);

        }

        public static void playSendMissingReportsThread(object parameterObj)
        {

            string missingAlerts = (string)parameterObj;
            using (MailMessage mail = new MailMessage())
            using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
            {

                mail.From = new MailAddress("bsafemonitoring@gmail.com", "Bsafe ", Encoding.UTF8);
                Setting settingInstance = Setting.Instance;
                mail.To.Add(settingInstance.email);
                mail.Subject = "Alerts collected from offline mode ";
                mail.Body = missingAlerts;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "rcza voco ctyq ptal");
                SmtpServer.EnableSsl = true;
                try
                {
                    SmtpServer.Send(mail);
                    DBclient dbInstance = DBclient.Instance;
                    dbInstance.RemoveReportImmediateTable();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("fail send missing alerts\n" + ex);
                }

                // ShowErrorDialog("send missingAlerts\n"+ missingAlerts);       

            }




        }

        public static void setDailyReport()
        {         
                  
            DateTime currentTime = DateTime.Now;            
            DateTime timeToReportToday = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 10, 0, 0);
            double tickTime = ConvertDateToSeconds(timeToReportToday) - ConvertDateToSeconds(currentTime);

            // if have some time until daily report
            if (tickTime>0) {
                // Initialization of periodicTimer  
                dailyTimer = new Timer(x => { createDailyReportFile(); }, null, TimeSpan.FromSeconds(tickTime), TimeSpan.FromSeconds(60 * 60 * 24));

            }
            else
            {
                dailyTimer = new Timer(x => { createDailyReportFile(); }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(60 * 60 * 24));

            }
           // dailyTimer = new Timer(x => { createDailyReportFile(); }, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(60 * 60 * 24));




        }
     
        private static void createDailyReportFile()
        {
            DateTime yesterdayDate = DateTime.Today;//.AddDays(-1)
            DBclient DBInstance = DBclient.Instance;
            
            try
            {
                string projectDirectory = Environment.CurrentDirectory;
                string path = Directory.GetParent(projectDirectory).Parent.FullName;
                Document Report = new Document();
                string date = DateTime.UtcNow.ToString("MM-dd-yyyy");
                date = date.Replace('-', '_');

                string reportName = Environment.UserName + "_" + date + "_DailyReport.pdf";

                if (!File.Exists(Path.Combine(path, reportName)))                 
                {
                    PdfWriter.GetInstance(Report, new FileStream(path + "/" + reportName, FileMode.Create));
                    Report.Open();
                    Image jpg = Image.GetInstance(path + "/logo.JPG");
                    jpg.ScalePercent(12f);
                    jpg.SetAbsolutePosition(Report.PageSize.Width - 410f,
                            Report.PageSize.Height - 130f);

                    Report.Add(jpg);
                    Report.Add(new Paragraph("\n\n\n\n\n"));
                    Report.Add(new Paragraph("DAILY REPORT FOR USER:" + Environment.UserName+"\n"+"DATE: "+ yesterdayDate.ToString()));
                    
                    // GET BROWSE HISTORY
                    string urls = DBInstance.getDailyUrlTable(yesterdayDate.ToString());
                    if (urls.Length > 0)
                    {
                        Report.Add(new Paragraph("\rWeb sites visited by the user yesterday:\r" + urls));
                    }
                    else
                    {
                        Report.Add(new Paragraph("NO SITETS TOREPORT"));
                    }
                    
                    // GET ALL PROCCESS 
                    string proccess = DBInstance.getDailyProcessTable(yesterdayDate.ToString());
                    if (proccess.Length > 0)
                    {
                        Report.Add(new Paragraph("\rProccess visited by the user yesterday:\r" + proccess));
                    }
                    else
                    {
                        Report.Add(new Paragraph("NO PROCCESS TOREPORT"));
                    }

                    Report.Close();
                    sendReportFileToMail("Daily Report "+ DateTime.UtcNow.ToString("MM-dd-yyyy") + ", USER: "+Environment.UserName, reportName);

                }

            }







            catch (Exception ex)
                {
                    ShowErrorDialog("" + ex);
                }

             

          
            
            DBInstance.RemoveDailyUrlTable(yesterdayDate.ToString());
        }

        public static double ConvertDateToSeconds(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

    }
}
