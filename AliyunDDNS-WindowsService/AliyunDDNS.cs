using System;
using System.IO;
using System.ServiceProcess;
using AliyunDDNSWindowsApp;

namespace AliyunDDNS_WindowsService
{
    public partial class AliyunDDNS : ServiceBase
    {
        public AliyunDDNS()
        {
            InitializeComponent();
        }
        private System.Threading.Timer threadTimer;
        private const int second = 1000;
        private const int minute = 59 * second;
        private readonly object thisLock = new object();

        private string Domain;
        private string RR;
        private string accessKeyId;
        private string accessKeySecret;
        
        private StreamWriter log;
        
        private void UpdateLogFile(string str)
        {
            //try
            //{
            //    log = new StreamWriter(@"AliyunDDNS.log", true, Encoding.UTF8);
            //}
            //catch (Exception)
            //{
            //    //MessageBox.Show(@"无法记录日志！" + Environment.NewLine + ex + Environment.NewLine, @"出错了",MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //log?.Write(str);
            //log?.Close();
        }
        
        private void Update(object state)
        {
            lock (thisLock)
            {
                var t1 = new DDNS(accessKeyId, accessKeySecret);

                var Value = DDNS.GetLocalIP(false);
                if (Value == @"0.0.0.0")
                {
                    UpdateLogFile(@"获取公网 IP 出错，解析记录未改变" + Environment.NewLine);
                    return;
                }
                
                UpdateLogFile(@"公网 IP: " + Value + Environment.NewLine);
                
                var SubDomain = RR + @"." + Domain;
                var lastValue = t1.GetSubDomainARecord(SubDomain);
                var lastRecordId = t1.GetSubDomainRecordId(SubDomain);

                if (lastValue != Value)
                {
                    if (t1.UpdateDomainRecord(RR, Domain, Value) == lastRecordId &&
                        t1.GetSubDomainARecord(SubDomain) == Value)
                    {
                        UpdateLogFile(@"解析记录更改成功:" + lastValue + @" → " + Value + Environment.NewLine);
                    }
                    else
                    {
                        UpdateLogFile(@"解析记录更改失败，请检查输入是否正确" + Environment.NewLine);
                    }

                }
                else
                {
                    UpdateLogFile(@"公网 IP 未改变" + Environment.NewLine);
                }
            }
        }
        
        protected override void OnStart(string[] args)
        {
            UpdateLogFile(@"服务启动");
           
            Domain = args[0];
            RR = args[1];
            accessKeyId = args[2];
            accessKeySecret = args[3];
            
            threadTimer?.Dispose();
            threadTimer = new System.Threading.Timer(Update, null, 0, 6 * minute);
        }

        protected override void OnStop()
        {
            UpdateLogFile(@"服务停止");
            threadTimer?.Dispose();
            threadTimer = null;
        }
    }
}
