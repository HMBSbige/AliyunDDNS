using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace AliyunDDNSWindowsApp
{
    internal static class Service
    {
        public static void Install()
        {
            string sysDisk = Environment.SystemDirectory.Substring(0, 3);
            string dotNetPath = sysDisk + @"WINDOWS\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe";
            string serviceEXEPath = Application.StartupPath + @"\AliyunDDNS-WindowsService.exe";
            string serviceInstallCommand = $@"{dotNetPath}  {serviceEXEPath}";//安装服务时使用的dos命令
            string serviceUninstallCommand = $@"{dotNetPath} -U {serviceEXEPath}";//卸载服务时使用的dos命令
            
            try
            {
                if (File.Exists(dotNetPath))
                {
                    string[] cmd = { serviceUninstallCommand };
                    Cmd(cmd);
                    CloseProcess(@"cmd.exe");
                }
            }
            catch
            {
                //
            }
            
            Thread.Sleep(1000);
            
            try
            {
                if (File.Exists(dotNetPath))
                {
                    string[] cmd =  { serviceInstallCommand };
                    Cmd(cmd);
                    CloseProcess(@"cmd.exe");
                }
            }
            catch
            {
                //
            }
            
            try
            {
                Thread.Sleep(3000);
                
                ServiceController sc = new ServiceController(@"AliyunDDNS");
                
                if (sc.Status.Equals(ServiceControllerStatus.Stopped) || 
                    sc.Status.Equals(ServiceControllerStatus.StopPending))
                {
                    sc.Start(
                    new []{ Application.StartupPath + @"\" + MainForm.configfile }
                    );
                }
                sc.Refresh();
            }
            catch
            {
                //
            }
        }
        
        /// <summary>
        /// 运行CMD命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        private static string Cmd(string[] cmd)
        {
            Process p = new Process
            {
                StartInfo =
                {
                    FileName = @"cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };
            p.Start();
            p.StandardInput.AutoFlush = true;
            foreach (string t in cmd)
            {
                p.StandardInput.WriteLine(t);
            }
            p.StandardInput.WriteLine(@"exit");
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            return strRst;
        }


        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="ProcName">进程名称</param>
        /// <returns></returns>
        private static bool CloseProcess(string ProcName)
        {
            bool result = false;
            System.Collections.ArrayList procList = new System.Collections.ArrayList();
            foreach (System.Diagnostics.Process thisProc in System.Diagnostics.Process.GetProcesses())
            {
                var tempName = thisProc.ToString();
                var begpos = tempName.IndexOf(@"(", StringComparison.Ordinal) + 1;
                var endpos = tempName.IndexOf(@")", StringComparison.Ordinal);
                tempName = tempName.Substring(begpos, endpos - begpos);
                procList.Add(tempName);
                if (tempName == ProcName)
                {
                    if (!thisProc.CloseMainWindow())
                        thisProc.Kill(); // 当发送关闭窗口命令无效时强行结束进程
                    result = true;
                }
            }
            return result;
        }
    }
}
