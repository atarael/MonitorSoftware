using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSide
{
    // This monitor work only in Live mode
    class MonitorProccess : Monitor
    {
      

        public bool ifLive;
        
        public MonitorProccess() { }
        public override void playThreadMonitor()
        {
           
            if (base.monitorAlive)
            {
                stopThreadMonitor();
            } 
            //ifLive = true;
            base.monitorAlive = true;
            base.monitorThread = new Thread(playmonitorProccess);
            base.monitorThread.Start();

        }
        public override void stopThreadMonitor()
        {
            ifLive = false;
        }

        private void playmonitorProccess()
        {
            while (monitorAlive)
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
                            sb.Append(Environment.NewLine);

                            // update procces in DB for daily report
                            string today = System.DateTime.Today.ToString();
                            DBInstance.fillDailyProcessTable(today, "Window Title:\t" + p.MainWindowTitle.ToString() +", Process Name:\t" + p.ProcessName.ToString()+"\r");

                        }

                    }
                    catch { }
                }

                if (ifLive)
                {
                     string processes = ListAllApplications();
                     updateProccess handler = Program.updateCurrentProcess;
                     handler(sb.ToString());                  

                }
                Thread.Sleep(6000); 
            }
            
            //ShowErrorDialog("playmonitorProccess finish");


        }
      
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


                        sb.Append(Environment.NewLine);
                    }
                }
                catch { }
            }

            return sb.ToString();
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}