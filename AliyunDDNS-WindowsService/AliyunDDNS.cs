using AliyunDDNSWindowsApp;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;

namespace AliyunDDNS_WindowsService
{
	[System.ComponentModel.DesignerCategory(@"Code")]
	public partial class AliyunDDNS : ServiceBase
	{
		public AliyunDDNS(IReadOnlyList<string> args)
		{
			InitializeComponent();
			if (args.Count == 5)
			{
				Domain = args[0];
				RR = args[1];
				accessKeyId = args[2];
				accessKeySecret = args[3];
				logPath = args[4];
			}
		}

		private System.Threading.Timer threadTimer;
		private const int second = 1000;
		private const int minute = 59 * second;
		private readonly object thisLock = new object();

		private string Domain;
		private string RR;
		private string accessKeyId;
		private string accessKeySecret;

		private string logPath;

		private void UpdateLog(string str)
		{
			File.AppendAllText(logPath, str, Encoding.UTF8);
		}

		private void Update(object state)
		{
			lock (thisLock)
			{
				var t1 = new DDNS(accessKeyId, accessKeySecret);

				var ip = Util.GetPublicIp();
				if (!Util.IsIPv4Address(ip))
				{
					UpdateLog(@"获取公网 IP 出错，解析记录未改变");
					return;
				}

				UpdateLog($@"公网 IP: {ip}");

				var subDomain = $@"{RR}.{Domain}";
				var lastIp = t1.GetSubDomainARecord(subDomain);
				var lastRecordId = t1.GetSubDomainRecordId(subDomain);

				if (lastIp != ip)
				{
					if (t1.UpdateDomainRecord(RR, Domain, ip) == lastRecordId &&
						t1.GetSubDomainARecord(subDomain) == ip)
					{
						UpdateLog($@"解析记录更改成功:{lastIp} → {ip}");
					}
					else
					{
						UpdateLog(@"解析记录更改失败，请检查输入是否正确");
					}
				}
				else
				{
					UpdateLog(@"公网 IP 未改变");
				}
			}
		}

		protected override void OnStart(string[] args)
		{
			UpdateLog(@"服务启动" + Environment.NewLine);
			if (args.Length == 5)
			{
				Domain = args[0];
				RR = args[1];
				accessKeyId = args[2];
				accessKeySecret = args[3];
				logPath = args[4];
			}
			threadTimer?.Dispose();
			threadTimer = new System.Threading.Timer(Update, null, 0, 6 * minute);
		}

		protected override void OnStop()
		{
			UpdateLog(@"服务停止" + Environment.NewLine);
			threadTimer?.Dispose();
			threadTimer = null;
		}
	}
}