using System;
using System.Windows.Forms;

namespace AliyunDDNSWindowsApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.huaji128;
            ChangeLogBox = UpdateLog;
        }
        private const int second = 1000;
        private const int minute = 59 * second;
        private string Domain;
        private string RR;
        private string accessKeyId, accessKeySecret;
        private readonly object thisLock = new object();

        private System.Threading.Timer threadTimer;

        private delegate void LogBoxCallBack(string str);

        private readonly LogBoxCallBack ChangeLogBox;

        private void UpdateLog(string str)
        {
            LogBox.AppendText(str);
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

                var Value = DDNS.GetLocalIP();
                if (Value == @"0.0.0.0")
                {
                    LogBox.Invoke(ChangeLogBox, @"获取公网 IP 出错，解析记录未改变" + Environment.NewLine);
                    return;
                }

                LogBox.Invoke(ChangeLogBox, DateTime.Now + ":\t" + Value + "\t");

                var SubDomain = RR + @"." + Domain;
                var lastValue = t1.GetSubDomainARecord(SubDomain);
                var lastRecordId = t1.GetSubDomainRecordId(SubDomain);

                if (lastValue != Value)
                {
                    if (t1.UpdateDomainRecord(RR, Domain, Value) == lastRecordId &&
                        t1.GetSubDomainARecord(SubDomain) == Value)
                    {
                        LogBox.Invoke(ChangeLogBox, @"解析记录更改成功" + Environment.NewLine);
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
                button1.Enabled = false;

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
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;

                threadTimer?.Dispose();
                LogBox.Invoke(ChangeLogBox, @"已停止..." + Environment.NewLine);

                ID_Box.Enabled = true;
                Secret_Box.Enabled = true;
                Domain_Box.Enabled = true;
                RR_Box.Enabled = true;

                button1.Text = @"Start";
                button1.Enabled = true;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            TriggerRun();
        }
    }
}