using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerSide
{
    class Report
    {
        public static void createReportPDFFile(string report,int clientID)
        {


            try
            {
                string[] repoerLines = report.Split('\r');

                string projectDirectory = Environment.CurrentDirectory;
                string path = Directory.GetParent(projectDirectory).Parent.FullName;
                Document Report = new Document();
                string reportName = clientID.ToString()+"_" +repoerLines[0].Split('\n')[0].Replace(' ', '_') + ".pdf";
                reportName = reportName.Replace('/','_').Replace(':','_');
                if (File.Exists(Path.Combine(path, reportName)))
                {

                    System.Diagnostics.Process.Start(Path.Combine(path, reportName));
                }
                else
                {
                    PdfWriter.GetInstance(Report, new FileStream(path + "/" + reportName, FileMode.Create));
                    Report.Open();
                    Image jpg = Image.GetInstance(path + "/logo.JPG");
                    jpg.ScalePercent(12f);
                    jpg.SetAbsolutePosition(Report.PageSize.Width - 410f,
                          Report.PageSize.Height - 130f);

                    Report.Add(jpg);
                    Report.Add(new Paragraph("\n\n\n\n\n"));

                    for (int i = 0; i < repoerLines.Length; i++)
                    {
                        Report.Add(new Paragraph(repoerLines[i]));
                    }

                    Report.Close();


                    System.Diagnostics.Process.Start(Path.Combine(path, reportName));
                }
                



            }

            catch (Exception ex)
            {
                ShowErrorDialog("" + ex);
            }

             

        }


        public static void ShowErrorDialog(string message)
            {
                MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
    }
}
