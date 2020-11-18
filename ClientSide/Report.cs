using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Mail;
using System.Net.Mime;


namespace ClientSide
{
    class Report
    {
        public void sendMail(){
            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            //mail.To.Add("sara05485@gmail.com");
            mail.To.Add("ataraelmal@gmail.com");
            mail.From = new MailAddress("bsafemonitoring@gmail.com", "Report from Bsafe ", System.Text.Encoding.UTF8);
            mail.Subject = "This mail is send from asp.net application";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = "זהו הצלחתי לשלוח מייל מהתוכנה ";
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("bsafemonitoring@gmail.com", "atara1998");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            string file = "n.txt";

            Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
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


    }
}
