using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using EncryptionHelper;
using NetDimension.Weibo;
using SHDocVw;
using WebBrowser = System.Windows.Forms.WebBrowser;

namespace WorkCheckout
{
    class WeiboAutoCheckInWebBrowser : IDisposable
    {
        SHDocVw.WebBrowser wb;
        mshtml.IHTMLDocument2 doc;
        WebBrowser webBrowserWb = new WebBrowser();
        private static NetDimension.Weibo.Client Sina;
        public string Type { get; set; }
        public void WeiboCheckIn( OAuth oauth,string type)
        {
            WeiboOauth = oauth;
            Type = type;
            UserName = CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "UserName", ""));
            PassWord = CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "PassWord", ""));
            webBrowserWb.ScriptErrorsSuppressed = true;
            webBrowserWb.DocumentCompleted += webBrowserAg_DocumentCompleted;
       
            try
            {
                Share.SuppressWininetBehavior();
                webBrowserWb.Navigate("http://rd.tencent.com/outsourcing/attendances/add");
                wb = webBrowserWb.ActiveXInstance as SHDocVw.WebBrowser;
                wb.NavigateComplete2 += new DWebBrowserEvents2_NavigateComplete2EventHandler(wb_NavigateComplete2);
            }
            catch (Exception)
            {

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

        public OAuth WeiboOauth { get; set; }
        public string PassWord { get; set; }

        public string UserName { get; set; }

        private void webBrowserAg_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string urlAdd = "http://rd.tencent.com/top/ptlogin/ptlogins/login?site=";
            string urlAdd2 = "http://tapd.tencent.com/ptlogin/ptlogins/login?";
            if ((webBrowserWb.Url != null && webBrowserWb.Url.ToString().Contains(urlAdd)) || (webBrowserWb.Url != null && webBrowserWb.Url.ToString().Contains(urlAdd2)))
            {
                //判断是否已加载完网页
                if (webBrowserWb.ReadyState == WebBrowserReadyState.Complete)
                {
                    //获取网页文档对象，相当于获取网页的全部源码
                    HtmlDocument htmlDoc = this.webBrowserWb.Document;
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
            HtmlElement htmlEle = webBrowserWb.Document.GetElementById("checkin_btn");

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
                        webBrowserWb.Invoke(new Action(() =>
                        {
                            HtmlDocument cd = webBrowserWb.Document;
                            HtmlElementCollection dhl = cd.GetElementsByTagName("BUTTON");

                            foreach (HtmlElement item in dhl)
                            {
                                if (item.InnerText == "提交" || item.InnerText == "Submit")
                                {
                                    item.InvokeMember("click");
                                    search = false;
                                    Share.wFiles.WriteString("WCO", "LastSignInTime", string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                                    FrmSet.CheckInTime = DateTime.Now;
                                    Sina = new Client(WeiboOauth);
                                    Sina.API.Entity.Statuses.Update(string.Format("{0} {1} {2}",Type, "成功签入", string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now)));
                                    webBrowserWb.Dispose();
                                }
                            }
                        }));

                        Thread.Sleep(1000);
                    }
                });
                submitT.Start();
            } 
        }

        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }
       bool  disposed = false;
        /// <summary>
        /// 非密封类修饰用protected virtual
        /// 密封类修饰用private
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {
                // 清理托管资源
                if (webBrowserWb != null)
                {
                    
                    webBrowserWb = null;
                }
            }
            // 清理非托管资源
            if (wb != null)
            {
               wb=null;
            }
            //让类型知道自己已经被释放
            disposed = true;
        }
    }
}
