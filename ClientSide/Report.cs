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
using System.Windows.Forms;

namespace ClientSide
{

    class Report
    {
        private static bool send;
        private static string screenPic;
        private static string cameraPic;

        public static void sendEmail(String picName)
        {

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add("sara05485@gmail.com");
            //mail.To.Add("ataraelmal@gmail.com");
            mail.From = new MailAddress("bsafemonitoring@gmail.com", "Bsafe ", System.Text.Encoding.UTF8);
            mail.Subject = "This mail is Alert of browse in forbidden site ";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "atara1998");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            /*
            String projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            string s = DateTime.Now.Day.ToString();
            String[] paths = new string[] { @filepath, "files" };
            filepath = Path.Combine(paths);
            string file = Path.Combine(filepath, @"setting_0.txt");
            


            bool exists = System.IO.Directory.Exists(filepath);
                System.IO.Directory.CreateDirectory(filepath);
            exists = System.IO.File.Exists(file);
            if (exists)
                File.Delete(file);
            */

            //ZipFile.CreateFromDirectory(filepath, file);

            string screenPic = @"C:\Users\sara\Desktop\Sara Ayash\MonitorSoftware\ClientSide\files\snapshot_" + picName;
            string cameraPic = @"C:\Users\sara\Desktop\Sara Ayash\MonitorSoftware\ClientSide\files\" + picName;


            Attachment data = new Attachment(screenPic, MediaTypeNames.Application.Octet);
            mail.Attachments.Add(new Attachment(cameraPic));
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(screenPic);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(screenPic);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(screenPic);
            // Add the file attachment to this email message.
            mail.Attachments.Add(data);
            // SmtpClient client = new SmtpClient(Server);
            // Add credentials if the SMTP server requires them.
            // client.Credentials = CredentialCache.DefaultNetworkCredentials;

            try
            {
                client.Send(mail);
                // Page.RegisterStartupScript("UserMsg", "<script>alert('Successfully Send...');if(alert){ window.location='SendMail.aspx';}</script>");
            }
            catch (Exception ex)
            {
                Exception ex2 = ex;
                string errorMessage = string.Empty;
                while (ex2 != null)
                {
                    errorMessage += ex2.ToString();
                    ex2 = ex2.InnerException;
                }
                // Page.RegisterStartupScript("UserMsg", "<script>alert('Sending Failed...');if(alert){ window.location='SendMail.aspx';}</script>");
            }
        }

        public static void sendAlertToMail(String picName)
        {
            //ShowErrorDialog("insert to sendAlert");
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
                    }
                    else
                    {
                        send = true;
                    }

                }
                else {
                    send = true;
                }
               
                ShowErrorDialog("send is: " + send);
                Thread sendMail = new Thread(SendMail);
                sendMail.Start();
                

            }
             



        }
        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void SendMail()
        {
            ShowErrorDialog("in thread "+ Thread.CurrentThread.Name);
            if (send)
            {
                try
                {
                    MailMessage mail = new System.Net.Mail.MailMessage();
                    mail.To.Add("sara05485@gmail.com");
                    //mail.To.Add("ataraelmal@gmail.com");
                    mail.From = new MailAddress("bsafemonitoring@gmail.com", "Bsafe ", Encoding.UTF8);
                    mail.Subject = "This mail is Alert of browse in forbidden site ";
                    mail.SubjectEncoding = Encoding.UTF8;
                    mail.BodyEncoding = Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    mail.Priority = MailPriority.High;
                    SmtpClient client = new SmtpClient();
                    client.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "atara1998");
                    client.Port = 587;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;

                    Attachment data = new Attachment(screenPic, MediaTypeNames.Application.Octet);
                    mail.Attachments.Add(new Attachment(cameraPic));
                    ContentDisposition disposition = data.ContentDisposition;
                    mail.Attachments.Add(data);

                    ShowErrorDialog("before send mail");
                    client.Send(mail);
                    ShowErrorDialog("after send mail");
                }
                catch (Exception ex)
                {
                    ShowErrorDialog("fail send mail:\n" + ex);
                     
                }
            }
            else {
                ShowErrorDialog("send is false");
            }
            send = false;
           
        } 
    } 
}