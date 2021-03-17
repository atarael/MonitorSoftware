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
    public class setSetting
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


        public setSetting(string settingString, string name, string id)
        {
            createFileStringSetting(settingString, name, id);
            this.settingString = settingString;  
            createOffensiveWordsList();
            reportFrequencyInSecond = buildReportFrequency();
            anotherSitesReport = buildAnotherSitesReportList();
            anotherSitesIgnore = buildAnotherSitesIgnoreList();
            buildCategoryList();// build triggersForAlert list and triggersForRepor list 
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
            if (settingStringSplited.Length > 5) {
                string setBadWord = settingStringSplited[4];
                if (setBadWord[0].Equals('1')) {
                    triggersForReport.Add("badWord");
                }
                if (setBadWord[1].Equals('1'))
                {
                    triggersForAlert.Add("badWord");
                }

                string[] anotherBadWord = settingStringSplited[5].Split(' ');
                foreach (string w in anotherBadWord)
                {
                    offensiveWords.Add(w);
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
            string[] settingStringSplited = settingString.Split('\n');
            if (settingStringSplited.Length > 6)
            {
                int frequency = int.Parse(settingStringSplited[6]);
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
                this.futureDateToReport = updateDate.ToString();
                //ShowErrorDialog("jjj"+updateDate.ToString());
            }
            else {
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

        /* private void createSettingFeature(string settingString)
         //The method will be updated with the settings features
         //The data obtained from the settings string from the server
         {

         }*/

        public void createFileStringSetting(string stringSetting, string name, string id)
        {

            String projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;


            String[] paths = new string[] { @filepath, "files" };
            filepath = Path.Combine(paths);
            // ShowErrorDialog("filepath in createFileStringSetting: " + filepath);
            // ShowErrorDialog("stringSetting createFileStringSetting: "+stringSetting);


            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            String settingFile = Path.Combine(filepath, "setting_" + id + ".txt");
            if (!System.IO.File.Exists(settingFile))
            {
                using (StreamWriter sw = System.IO.File.CreateText(settingFile)) ;

                System.IO.File.WriteAllText(settingFile, name + "\r\n" + id + "\r\n" + stringSetting);

            }


            /*
            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath)) ;
            }
            using (FileStream sw = File.OpenWrite(filepath))
            {
                //  sw.Write(stringSetting,0,stringSetting.Length);

                Byte[] info = new UTF8Encoding(true).GetBytes(name+"\r\n"+id+"\r\n"+stringSetting); // Add some information to the file.
                //sw.Write(info, 0, info.Length);‏
                sw.Write(info, 0, info.Length);
            }
            */
        }
        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
