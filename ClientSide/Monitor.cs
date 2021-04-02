using System; 
using System.Threading; 
using System.Drawing;
using System.Drawing.Imaging;
using System.IO; 
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;


namespace ClientSide
{

    abstract class Monitor
    {
        private static string screenPic;
        private static string cameraPic;

        protected Thread monitorThread;
        protected bool monitorAlive;
        
        protected DBclient DBInstance = DBclient.Instance;
        protected Setting SettingInstance = Setting.Instance;

        public Monitor()
        {


        }

        public abstract void playThreadMonitor();
        public abstract void stopThreadMonitor();

        // take screen picture 
        public string ScreenCapture()
        {
            Bitmap printscreen = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
            Graphics graphics = Graphics.FromImage(printscreen as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            String projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            string s = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".jpg";
            String[] paths = new string[] { @filepath, "files", s };
            filepath = Path.Combine(paths);

            if (!File.Exists(filepath))
            {
                printscreen.Save(filepath, ImageFormat.Jpeg);
            }
            return s;
        }
        // take user picture
        public void CaptureCamera(string picName)
        {
            try
            {
                Camera c = new Camera(picName);
                c.Show();
                c.Visible = false;
                Thread.Sleep(800);
                c.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("fail camera:\n" + ex);
            }

        }

        // send immediate alert to server
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
                Console.WriteLine("fail send mailllll: \n" + ex);
                Thread alertThread = new Thread(playSendAlertThread);
                alertThread.Start(args);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("fail send mail: \n" + ex);
                Console.WriteLine("fail send mail: \n" + ex);
            }






            //Thread reportThread = new Thread(playSendReportThread);
            //reportThread.Start();

        }



    }
}
