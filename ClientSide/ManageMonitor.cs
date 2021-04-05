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

        public ManageMonitor() {
             monitorProccess = new MonitorProccess();
             monitorTyping = new MonitorTyping();
             monitorSite = new MonitorSite();
             monitorInstallations = new MonitorInstallations();
        }
       

        public void playAllTriggers(){
            // play Monitor proccess
           monitorProccess.playThreadMonitor();

            // play Monitor Site trigger   
             monitorSite.playThreadMonitor();

            // play MonitorTyping trigger
            
            monitorTyping.playThreadMonitor();

            // play Monitor installations trigger
            monitorInstallations.playThreadMonitor();
            
        }
        public void stopAllTriggers()
        {
            // stop Monitor proccess
            monitorProccess.stopThreadMonitor();

            // stop Monitor Site trigger
            monitorSite.stopThreadMonitor();

            // stop MonitorTyping trigger
            monitorTyping.stopThreadMonitor();

            // stop Monitor installations trigger
            monitorInstallations.stopThreadMonitor();
        }

        public void playLiveMode() {

            monitorProccess.ifLive = true;
            monitorProccess.playThreadMonitor();
            monitorTyping.ifLive = true;
            monitorSite.ifLive = true;
        }
        public void stopLiveMode()
        {
            monitorProccess.ifLive = false;
            monitorSite.ifLive = false;
            monitorSite.ifLive = false;
        }

    }
}
