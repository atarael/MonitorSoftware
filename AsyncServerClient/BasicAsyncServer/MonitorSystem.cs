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
        
        public MonitorSystem(ref String text)
        {
            InitializeComponent();
            text += " press OK"; ;
 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            this.Close();
            

        }


        public String sendSystem() {
            system += "try sent..";
            return system;


        }
    }
}
