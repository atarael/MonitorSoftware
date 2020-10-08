using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BasicAsyncServer
{
    public partial class MonitorSystem : Form
    {

        private static String system ="";
        private static String clientName = "";

        public MonitorSystem(String name)
        {
            InitializeComponent();
            clientName = name;
 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            this.Close();
        }


        public String sendSystem() {
            system = clientName + " set system";
            return system;


        }
    }
}
