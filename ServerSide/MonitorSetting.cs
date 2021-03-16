using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace ServerSide
{
 
    public partial class MonitorSetting : Form
    {

        public String setting = "";
        public int id;
        private List<String> CategorySite;
        private string addSiteToMonitor = "";
        private string addSiteToCancelMonitor = "";
        private string AddBadWords = "";
        public MonitorSetting(int id)
        {
            this.id = id;
            InitializeComponent();
            
            // insert category to grid
            CategorySite = new List<String>();
            CategorySite.Add("News");
            CategorySite.Add("Sport");
            CategorySite.Add("Shopping");
            CategorySite.Add("Vocation");
            CategorySite.Add("Economy");
            CategorySite.Add("Social");
            CategorySite.Add("Vocation");

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            setting = "";
            // insert first line - all category. format: <CategorySite>XXX, where X is 1-selected or 0-not selected
            int numOfCategory = dtgCategorySites.Rows.Count;

            for (int i = 0; i < numOfCategory; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dtgCategorySites.Rows[i];

                // insert Category Name
                setting += row.Cells[0].Value.ToString().ToLower() + " ";

                // Report immediately
                if (row.Cells["ReportImmediately"].Value != null)
                    setting += "1";
                else setting += "0";
                // Update report
                if (row.Cells["UpdateReport"].Value != null)
                    setting += "1";
                else setting += "0";


                setting += " ";

            }

            setting += "\r\n";

            // insert second line - link to another sites to block             
            setting += addSiteToMonitor + "\r\n";

            // insert third line - link to sites to unblock
            setting += addSiteToCancelMonitor + "\r\n";

            // insert forth line - application installation.  format: XXX, where X is 1-selected or 0-not selected
            if (chbAppInstallUpdate.Checked)
                setting += "1";
            else setting += "0";
            if (chbAppInstallReport.Checked)
                setting += "1";
            else setting += "0";

            setting += "\r\n";

            // insert five line - Typing inappropriate words
            if (chbBadWordUpdate.Checked)
                setting += "1";
            else setting += "0";
            if (chbBadWordReport.Checked)
                setting += "1";
            else setting += "0";
            setting += "\r\n";

            // insert six line -  Typing inappropriate words
            setting += AddBadWords + "\r\n";

            // insert seven line - mail to get report
            if (IsValidEmail(txbEmail.Text))
                setting += txbEmail.Text + "\r\n";
            else {
                ShowErrorDialog("Must fill Email address report");
                return;
            }

            // insert eigth line - report time
            int select = chblFrequency.SelectedIndex;
            if (select < 0) { // if check minute 
                string minuteStr = nudMinuteReport.Value.ToString();
                int Minute = Int32.Parse(minuteStr);
                if (Minute > 0)
                {
                    setting += "minute " + minuteStr + "\r\n";
                    this.Close();
                }
                else {
                    ShowErrorDialog("Must Select Frequency or insert minutes to get report");
                    return;
                }
                    
            }
               
            else
            {
                setting += select + "\r\n";
                this.Close();
            }
            setSetting handler = Program.setSettingDeleGate;
            handler(id, setting);

           
           
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private string correctURL(string url)
        {
            
            if (!url.StartsWith("http"))
            {
                url = "http://" + url;
            }

            bool isUri = UrlIsValid(url);
            if (!isUri)
            {
                return string.Empty;
            }
            else
            {
                if (url.StartsWith("http"))
                    url = url.Replace("//", "-").Split(new Char[] { '-' }, 2)[1];
                if (url.StartsWith("www"))
                    url = url.Replace("www.", "-").Split(new Char[] { '-' }, 2)[1];

                return url;
            }

        }
         
        private void label5_Click(object sender, EventArgs e)
        {

        }


        private void MonitorSystem_Load(object sender, EventArgs e)
        {
            addSiteToMonitor = "";
            addSiteToCancelMonitor = "";
            for (int i = 0; i < CategorySite.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dtgCategorySites.Rows[i].Clone();
                row.Cells[0].Value = CategorySite[i];
                dtgCategorySites.Rows.Add(row);
            }
            dtgCategorySites.AllowUserToAddRows = false;





        }
        public static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void chblFrequency_SelectedIndexChanged(object sender, EventArgs e)
        {
            int select = chblFrequency.SelectedIndex;

            for (int i = 0; i < chblFrequency.Items.Count; i++)
                if (select != i)
                    chblFrequency.SetItemChecked(i, false);
        }

        private void btnAddSiteToMonitoring_Click(object sender, EventArgs e)
        {
            
            string url = correctURL(txbBlockedSites.Text);
            if (url == string.Empty)
            {
                ShowErrorDialog("URL: " + url + " invalid, insert again!");
            }
            else
            {
                addSiteToMonitor += url + " ";
                txbBlockedSites.Text = "";
            }

        }
        private void btnAddSiteToCancelMonitoring_Click(object sender, EventArgs e)
        {

            string url = correctURL(txbUnblockedSites.Text);
            if (url == string.Empty)
            {
                ShowErrorDialog("URL: " + url + " invalid, insert again!");
            }
            else
            {
                addSiteToCancelMonitor += url + " ";
                txbUnblockedSites.Text = "";
            }
        }

        public bool UrlIsValid(string url)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.Timeout = 5000; //set the timeout to 5 seconds to keep the user from waiting too long for the page to load
                request.Method = "HEAD"; //Get only the header information -- no need to download any content

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    int statusCode = (int)response.StatusCode;
                    if (statusCode >= 100 && statusCode < 400) //Good requests
                    {
                        return true;
                    }
                    else if (statusCode >= 500 && statusCode <= 510) //Server Errors
                    {
                        //log.Warn(String.Format("The remote server has thrown an internal error. Url is not valid: {0}", url));
                        ShowErrorDialog(String.Format("The remote server has thrown an internal error. Url is not valid: {0}", url));
                        return false;
                    }
                }

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError) //400 errors
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }

        private void btnAddBadWords_Click(object sender, EventArgs e)
        {
            AddBadWords += txbAddBadWords.Text + " ";
            txbAddBadWords.Text = "";
        }
    }
}

