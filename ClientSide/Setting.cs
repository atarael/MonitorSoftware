using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Tools.TagLib.Riff;

namespace ClientSide
{


    public sealed class Setting
    {
        private string settingString = "";
        public List<string> triggersForAlert = new List<string>();  //Triggers for immediate reporting
        public List<string> triggersForReport = new List<string>();
        public List<string> anotherSitesReport;  //another sites to monitoring -Sites that are not included in the categories
        public List<string> anotherSitesIgnore;//Sites that the server does not want to be reported
        // private string[] wordImmediateReport = { "kill", "ostracism", "stab" };
        public string futureDateToReport = ""; //Future date for reporting in string
        public double reportFrequencyInSecond; //Frequency of reporting in seconds
        public string reportFrequencyInWord = "";//daily,weekly..
        private List<string> offensiveWords = new List<string>();
        public string email;


        private static readonly Setting instance = new Setting();
        static Setting()
        {
        }
        private Setting()
        {

            settingString = readSettingFromDB();
            createOffensiveWordsList();
            installationSetting();
            reportFrequencyInSecond = buildReportFrequency();
            anotherSitesReport = buildAnotherSitesReportList();
            anotherSitesIgnore = buildAnotherSitesIgnoreList();
            buildCategoryList();// build triggersForAlert list and triggersForRepor list 
            getEmail(settingString);

            // connect to DB
            DBclient DBInstance = DBclient.Instance;
            DBInstance.removeIgnoredSites(anotherSitesIgnore.ToArray());
            DBInstance.funAddCategorySiteTable(anotherSitesReport.ToArray(), "anotherSitesReport");


        }
        public static Setting Instance
        {
            get
            {
                return instance;
            }
        }




        private void getEmail(string settingString)
        {
            string[] settingStringSplited = settingString.Split('\n');
            if (settingStringSplited.Length > 6)
            {
                email = settingStringSplited[6].Split('\r')[0];
                //ShowErrorDialog("email is: |" + email + "|");

            }
        }
        // s atat atara sara 
        private void installationSetting()
        {
            string[] settingStringSplited = settingString.Split('\n');
            if (settingStringSplited.Length > 3)
            {
                string setInstallation = settingStringSplited[3];
                if (setInstallation[0].Equals('1'))
                {
                    triggersForReport.Add("installation");
                }
                if (setInstallation[1].Equals('1'))
                {
                    triggersForAlert.Add("installation");
                }

            }
        }

        private void createOffensiveWordsList()
        {
            String projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            String[] paths = new string[] { @filepath, "bad-words.txt" };
            filepath = Path.Combine(paths);
            string content = System.IO.File.ReadAllText(filepath);
            string word = "";
            foreach (char s in content)
            {
                if (s != '\n')
                    word += s;
                else
                {
                    offensiveWords.Add(word.Split('\r')[0]);
                    word = "";
                }
            }

            string[] settingStringSplited = settingString.Split('\n');
            if (settingStringSplited.Length > 5)
            {
                string setBadWord = settingStringSplited[4];
                if (setBadWord[0].Equals('1'))
                {
                    triggersForReport.Add("badWord");
                }
                if (setBadWord[1].Equals('1'))
                {
                    triggersForAlert.Add("badWord");
                }

                string[] anotherBadWord = settingStringSplited[5].Split(' ');
                foreach (string w in anotherBadWord)
                {
                    offensiveWords.Add(w.Split('\r')[0]);
                }
            }






            // foreach (string l in offensiveWords) { ShowErrorDialog(l); }
        }


        private double buildReportFrequency()
        {
            int second = 0;
            DateTime updateDate = DateTime.Now;

            /* foreach(var x in settingString.Split('\n'))
             {
                 ShowErrorDialog("settttt" + x);
             }*/
             if(settingString == string.Empty)
            {
                return 0;
            }
            string[] settingStringSplited = settingString.Split('\n');


            if (settingStringSplited.Length > 7)
            {
                // frequency by minutes
                string frequencyStr = settingStringSplited[7];
                if (frequencyStr.Split(' ')[0] == "minute")
                {

                    second = int.Parse(frequencyStr.Split(' ')[1]) * 60;
                    updateDate = updateDate.AddDays(0);


                }
                // frequency by days
                else
                {
                    int frequency = int.Parse(settingStringSplited[7]);
                    if (frequency == 0)//if frequency=each day
                    {
                        reportFrequencyInWord = "daily";
                        second = 60 * 60 * 24;
                        updateDate = updateDate.AddDays(1);
                    }

                    if (frequency == 1)//if frequency=each week
                    {
                        reportFrequencyInWord = "weekly";
                        second = 60 * 60 * 24 * 7;
                        updateDate = updateDate.AddDays(7);
                    }
                    if (frequency == 2)//if frequency=once a  two weeks 
                    {
                        reportFrequencyInWord = "bi-monthly";
                        second = 60 * 60 * 24 * 14;
                        updateDate = updateDate.AddDays(14);
                    }
                    if (frequency == 3)
                    {
                        reportFrequencyInWord = "monthly";
                        second = 60 * 60 * 24 * 30;//if frequency=once a month
                        updateDate = updateDate.AddMonths(1);
                    }
                }



                this.futureDateToReport = updateDate.ToString();
                //ShowErrorDialog("jjj"+updateDate.ToString());
            }
            else
            {
                ShowErrorDialog("frequency not exist");
            }
            return Convert.ToDouble(second);

        }

        private List<string> buildAnotherSitesIgnoreList()
        {
            anotherSitesIgnore = new List<string>();
            string[] settingStringSplited = settingString.Split('\n');
            if (settingStringSplited.Length > 2)
            {
                string[] settingStringArray = settingStringSplited[2].Split(' ');
                foreach (var word in settingStringArray)
                {
                    if (word != "" && word != "\n")
                    {
                        anotherSitesIgnore.Add(word);
                        //ShowErrorDialog("ignored Sites: " + word + "|");
                    }
                }
            }
            return anotherSitesIgnore;
        }

        private List<string> buildAnotherSitesReportList()
        {
            anotherSitesReport = new List<string>();
            string[] settingStringArray = settingString.Split('\n')[1].Split(' ');
            foreach (var word in settingStringArray)
            {
                if (word != "" && word != "\t")
                {
                    anotherSitesReport.Add(word);
                    // ShowErrorDialog("report Sites: " + word + "|");
                }

            }
            //ShowErrorDialog("report Sites: " + arr[1]);
            return anotherSitesReport;
        }

        //The method gets the settings string and builds a list that contains all the categories of sites 
        //that the user surfsthat require an alert and another list for all the categories that require reporting
        private void buildCategoryList()
        {
            string[] settingStringArray = settingString.Split('\n')[0].Split(' ');
            for (int i = 0; i < settingStringArray.Length - 1; i = i + 2)
            {
                string category = settingStringArray[i];
                string settingArray = settingStringArray[i + 1];

                if (settingArray[0] == '1')
                    triggersForAlert.Add(category);
                if (settingArray[1] == '1')
                    triggersForReport.Add(category);
            }
            triggersForAlert.Add("anotherSitesReport");
            triggersForReport.Add("anotherSitesReport");

        }

        public List<string> getWord()
        {

            return offensiveWords;
        }

        public string readSettingFromDB()
        {
            //string userName = Environment.UserName;

            //String projectDirectory = Environment.CurrentDirectory;
            //string filepath = Directory.GetParent(projectDirectory).Parent.FullName;
            //String[] paths = new string[] { @filepath, "files" };
            //filepath = Path.Combine(paths);
            //string setting = "";
            //DirectoryInfo d = new DirectoryInfo(filepath);//Assuming Test is your Folder
            ////ShowErrorDialog("filepath is: \n" + filepath);
            //if (!Directory.Exists(filepath))
            //{
            //    return "";
            //}

            //using (StreamReader sr = System.IO.File.OpenText(Path.Combine(filepath, "setting_" + userName + ".txt")))
            //{
            //    sr.ReadLine();
            //    sr.ReadLine();

            //    //ShowErrorDialog("id\n" + id);
            //    string line = "";
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        //ShowErrorDialog("line\n" + line);
            //        setting += line + "\r\n";
            //    }
            //}

            DBclient DBInstance = DBclient.Instance;
            string setting = DBInstance.getGeneralDetailsTable("setting");
            return setting;
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
