using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Win32;
using System.Windows.Forms;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using System.Management;
using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;
using System.Windows.Media;
using VisioForge.Shared.MediaFoundation.OPM;

namespace ClientSide
{
    class MonitorInstallations
    {




        public static string ListAllApplications()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Process p in Process.GetProcesses("."))
            {

                try
                {

                    if (p.MainWindowTitle.Length > 0)
                    {
                        sb.Append("Window Title:\t" + p.MainWindowTitle.ToString() + Environment.NewLine);
                        sb.Append("Process Name:\t" + p.ProcessName.ToString() + Environment.NewLine);
                        sb.Append("Window Handle:\t" + p.MainWindowHandle.ToString() + Environment.NewLine);
                        sb.Append("Memory Allocation:\t" + p.PrivateMemorySize64.ToString() + Environment.NewLine);
                        sb.Append("Memory Allocation:\t" + p.Handle.ToString() + Environment.NewLine);


                        sb.Append(Environment.NewLine);
                    }
                }
                catch { }
            }

            return sb.ToString();
        }
        public static bool IsProgramInstalled()
        {


            foreach (var item in Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall").GetSubKeyNames())
            {

                string programName = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + item).GetValue("DisplayName").ToString();

                ShowErrorDialog(programName);

                /* if (string.Equals(programName, programDisplayName))
                 {
                     Console.WriteLine("Install status: INSTALLED");
                     return true;
                 }*/
            }

            return false;
        }

        public static string GetDownloadFolderPath()
        {
            return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders", "{374DE290-123F-4565-9164-39C4925E467B}", String.Empty).ToString();
        }
        public static void ListAll()
        {
            string h = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
            foreach (ManagementObject mgmtObjectin in searcher.Get())
            {
                if (mgmtObjectin["Name"] != null)
                    h += mgmtObjectin["Name"].ToString() + "\n";
            }
            

        }
        public static void createFileStringSetting(string stringSetting, string name, string id)
        {

            String projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;


            String[] paths = new string[] { @filepath, "hhh" };
            filepath = Path.Combine(paths);
            // ShowErrorDialog("filepath in createFileStringSetting: " + filepath);
            // ShowErrorDialog("stringSetting createFileStringSetting: "+stringSetting);


            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            String settingFile = Path.Combine(filepath, "setting_" + id + ".txt");
            if (!System.IO.File.Exists(settingFile))
            {
                using (StreamWriter sw = System.IO.File.CreateText(settingFile)) ;

                System.IO.File.WriteAllText(settingFile, name + "\r\n" + id + "\r\n" + stringSetting);

            }


            /*
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath)) ;
            }
            using (FileStream sw = File.OpenWrite(filepath))
            {
                //  sw.Write(stringSetting,0,stringSetting.Length);

                Byte[] info = new UTF8Encoding(true).GetBytes(name+"\r\n"+id+"\r\n"+stringSetting); // Add some information to the file.
                //sw.Write(info, 0, info.Length);‏
                sw.Write(info, 0, info.Length);
            }
            */
        }
        public static void GetInstalledApps()
        {
            // GUID taken from https://docs.microsoft.com/en-us/windows/win32/shell/knownfolderid
            var FOLDERID_AppsFolder = new Guid("{1e87508d-89c2-42f0-8a7e-645a0f50ca58}");
            ShellObject appsFolder = (ShellObject)KnownFolderHelper.FromKnownFolderId(FOLDERID_AppsFolder);
            string date = "";
            foreach (var app in (IKnownFolder)appsFolder)
            {
                // The friendly app name
                string name = app.Name;
                //ShowErrorDialog(app.Properties.ToString());
                // The ParsingName property is the AppUserModelID
                string appUserModelID = app.ParsingName; // or app.Properties.System.AppUserModel.ID
                // 
                date += name+"=="+ appUserModelID +"\n";
                // You can even get the Jumbo icon in one shot
                ImageSource icon = app.Thumbnail.ExtraLargeBitmapSource;
            }
            //And that's all there is to it. You can also start the apps using
            createFileStringSetting(date, "66", "61");
            // System.Diagnostics.Process.Start("explorer.exe", @" shell:appsFolder\" + appModelUserID);
           ShellObjectWatcher sow = new ShellObjectWatcher(appsFolder, false);
            sow.AllEvents += (s, e) => DoWhatever(appsFolder);
            sow.Start();
        }

        private static void DoWhatever(ShellObject appsFolder)
        {
            ShowErrorDialog(appsFolder.ParsingName);
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



    }


}
