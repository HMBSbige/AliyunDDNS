using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AliyunDDNSWindowsApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.huaji128;
            notifyIcon1.Icon = Properties.Resources.huaji128;
            ChangeLogBox = UpdateLog;
            if (!File.Exists(configfile))
            {
                UpdateLog(@"未找到配置文件" + Environment.NewLine);
            }
            else
            {
                try
                {
                    var config = Config.ReadConfig(configfile);
                    Domain_Box.Text = config[0];
                    RR_Box.Text = config[1];
                    ID_Box.Text = config[2];
                    Secret_Box.Text = config[3];
                }
                catch
                {
                    UpdateLog(@"读取配置失败！" + Environment.NewLine);
                    return;
                }
                UpdateLog(@"读取配置成功！" + Environment.NewLine);
            }
        }
        public const string configfile = @"AliyunDDNSconfig.dat";
        public const string logfile = @"AliyunDDNS.log";
        private const int second = 1000;
        private const int minute = 59 * second;
        private string Domain;
        private string RR;
        private string accessKeyId, accessKeySecret;
        private readonly object thisLock = new object();

        private StreamWriter log;
        private System.Threading.Timer threadTimer;

        private delegate void LogBoxCallBack(string str);

        private readonly LogBoxCallBack ChangeLogBox;

        private void UpdateLog(string str)
        {
            LogBox.AppendText(DateTime.Now + "\t" + str);
            if (checkBox2.Checked)
            {
                UpdateLogFile(DateTime.Now + "\t" + str);
            }
        }
        private void UpdateLogFile(string str)
        {
            try
            {
                log = new StreamWriter(logfile, true, Encoding.UTF8);
            }
            catch (Exception)
            {
                //MessageBox.Show(@"无法记录日志！" + Environment.NewLine + ex + Environment.NewLine, @"出错了",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            log?.Write(str);
            log?.Close();
        }
        private void TriggerMainFormDisplay()
        {
            Visible = !Visible;
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            TriggerMainFormDisplay();
        }

        private void 显示隐藏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TriggerMainFormDisplay();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Update(object state)
        {
            lock (thisLock)
            {
                var t1 = new DDNS(accessKeyId, accessKeySecret);

                var Value = DDNS.GetLocalIP(checkBox1.Checked);
                if (Value == @"0.0.0.0")
                {
                    LogBox.Invoke(ChangeLogBox, @"获取公网 IP 出错，解析记录未改变" + Environment.NewLine);
                    return;
                }

                LogBox.Invoke(ChangeLogBox, @"公网 IP: "+ Value + Environment.NewLine);

                var SubDomain = RR + @"." + Domain;
                var lastValue = t1.GetSubDomainARecord(SubDomain);
                var lastRecordId = t1.GetSubDomainRecordId(SubDomain);

                if (lastValue != Value)
                {
                    if (t1.UpdateDomainRecord(RR, Domain, Value) == lastRecordId &&
                        t1.GetSubDomainARecord(SubDomain) == Value)
                    {
                        LogBox.Invoke(ChangeLogBox, @"解析记录更改成功:" + lastValue + @" → " + Value + Environment.NewLine);
                    }
                    else
                    {
                        LogBox.Invoke(ChangeLogBox, @"解析记录更改失败，请检查输入是否正确" + Environment.NewLine);
                    }

                }
                else
                {
                    LogBox.Invoke(ChangeLogBox, @"公网 IP 未改变" + Environment.NewLine);
                }
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                return;
            TriggerMainFormDisplay();
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Config.WriteConfig(Domain_Box.Text, RR_Box.Text, ID_Box.Text, Secret_Box.Text);
            }
            catch
            {
                LogBox.Invoke(ChangeLogBox, @"保存失败" + Environment.NewLine);
                return;
            }
            LogBox.Invoke(ChangeLogBox, @"保存成功" + Environment.NewLine);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                TriggerMainFormDisplay();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (button1.Text == @"Stop")
            {
                StopListen();
            }
        }

        private void StopListen()
        {
            button1.Enabled = false;
            TriggerRun_MenuItem.Enabled = false;

            threadTimer?.Dispose();
            LogBox.Invoke(ChangeLogBox, @"已停止..." + Environment.NewLine);

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
            threadTimer = new System.Threading.Timer(Update, null, 0, 6 * minute);

            button1.Text = @"Stop";
            TriggerRun_MenuItem.Text = @"Stop";
            button1.Enabled = true;
            TriggerRun_MenuItem.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Service.Install();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TriggerRun();
        }
    }
}