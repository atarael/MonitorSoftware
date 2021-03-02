using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ClientSide
{
    public partial class sqliteForm : Form
    {
        MySqlConnection conn;
        string connString;
        public sqliteForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
           
            //connString = "Persist Security Info=False;Server=;Database=saraay_formika;Uid=saraay_our_store;Pwd=318411840;";

            // connString = "server=62.90.88.37;port=2083; database=atarael_BeSafe;uid=atarael_atarael ; pwd =207233990;connection Timeout=60;";
            connString= "Server = localhost; Database = atarael_BeSafe; Uid = atarael_atarael; Pwd = 207233990;Connection Timeout=60;";
            try
            {
                conn = new MySqlConnection(connString);
                conn.Open();
                MessageBox.Show("connection success");
                 
            }
            catch(MySqlException ex) {
                Console.WriteLine("==========================\n MySqlException fail:\n" + ex+ "\n==========================\n");
                MessageBox.Show("MySqlException fail:\n" + ex);
            }
           


        }
    }
}
