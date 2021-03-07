using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide
{
    class ManageMonitor
    {
        public MonitorProccess monitorProccess;
        public MonitorTyping monitorTyping;
        public MonitorSite monitorSite;
        public MonitorInstallations monitorInstallations;

        public ManageMonitor()
        {
            // play Monitor proccess
            monitorProccess = new MonitorProccess();
            monitorProccess.playThreadMonitor();

            // play Monitor Site trigger
            monitorSite = new MonitorSite();
            monitorSite.playThreadMonitor();

            // play MonitorTyping trigger
            monitorTyping = new MonitorTyping();
            monitorTyping.playThreadMonitor();

            // play Monitor installations trigger
            monitorInstallations = new MonitorInstallations();
            monitorInstallations.playThreadMonitor();
        }



    }
}
