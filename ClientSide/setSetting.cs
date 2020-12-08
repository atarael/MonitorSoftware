using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisioForge.Tools.TagLib.Riff;

namespace ClientSide
{
   public class setSetting
    {
       private  string settingString="";
        public  List<string> triggersForAlert = new List<string>();  //Triggers for immediate reporting
        public List<string> triggersForReport = new List<string>();
        public List<string> anotherSitesReport = new List<string>();  //another sites to monitoring -Sites that are not included in the categories
        public List<string> anotherSitesIgnore = new List<string>();//Sites that the server does not want to be reported
        private string[] wordImmediateReport= { "kill", "ostracism", "stab" };

        public setSetting(string settingString, string name, string id)
        {
            this.settingString = settingString;
            createFileStringSetting(settingString, name, id);
            buildCategoryList();// build triggersForAlert list and triggersForRepor list 
            buildAnotherSitesReportList();
           buildAnotherSitesIgnoreList();
        }

        private void buildAnotherSitesIgnoreList()
        {
            string [] settingStringArray = settingString.Split('\n')[2].Split(' ');
            foreach (var word in settingStringArray)
            {
                if (word != ""&&word!="\n") 
                {
                    anotherSitesIgnore.Add(word);
                    ShowErrorDialog("ignored Sites: " + word + "|");
                }
               
            }
        }

        private void buildAnotherSitesReportList()
        {
           string  [] settingStringArray = settingString.Split('\n')[1].Split(' ');
            foreach (var word in settingStringArray)
            {
                if (word != "" && word != "\t")
                {
                    anotherSitesReport.Add(word);
                    ShowErrorDialog("report Sites: " + word + "|"); 
                }
            
            }
            //ShowErrorDialog("report Sites: " + arr[1]);
        }

        //The method gets the settings string and builds a list that contains all the categories of sites 
        //that the user surfsthat require an alert and another list for all the categories that require reporting
        private void buildCategoryList()
        {
            string [] settingStringArray = settingString.Split('\n')[0].Split(' ');
            for(int i=0;i< settingStringArray.Length-1; i=i+2)
            {
                string category = settingStringArray[i];
                string settingArray = settingStringArray[i+1];
                
                if (settingArray[0] == '1')
                    triggersForAlert.Add(category);
                if (settingArray[1] == '1')
                    triggersForReport.Add(category);
            }

        

        }


      
        public string[] getWord()
        {

            return wordImmediateReport;
        }

       /* private void createSettingFeature(string settingString)
        //The method will be updated with the settings features
        //The data obtained from the settings string from the server
        {
          
        }*/

        public void createFileStringSetting(string stringSetting, string name, string id) {
           
            String projectDirectory = Environment.CurrentDirectory;
            string filepath = Directory.GetParent(projectDirectory).Parent.FullName;


            String[] paths = new string[] {@filepath, "files"};
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

                System.IO.File.WriteAllText(settingFile, name + "\r\n" + id+ "\r\n" + stringSetting);
                
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
