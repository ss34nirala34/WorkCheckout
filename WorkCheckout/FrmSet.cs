using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Win32;
using NetDimension.Weibo;
using ReadWriteIni;
using System.Threading;
using EncryptionHelper;
using RunCmd;
using Timer = System.Windows.Forms.Timer;
using HtmlDocument = System.Windows.Forms.HtmlDocument;
using HtmlDocumentAgilityPack = HtmlAgilityPack.HtmlDocument;
using Common;

namespace WorkCheckout
{
    public partial class FrmSet : Form
    {
        [DllImport("user32.dll")]
        public extern static bool ShutdownBlockReasonCreate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string pwszReason);


        [DllImport("user32.dll")]
        public extern static bool ShutdownBlockReasonDestroy(IntPtr hWnd);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr SetProcessShutdownParameters(int dwLevel, int dwFlags);
        private DateTime date;
        public static DateTime lastCheckInTime;
        bool lastSignInTimeStrNull;


        public FrmSet()
        {
            InitializeComponent();
            IntPtr result = SetProcessShutdownParameters(0x4FF, 0x00000001);//提升进程关闭等级
            //FrmAfterWork frm=new FrmAfterWork();
            // frm.Show();
            Ini();
            date = DateTime.Now;

            CheckIn();//上班签入
            
            #region 下班签出

            CheckOutAW();

            #endregion
        }
        #region  上班签入
        //public static string LastCheckInTime;
        WebBrowser webBrowser = new WebBrowser();
        public string Week()
        {
            string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            string week = weekdays[Convert.ToInt32(DateTime.Now.DayOfWeek)];
            return week;
        }
        void CheckIn()
        {
            timer.Tick += timer_Tick;
            timer.Interval = 1000;
            string lastSignInTimeStr = Share.wFiles.ReadString("WCO", "LastSignInTime", "");
            lastSignInTimeStrNull = lastSignInTimeStr != string.Empty;
            if (chkAutoTipsSW.Checked)
            {
                if (lastSignInTimeStr != string.Empty)
                {

                    lastCheckInTime = DateTime.ParseExact(lastSignInTimeStr, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                    if (!Share.isToday(lastCheckInTime))
                    {
                        if (date.Hour >= 8)
                        {
                            if (!lstCheckInDay.Items.Contains(Week())) return;
                            CheckLastCheckInTime();
                        }
                    }

                }
                else
                {
                    if (date.Hour >= 8)
                    {
                        if (!lstCheckInDay.Items.Contains(Week())) return;
                        CheckLastCheckInTime();
                    }
                    else
                    {


                        timer.Enabled = true;

                    }
                }
            }
            if (chkTimingTipsSW.Checked)
            {
                timer.Enabled = true;
            }

        }
        Timer timer = new Timer();
        void CheckLastCheckInTime()
        {
            webBrowser.ScriptErrorsSuppressed = true;
            if (UserName == string.Empty || PassWord == string.Empty)
            {
                MessageBox.Show("用户名或密码为空，请在软件设置中输入用户名和密码", "用户名或密码为空", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                this.WindowState = FormWindowState.Normal;
                this.Visible = true;

                return;
            }
            string[] urStrings = { "rd.tencent.com" };
            int error = 0;
            while (true)
            {

                if (Share.MyPing(urStrings, out error))
                {
                    break;
                }
                Thread.Sleep(1);
            }
            webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
            try
            {
                Share.SuppressWininetBehavior();
                webBrowser.Navigate("http://rd.tencent.com/outsourcing/attendances/add");

                //waitwebCompleted();
                //SHDocVw.WebBrowser wb = this.webBrowser1.ActiveXInstance as SHDocVw.WebBrowser;
                //wb.NavigateComplete2 += new SHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(wb_NavigateComplete2);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string urlAdd = "http://rd.tencent.com/top/ptlogin/ptlogins/login?site=";
            string urlAdd2 = "http://tapd.tencent.com/ptlogin/ptlogins/login?";
            if ((webBrowser.Url != null && webBrowser.Url.ToString().Contains(urlAdd)) || (webBrowser.Url != null && webBrowser.Url.ToString().Contains(urlAdd2)))
            {
                //判断是否已加载完网页
                if (webBrowser.ReadyState == WebBrowserReadyState.Complete)
                {
                    //获取网页文档对象，相当于获取网页的全部源码
                    HtmlDocument htmlDoc = this.webBrowser.Document;
                    //设置帐号
                    HtmlElement id = htmlDoc.GetElementById("username");
                    id.SetAttribute("value", UserName);
                    //设置密码
                    HtmlElement pwd = htmlDoc.GetElementById("password_input");
                    pwd.SetAttribute("value", PassWord);
                    //登录
                    HtmlElement btn = htmlDoc.GetElementById("login_button");
                    if (btn != null)
                    {
                        btn.InvokeMember("click");
                    }
                }
            }
            if (webBrowser.ReadyState != WebBrowserReadyState.Complete) return;
            string urlCheckOut = "http://om.tencent.com/attendances/check_out";
            string urlCheckIn = "http://rd.tencent.com/outsourcing/attendances/add";
            if (webBrowser.Url != null)
            {
                if (webBrowser.Url.ToString().Contains(urlCheckOut) || webBrowser.Url.ToString().Contains(urlCheckIn))
                {
                    //判断是否已加载完网页
                    if (webBrowser.ReadyState == WebBrowserReadyState.Complete)
                    {
                        HtmlDocumentAgilityPack htmlDoc1 = new HtmlDocumentAgilityPack();
                        htmlDoc1.LoadHtml(webBrowser.DocumentText);
                        string txt = "";
                        try
                        {
                            txt = htmlDoc1.DocumentNode.SelectSingleNode(@"./html[1]/body[1]/div[3]/div[1]/div[1]/div[4]/div[1]/p[1]/span[1]").InnerText.TrimEnd();
                        }
                        catch (Exception ex)
                        {
                            LogUtil.WriteError(ex);
                            LogUtil.WriteLog("Code:196 txt="+txt, LogType.Info);

                        }


                        if (txt != string.Empty)
                        {

                            try
                            {
                                lastCheckInTime = CheckInTime = DateTime.ParseExact(txt, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                                Share.wFiles.WriteString("WCO", "LastSignInTime", string.Format("{0:yyyy-MM-dd HH:mm:ss}", CheckInTime));
                                if (!Share.isToday(CheckInTime))
                                {
                                    if (chkAutoCheckIn.Checked)
                                    {
                                        FrmBrowser frm = new FrmBrowser();

                                        frm.TopMost = true;
                                        frm.Show();

                                    }
                                    else
                                    {
                                        FrmStartWork frmStart = new FrmStartWork();
                                        frmStart.TopMost = true;
                                        frmStart.Show();
                                    }
                                    webBrowser.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {

                                LogUtil.WriteError(ex);
                                LogUtil.WriteLog("Code:206 lastCheckInTime=" + lastCheckInTime, LogType.Info);
                                if (chkAutoCheckIn.Checked)
                                {
                                    FrmBrowser frm = new FrmBrowser();

                                    frm.TopMost = true;
                                    frm.Show();

                                }
                                else
                                {
                                    FrmStartWork frmStart = new FrmStartWork();
                                    frmStart.TopMost = true;
                                    frmStart.Show();
                                }
                                webBrowser.Dispose();
                            }



                        }
                        else
                        {
                            if (chkAutoCheckIn.Checked)
                            {
                                FrmBrowser frm = new FrmBrowser();
                                frm.TopMost = true;
                                frm.Show();
                            }
                            else
                            {
                                FrmStartWork frmStart = new FrmStartWork();
                                frmStart.TopMost = true;
                                frmStart.Show();
                            }
                            webBrowser.Dispose();
                        }

                    }

                }
            }
        }

        private bool isStop;
        DateTime timeTiming = DateTime.MinValue;
        DateTime RandomTime()
        {
            string setTimeStr = Share.wFiles.ReadString("WCO", "TimingSW", "09:00:00");
            if (setTimeStr == string.Empty) return DateTime.MinValue;
            DateTime SetTime = DateTime.ParseExact(setTimeStr, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            Random r = new Random();
            int i = r.Next(30, 600);
            DateTime time = SetTime.AddSeconds(-i);//减去i秒
            LogUtil.WriteLog(time.ToString(), LogType.Info);
            return time;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            
            if (DateTime.Now.Hour==0&&DateTime.Now.Minute==0)
            {
                timeTiming = RandomTime();
            }
            if (lastSignInTimeStrNull)
            {
                if (!Share.isToday(lastCheckInTime))
                {

                    //if (timeTiming == DateTime.MinValue) return;
                    //if (timeTiming == DateTime.MinValue) timeTiming = DateTime.ParseExact("09:00:00", "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    if ((int)Share.DateDiffTotalSecond(timeTiming) == 0)
                    {
                        if (isStop)
                        {
                            return;
                        }
                        isStop = true;
                        
                        if (chkAutoCheckIn.Checked)
                        {

                            if (!lstCheckInDay.Items.Contains(Week())) return;

                            
                            if (chkWeiboRemote.Checked)
                            {
                                weiboAutoCheck = new WeiboAutoCheckInWebBrowser();
                                weiboAutoCheck.WeiboCheckIn(oauth, "自动");
                            }
                            else
                            {
                                FrmBrowser frm = new FrmBrowser();
                                frm.TopMost = true;
                                frm.Show();
                            }
                            
                        }
                        else
                        {
                            if (!lstCheckInDay.Items.Contains(Week())) return;
                            FrmStartWork frmStart = new FrmStartWork();  //提示签入
                            frmStart.TopMost = true;
                            frmStart.Show();
                            
                        }

                    }
                    else
                    {
                        isStop = false;  
                    }


                }
                else
                {
                    isStop = false;
                }
            }
            else
            {

                if (timeTiming == DateTime.MinValue) return;
                
                if ((int)Share.DateDiffTotalSecond(timeTiming) == 0)
                {
                    if (isStop)
                    {
                        return;
                    }
                    isStop = true;
                    if (chkAutoCheckIn.Checked)
                    {
                        if (!lstCheckInDay.Items.Contains(Week())) return;
                        if (chkWeiboRemote.Checked)
                        {
                            weiboAutoCheck = new WeiboAutoCheckInWebBrowser();
                            weiboAutoCheck.WeiboCheckIn(oauth, "自动");
                        }
                        else
                        {
                            FrmBrowser frm = new FrmBrowser();
                            frm.TopMost = true;
                            frm.Show();
                        }
                    }
                    else
                    {
                        if (!lstCheckInDay.Items.Contains(Week())) return;
                        FrmStartWork frmStart = new FrmStartWork();
                        frmStart.TopMost = true;
                        frmStart.Show();
                    }

                }
                else
                {
                    isStop = false;
                }
            }

        }
        #endregion
        CheckTimeAgWB checkAg;
        Timer CheckAgTime = new Timer();
        void CheckAgTime_Tick(object sender, EventArgs e)
        {
            string lastSignInTimeStr = Share.wFiles.ReadString("WCO", "LastSignInTime", ""); 
            if (lastSignInTimeStr != string.Empty)
            {
                DateTime lastSignInTime = DateTime.ParseExact(lastSignInTimeStr, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);


                if (!Share.isToday(lastSignInTime))
                {
                    if (checkAg != null)
                    {
                        checkAg.Dispose(); 
                    }
                    checkAg = new CheckTimeAgWB();
                    checkAg.CheckAg();
                }

            }
            else
            {
                if (checkAg != null)
                {
                    checkAg.Dispose();
                }
                checkAg = new CheckTimeAgWB();
                checkAg.CheckAg();
            }
            CheckAgTime.Enabled = false;
        }

        #region  下班签出
        HidWebBrowser hidWebBroser;
        public static DateTime CheckInTime;
        public static double WorkingSeconds;//工作了多少秒
        private static string StrTips = "";
        private void CheckOutAW() //下班签出
        {
            blocked = Share.wFiles.ReadBool("WCO", "TipsAW", false);
            if (blocked)
            {
                Version ver = System.Environment.OSVersion.Version;
                if (ver.Major >= 6)
                {
                    ShutdownBlockReasonCreate(this.Handle, "您今天签出了吗？点击取消可返回！！！！！！！！！");
                }
            }
            Tips9AWTimer.Interval = 1000;
            Tips9AWTimer.Tick += Tips9AWTimer_Tick;
            Tips9AWTimer.Enabled = true;
            ApartTipsAWTimer.Interval = Convert.ToInt32(apartMinutes) * 60 * 1000;

            ApartTipsAWTimer.Tick += ApartTipsAWTimer_Tick;
            ApartTipsAWTimer.Enabled = true;
            //ApartTipsAWTimer.Start(); //屏蔽

            ApartTipsAWTim.Interval = 1000;
            ApartTipsAWTim.Tick+=ApartTipsAWTim_Tick;
            ApartTipsAWTim.Enabled = true;
            ApartTipsAWTim.Start();

            //if (chkApartTipsAW.Checked)
            //{
            //    ApartCheckOut();

            //}

        }

        private void ApartTipsAWTim_Tick(object sender, EventArgs e)
        {
            TimCheckOut();
        }

        public static DateTime LastCheckOutTime;
        void ApartTipsAWTimer_Tick(object sender, EventArgs e)
        {
            ApartCheckOut();
            
        }

        DateTime RandomCheckOutTime()
        {
            string setTimeStr = Share.wFiles.ReadString("WCO", "dtpTimOut", "19:00:00");
            if (setTimeStr == string.Empty) return DateTime.MinValue;
            DateTime SetTime = DateTime.ParseExact(setTimeStr, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

            Random r = new Random();
            int i = r.Next(60, 1500);
            DateTime time = SetTime.AddSeconds(i);//减去i秒
            LogUtil.WriteLog(time.ToString(), LogType.Info);
            return time;
        }
        DateTime timCheckOutTimg=new DateTime();

        public delegate void ShowWindowsDele();

        public ShowWindowsDele showWindowsDele;
        private bool IsCheckOut =true;
        void TimCheckOut()
        {
            if (DateTime.Now.Hour == 0 && DateTime.Now.Minute == 0)
            {
                timCheckOutTimg = RandomCheckOutTime();
                IsCheckOut = true;
            }
            DateTime nowtime = DateTime.Now;
            int nowHour = nowtime.Hour;
            if (Share.isToday(CheckInTime))
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)Share.DateDiffTotalSecond(CheckInTime));
                StrTips = "你已经工作了：" + (int)ts.TotalHours + "小时" + ts.Minutes + "分钟" + ts.Seconds + "秒";
                notifyIcon1.Text = StrTips;
              
                if (nowHour >= 18 && (int)Share.DateDiffTotalSecond(timCheckOutTimg) == 0)
                {
                    #region 静默处理
                    //if (!chkTim.Checked) return; //如果关闭则返回

                    //if (hidWebBroser != null)
                    //{
                    //    hidWebBroser.Dispose();
                    //}
                    //hidWebBroser = new HidWebBrowser
                    //{
                    //    showTipsDelegate = showTips,
                    //    _showWindows = new ShowWindowsDele(() =>
                    //    {

                    //        this.Invoke(new Action(() =>
                    //        {
                    //            FrmBrowserAW frm = new FrmBrowserAW();
                    //            frm.TopMost = true;
                    //            frm.showTipsDelegate = showTips;
                    //            frm.ShowInTaskbar = true;
                    //            frm.Exit = false;
                    //            frm.Show();
                    //        }));
                    //    })
                      
                    
                    //};
                    //hidWebBroser.HidWebBrowserRun();
                    //Thread agThread=new Thread(() =>
                    //{
                    //    Thread.Sleep(120000);
                    //    this.Invoke(new Action(() =>
                    //    {
                    //        hidWebBroser = new HidWebBrowser
                    //        {
                    //            showTipsDelegate = showTips,
                    //            _showWindows = new ShowWindowsDele(() =>
                    //            {

                    //              this.Invoke(new Action(()=>
                    //                {
                    //                    FrmBrowserAW frm = new FrmBrowserAW();
                    //                    frm.TopMost = true;
                    //                    frm.showTipsDelegate = showTips;
                    //                    frm.ShowInTaskbar = true;
                    //                    frm.Exit = false;
                    //                    frm.Show();
                    //                }));
                                   
                    //            })
                    //        };
                    //        hidWebBroser.HidWebBrowserRun();
                    //    }));
                    //});
                    //agThread.Start();
                    #endregion

                    if (IsCheckOut==false)
                    {
                        return;
                    }
                    FrmBrowserAW frm = new FrmBrowserAW();
                    frm.showTipsDelegate = showTips;
                    frm.ShowInTaskbar = true;
                    frm.Exit = false;
                    frm.Show();
                    Thread agThread = new Thread(() =>
                    {
                        Thread.Sleep(120000);
                        this.Invoke(new Action(() =>
                        {

                            FrmBrowserAW frmAW = new FrmBrowserAW();
                            frmAW.showTipsDelegate = showTips;
                            frmAW.ShowInTaskbar = true;
                            frmAW.Exit = false;
                            frmAW.Show();
                            
                        }));
                    });
                    agThread.Start();
                    IsCheckOut = false;
                }

            }
            else
            {
                notifyIcon1.Text = "你今天好像还没签到？";
                CheckAgTime.Enabled = true;
            }
        }
        private void ApartCheckOut()
        {
            int nowHour = DateTime.Now.Hour;

            if (Share.isToday(CheckInTime))
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)Share.DateDiffTotalSecond(CheckInTime));
                StrTips = "你已经工作了：" + (int)ts.TotalHours + "小时" + ts.Minutes + "分钟" + ts.Seconds + "秒";
                notifyIcon1.Text = StrTips;
                if (nowHour >= Convert.ToInt32(startHour) && nowHour <= Convert.ToInt32(endHour))
                {
                    if (nowHour == Convert.ToInt32(endHour) && DateTime.Now.Minute > Convert.ToInt32(apartMinutes))
                    {
                        return;
                    }
                    if (!chkApartTipsAW.Checked) return; //如果关闭则返回

                    if (hidWebBroser != null)
                    {
                        hidWebBroser.Dispose();
                    }
                    hidWebBroser = new HidWebBrowser { showTipsDelegate = showTips };
                    hidWebBroser.HidWebBrowserRun();
                }

            }
            else
            {
                notifyIcon1.Text = "你今天好像还没签到？";
                CheckAgTime.Enabled = true;
            }

        }
        private string startHour;
        string endHour;
        private string apartMinutes;
        Timer ApartTipsAWTimer = new Timer();
        Timer Tips9AWTimer = new Timer();
        Timer ApartTipsAWTim = new Timer();
        public delegate void ShowTipsDelegate();
        public void showTips()
        {

            notifyIcon1.ShowBalloonTip(3, "提示", string.Format("{0}\n{1}", "成功自动签出", StrTips), ToolTipIcon.Info);
        }
        private bool Tips9AWTimerExit = false;
        void Tips9AWTimer_Tick(object sender, EventArgs e)
        {

            if (Share.isToday(CheckInTime))
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)Share.DateDiffTotalSecond(CheckInTime));
                if (!chkTim.Checked)
                {
                    StrTips = "你已经工作了：" + (int)ts.TotalHours + "小时" + ts.Minutes + "分钟" + ts.Seconds + "秒";
                    notifyIcon1.Text = StrTips;
                }
                
                if (Tips9AWTimerExit) return;
               
                if (chkTips9AW.Checked&&DateTime.Now.Hour>=18)
                {

                    string LastCheckOutTimeStr = Share.wFiles.ReadString("WCO", "LastCheckOutTime", "");
                    bool LastCheckOutTimeStrNull = LastCheckOutTimeStr != string.Empty;
                    if (LastCheckOutTimeStrNull)
                    {
                        LastCheckOutTime = DateTime.ParseExact(LastCheckOutTimeStr, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        LastCheckOutTime = CheckInTime;
                    }
                    TimeSpan tsLast = new TimeSpan(0, 0, (int)Share.DateTotalSecond(CheckInTime, LastCheckOutTime));
                    //var isToday = Share.isToday(LastCheckOutTime);
                    //var totalHours = (int) tsLast.TotalHours > 9;
                    if (Share.isToday(LastCheckOutTime) && (int)tsLast.TotalHours > 9 || Tips9AWTimerExit == true) return;
                    if ((int)ts.TotalHours >= 9)
                    {
                        Tips9AWTimerExit = true;
                        if (hidWebBroser != null)
                        {
                            hidWebBroser.Dispose();
                        }
                        LogUtil.WriteLog("Tips9AW:Try To CheckOut At:" + DateTime.Now, LogType.Info);
                        FrmBrowserAW frmAW = new FrmBrowserAW();
                        frmAW.showTipsDelegate = showTips;
                        frmAW.ShowInTaskbar = true;
                        frmAW.Exit = false;
                        frmAW.Show();
                        //hidWebBroser = new HidWebBrowser { showTipsDelegate = showTips };
                        //hidWebBroser.HidWebBrowserRun();
                        
                        //Tips9AWTimer.Enabled = false;
                    }
                    else
                    {
                        Tips9AWTimerExit = false;
                    }
                }


            }
            else
            {
                Tips9AWTimerExit = false;
                notifyIcon1.Text = "你今天好像还没签到？";
                CheckAgTime.Enabled = true;
            }
        }
        #endregion

        //最小化至托盘
        private void NormalToMinimized()
        {
            this.Visible = false;
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
        }

        public static string UserName;
        public static string PassWord;
        void Ini()
        {
            rdoDefaultBrowser.Enabled = false;
            this.Closing += FrmSet_Closing;
            NormalToMinimized();
            var icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.Icon = icon;
            #region 常规设置
            notifyIcon1.Icon = icon;
            notifyIcon1.Visible = true;
            notifyIcon1.Text = "签入签出工具";
            MenuItem set = new MenuItem("设置");
            MenuItem exit = new MenuItem("退出");
            MenuItem ManualCheckIn = new MenuItem("手动签入");
            MenuItem ManualCheckOut = new MenuItem("手动签出");
            set.Click += set_Click;
            exit.Click += exit_Click;
            ManualCheckIn.Click += ManualCheckIn_Click;
            ManualCheckOut.Click += ManualCheckOut_Click;
            MenuItem[] childenItem = new MenuItem[] { ManualCheckOut, ManualCheckIn, set, exit };
            notifyIcon1.ContextMenu = new ContextMenu(childenItem);


            AutoStartupCheckBox.Checked = WindowsAPI.IsAutoStartup();
            AutoStartupCheckBox.CheckStateChanged += AutoStartupCheckBox_CheckStateChanged;

            txtUserName.Text = UserName = CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "UserName", ""));
            txtPassWord.Text = PassWord = CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "PassWord", ""));
            btnUserOk.Enabled = false;
            rdoOwnBrowser.Checked = Share.wFiles.ReadBool("WCO", "OwnBrowser", true);
            rdoDefaultBrowser.Checked = Share.wFiles.ReadBool("WCO", "DefaultBrowser", false);
            rdoOwnBrowser.CheckedChanged += rdoOwnBrowser_CheckedChanged;
            rdoDefaultBrowser.CheckedChanged += rdoDefaultBrowser_CheckedChanged;
            #endregion
            #region 上班签入
            timeTiming = DateTime.ParseExact("09:00:00", "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);//初始化登录时间
            timeTiming = RandomTime();//获取随机时间
            timCheckOutTimg = DateTime.ParseExact("19:00:00", "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);//初始化时间
            timCheckOutTimg = RandomCheckOutTime();
            CheckAgTime.Interval = 300000;
            //CheckAgTime.Interval = 30000;
            CheckAgTime.Tick += new EventHandler(CheckAgTime_Tick);
           
            string checkInDayStr = Share.wFiles.ReadString("WCO", "CheckInDay", "星期一,星期二,星期三,星期四,星期五");
            char[] chr = { ',' };
            string[] checkInDay = checkInDayStr.Split(chr, StringSplitOptions.RemoveEmptyEntries);
            foreach (string day in checkInDay)
            {
                lstCheckInDay.Items.Add(day);
            }
            chkAutoCheckIn.Checked = Share.wFiles.ReadBool("WCO", "AutoCheckIn", false);
            chkAutoTipsSW.Checked = Share.wFiles.ReadBool("WCO", "AutoTipsSW", true);

            chkTimingTipsSW.Checked = Share.wFiles.ReadBool("WCO", "TimingTipsSW", false);

            chkAutoCheckIn.CheckStateChanged += chkAutoCheckIn_CheckStateChanged;

            dtpTimingSW.Format = DateTimePickerFormat.Custom;
            dtpTimingSW.CustomFormat = "HH点:mm分:ss秒";
            dtpTimingSW.ShowUpDown = true;
            dtpTimingSW.ValueChanged += dtpTimingSW_ValueChanged;
            string dtpTimingSWValue = Share.wFiles.ReadString("WCO", "TimingSW", "");
            if (dtpTimingSWValue != string.Empty)
            {
                DateTime dtpPowerOffEveryDayTime = DateTime.ParseExact(dtpTimingSWValue, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                dtpTimingSW.Value = dtpPowerOffEveryDayTime;
            }
            #endregion
            #region 下班签出
          
            string lastSignInTimeStr = Share.wFiles.ReadString("WCO", "LastSignInTime", "");
            if (lastSignInTimeStr != "")
            {
                CheckInTime = DateTime.ParseExact(lastSignInTimeStr, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            chkAutoCheckOut.Checked = Share.wFiles.ReadBool("WCO", "AutoCheckOut", false);
            chkTipsAW.Checked = Share.wFiles.ReadBool("WCO", "TipsAW", false);
            chkTips9AW.Checked = Share.wFiles.ReadBool("WCO", "Tips9AW", false);
            chkApartTipsAW.Checked = Share.wFiles.ReadBool("WCO", "ApartTipsAW", false);
            chkTim.Checked = Share.wFiles.ReadBool("WCO", "chkTim", false);
            chkAutoCheckOut.CheckStateChanged += chkAutoCheckOut_CheckStateChanged;
            chkTipsAW.CheckStateChanged += chkTipsAW_CheckStateChanged;
            chkTips9AW.CheckStateChanged += chkTips9AW_CheckStateChanged;
            chkApartTipsAW.CheckedChanged += chkApartTipsAW_CheckedChanged;
            chkTim.CheckedChanged += chkTim_CheckedChanged;
            startHour = cmbStart.Text = Share.wFiles.ReadString("WCO", "ApartStartTime", "18");
            endHour = cmbEnd.Text = Share.wFiles.ReadString("WCO", "ApartEndTime", "21");
            cmbStart.TextChanged += cmbStart_TextChanged;
            cmbEnd.TextChanged += cmbEnd_TextChanged;
            apartMinutes = txtApart.Text = Share.wFiles.ReadString("WCO", "ApartMinutes", "15");
            txtApart.TextChanged += txtApart_TextChanged;
            bool tipsAwCheck = chkTipsAW.Checked;
            RegSet(tipsAwCheck);
            dtpTimOut.Format = DateTimePickerFormat.Custom;
            dtpTimOut.Format = DateTimePickerFormat.Custom;
            dtpTimOut.CustomFormat = "HH点:mm分:ss秒";
            dtpTimOut.ShowUpDown = true;
            dtpTimOut.ValueChanged+=dtpTimOut_ValueChanged; ;
            string dtpTimOutValue = Share.wFiles.ReadString("WCO", "dtpTimOut", "");
            if (dtpTimOutValue != string.Empty)
            {
                DateTime dtpPowerOffEveryDayTime = DateTime.ParseExact(dtpTimOutValue, "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                dtpTimOut.Value = dtpPowerOffEveryDayTime;
            }
            else
            {
                DateTime dtpPowerOffEveryDayTime = DateTime.ParseExact("19:00:00", "HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                dtpTimOut.Value = dtpPowerOffEveryDayTime;
            }
            #endregion

            #region 微博遥控
            txtWeiboUserName.Text = WeiboUserName = CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "WeiboUserName", ""));
            txttxtWeiboPassword.Text = WeiboPassword = CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "WeiboPassword", ""));
            LoadchkWeiboRemoteChk = true;
            chkWeiboRemote.Checked = Share.wFiles.ReadBool("WCO", "ChkWeiboRemote", false);
            StartWeiboRemote();
            LoadchkWeiboRemoteChk = false;

            #endregion
            ///屏蔽微博遥控功能,每隔一段时间签出功能
            chkWeiboRemote.Checked = false;
            chkWeiboRemote.Enabled = false;
            chkApartTipsAW.Checked = false;
            chkApartTipsAW.Enabled = false;


        }

       
        #region 微博遥控

        private static NetDimension.Weibo.Client Sina;
        Timer weiboTimer = new Timer();
        void StartWeiboRemote()
        {
            if (!chkWeiboRemote.Checked) return;
            if (WeiboUserName == string.Empty || WeiboPassword == string.Empty)
            {
                MessageBox.Show("微博登录用户名或密码为空，请重新设置", "微博登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkWeiboRemote.Checked = false;
                return;
            }
            weiboTimer.Interval = 180000;
            //weiboTimer.Interval = 20000;
            //weiboTimer.Enabled = true;
            weiboTimer.Tick += weiboTimer_Tick;
            //oauth = weiboRemote.LogInWeibo();

        }

        private OAuth oauth=null;
        WeiboRemote weiboRemote=new WeiboRemote();
        void weiboTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour < 8) return;
            try
            {
                TokenResult result;
                if (string.IsNullOrEmpty(oauth.AccessToken))
                {
                    return;
                }
                try
                {
                    result = oauth.VerifierAccessToken();//测试保存的AccessToken的有效性
                }
                catch (Exception ex)
                {
                    result = TokenResult.Other;
                    //throw new Exception();
                }
                if (result != TokenResult.Success)//如果测试无效，重新登录
                {
                    oauth = weiboRemote.LogInWeibo();
                }
                if (Sina != null)
                {
                    Sina = null;
                }
               
                Sina = new Client(oauth);
                var json = Sina.API.Entity.Statuses.UserTimeline(count: 1);
                //var json1 = Sina.API.Entity.Statuses.PublicTimeline (count: 1);
               
                if (json.Statuses == null) return;
                string command = null;
                foreach (var status in json.Statuses)
                {
                    if (status.User == null)
                        continue;
                    Console.WriteLine(status.Text);
                    command = status.Text;
                }
                TodoWork(command);
            }
            catch (Exception ex)
            {

                LogUtil.WriteError(ex);
            }
            
        }
        Cmd cmd = new Cmd();
        WeiboAutoCheckInWebBrowser weiboAutoCheck;
        WeiboAutoCheckOutWebBrowser weiboAutoCheckOut;
        public DateTime checkinWeiboTimeOld;
        //public DateTime checkinWeiboTimeNew;
        private CheckTimeAgWB check;
        void TodoWork(string str)
        {
            if (KeyCheck(str,"签入","CheckIn"))
            {
                if (Share.isToday(CheckInTime))
                {
                    Sina = new Client(oauth);
                    Sina.API.Entity.Statuses.Update(string.Format("{0} {1}",  "今天已经签入，时间在：", string.Format("{0:yyyy-MM-dd HH:mm:ss}", CheckInTime)));
                    return;
                }
                if (Share.isToday(checkinWeiboTimeOld))
                {
                    if (check != null)
                    {
                        check.Dispose();
                    }
                    check = new CheckTimeAgWB();
                    check.CheckAg();
                }
                if (weiboAutoCheck != null)
                {
                    weiboAutoCheck.Dispose();
                }
                weiboAutoCheck = new WeiboAutoCheckInWebBrowser();
                weiboAutoCheck.WeiboCheckIn(oauth,"微博遥控");
                checkinWeiboTimeOld = DateTime.Now;
            }
            else if (KeyCheck(str,"签出","CheckOut"))
            {
                if (weiboAutoCheckOut != null)
                {
                    weiboAutoCheckOut.Dispose();
                }
                weiboAutoCheckOut = new WeiboAutoCheckOutWebBrowser();
                weiboAutoCheckOut.WeiboCheckOut(oauth);
            }
            else if (KeyCheck(str,"关机","Shutdown"))       
            {
                FrmSet.CloseCance = false;
                Sina = new Client(oauth);
                Sina.API.Entity.Statuses.Update(string.Format("{0} {1}", "执行关机", string.Format("{0:yyyy-MM-dd   HH:mm:ss}", DateTime.Now)));
                cmd.RunCmd("Shutdown -s -t 00", Application.StartupPath);
            }
            else if (KeyCheck(str,"重启","Restart")) 
            {
                FrmSet.CloseCance = false;
                Sina = new Client(oauth);
                Sina.API.Entity.Statuses.Update(string.Format("{0} {1}", "执行重启", string.Format("{0:yyyy-MM-dd   HH:mm:ss}", DateTime.Now)));
                cmd.RunCmd("Shutdown -r -t 00", Application.StartupPath);
            }
        }
        #endregion

        bool KeyCheck(string str,params string[] keys)
        {
            bool result=false;
            foreach (string key in keys)
            {
                //Regex regex = new Regex(string.Format("^c{0} 我在:.*"));
                if (str == key || str.ToLower() == key.ToLower()||str.Contains(string.Format("{0} 我在",key)))
                {
                    result = true;
                    break;
                   
                }
            }
            return result;
        }
        #region 常规设置
        void ManualCheckIn_Click(object sender, EventArgs e)
        {
            FrmBrowser frm = new FrmBrowser();
            frm.TopMost = true;
            frm.ShowInTaskbar = true;
            frm.Show();
        }
        void ManualCheckOut_Click(object sender, EventArgs e)
        {
            FrmBrowserAW frm = new FrmBrowserAW();
            frm.TopMost = true;
            frm.showTipsDelegate = showTips;
            frm.ShowInTaskbar = true;
            frm.Exit = false;
            frm.Show();
        }

        private void btnUserOk_Click(object sender, EventArgs e)
        {
            webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
            try
            {
                Share.SuppressWininetBehavior();
                webBrowser.Navigate("http://om.tencent.com/attendances/check_out/");

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            btnUserOk.Enabled = false;
        }

        void rdoDefaultBrowser_CheckedChanged(object sender, EventArgs e)
        {
            Share.wFiles.WriteBool("WCO", "DefaultBrowser", rdoDefaultBrowser.Checked);
        }

        private void rdoOwnBrowser_CheckedChanged(object sender, EventArgs e)
        {
            Share.wFiles.WriteBool("WCO", "OwnBrowser", rdoOwnBrowser.Checked);
        }

        public static bool CloseCance = true;
        void FrmSet_Closing(object sender, CancelEventArgs e)
        {
            if (CloseCance)
            {
                e.Cancel = true;
                NormalToMinimized();
            }

        }

        void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void set_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            string str = txtUserName.Text.Trim();

            Share.wFiles.WriteString("WCO", "UserName", CryptHelper.EncryptString(str));
            UserName = str;
            btnUserOk.Enabled = true;
        }

        private void txtPassWord_TextChanged(object sender, EventArgs e)
        {
            string str = txtPassWord.Text.Trim();

            Share.wFiles.WriteString("WCO", "PassWord", CryptHelper.EncryptString(str));
            PassWord = str;
            btnUserOk.Enabled = true;
        }
        #endregion

        #region 上班签入
        private void cmbCheckInDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCheckInDay.SelectedText != null)
            {
                if (!lstCheckInDay.Items.Contains(cmbCheckInDay.SelectedItem))
                {
                    lstCheckInDay.Items.Add(cmbCheckInDay.SelectedItem);
                    string days = null;
                    foreach (string day in lstCheckInDay.Items)
                    {
                        days += day + ",";
                    }
                    Share.wFiles.WriteString("WCO", "CheckInDay", days);
                }
            }
        }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lstCheckInDay.SelectedItem == null)
            {
                return;
            }
            lstCheckInDay.Items.Remove(lstCheckInDay.SelectedItem);
            string days = null;
            foreach (string day in lstCheckInDay.Items)
            {
                days += day + ",";
            }
            Share.wFiles.WriteString("WCO", "CheckInDay", days);
        }
        private void 默认ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lstCheckInDay.Items.Clear();
            Share.wFiles.WriteString("WCO", "CheckInDay", null);
            string checkInDayStr = Share.wFiles.ReadString("WCO", "CheckInDay", "星期一,星期二,星期三,星期四,星期五");
            char[] chr = { ',' };
            string[] checkInDay = checkInDayStr.Split(chr, StringSplitOptions.RemoveEmptyEntries);
            foreach (string day in checkInDay)
            {
                lstCheckInDay.Items.Add(day);
            }
        }
        void chkAutoCheckIn_CheckStateChanged(object sender, EventArgs e)
        {
            Share.wFiles.WriteBool("WCO", "AutoCheckIn", chkAutoCheckIn.Checked);
        }
        void dtpTimingSW_ValueChanged(object sender, EventArgs e)
        {
            Share.wFiles.WriteString("WCO", "TimingSW", string.Format("{0:HH:mm:ss}", dtpTimingSW.Value));
            timeTiming = RandomTime();//获取随机时间
        }
        private void chkAutoTipsSW_CheckStateChanged(object sender, EventArgs e)
        {
            Share.wFiles.WriteBool("WCO", "TimingTipsSW", chkAutoTipsSW.Checked);
        }

        private void chkTimingTipsSW_CheckStateChanged(object sender, EventArgs e)
        {
            Share.wFiles.WriteBool("WCO", "AutoTipsSW", chkTimingTipsSW.Checked);
        }


        private void AutoStartupCheckBox_CheckStateChanged(object sender, EventArgs e)
        {

            if (AutoStartupCheckBox.Checked)
            {
                try
                {
                    WindowsAPI.AddStartupItem("WorkCheckout", Assembly.GetEntryAssembly().Location);
                }
                catch (Exception ex)
                {

                    MessageBox.Show("设置开机启动失败，可能是杀软拦截了，可尝试退出杀软后再设置。", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AutoStartupCheckBox.Checked = WindowsAPI.IsAutoStartup();
                }

            }
            else
            {
                WindowsAPI.RemoveStartupItem("WorkCheckout");
            }
        }
        #endregion
        #region 下班签出

        public static bool blocked = false;
        protected override void WndProc(ref Message aMessage)
        {
            const int WM_QUERYENDSESSION = 0x0011;
            const int WM_ENDSESSION = 0x0016;

            if (CloseCance & blocked && (aMessage.Msg == WM_QUERYENDSESSION || aMessage.Msg == WM_ENDSESSION))
            {
                aMessage.Result = (IntPtr)0;
                bool tipsAwCheck = chkTipsAW.Checked;
                RegSet(tipsAwCheck);
                FrmAfterWork frm = new FrmAfterWork();
                frm.TopMost = true;
                frm.Show();
                blocked = false;
            }
            base.WndProc(ref aMessage);
        }
        private void dtpTimOut_ValueChanged(object sender, EventArgs e)
        {
            Share.wFiles.WriteString("WCO", "dtpTimOut", string.Format("{0:HH:mm:ss}", dtpTimOut.Value));
            timCheckOutTimg = RandomCheckOutTime();
        }

        void chkAutoCheckOut_CheckStateChanged(object sender, EventArgs e)
        {
            bool check = chkAutoCheckOut.Checked;
            chkApartTipsAW.Enabled = check;
            chkTips9AW.Enabled = check;
            Share.wFiles.WriteBool("WCO", "AutoCheckOut", check);
            if (!check)
            {
                chkApartTipsAW.Checked = false;
                chkTips9AW.Checked = false;
            }
        }
        void chkApartTipsAW_CheckedChanged(object sender, EventArgs e)
        {
            bool check = chkApartTipsAW.Checked;
            cmbEnd.Enabled = check;
            cmbStart.Enabled = check;
            txtApart.Enabled = check;
            Share.wFiles.WriteBool("WCO", "ApartTipsAW", check);
        }

        void chkTips9AW_CheckStateChanged(object sender, EventArgs e)
        {
            Share.wFiles.WriteBool("WCO", "Tips9AW", chkTips9AW.Checked);
            if (chkTips9AW.Checked)
            {

                Tips9AWTimer.Enabled = chkTips9AW.Checked;

            }

        }

        void chkTipsAW_CheckStateChanged(object sender, EventArgs e)
        {
            bool tipsAwCheck = chkTipsAW.Checked;
            Share.wFiles.WriteBool("WCO", "TipsAW", tipsAwCheck);
            RegSet(tipsAwCheck);
        }
        void chkTim_CheckedChanged(object sender, EventArgs e)
        {
            bool chkTimb = chkTim.Checked;
            Share.wFiles.WriteBool("WCO", "chkTim", chkTimb);
            ApartTipsAWTim.Enabled = chkTimb;

            
        }


        void RegSet(bool chk)
        {

            RegistryKey key = Registry.CurrentUser;
            if (chk)
            {

                RegistryKey autoEndTasks = key.OpenSubKey("Control Panel\\Desktop\\", true); //该项必须已存在
                if (autoEndTasks != null) autoEndTasks.SetValue("AutoEndTasks", "0");
                RegistryKey hungAppTimeout = key.OpenSubKey("Control Panel\\Desktop\\", true); //该项必须已存在
                if (hungAppTimeout != null) hungAppTimeout.SetValue("HungAppTimeout", "5000");
                RegistryKey waitToKillAppTimeout = key.OpenSubKey("Control Panel\\Desktop\\", true); //该项必须已存在
                if (waitToKillAppTimeout != null) waitToKillAppTimeout.SetValue("WaitToKillAppTimeout", "20000");
            }
            else
            {
                RegistryKey autoEndTasks = key.OpenSubKey("Control Panel\\Desktop\\", true); //该项必须已存在
                if (autoEndTasks != null) autoEndTasks.SetValue("AutoEndTasks", "1");
                RegistryKey hungAppTimeout = key.OpenSubKey("Control Panel\\Desktop\\", true); //该项必须已存在
                if (hungAppTimeout != null) hungAppTimeout.SetValue("HungAppTimeout", "3000");
                RegistryKey waitToKillAppTimeout = key.OpenSubKey("Control Panel\\Desktop\\", true); //该项必须已存在
                if (waitToKillAppTimeout != null) waitToKillAppTimeout.SetValue("WaitToKillAppTimeout", "10000");
            }
        }

        private void cmbEnd_TextChanged(object sender, EventArgs e)
        {
            var value = cmbEnd.Text;
            if (IsNumberic(value))
            {
                endHour = value;
                Share.wFiles.WriteString("WCO", "ApartEndTime", value);
            }

        }

        void cmbStart_TextChanged(object sender, EventArgs e)
        {
            var value = cmbStart.Text;
            if (IsNumberic(value))
            {
                startHour = value;
                Share.wFiles.WriteString("WCO", "ApartStartTime", value);
            }
        }
        private void txtApart_TextChanged(object sender, EventArgs e)
        {
            var value = txtApart.Text;
            if (IsNumberic(value))
            {
                apartMinutes = value;
                Share.wFiles.WriteString("WCO", "ApartMinutes", value);
            }
        }
        public bool IsNumberic(string oText)
        {
            try
            {
                int var1 = Convert.ToInt32(oText);
                return true;
            }
            catch
            {
                return false;
            }
        }


        #endregion

        public static string WeiboUserName = "";
        public static string WeiboPassword = "";
        private void txtWeiboUserName_TextChanged(object sender, EventArgs e)
        {
            string str = txtWeiboUserName.Text.Trim();
            if (str!=CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "WeiboUserName", "")))
            {
                Share.wFiles.WriteString("WCO", "AcessToken", CryptHelper.EncryptString(""));
            }
            
            Share.wFiles.WriteString("WCO", "WeiboUserName", CryptHelper.EncryptString(str));
            
            WeiboUserName = str;
            
        }

        private void txttxtWeiboPassword_TextChanged(object sender, EventArgs e)
        {
            string str = txttxtWeiboPassword.Text.Trim();
            if (str!= CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "WeiboPassword", "")))
            {
                Share.wFiles.WriteString("WCO", "AcessToken", CryptHelper.EncryptString(""));
            }
            Share.wFiles.WriteString("WCO", "WeiboPassword", CryptHelper.EncryptString(str));
           
            WeiboPassword = str;

        }

        private bool LoadchkWeiboRemoteChk;
        private void chkWeiboRemote_CheckStateChanged(object sender, EventArgs e)
        {
            Share.wFiles.WriteBool("WCO", "ChkWeiboRemote", chkWeiboRemote.Checked);
            if (chkWeiboRemote.Checked)
            {
                if (LoadchkWeiboRemoteChk)
                {
                    return;
                }
                StartWeiboRemote();
            }
        }




    }
}
