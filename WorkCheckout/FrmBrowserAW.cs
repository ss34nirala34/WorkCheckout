using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using RunCmd;
using SHDocVw;
using Timer = System.Windows.Forms.Timer;

namespace WorkCheckout
{
    public partial class FrmBrowserAW : Form
    {
        [DllImport("user32.dll")]
        public extern static bool ShutdownBlockReasonCreate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string pwszReason);

        [DllImport("user32.dll")]
        public extern static bool ShutdownBlockReasonDestroy(IntPtr hWnd);

        public bool Exit;

        SHDocVw.WebBrowser wb;
        mshtml.IHTMLDocument2 doc;

        public FrmSet.ShowTipsDelegate showTipsDelegate;
        public FrmBrowserAW()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            var icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.Icon = icon;
            Version ver = System.Environment.OSVersion.Version;
            if (ver.Major >= 6)
            {
                ShutdownBlockReasonCreate(this.Handle, "您今天签出了吗？点击取消可返回！！！！！！！！！");
            }
        }
        protected override void WndProc(ref Message aMessage)
        {
            const int WM_QUERYENDSESSION = 0x0011;
            const int WM_ENDSESSION = 0x0016;

            if ((aMessage.Msg == WM_QUERYENDSESSION || aMessage.Msg == WM_ENDSESSION))
            {
                aMessage.Result = (IntPtr)0;

            }
            base.WndProc(ref aMessage);

        }

        private void FrmBrowserAW_Load(object sender, EventArgs e)
        {

            try
            {
                //timer.Enabled = true;
                //timer.Interval = 1000;
                //timer.Tick += timer_Tick;
                search = true;
                Share.SuppressWininetBehavior();
                webBrowser1.Navigate("http://om.tencent.com/attendances/check_out/");
                wb = webBrowser1.ActiveXInstance as SHDocVw.WebBrowser;
                wb.NavigateComplete2 += new DWebBrowserEvents2_NavigateComplete2EventHandler(wb_NavigateComplete2);
                //waitwebCompleted();
                //SHDocVw.WebBrowser wb = this.webBrowser1.ActiveXInstance as SHDocVw.WebBrowser;
                //wb.NavigateComplete2 += new SHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(wb_NavigateComplete2);

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void wb_NavigateComplete2(object pdisp, ref object url)
        {
            try
            {
                doc = wb.Document as mshtml.IHTMLDocument2;
                //执行javascript脚本，覆盖系统的confirm函数，直接return true，这样调用confirm函数都会执行到确认按钮了，同理可以重写系统中的其他函数
                doc.parentWindow.execScript("function confirm(){return true;}", "javascript");
                doc.parentWindow.execScript("function alert(){}", "javascript");//设置为alert为空函数体，就不会挂起javascript代码执行了
            }
            catch (Exception)
            {


            }
        }

        Timer timer = new Timer();
        void timer_Tick(object sender, EventArgs e)
        {
            //string urlAdd = "https://rd.tencent.com/top/ptlogin/ptlogins/login?site=";
            //if (webBrowser1.Url != null && webBrowser1.Url.ToString().Contains(urlAdd))
            //{
            //    //判断是否已加载完网页
            //    if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            //    {
            //        //获取网页文档对象，相当于获取网页的全部源码
            //        HtmlDocument htmlDoc = this.webBrowser1.Document;
            //        //设置帐号
            //        HtmlElement id = htmlDoc.GetElementById("username");
            //        id.SetAttribute("value", "v_xqliang");
            //        //设置密码
            //        HtmlElement pwd = htmlDoc.GetElementById("password_input");
            //        pwd.SetAttribute("value", "nr5674NRU");
            //        //登录
            //        HtmlElement btn = htmlDoc.GetElementById("login_button");
            //        if (btn != null)
            //        {
            //            btn.InvokeMember("click");
            //        }
            //    }
            //}
        }
        bool search = true;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            string urlAdd = "https://rd.tencent.com/top/ptlogin/ptlogins/login?";
            string urlAdd2 = "https://tapd.tencent.com/ptlogin/ptlogins/login?";
            if ((webBrowser1.Url != null && webBrowser1.Url.ToString().Contains(urlAdd)) || (webBrowser1.Url != null && webBrowser1.Url.ToString().Contains(urlAdd2)))
            {
                //判断是否已加载完网页
                if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
                {
                    //获取网页文档对象，相当于获取网页的全部源码
                    HtmlDocument htmlDoc = this.webBrowser1.Document;
                    //设置帐号
                    HtmlElement id = htmlDoc.GetElementById("username");
                    id.SetAttribute("value", FrmSet.UserName);
                    //设置密码
                    HtmlElement pwd = htmlDoc.GetElementById("password_input");
                    pwd.SetAttribute("value", FrmSet.PassWord);
                    //登录
                    HtmlElement btn = htmlDoc.GetElementById("login_button");
                    if (btn != null)
                    {
                        btn.InvokeMember("click");
                    }
                }
            }
            // 根据id找到对应的元素  
            HtmlElement htmlEle = webBrowser1.Document.GetElementById("checkout_btn");

            if (htmlEle != null)
            {
                // 激活html元素的 click 成员  
                if (search)
                {
                    htmlEle.InvokeMember("click");

                }

                System.Threading.Thread submitT = new Thread(() =>
                {
                    Thread.Sleep(1000);
                    try
                    {
                        while (search)
                        {
                            this.Invoke(new Action(() =>
                            {
                                HtmlDocument cd = webBrowser1.Document;
                                HtmlElementCollection dhl = cd.GetElementsByTagName("BUTTON");

                                foreach (HtmlElement item in dhl)
                                {
                                    if (item.InnerText == "提交" || item.InnerText == "Submit")
                                    {
                                        item.InvokeMember("click");
                                        search = false;
                                        Share.wFiles.WriteString("WCO", "LastCheckOutTime", string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                                        if (Exit)
                                        {
                                            showTipsDelegate();
                                            this.Close();

                                        }

                                    }
                                }
                            }));

                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        LogUtil.WriteError(ex);
                    }

                    
                });
                submitT.Start();
            }
        }

        private void webBrowser1_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            progressBar1.Visible = true;
            if ((e.CurrentProgress > 0) && (e.MaximumProgress > 0))
            {
                progressBar1.Maximum = Convert.ToInt32(e.MaximumProgress);
                progressBar1.Step = Convert.ToInt32(e.CurrentProgress);
                progressBar1.PerformStep();
            }
            else if (webBrowser1.ReadyState == WebBrowserReadyState.Complete)
            {
                progressBar1.Value = 0;
                progressBar1.Visible = false;
            }
        }
        Cmd cmd = new Cmd();
        private void btnShutDown_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定现在要关机吗？", "关机", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                FrmSet.CloseCance = false;
                cmd.RunCmd("Shutdown -s -t 00", Application.StartupPath);

            }

        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定现在要重启吗？", "重启", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                FrmSet.CloseCance = false;
                cmd.RunCmd("Shutdown -r -t 00", Application.StartupPath);

            }

        }

    }
}
