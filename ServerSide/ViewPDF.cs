using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    public partial class ViewPDF : Form
    {
        public ViewPDF()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             
             
            string projectDirectory = Environment.CurrentDirectory;
            string path = Directory.GetParent(projectDirectory).Parent.FullName;

            System.Diagnostics.Process.Start(Path.Combine(path,"Report.pdf"));

        }
    }
}
