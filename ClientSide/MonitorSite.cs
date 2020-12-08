
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using Condition = System.Windows.Automation.Condition;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

namespace ClientSide
{
    class MonitorSite
    {

        public DBclient dbs;
        public setSetting set;
        public bool monitor;

        public MonitorSite(DBclient dbs, setSetting set)
        {
            this.dbs = dbs;
            this.set = set;
            monitor = true;
           
            Thread siteMonitor = new Thread(playSiteMonitor);
            siteMonitor.Start();

        }
         
        public void playSiteMonitor() {
            string prev = "";
            while (monitor)
            {
                Process[] procsChrome = Process.GetProcessesByName("chrome");
                foreach (Process chrome in procsChrome)
                {
                    // the chrome process must have a window
                    if (chrome.MainWindowHandle == IntPtr.Zero)
                    {
                        continue;
                    }
                    try
                    {
                        AutomationElement element = AutomationElement.FromHandle(chrome.MainWindowHandle);
                        AutomationElement elm1 = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "Google Chrome"));
                        if (element == null)
                        {
                            continue;
                        } 
                        AutomationElement elm2 = TreeWalker.RawViewWalker.GetLastChild(elm1);
                        AutomationElement elm3 = elm2.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, ""));
                        AutomationElement elm4 = elm3.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ToolBar));
                        AutomationElement elementx = elm1.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
                        if (elementx == null)
                        {
                            continue;
                        }

                        if (!(bool)elementx.GetCurrentPropertyValue(AutomationElement.HasKeyboardFocusProperty))
                        {
                            string fullURL = ((ValuePattern)elementx.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
                            //string sURL = findHostName(fullURL);
                            if (!fullURL.Equals(prev))
                            {
                                ShowErrorDialog("URL: " + fullURL);
                                prev = fullURL;
                                dbs.connectToDatabase();
                                string category = dbs.getCategorySites(fullURL);
                                //ShowErrorDialog("categ|"+category+"|");
                                if (category != string.Empty)
                                {

                                    ShowErrorDialog(category);
                                    
                                    string cat = "";
                                    foreach (var x in set.triggersForAlert) {
                                        cat += "|"+x+"| ";
                                    }
                                    
                                    string ignor = "";
                                    foreach (var x in set.triggersForReport)
                                    {
                                        ignor += "|" + x + "| ";
                                    }

                                    ShowErrorDialog("triggersForAlert: " + cat);
                                    ShowErrorDialog("triggersForReport: " + ignor);

                                    if (set.triggersForAlert.Contains(category) == true)
                                       {
                                         string FilePic = Picters.ScreenCapture();
                                         Picters.CaptureCamera(FilePic);
                                         Report.sendAlertToMail(FilePic);
                                       }
                                    if (set.triggersForReport.Contains(category) == true)
                                        {
                                        dbs.connectToDatabase();
                                        dbs.fillTable(2, DateTime.Now.ToString(), "User browes in site: " + fullURL );
                                        }
                                }
                            }
                            

                        }
                    }
                    catch (Exception ex)
                    {
                        ShowErrorDialog("fail: \n" + ex);
                        continue;

                    }

                }
            }



        }

      

        private static void killTab(string fullURL)
        {
          
            
        }
      

        private static string findHostName(string sURL)
        {
            var uri = new Uri("http://" + sURL);
            var host = uri.Host;
            if (IsValidDomainName(host))
                return host as string;
            return string.Empty;
        }
        private static bool IsValidDomainName(string name)
        {
            return Uri.CheckHostName(name) != UriHostNameType.Unknown;
        }
        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


    }
}