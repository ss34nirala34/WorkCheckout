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
using mshtml;
using SHDocVw;

namespace WorkCheckout
{
    public partial class FrmBrowser : Form
    {
        //禁用关闭按钮
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hwnd, bool bRevert);
        [DllImport("user32.dll")]
        static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);
        [DllImport("user32.dll")]
        static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        const uint SC_MOVE = 0xF010; //移动    
        const uint SC_CLOSE = 0xF060;//关闭    

        const uint MF_BYCOMMAND = 0x00; //按命令方式    
        const uint MF_GRAYED = 0x01;    //灰掉    
        const uint MF_DISABLED = 0x02;  //不可用  
        SHDocVw.WebBrowser wb;
        private mshtml.IHTMLDocument2 doc;
        public FrmBrowser()
        {
            var icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.Icon = icon;
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            //IntPtr hMenu = GetSystemMenu(this.Handle, false); //获取程序窗体的句柄    

            //if (hMenu != IntPtr.Zero)
            //{
            //    DeleteMenu(hMenu, SC_MOVE, MF_BYCOMMAND); //删除移动菜单，禁用移动功能    
            //    EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED | MF_DISABLED); //禁用关闭功能    
            //}
            this.Closed += FrmBrowser_Closed;
        }

        void FrmBrowser_Closed(object sender, EventArgs e)
        {
            //Share.wFiles.WriteString("WCO", "LastSignInTime", string.Format("{0:yyyy-MM-dd HH:mm:ss}",DateTime.Now));
        }

        private void FrmBrowser_Load(object sender, EventArgs e)
        {

            try
            {
                Share.SuppressWininetBehavior();
                webBrowser1.Navigate("http://rd.tencent.com/outsourcing/attendances/add");
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

        //private void wb_NavigateComplete2(object pdisp, ref object url)
        //{
        //    mshtml.IHTMLDocument2 doc = (this.webBrowser1.ActiveXInstance as SHDocVw.WebBrowser).Document as mshtml.IHTMLDocument2;
        //    doc.parentWindow.execScript("function alert(str){return ''}", "javascript");
        //}
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
        private void waitwebCompleted()
        {
            do
            {
                if (webBrowser1.IsBusy)
                {
                    Application.DoEvents();
                    //如果isbusy就继续执行
                }
                else
                {
                    System.DateTime tmpNow = DateTime.Now;
                    while (!(webBrowser1.IsBusy | DateTime.Now.Subtract(tmpNow).Seconds > 0.5))
                    {
                        Application.DoEvents();
                    }
                    if (webBrowser1.IsBusy)
                    {
                        continue;
                    }
                    else
                    {
                        return;
                    }
                    //如果在0.5秒之内IsBusy始终为false没有变化,就结束
                }
            } while (true);
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

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            string urlAdd = "http://rd.tencent.com/top/ptlogin/ptlogins/login?site=";
            string urlAdd2 = "http://tapd.tencent.com/ptlogin/ptlogins/login?";
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
            HtmlElement htmlEle = webBrowser1.Document.GetElementById("checkin_btn");

            if (htmlEle != null)
            {
                // 激活html元素的 click 成员  
                htmlEle.InvokeMember("click");

                System.Threading.Thread submitT = new Thread(() =>
                    {
                        Thread.Sleep(1000);
                        bool search = true;
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
                                            Share.wFiles.WriteString("WCO", "LastSignInTime", string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                                    FrmSet.lastCheckInTime=FrmSet.CheckInTime = DateTime.Now;

                                        }
                                    }
                                }));

                            Thread.Sleep(1000);
                        }
                    });
                submitT.Start();
            }
        }
        CheckTimeAgWB check;
        private void FrmBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            string lastSignInTimeStr = Share.wFiles.ReadString("WCO", "LastSignInTime", "");
            DateTime date = DateTime.Now;
            if (lastSignInTimeStr != string.Empty)
            {
                DateTime lastSignInTime = DateTime.ParseExact(lastSignInTimeStr, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);


                if (!Share.isToday(lastSignInTime))
                {
                    if (check!=null)
                    {
                       check.Dispose(); 
                    }
                    check = new CheckTimeAgWB();
                    check.CheckAg();
                }

            }
            else
            {
                if (check != null)
                {
                    check.Dispose();
                }
                check = new CheckTimeAgWB();
                check.CheckAg();
            }
        }

    }
}
