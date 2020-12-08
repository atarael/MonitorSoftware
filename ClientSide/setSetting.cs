using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientSide
{
   public class setSetting
    {
       private  string settingString="";
        private string[] triggersImmediateReport;//Triggers for immediate reporting
        private string[] triggersForReport; //Triggers for  reporting
        private string[] triggersForBlock;//Triggers to block operation
        private string sitesBlockReport; //Sites to block
        private string[] sitesEnable; //Sites to enable
        private string[] wordImmediateReport= { "kill", "ostracism", "stab" };

        public setSetting(string settingString, string name, string id)
        {
            createFileStringSetting(settingString, name, id);
            //createSettingFeature(settingString);
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
            if (!File.Exists(settingFile))
            {
                using (StreamWriter sw = File.CreateText(settingFile));               
                File.WriteAllText(settingFile, name + "\r\n" + id.Split('\r', '\n')[0] + "\r\n" + stringSetting);
                
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
