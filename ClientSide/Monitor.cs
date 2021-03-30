using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientSide
{

    abstract class Monitor
    {
        protected Thread monitorThread;
        protected bool monitorAlive;
        
        protected DBclient DBInstance = DBclient.Instance;
        protected Setting SettingInstance = Setting.Instance;

        public Monitor()
        {


        }

        public abstract void playThreadMonitor();
        public abstract void stopThreadMonitor();
    }
}
