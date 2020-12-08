using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private static String clientName = "";
        private List<String> CategorySite;
        private string addSiteToMonitor = "";
        private string addSiteToCancelMonitor = "";

        public MonitorSetting(String name)
        {
            InitializeComponent();
            clientName = name;

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
                // Blocked
                if (row.Cells["Blocked"].Value != null)
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
            if (chbReportImmediatelyLimitApp.Checked)
                setting += "1";
            else setting += "0";
            if (chbUpdateReportLimitApp.Checked)
                setting += "1";
            else setting += "0";
            if (chbBlockLimitApp.Checked)
                setting += "1";
            else setting += "0";

            setting += "\r\n";

            // insert five line -  Typing inappropriate words
            if (chbUpdateReportIinappropriateWords.Checked)
                setting += "1";
            else setting += "0";
            if (chbUpdateReportIinappropriateWords.Checked)
                setting += "1";
            else setting += "0";

            setting += "\r\n";

            // insert six line - num of dayly hour to limit
            if (txbNumOfLimitHours.Text.Equals(""))
                setting += "0\r\n";
            else
                setting += txbNumOfLimitHours.Text + "\r\n";


            // insert seven line -  Hours of use limitation
            // range1
            String range = rangeOfTime(dtpFrom1, dtpTo1);
            setting += range + " ";

            // range2
            range = rangeOfTime(dtpFrom2, dtpTo2);
            setting += range + " ";

            // range3
            range = rangeOfTime(dtpFrom3, dtpTo3);
            setting += range + " ";

            setting += "\r\n";

            // insert seven line - report time
            int select = chblFrequency.SelectedIndex;
            if (select < 0)
                ShowErrorDialog("Must Select Frequency to get report");
            else
            {
                setting += select + "\r\n";
                this.Close();
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
        private static bool IsValidDomainName(string name)
        {
            return Uri.CheckHostName(name) != UriHostNameType.Unknown;
        }

        private string rangeOfTime(DateTimePicker dtpFrom, DateTimePicker dtpTo)
        {
            if (dtpFrom.Value.TimeOfDay < dtpTo.Value.TimeOfDay)
            {
                String from = dtpFrom.Value.ToLongTimeString();
                String to = dtpTo.Value.ToLongTimeString();
                if (from != to)
                    return from + "-" + to;
            }
            return "NULL";
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

      
    }
}

