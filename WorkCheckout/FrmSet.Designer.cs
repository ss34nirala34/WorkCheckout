namespace WorkCheckout
{
    partial class FrmSet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnUserOk = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPassWord = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoDefaultBrowser = new System.Windows.Forms.RadioButton();
            this.rdoOwnBrowser = new System.Windows.Forms.RadioButton();
            this.AutoStartupCheckBox = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkTimingTipsSW = new System.Windows.Forms.CheckBox();
            this.chkAutoTipsSW = new System.Windows.Forms.CheckBox();
            this.lstCheckInDay = new System.Windows.Forms.ListBox();
            this.ctmLstCheckInDay = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.默认ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbCheckInDay = new System.Windows.Forms.ComboBox();
            this.chkAutoCheckIn = new System.Windows.Forms.CheckBox();
            this.dtpTimingSW = new System.Windows.Forms.DateTimePicker();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbEnd = new System.Windows.Forms.ComboBox();
            this.cmbStart = new System.Windows.Forms.ComboBox();
            this.chkAutoCheckOut = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkApartTipsAW = new System.Windows.Forms.CheckBox();
            this.txtApart = new System.Windows.Forms.TextBox();
            this.chkTips9AW = new System.Windows.Forms.CheckBox();
            this.chkTipsAW = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkWeiboRemote = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txttxtWeiboPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtWeiboUserName = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.chkTim = new System.Windows.Forms.CheckBox();
            this.dtpTimOut = new System.Windows.Forms.DateTimePicker();
            this.tab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.ctmLstCheckInDay.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabPage1);
            this.tab.Controls.Add(this.tabPage2);
            this.tab.Controls.Add(this.tabPage3);
            this.tab.Controls.Add(this.tabPage5);
            this.tab.Controls.Add(this.tabPage4);
            this.tab.Location = new System.Drawing.Point(12, 12);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(398, 296);
            this.tab.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.AutoStartupCheckBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(390, 270);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnUserOk);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtPassWord);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtUserName);
            this.groupBox3.Location = new System.Drawing.Point(6, 29);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(378, 78);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "自动签出签入用户名密码";
            // 
            // btnUserOk
            // 
            this.btnUserOk.Location = new System.Drawing.Point(245, 41);
            this.btnUserOk.Name = "btnUserOk";
            this.btnUserOk.Size = new System.Drawing.Size(75, 23);
            this.btnUserOk.TabIndex = 4;
            this.btnUserOk.Text = "确定";
            this.btnUserOk.UseVisualStyleBackColor = true;
            this.btnUserOk.Click += new System.EventHandler(this.btnUserOk_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "密码：";
            // 
            // txtPassWord
            // 
            this.txtPassWord.Location = new System.Drawing.Point(66, 43);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(100, 21);
            this.txtPassWord.TabIndex = 2;
            this.txtPassWord.TextChanged += new System.EventHandler(this.txtPassWord_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "用户名：";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(66, 17);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(100, 21);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoDefaultBrowser);
            this.groupBox1.Controls.Add(this.rdoOwnBrowser);
            this.groupBox1.Location = new System.Drawing.Point(3, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(378, 65);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "签入签出方式";
            // 
            // rdoDefaultBrowser
            // 
            this.rdoDefaultBrowser.AutoSize = true;
            this.rdoDefaultBrowser.Location = new System.Drawing.Point(6, 42);
            this.rdoDefaultBrowser.Name = "rdoDefaultBrowser";
            this.rdoDefaultBrowser.Size = new System.Drawing.Size(107, 16);
            this.rdoDefaultBrowser.TabIndex = 1;
            this.rdoDefaultBrowser.TabStop = true;
            this.rdoDefaultBrowser.Text = "调用系统浏览器";
            this.rdoDefaultBrowser.UseVisualStyleBackColor = true;
            // 
            // rdoOwnBrowser
            // 
            this.rdoOwnBrowser.AutoSize = true;
            this.rdoOwnBrowser.Location = new System.Drawing.Point(6, 20);
            this.rdoOwnBrowser.Name = "rdoOwnBrowser";
            this.rdoOwnBrowser.Size = new System.Drawing.Size(215, 16);
            this.rdoOwnBrowser.TabIndex = 0;
            this.rdoOwnBrowser.TabStop = true;
            this.rdoOwnBrowser.Text = "使用工具自带浏览器(支持自动签出)";
            this.rdoOwnBrowser.UseVisualStyleBackColor = true;
            // 
            // AutoStartupCheckBox
            // 
            this.AutoStartupCheckBox.AutoSize = true;
            this.AutoStartupCheckBox.Location = new System.Drawing.Point(6, 6);
            this.AutoStartupCheckBox.Name = "AutoStartupCheckBox";
            this.AutoStartupCheckBox.Size = new System.Drawing.Size(96, 16);
            this.AutoStartupCheckBox.TabIndex = 0;
            this.AutoStartupCheckBox.Text = "设置开机启动";
            this.AutoStartupCheckBox.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkTimingTipsSW);
            this.tabPage2.Controls.Add(this.chkAutoTipsSW);
            this.tabPage2.Controls.Add(this.lstCheckInDay);
            this.tabPage2.Controls.Add(this.label10);
            this.tabPage2.Controls.Add(this.cmbCheckInDay);
            this.tabPage2.Controls.Add(this.chkAutoCheckIn);
            this.tabPage2.Controls.Add(this.dtpTimingSW);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(390, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "上班签入";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkTimingTipsSW
            // 
            this.chkTimingTipsSW.AutoSize = true;
            this.chkTimingTipsSW.Location = new System.Drawing.Point(6, 222);
            this.chkTimingTipsSW.Name = "chkTimingTipsSW";
            this.chkTimingTipsSW.Size = new System.Drawing.Size(102, 16);
            this.chkTimingTipsSW.TabIndex = 9;
            this.chkTimingTipsSW.Text = "定时提醒/签入";
            this.chkTimingTipsSW.UseVisualStyleBackColor = true;
            this.chkTimingTipsSW.CheckStateChanged += new System.EventHandler(this.chkTimingTipsSW_CheckStateChanged);
            // 
            // chkAutoTipsSW
            // 
            this.chkAutoTipsSW.AutoSize = true;
            this.chkAutoTipsSW.Location = new System.Drawing.Point(7, 188);
            this.chkAutoTipsSW.Name = "chkAutoTipsSW";
            this.chkAutoTipsSW.Size = new System.Drawing.Size(138, 16);
            this.chkAutoTipsSW.TabIndex = 8;
            this.chkAutoTipsSW.Text = "开机后自动提醒/签入";
            this.chkAutoTipsSW.UseVisualStyleBackColor = true;
            this.chkAutoTipsSW.CheckStateChanged += new System.EventHandler(this.chkAutoTipsSW_CheckStateChanged);
            // 
            // lstCheckInDay
            // 
            this.lstCheckInDay.ContextMenuStrip = this.ctmLstCheckInDay;
            this.lstCheckInDay.FormattingEnabled = true;
            this.lstCheckInDay.ItemHeight = 12;
            this.lstCheckInDay.Location = new System.Drawing.Point(7, 64);
            this.lstCheckInDay.Name = "lstCheckInDay";
            this.lstCheckInDay.Size = new System.Drawing.Size(213, 112);
            this.lstCheckInDay.TabIndex = 7;
            // 
            // ctmLstCheckInDay
            // 
            this.ctmLstCheckInDay.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.移除ToolStripMenuItem,
            this.默认ToolStripMenuItem});
            this.ctmLstCheckInDay.Name = "ctmLstCheckInDay";
            this.ctmLstCheckInDay.Size = new System.Drawing.Size(101, 48);
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.移除ToolStripMenuItem.Text = "移除";
            this.移除ToolStripMenuItem.Click += new System.EventHandler(this.移除ToolStripMenuItem_Click);
            // 
            // 默认ToolStripMenuItem
            // 
            this.默认ToolStripMenuItem.Name = "默认ToolStripMenuItem";
            this.默认ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.默认ToolStripMenuItem.Text = "默认";
            this.默认ToolStripMenuItem.Click += new System.EventHandler(this.默认ToolStripMenuItem_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "添加自动签入日";
            // 
            // cmbCheckInDay
            // 
            this.cmbCheckInDay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCheckInDay.FormattingEnabled = true;
            this.cmbCheckInDay.Items.AddRange(new object[] {
            "星期日",
            "星期一",
            "星期二",
            "星期三",
            "星期四",
            "星期五",
            "星期六"});
            this.cmbCheckInDay.Location = new System.Drawing.Point(99, 34);
            this.cmbCheckInDay.Name = "cmbCheckInDay";
            this.cmbCheckInDay.Size = new System.Drawing.Size(121, 20);
            this.cmbCheckInDay.TabIndex = 5;
            this.cmbCheckInDay.SelectedIndexChanged += new System.EventHandler(this.cmbCheckInDay_SelectedIndexChanged);
            // 
            // chkAutoCheckIn
            // 
            this.chkAutoCheckIn.AutoSize = true;
            this.chkAutoCheckIn.Location = new System.Drawing.Point(7, 14);
            this.chkAutoCheckIn.Name = "chkAutoCheckIn";
            this.chkAutoCheckIn.Size = new System.Drawing.Size(72, 16);
            this.chkAutoCheckIn.TabIndex = 4;
            this.chkAutoCheckIn.Text = "自动签入";
            this.chkAutoCheckIn.UseVisualStyleBackColor = true;
            // 
            // dtpTimingSW
            // 
            this.dtpTimingSW.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTimingSW.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTimingSW.Location = new System.Drawing.Point(113, 212);
            this.dtpTimingSW.Name = "dtpTimingSW";
            this.dtpTimingSW.Size = new System.Drawing.Size(185, 26);
            this.dtpTimingSW.TabIndex = 3;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox2);
            this.tabPage3.Controls.Add(this.chkTipsAW);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(390, 270);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "下班签出";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpTimOut);
            this.groupBox2.Controls.Add(this.chkTim);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.cmbEnd);
            this.groupBox2.Controls.Add(this.cmbStart);
            this.groupBox2.Controls.Add(this.chkAutoCheckOut);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chkApartTipsAW);
            this.groupBox2.Controls.Add(this.txtApart);
            this.groupBox2.Controls.Add(this.chkTips9AW);
            this.groupBox2.Location = new System.Drawing.Point(3, 35);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(378, 183);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "自动签出选项";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(181, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "点，每隔";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "点—";
            // 
            // cmbEnd
            // 
            this.cmbEnd.FormattingEnabled = true;
            this.cmbEnd.Items.AddRange(new object[] {
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cmbEnd.Location = new System.Drawing.Point(104, 94);
            this.cmbEnd.Name = "cmbEnd";
            this.cmbEnd.Size = new System.Drawing.Size(71, 20);
            this.cmbEnd.TabIndex = 7;
            // 
            // cmbStart
            // 
            this.cmbStart.FormattingEnabled = true;
            this.cmbStart.Items.AddRange(new object[] {
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23"});
            this.cmbStart.Location = new System.Drawing.Point(8, 94);
            this.cmbStart.Name = "cmbStart";
            this.cmbStart.Size = new System.Drawing.Size(61, 20);
            this.cmbStart.TabIndex = 6;
            // 
            // chkAutoCheckOut
            // 
            this.chkAutoCheckOut.AutoSize = true;
            this.chkAutoCheckOut.Location = new System.Drawing.Point(6, 20);
            this.chkAutoCheckOut.Name = "chkAutoCheckOut";
            this.chkAutoCheckOut.Size = new System.Drawing.Size(96, 16);
            this.chkAutoCheckOut.TabIndex = 5;
            this.chkAutoCheckOut.Text = "开启自动签出";
            this.chkAutoCheckOut.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(331, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "分钟";
            // 
            // chkApartTipsAW
            // 
            this.chkApartTipsAW.AutoSize = true;
            this.chkApartTipsAW.Location = new System.Drawing.Point(6, 64);
            this.chkApartTipsAW.Name = "chkApartTipsAW";
            this.chkApartTipsAW.Size = new System.Drawing.Size(120, 16);
            this.chkApartTipsAW.TabIndex = 1;
            this.chkApartTipsAW.Text = "每隔一段时间签出";
            this.chkApartTipsAW.UseVisualStyleBackColor = true;
            // 
            // txtApart
            // 
            this.txtApart.Location = new System.Drawing.Point(245, 94);
            this.txtApart.Name = "txtApart";
            this.txtApart.Size = new System.Drawing.Size(80, 21);
            this.txtApart.TabIndex = 3;
            // 
            // chkTips9AW
            // 
            this.chkTips9AW.AutoSize = true;
            this.chkTips9AW.Location = new System.Drawing.Point(6, 42);
            this.chkTips9AW.Name = "chkTips9AW";
            this.chkTips9AW.Size = new System.Drawing.Size(114, 16);
            this.chkTips9AW.TabIndex = 2;
            this.chkTips9AW.Text = "累计够9小时签出";
            this.chkTips9AW.UseVisualStyleBackColor = true;
            // 
            // chkTipsAW
            // 
            this.chkTipsAW.AutoSize = true;
            this.chkTipsAW.Location = new System.Drawing.Point(9, 13);
            this.chkTipsAW.Name = "chkTipsAW";
            this.chkTipsAW.Size = new System.Drawing.Size(228, 16);
            this.chkTipsAW.TabIndex = 0;
            this.chkTipsAW.Text = "关机前签出(开启需要重启系统才生效)";
            this.chkTipsAW.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox5);
            this.tabPage5.Controls.Add(this.groupBox4);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(390, 270);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "微博遥控";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.chkWeiboRemote);
            this.groupBox5.Location = new System.Drawing.Point(6, 102);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(378, 145);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "微博遥控";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(7, 44);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(217, 85);
            this.label9.TabIndex = 1;
            this.label9.Text = "命令：\r\n签入：{中文：签入，英文：CheckIn}\r\n签出：{中文：签出，英文：CheckOut}\r\n重启：{中文：重启，英文：Restart}\r\n关机：{中文" +
    "：关机，英文：Shutdown}";
            // 
            // chkWeiboRemote
            // 
            this.chkWeiboRemote.AutoSize = true;
            this.chkWeiboRemote.Location = new System.Drawing.Point(7, 21);
            this.chkWeiboRemote.Name = "chkWeiboRemote";
            this.chkWeiboRemote.Size = new System.Drawing.Size(144, 16);
            this.chkWeiboRemote.TabIndex = 0;
            this.chkWeiboRemote.Text = "开启微博遥控签入签出";
            this.chkWeiboRemote.UseVisualStyleBackColor = true;
            this.chkWeiboRemote.CheckStateChanged += new System.EventHandler(this.chkWeiboRemote_CheckStateChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.txttxtWeiboPassword);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.txtWeiboUserName);
            this.groupBox4.Location = new System.Drawing.Point(4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(380, 92);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "微博登录用户名和密码";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "密码：";
            // 
            // txttxtWeiboPassword
            // 
            this.txttxtWeiboPassword.Location = new System.Drawing.Point(65, 49);
            this.txttxtWeiboPassword.Name = "txttxtWeiboPassword";
            this.txttxtWeiboPassword.PasswordChar = '*';
            this.txttxtWeiboPassword.Size = new System.Drawing.Size(185, 21);
            this.txttxtWeiboPassword.TabIndex = 7;
            this.txttxtWeiboPassword.TextChanged += new System.EventHandler(this.txttxtWeiboPassword_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 6;
            this.label8.Text = "用户名：";
            // 
            // txtWeiboUserName
            // 
            this.txtWeiboUserName.Location = new System.Drawing.Point(65, 23);
            this.txtWeiboUserName.Name = "txtWeiboUserName";
            this.txtWeiboUserName.Size = new System.Drawing.Size(185, 21);
            this.txtWeiboUserName.TabIndex = 5;
            this.txtWeiboUserName.TextChanged += new System.EventHandler(this.txtWeiboUserName_TextChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(390, 270);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "关于";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(225, 19);
            this.label4.TabIndex = 0;
            this.label4.Text = "MailTo:xinbaishui@qq.com";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // chkTim
            // 
            this.chkTim.AutoSize = true;
            this.chkTim.Location = new System.Drawing.Point(8, 130);
            this.chkTim.Name = "chkTim";
            this.chkTim.Size = new System.Drawing.Size(96, 16);
            this.chkTim.TabIndex = 10;
            this.chkTim.Text = "设定签出时间";
            this.chkTim.UseVisualStyleBackColor = true;
            // 
            // dtpTimOut
            // 
            this.dtpTimOut.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTimOut.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpTimOut.Location = new System.Drawing.Point(8, 151);
            this.dtpTimOut.Name = "dtpTimOut";
            this.dtpTimOut.Size = new System.Drawing.Size(185, 26);
            this.dtpTimOut.TabIndex = 11;
            // 
            // FrmSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(422, 320);
            this.Controls.Add(this.tab);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.tab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ctmLstCheckInDay.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox AutoStartupCheckBox;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DateTimePicker dtpTimingSW;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoDefaultBrowser;
        private System.Windows.Forms.RadioButton rdoOwnBrowser;
        private System.Windows.Forms.CheckBox chkTips9AW;
        private System.Windows.Forms.CheckBox chkApartTipsAW;
        private System.Windows.Forms.CheckBox chkTipsAW;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtApart;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkAutoCheckOut;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbEnd;
        private System.Windows.Forms.ComboBox cmbStart;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPassWord;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkAutoCheckIn;
        private System.Windows.Forms.Button btnUserOk;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txttxtWeiboPassword;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtWeiboUserName;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox chkWeiboRemote;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbCheckInDay;
        private System.Windows.Forms.ListBox lstCheckInDay;
        private System.Windows.Forms.ContextMenuStrip ctmLstCheckInDay;
        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 默认ToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkAutoTipsSW;
        private System.Windows.Forms.CheckBox chkTimingTipsSW;
        private System.Windows.Forms.DateTimePicker dtpTimOut;
        private System.Windows.Forms.CheckBox chkTim;

    }
}