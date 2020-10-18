using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ClientSide
{
    class ShowAllProcess
    {

     
            /// <summary>
            /// Returns a string containing information on running processes
            /// </summary>
            /// <param name="tb"></param>
            public static string ListAllProcesses()
            {
                StringBuilder sb = new StringBuilder();

                // list out all processes and write them into a stringbuilder
                ManagementClass MgmtClass = new ManagementClass("Win32_Process");

                foreach (ManagementObject mo in MgmtClass.GetInstances())
                {
                    sb.Append("Name:\t" + mo["Name"] + Environment.NewLine);
                    sb.Append("ID:\t" + mo["ProcessId"] + Environment.NewLine);
                    sb.Append(Environment.NewLine);
                }

                return sb.ToString();
            }



            /// <summary>
            /// Returns a string containing information on running processes
            /// </summary>
            /// <returns></returns>
            public static string ListAllApplications()
            {
                StringBuilder sb = new StringBuilder();

                foreach (Process p in Process.GetProcesses("."))
                {
                    try
                    {
                        if (p.MainWindowTitle.Length > 0)
                        {
                            sb.Append("Window Title:\t" + p.MainWindowTitle.ToString() + Environment.NewLine);
                            sb.Append("Process Name:\t" + p.ProcessName.ToString() + Environment.NewLine);
                            sb.Append("Window Handle:\t" + p.MainWindowHandle.ToString() + Environment.NewLine);
                            sb.Append("Memory Allocation:\t" + p.PrivateMemorySize64.ToString() + Environment.NewLine);
                            sb.Append(Environment.NewLine);
                        }
                    }
                    catch { }
                }

                return sb.ToString();
            }



            /// <summary>
            /// List all processes by image name
            /// </summary>
            /// <returns></returns>
            public static string ListAllByImageName()
            {

                StringBuilder sb = new StringBuilder();

                foreach (Process p in Process.GetProcesses("."))
                {
                    try
                    {
                        foreach (ProcessModule pm in p.Modules)
                        {
                            sb.Append("Image Name:\t" + pm.ModuleName.ToString() + Environment.NewLine);
                            sb.Append("File Path:\t\t" + pm.FileName.ToString() + Environment.NewLine);
                            sb.Append("Memory Size:\t" + pm.ModuleMemorySize.ToString() + Environment.NewLine);
                            sb.Append("Version:\t\t" + pm.FileVersionInfo.FileVersion.ToString() + Environment.NewLine);
                            sb.Append(Environment.NewLine);
                        }
                    }
                    catch { }
                }

                return sb.ToString();
            }



            /// <summary>
            /// Determine if a process is running by name
            /// </summary>
            /// <param name="processName"></param>
            /// <returns></returns>
            public static bool CheckForProcessByName(string processName)
            {

                ManagementClass MgmtClass = new ManagementClass("Win32_Process");
                bool rtnVal = false;

                foreach (ManagementObject mo in MgmtClass.GetInstances())
                {
                    if (mo["Name"].ToString().ToLower() == processName.ToLower())
                    {
                        rtnVal = true;
                    }
                }

                return rtnVal;
            }


            /// <summary>
            /// Determine if a process is running by image name
            /// </summary>
            /// <param name="processName"></param>
            /// <returns></returns>
            public static bool CheckForProcessByImageName(string processImageName)
            {

                bool rtnVal = false;

                foreach (Process p in Process.GetProcesses("."))
                {
                    try
                    {
                        foreach (ProcessModule pm in p.Modules)
                        {
                            if (pm.ModuleName.ToLower() == processImageName.ToLower())
                                rtnVal = true;
                        }
                    }
                    catch { }
                }

                return rtnVal;
            }


            /// <summary>
            /// Determine if an application is running by name
            /// </summary>
            /// <param name="AppName"></param>
            /// <returns></returns>
            public static bool CheckForApplicationByName(string AppName)
            {
                bool bRtn = false;

                foreach (Process p in Process.GetProcesses("."))
                {
                    try
                    {
                        if (p.ProcessName.ToString().ToLower() == AppName.ToLower())
                        {
                            bRtn = true;
                        }
                    }
                    catch { }
                }

                return bRtn;
            }




            /// <summary>
            /// Check for the existence of a process by ID; if the ID
            /// is found, the method will return a true
            /// </summary>
            /// <param name="processId"></param>
            /// <returns></returns>
            public static bool FindProcessById(string processId)
            {
                ManagementClass MgmtClass = new ManagementClass("Win32_Process");
                bool rtnVal = false;

                foreach (ManagementObject mo in MgmtClass.GetInstances())
                {
                    if (mo["ProcessId"].ToString() == processId)
                    {
                        rtnVal = true;
                    }
                }

                return rtnVal;
            }


        }
}
