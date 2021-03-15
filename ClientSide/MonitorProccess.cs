using System;
using System.Collections.Generic;
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
        public static Program program;
        public Thread monitorProccess;

        public MonitorProccess() { }
        public override void playThreadMonitor()
        {
           
            if (base.monitorAlive)
            {
                stopThreadMonitor();
            } 
            ifLive = true;
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
            while (ifLive) {
                string processes = ShowAllProcess.ListAllApplications();
                updateProccess handler = Program.updateCurrentProcess;
                handler(processes);
                Thread.Sleep(60000);

            }
            ShowErrorDialog("playmonitorProccess finish");


        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}