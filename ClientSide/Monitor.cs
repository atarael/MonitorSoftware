using System; 
using System.Threading; 
using System.Drawing;
using System.Drawing.Imaging;
using System.IO; 
using System.Windows.Forms;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;

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
        
        const int ENUM_CURRENT_SETTINGS = -1;
        public Monitor()
        {


        }

        public abstract void playThreadMonitor();
        public abstract void stopThreadMonitor();

        // take screen picture 
        public string ScreenCapture()
        {

            foreach (Screen screen in Screen.AllScreens)
            {
                DEVMODE dm = new DEVMODE();
                dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
                EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);

                using (Bitmap bmp = new Bitmap(dm.dmPelsWidth, dm.dmPelsHeight))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(dm.dmPositionX, dm.dmPositionY, 0, 0, bmp.Size);
                    Console.WriteLine(screen.DeviceName.Split('\\').Last() + ".jpeg");

                   //bmp.Save(screen.DeviceName.Split('\\').Last() + ".jpeg");
                
                String projectDirectory = Environment.CurrentDirectory;
                string filepath = Directory.GetParent(projectDirectory).Parent.FullName;

                String[] paths = new string[] { @filepath, "files" };
                filepath = Path.Combine(paths);

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }
                string s = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".jpg";
                paths = new string[] { @filepath, s };
                filepath = Path.Combine(paths);

                if (!File.Exists(filepath))
                { 
                    bmp.Save(filepath);
                }
                
                return s;
            }}
            return "";
        }

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }













        //Create a new bitmap.
        //var printscreen = new Bitmap(SystemInformation.VirtualScreen.Width,
        //                   SystemInformation.VirtualScreen.Height,
        //                   PixelFormat.Format32bppArgb);
        //Graphics screenGraph = Graphics.FromImage(printscreen);
        //screenGraph.CopyFromScreen(SystemInformation.VirtualScreen.X,
        //                           SystemInformation.VirtualScreen.Y,
        //                           0,
        //                           0,
        //                           SystemInformation.VirtualScreen.Size,
        //                           CopyPixelOperation.SourceCopy);
        //String projectDirectory = Environment.CurrentDirectory;
        //    string filepath = Directory.GetParent(projectDirectory).Parent.FullName;

        //    String[] paths = new string[] { @filepath, "files" };
        //    filepath = Path.Combine(paths);

        //    if (!Directory.Exists(filepath))
        //    {
        //        Directory.CreateDirectory(filepath);
        //    }
        //    string s = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Second.ToString() + ".jpg";
        //    paths = new string[] { @filepath, s };
        //    filepath = Path.Combine(paths);

        //    if (!File.Exists(filepath))
        //    {
        //        printscreen.Save(filepath, ImageFormat.Jpeg);
        //    }
        //    return s;







    
       
        
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
            string triggerName = args[3];

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

                switch (triggerName)
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
                Console.WriteLine("fail send mailllll: \n" + ex);

                // if no internet - save imadiate alert in DB 
                string date = DateTime.Now.ToString();

                DBclient DBInstance = DBclient.Instance;
                DBInstance.fillReportImmediateTable(triggerName, TriggerDescription, triggerDetails, date);

                 

            }
            catch (Exception ex)
            {
                
                Console.WriteLine("fail send mail: \n" + ex);
            }






            //Thread reportThread = new Thread(playSendReportThread);
            //reportThread.Start();

        }



    }
}
