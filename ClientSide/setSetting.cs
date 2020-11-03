using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public setSetting(string settingString)
        {
            createFileStringSetting(settingString);
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

        public void createFileStringSetting(string stringSetting)
        {
           
            String filepath = Environment.CurrentDirectory;

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            filepath = (filepath + @"\setting.txt");

            if (!File.Exists(filepath))
            {
                using (StreamWriter sw = File.CreateText(filepath)) ;
            }
            using (FileStream sw = File.OpenWrite(filepath))
            {
                //  sw.Write(stringSetting,0,stringSetting.Length);

                Byte[] info = new UTF8Encoding(true).GetBytes(stringSetting); // Add some information to the file.
                //sw.Write(info, 0, info.Length);‏
                sw.Write(info, 0, info.Length);
            }

        }
       
    }
}
