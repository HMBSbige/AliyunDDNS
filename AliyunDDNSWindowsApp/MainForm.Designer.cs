namespace AliyunDDNSWindowsApp
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.NotifyMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示隐藏ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TriggerRun_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.ID_Box = new System.Windows.Forms.TextBox();
            this.Secret_Box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.RR_Box = new System.Windows.Forms.TextBox();
            this.Domain_Box = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.NotifyMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.NotifyMenu;
            this.notifyIcon1.Text = "AliyunDDNS-WindowsApp";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // NotifyMenu
            // 
            this.NotifyMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示隐藏ToolStripMenuItem,
            this.TriggerRun_MenuItem,
            this.toolStripSeparator1,
            this.退出ToolStripMenuItem});
            this.NotifyMenu.Name = "NotifyMenu";
            this.NotifyMenu.Size = new System.Drawing.Size(130, 76);
            // 
            // 显示隐藏ToolStripMenuItem
            // 
            this.显示隐藏ToolStripMenuItem.Name = "显示隐藏ToolStripMenuItem";
            this.显示隐藏ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.显示隐藏ToolStripMenuItem.Text = "显示/隐藏";
            this.显示隐藏ToolStripMenuItem.Click += new System.EventHandler(this.显示隐藏ToolStripMenuItem_Click);
            // 
            // TriggerRun_MenuItem
            // 
            this.TriggerRun_MenuItem.Name = "TriggerRun_MenuItem";
            this.TriggerRun_MenuItem.Size = new System.Drawing.Size(129, 22);
            this.TriggerRun_MenuItem.Text = "Start";
            this.TriggerRun_MenuItem.Click += new System.EventHandler(this.TriggerRun_MenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(126, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.Color.White;
            this.LogBox.Location = new System.Drawing.Point(12, 12);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.LogBox.Size = new System.Drawing.Size(544, 160);
            this.LogBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(481, 321);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ID_Box
            // 
            this.ID_Box.Location = new System.Drawing.Point(117, 209);
            this.ID_Box.Name = "ID_Box";
            this.ID_Box.Size = new System.Drawing.Size(197, 21);
            this.ID_Box.TabIndex = 3;
            // 
            // Secret_Box
            // 
            this.Secret_Box.Location = new System.Drawing.Point(117, 245);
            this.Secret_Box.Name = "Secret_Box";
            this.Secret_Box.PasswordChar = '*';
            this.Secret_Box.Size = new System.Drawing.Size(197, 21);
            this.Secret_Box.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "accessKeyId:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 248);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "accessKeySecret:";
            // 
            // RR_Box
            // 
            this.RR_Box.Location = new System.Drawing.Point(117, 286);
            this.RR_Box.Name = "RR_Box";
            this.RR_Box.Size = new System.Drawing.Size(197, 21);
            this.RR_Box.TabIndex = 7;
            // 
            // Domain_Box
            // 
            this.Domain_Box.Location = new System.Drawing.Point(117, 323);
            this.Domain_Box.Name = "Domain_Box";
            this.Domain_Box.Size = new System.Drawing.Size(197, 21);
            this.Domain_Box.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 291);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "主机记录：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 326);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "主域名：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(481, 292);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "保存信息";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 356);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Domain_Box);
            this.Controls.Add(this.RR_Box);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Secret_Box);
            this.Controls.Add(this.ID_Box);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LogBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "AliyunDDNS-WindowsApp";
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.NotifyMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip NotifyMenu;
        private System.Windows.Forms.ToolStripMenuItem 显示隐藏ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox ID_Box;
        private System.Windows.Forms.TextBox Secret_Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox RR_Box;
        private System.Windows.Forms.TextBox Domain_Box;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem TriggerRun_MenuItem;
        private System.Windows.Forms.Button button2;
    }
}

