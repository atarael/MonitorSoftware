using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSide
{

    class MonitorInstallations:Monitor
    {

        public MonitorInstallations() {
        }



        public override void playThreadMonitor() {
            if (base.monitorAlive)
            {
                stopThreadMonitor();
            }
            base.monitorAlive = true;
            base.monitorThread = new Thread(playMonitorInstallations);
            base.monitorThread.Start();

        }
        public override void stopThreadMonitor() {
            base.monitorAlive = false;
        }

        public void playMonitorInstallations()
        {
            int hour = 60000; // 1 hour
            while (base.monitorAlive)
            {
                bool install = false;
                // get list of app install files from recent hour 
                List<FileInfo> resentAppOrTool = recentFiles();
                // check if have new installation files
                if (resentAppOrTool != null)
                {
                   
                    foreach (FileInfo fi in resentAppOrTool)
                    {
                        string progNmae = orderProgramName(fi.Name);
                        // ShowErrorDialog("progNmae: " + progNmae + "\nfi.Name: " + fi.Name);
                        if (IsSoftwareInstalled(progNmae))
                        {
                            // ShowErrorDialog("progNmae: " + progNmae + "\nINSTALLLL!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            install = true;
                        }
                        reportOrSendAlert(fi.Name, install);

                    }
                }
                Thread.Sleep(hour); // 3,600,000 ms = 1 hr
            }
             
        }
     
        private void reportOrSendAlert(string programName, bool ifInstall)
        {

            string statusInstall = "";
            if (ifInstall)
            {
                statusInstall = "and inatall in computer";
            }

            
            if (base.SettingInstance.triggersForAlert.Contains("installation") == true)
            {
                string FilePic = ScreenCapture();
                CaptureCamera(FilePic);
                sendAlertToMail(FilePic, "installation trigger occur", programName, "installationTrigger");
                ShowErrorDialog("send alert to mail\nInstallation trigger occur\nProgramName: " + programName);

           }

           if (base.SettingInstance.triggersForReport.Contains("installation") == true)
            {
                base.DBInstance.fillTriggersTable(3, DateTime.Now.ToString(), programName + " download " + statusInstall);
                ShowErrorDialog("update DB\nInstallation trigger occur\nProgramName: " + programName + "\n"+ programName + " download " + statusInstall);
            }
        }

        private string orderProgramName(string programName)
        {
            var capitalLatterRegex = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) | (?<=[^A-Z])(?=[A-Z]) | (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            programName = capitalLatterRegex.Replace(programName, " ");
            string[] progNameSplited = programName.Replace('-', ' ').Replace('_', ' ').Split(' ');
            if (progNameSplited.Length >= 2)
            {
                //return "" + progNameSplited[0] + " " + progNameSplited[1];
            }
            return progNameSplited[0];
        }

        public static List<FileInfo> recentFiles()
        {


            string downloadsPath = GetPath(KnownFolder.Downloads);
            var directory = new DirectoryInfo(downloadsPath);


            //ShowErrorDialog(DateTime.Now.AddHours(-1).ToString());

            List<FileInfo> myFile = (from f in directory.GetFiles() orderby f.LastWriteTime descending select f)
                .Where(f => f.CreationTime >= DateTime.Now.AddHours(-1) && (f.Name.EndsWith(".apk") || f.Name.EndsWith(".exe") || f.Name.EndsWith(".ink") || f.Name.EndsWith(".msi")))
                .ToList();
            
            string desktop = GetPath(KnownFolder.Desktop);
            directory = new DirectoryInfo(desktop);

            List<FileInfo> myFile1 = (from f in directory.GetFiles() orderby f.LastWriteTime descending select f)
                .Where(f => f.CreationTime >= DateTime.Now.AddHours(-1) && (f.Name.EndsWith(".apk") || f.Name.EndsWith(".exe") || f.Name.EndsWith(".ink") || f.Name.EndsWith(".msi")))
                .ToList();

            string docs = GetPath(KnownFolder.Documents);
            directory = new DirectoryInfo(docs);

            List<FileInfo> myFile2 = (from f in directory.GetFiles() orderby f.LastWriteTime descending select f)
                .Where(f => f.CreationTime >= DateTime.Now.AddHours(-1) && (f.Name.EndsWith(".apk") || f.Name.EndsWith(".exe") || f.Name.EndsWith(".ink") || f.Name.EndsWith(".msi")))
                .ToList();

            string allFI = "";
            foreach (FileInfo fi in myFile)
            {
                myFile.Append(fi);
                allFI += fi.Name + "\n";
            }

            return myFile;
        }


        public static bool IsSoftwareInstalled(string softwareName)
        {
            string registry_key = @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
            string allProgs = "";
            bool display = false;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    //ShowErrorDialog("subkey_name: " + subkey_name);

                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        // check if softwareName display in DisplayName 
                        if (subkey.GetValue("DisplayName") != null)
                        {
                            string DisplayName = subkey.GetValue("DisplayName").ToString().ToLower();
                            DisplayName.Replace('-', ' ');
                            allProgs += "DisplayName: " + DisplayName + "\n";
                            if (string.Equals(DisplayName, softwareName) || DisplayName.Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(DisplayName))
                            {
                                
                                display = true;
                            }
                        }
                        // check if softwareName display in Publisher
                        if (subkey.GetValue("Publisher") != null)
                        {
                            string Publisher = subkey.GetValue("Publisher").ToString().ToLower();
                            Publisher.Replace('-', ' ');
                            allProgs += "Publisher: " + Publisher + "\n";
                            if (string.Equals(Publisher, softwareName) || Publisher.Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(Publisher))
                            {
                                
                                display = true;
                            }
                        }

                        // check if softwareName display in Icon
                        if (subkey.GetValue("DisplayIcon") != null)
                        {
                            string DisplayIcon = subkey.GetValue("DisplayIcon").ToString().ToLower();
                            DisplayIcon.Replace('-', ' ');
                            allProgs += "DisplayIcon: " + DisplayIcon + "\n";
                            if (string.Equals(DisplayIcon, softwareName) || DisplayIcon.Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(DisplayIcon))
                            {
                                
                                display = true;
                            }
                        }

                        if (subkey != null)
                        {
                            allProgs += "subkey: " + subkey + "\n";
                            if (string.Equals(subkey, softwareName) || subkey.ToString().Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(subkey.ToString()))
                            {
                               
                                display = true;
                            }
                        }




                    }
                }
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    //ShowErrorDialog("subkey_name: " + subkey_name);

                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        // check if softwareName display in DisplayName 
                        if (subkey.GetValue("\nDisplayName") != null)
                        {
                            string DisplayName = subkey.GetValue("DisplayName").ToString().ToLower();
                            allProgs += "DisplayName: " + DisplayName + "\n";
                            if (string.Equals(DisplayName, softwareName) || DisplayName.Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(DisplayName))
                            {
                                ShowErrorDialog("status: INSTALLED");
                                display = true;
                            }
                        }
                        // check if softwareName display in Publisher
                        if (subkey.GetValue("Publisher") != null)
                        {
                            string Publisher = subkey.GetValue("Publisher").ToString().ToLower();
                            allProgs += "Publisher: " + Publisher + "\n";
                            if (string.Equals(Publisher, softwareName) || Publisher.Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(Publisher))
                            {
                                ShowErrorDialog("status: INSTALLED");
                                display = true;
                            }
                        }

                        if (subkey != null)
                        {
                            allProgs += "subkey: " + subkey + "\n";
                            if (string.Equals(subkey, softwareName) || subkey.ToString().Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(subkey.ToString()))
                            {
                                ShowErrorDialog("status: INSTALLED");
                                display = true;
                            }
                        }

                        // check if softwareName display in Icon
                        if (subkey.GetValue("DisplayIcon") != null)
                        {
                            string DisplayIcon = subkey.GetValue("DisplayIcon").ToString().ToLower();
                            DisplayIcon.Replace('-', ' ');
                            allProgs += "DisplayIcon: " + DisplayIcon + "\n";
                            if (string.Equals(DisplayIcon, softwareName) || DisplayIcon.Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(DisplayIcon))
                            {
                                ShowErrorDialog("status: INSTALLED");
                                display = true;
                            }
                        }


                    }
                }
            }

            ShowErrorDialog(allProgs);

            string path = @"C:\Users\sara\Desktop\Sara Ayash\MonitorSoftware\ClientSide\Allproc.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(allProgs);
                    sw.Close();
                }

            }


            return display;
        }

        public static List<string> InstalledProgram()
        {
            string registry_key = @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
            string allProgs = "";
            List<string> InstalledProgram = new List<string>();
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    //ShowErrorDialog("subkey_name: " + subkey_name);

                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        // check if softwareName display in DisplayName 
                        if (subkey.GetValue("DisplayName") != null)
                        {
                            InstalledProgram.Append(subkey.GetValue("DisplayName").ToString());
                        }
                        // check if softwareName display in Publisher
                        if (subkey.GetValue("Publisher") != null)
                        {
                            InstalledProgram.Append(subkey.GetValue("Publisher").ToString());
                        }

                        // check if softwareName display in Icon
                        if (subkey.GetValue("DisplayIcon") != null)
                        {
                            InstalledProgram.Append(subkey.GetValue("DisplayIcon").ToString());
                        }

                        if (subkey != null)
                        {
                            InstalledProgram.Append(subkey.ToString());
                        }




                    }
                }
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    //ShowErrorDialog("subkey_name: " + subkey_name);

                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        // check if softwareName display in DisplayName 
                        if (subkey.GetValue("DisplayName") != null)
                        {
                            InstalledProgram.Append(subkey.GetValue("DisplayName").ToString());
                        }
                        // check if softwareName display in Publisher
                        if (subkey.GetValue("Publisher") != null)
                        {
                            InstalledProgram.Append(subkey.GetValue("Publisher").ToString());
                        }

                        // check if softwareName display in Icon
                        if (subkey.GetValue("DisplayIcon") != null)
                        {
                            InstalledProgram.Append(subkey.GetValue("DisplayIcon").ToString());
                        }

                        if (subkey != null)
                        {
                            InstalledProgram.Append(subkey.ToString());
                        }



                    }
                }
            }

            return InstalledProgram;
        }

   
        public static bool AllPrograms(string softwareName)
        {
            string registry_key = @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
            string allProgs = "";
            bool display = false;
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(registry_key))
            {
                foreach (string subkey_name in key.GetSubKeyNames())
                {
                    //ShowErrorDialog("subkey_name: " + subkey_name);

                    using (RegistryKey subkey = key.OpenSubKey(subkey_name))
                    {
                        // check if softwareName display in DisplayName 
                        if (subkey.GetValue("DisplayName") != null)
                        {
                            string DisplayName = subkey.GetValue("DisplayName").ToString().ToLower();
                            if (string.Equals(DisplayName, softwareName) ||
                                DisplayName.Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(DisplayName))
                            {
                                ShowErrorDialog("status: INSTALLED");
                                display = true;
                            }
                        }
                        // check if softwareName display in Publisher
                        else if (subkey.GetValue("Publisher") != null)
                        {
                            string Publisher = subkey.GetValue("Publisher").ToString().ToLower();
                            if (string.Equals(Publisher, softwareName) ||
                                Publisher.Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(Publisher))
                            {
                                ShowErrorDialog("status: INSTALLED");
                                display = true;
                            }
                        }

                        else if (subkey != null)
                        {

                            if (string.Equals(subkey, softwareName) ||
                                subkey.ToString().Contains(softwareName.ToLower()) || softwareName.ToLower().Contains(subkey.ToString()))
                            {
                                ShowErrorDialog("status: INSTALLED");
                                display = true;
                            }
                        }




                    }
                }
            }
            string path = @"C:\Users\sara\Desktop\Sara Ayash\MonitorSoftware\ClientSide\Allproc.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(allProgs);
                    sw.Close();
                }

            }


            return display;
        }
        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Class containing methods to retrieve specific file system paths.
        /// </summary>

        private static string[] _knownFolderGuids = new string[]
        {
        "{56784854-C6CB-462B-8169-88E350ACB882}", // Contacts
        "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
        "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", // Documents
        "{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
        "{1777F761-68AD-4D8A-87BD-30B759FA33DD}", // Favorites
        "{BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968}", // Links
        "{4BD8D571-6D19-48D3-BE97-422220080E43}", // Music
        "{33E28130-4E1E-4676-835A-98395C3BC3BB}", // Pictures
        "{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}", // SavedGames
        "{7D1D3A04-DEBB-4115-95CF-2F29DA2920DA}", // SavedSearches
        "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", // Videos
        };

        /// <summary>
        /// Gets the current path to the specified known folder as currently configured. This does
        /// not require the folder to be existent.
        /// </summary>
        /// <param name="knownFolder">The known folder which current path will be returned.</param>
        /// <returns>The default path of the known folder.</returns>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
        ///     could not be retrieved.</exception>
        public static string GetPath(KnownFolder knownFolder)
        {
            return GetPath(knownFolder, false);
        }

        /// <summary>
        /// Gets the current path to the specified known folder as currently configured. This does
        /// not require the folder to be existent.
        /// </summary>
        /// <param name="knownFolder">The known folder which current path will be returned.</param>
        /// <param name="defaultUser">Specifies if the paths of the default user (user profile
        ///     template) will be used. This requires administrative rights.</param>
        /// <returns>The default path of the known folder.</returns>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
        ///     could not be retrieved.</exception>
        public static string GetPath(KnownFolder knownFolder, bool defaultUser)
        {
            return GetPath(knownFolder, KnownFolderFlags.DontVerify, defaultUser);
        }

        private static string GetPath(KnownFolder knownFolder, KnownFolderFlags flags, bool defaultUser)
        {
            int result = SHGetKnownFolderPath(new Guid(_knownFolderGuids[(int)knownFolder]),
                (uint)flags, new IntPtr(defaultUser ? -1 : 0), out IntPtr outPath);
            if (result >= 0)
            {
                string path = Marshal.PtrToStringUni(outPath);
                Marshal.FreeCoTaskMem(outPath);
                return path;
            }
            else
            {
                throw new ExternalException("Unable to retrieve the known folder path. It may not "
                    + "be available on this system.", result);
            }
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)]Guid rfid, uint dwFlags, IntPtr hToken,
            out IntPtr ppszPath);

        [Flags]
        private enum KnownFolderFlags : uint
        {
            SimpleIDList = 0x00000100,
            NotParentRelative = 0x00000200,
            DefaultPath = 0x00000400,
            Init = 0x00000800,
            NoAlias = 0x00001000,
            DontUnexpand = 0x00002000,
            DontVerify = 0x00004000,
            Create = 0x00008000,
            NoAppcontainerRedirection = 0x00010000,
            AliasOnly = 0x80000000
        }


        /// <summary>
        /// Standard folders registered with the system. These folders are installed with Windows Vista
        /// and later operating systems, and a computer will have only folders appropriate to it
        /// installed.
        /// </summary>
        public enum KnownFolder
        {
            Contacts,
            Desktop,
            Documents,
            Downloads,
            Favorites,
            Links,
            Music,
            Pictures,
            SavedGames,
            SavedSearches,
            Videos
        }

    }
}


