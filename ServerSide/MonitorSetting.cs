using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
        private string stringAllBlockSites="";
        private string stringAllUnBlockSites="";
        private string stringAllBadWords="";

        public MonitorSetting(int id)
        {
            this.id = id;
            //DBserver intance = DBserver.Instance;
            //string setting = intance.getStringById(id);
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

        private void fillAllSetting(string setting)
        {
            string[] allsetting = setting.Split('\n');

            // set all category site setting
            string[] categoryString = allsetting[0].Split('\n')[0].Split(' ');
            List<string> categorySetting = new List<String>(); 
            for(int i=1; i< categoryString.Length; i += 2)
            {
                categorySetting.Add(categoryString[i]);
            }
          
            for (int i = 0; i < CategorySite.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dtgCategorySites.Rows[i].Clone();
                row.Cells[0].Value = CategorySite[i];
                String settingArray = categorySetting[i];

                if (settingArray[0] == '1')
                {
                    row.Cells[1].Value = true;

                }

                if (settingArray[1] == '1')
                {
                    row.Cells[2].Value = true;

                }
                
                dtgCategorySites.Rows.Add(row);
            }
            dtgCategorySites.AllowUserToAddRows = false;
            if (allsetting.Length > 7) {
                
                // block site
                if (allsetting[1] != "\r")
                {
                    string[] allBlockSites = allsetting[1].Replace('\r',' ').Split(' ');
                    for (int i = 0; i < allBlockSites.Length; i++) {
                        addSiteToMonitor += allBlockSites[i]+" ";
                    }
                     
                }
               
                // unblocked site
                if (allsetting[2] != "\r") 
                {
                    string[] allUnBlockSites = allsetting[2].Replace('\r', ' ').Split(' ');
                    for (int i = 0; i < allUnBlockSites.Length; i++)
                    {
                       addSiteToCancelMonitor += allUnBlockSites[i] + " ";
                        
                    }
                     
                }
               
                // installation
                if (allsetting[3] != "\r")
                {
                
                    string installationSetting = allsetting[3];

                    if (installationSetting[0] == '1')
                    {
                        chbAppInstallUpdate.Checked = true;
                    }
                    if (installationSetting[1] == '1')
                    {
                        chbAppInstallReport.Checked = true;
                    }

                }
                
                // bad word
                if (allsetting[4] != "\r")
                {
                    string BadWordSetting = allsetting[4];

                    if (BadWordSetting[0] == '1')
                    {
                        chbBadWordUpdate.Checked = true;
                    }
                    if (BadWordSetting[1] == '1')
                    {
                        chbBadWordReport.Checked = true;
                    }

                }
     
                // checked bad word 
                if (allsetting[5] != "\r") 
                {
                     
                    string[] allBadWord = allsetting[5].Replace('\r',' ').Split(' ');
                     
                    for (int i = 0; i < allBadWord.Length; i++)
                    {                     
                        stringAllBadWords += allBadWord[i] + "\n";
                        AddBadWords += allBadWord[i] + " ";
                    }


                }
           
                // mail address
                if (allsetting[6] != "\r")  
                {
                    txbEmail.Text = allsetting[6].Split('\r')[0];

                }
        
                // frequency
                if (allsetting[7] != "\r") 
                {
                    string frequencySetting = allsetting[7];
                    

                }

            }


        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            setting = "";
            // insert first line - all category. format: <CategorySite>XXX, where X is 1-selected or 0-not selected
            int numOfCategory = dtgCategorySites.Rows.Count;

            for (int i = 0; i < dtgCategorySites.Rows.Count; i++)
            {
                DataGridViewRow row = (DataGridViewRow)dtgCategorySites.Rows[i];

                // insert Category Name
                if(row.Cells[0].Value!= null)
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
            ShowErrorDialog(setting);
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
       

        DBserver DBInstance = DBserver.Instance;
        string setting = DBInstance.getSttingById(id);
        if (setting != string.Empty)
        {
            fillAllSetting(setting);
        }
        else
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
                if (lblUnBlockSites.Text.Length == 0)
                {
                    lblUnBlockSites.Text += url;
                }
                else
                {
                    lblUnBlockSites.Text += ", " + url;
                }

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

        private void btnShowBlockedSites_Click(object sender, EventArgs e)
        {
            if (addSiteToMonitor.Length > 0)
            {
                string siteblock = Regex.Replace(addSiteToMonitor, @"\  \b", "\n");
                ShowErrorDialog("All added blocking sites is:\n" + siteblock);
            }
            else
            {
                ShowErrorDialog("NO blocking sites to show");
            }
        }

        private void btnShowUnBlockedSites_Click(object sender, EventArgs e)
        {
            if (addSiteToCancelMonitor.Length >0)
            {

                string siteUnblock = Regex.Replace(addSiteToCancelMonitor, @"\  \b", "\n");
                ShowErrorDialog("All added blocking sites is:\n" + siteUnblock);
            }
            else
            {
                ShowErrorDialog("NO Unblocking sites to show");
            }
        }

        private void btnShowBadWord_Click(object sender, EventArgs e)
        {
             
            if (AddBadWords.Length >0)
            {
 

                string newWords = Regex.Replace(AddBadWords, @"\  \b", "\n");
                 
                ShowErrorDialog("All added Bad Words is:\n" + newWords );
            }
            else
            {
                ShowErrorDialog("NO Words to show");
            }
        }
    
    }
}

