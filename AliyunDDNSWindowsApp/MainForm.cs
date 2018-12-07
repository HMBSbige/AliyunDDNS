using AliyunDDNSWindowsApp.Properties;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace AliyunDDNSWindowsApp
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			Icon = Resources.huaji;
			notifyIcon1.Icon = Resources.huaji;
		}

		public const string configFile = @"AliyunDDNSconfig.dat";
		private const string logfile = @"AliyunDDNS.log";
		private const int second = 1000;
		private const int minute = 59 * second;
		private string Domain;
		private string RR;
		private string accessKeyId, accessKeySecret;
		private readonly object thisLock = new object();

		private Timer threadTimer;

		private void MainForm_Load(object sender, EventArgs e)
		{
			LoadConfig();
		}

		#region 主窗口显示隐藏

		private void TriggerMainFormDisplay()
		{
			Visible = !Visible;
			if (WindowState == FormWindowState.Minimized)
			{
				WindowState = FormWindowState.Normal;
			}
		}

		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			TriggerMainFormDisplay();
		}

		private void 显示隐藏ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			TriggerMainFormDisplay();
		}

		private void MainForm_SizeChanged(object sender, EventArgs e)
		{
			if (WindowState != FormWindowState.Minimized)
			{
				return;
			}
			TriggerMainFormDisplay();
		}

		#endregion

		#region 退出

		private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Exit();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing)
			{
				var dr = MessageBox.Show(@"「是」退出，「否」最小化", @"是否退出？", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
				if (dr == DialogResult.Yes)
				{
					Exit();
				}
				else if (dr == DialogResult.No)
				{
					e.Cancel = true;
					TriggerMainFormDisplay();
				}
				else
				{
					e.Cancel = true;
				}
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			//TODO
			if (button1.Text == @"Stop")
			{
				StopListen();
			}
		}

		private void Exit()
		{
			Dispose();
			Environment.Exit(0);
		}

		#endregion

		#region StartDDNS

		private void Update(object state)
		{
			lock (thisLock)
			{
				var t1 = new DDNS(accessKeyId, accessKeySecret);

				var ip = Util.GetPublicIp(checkBox1.Checked);
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
					if (t1.UpdateDomainRecord(RR, Domain, ip) == lastRecordId && t1.GetSubDomainARecord(subDomain) == ip)
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

		private void TriggerRun()
		{
			if (button1.Text == @"Start")
			{
				StartListen();
			}
			else
			{
				StopListen();
			}
		}

		private void TriggerRun_MenuItem_Click(object sender, EventArgs e)
		{
			TriggerRun();
		}

		private void StopListen()
		{
			button1.Enabled = false;
			TriggerRun_MenuItem.Enabled = false;

			threadTimer?.Dispose();
			UpdateLog(@"已停止...");

			ID_Box.Enabled = true;
			Secret_Box.Enabled = true;
			Domain_Box.Enabled = true;
			RR_Box.Enabled = true;

			button1.Text = @"Start";
			TriggerRun_MenuItem.Text = @"Start";
			button1.Enabled = true;
			TriggerRun_MenuItem.Enabled = true;
		}

		private void StartListen()
		{
			button1.Enabled = false;
			TriggerRun_MenuItem.Enabled = false;

			accessKeyId = ID_Box.Text;
			ID_Box.Enabled = false;
			accessKeySecret = Secret_Box.Text;
			Secret_Box.Enabled = false;
			Domain = Domain_Box.Text;
			Domain_Box.Enabled = false;
			RR = RR_Box.Text;
			RR_Box.Enabled = false;

			threadTimer?.Dispose();
			threadTimer = new Timer(Update, null, 0, 6 * minute);

			button1.Text = @"Stop";
			TriggerRun_MenuItem.Text = @"Stop";
			button1.Enabled = true;
			TriggerRun_MenuItem.Enabled = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			TriggerRun();
		}

		#endregion

		#region Log

		private void UpdateLog(string str)
		{
			var sb = $@"{DateTime.Now}	{str}{Environment.NewLine}";
			LogBox.Invoke(new Action(() => { LogBox.AppendText(sb); }));
			UpdateLogFile(sb);
		}

		private static void UpdateLogFile(string str)
		{
			File.AppendAllText(logfile, str, Encoding.UTF8);
		}

		#endregion

		#region Config

		private void LoadConfig()
		{
			if (!File.Exists(configFile))
			{
				UpdateLog(@"未找到配置文件");
			}
			else
			{
				try
				{
					var config = Config.ReadConfig(configFile);
					Domain_Box.Text = config[0];
					RR_Box.Text = config[1];
					ID_Box.Text = config[2];
					Secret_Box.Text = config[3];
				}
				catch
				{
					UpdateLog(@"读取配置失败！");
					return;
				}

				UpdateLog(@"读取配置成功！");
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			try
			{
				Config.WriteConfig(Domain_Box.Text, RR_Box.Text, ID_Box.Text, Secret_Box.Text);
			}
			catch
			{
				UpdateLog(@"保存失败");
				return;
			}

			UpdateLog(@"保存成功");
		}

		#endregion

		#region WindowsService

		private void button3_Click(object sender, EventArgs e)
		{
			try
			{
				button3.Enabled = false;
				var t = new Task(() =>
				{
					Service.ReInstall();
					Service.AutoStartup(new[]
					{
							Domain_Box.Text, RR_Box.Text, ID_Box.Text, Secret_Box.Text,
							Application.StartupPath + @"\" + logfile
					});
					Service.SetRecoveryOptions();
					Service.Run();
				});
				t.Start();
				t.ContinueWith(task =>
				{
					BeginInvoke(new Action(() =>
					{
						if (Service.ServiceIsExisted())
						{
							MessageBox.Show(@"Windows 服务安装成功，并且已设置开机自启动", @"成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else
						{
							MessageBox.Show(@"Windows 服务安装失败", @"出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}

						button3.Enabled = true;
					}));
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, @"出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			try
			{
				button4.Enabled = false;
				var t = new Task(Service.UnInstall);
				t.Start();
				t.ContinueWith(task =>
				{
					BeginInvoke(new Action(() =>
					{
						if (!Service.ServiceIsExisted())
						{
							MessageBox.Show(@"Windows 服务卸载成功", @"成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else
						{
							MessageBox.Show(@"Windows 服务卸载失败", @"出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}

						button4.Enabled = true;
					}));
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, @"出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion



	}
}