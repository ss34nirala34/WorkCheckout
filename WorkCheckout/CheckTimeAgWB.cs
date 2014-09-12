using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EncryptionHelper;
using HtmlDocument = System.Windows.Forms.HtmlDocument;
using HtmlDocumentAgilityPack = HtmlAgilityPack.HtmlDocument;

namespace WorkCheckout
{
    class CheckTimeAgWB
    {
        #region 再次检查上次登录时间

        public delegate void CheckLastChkInTAg();
        WebBrowser webBrowserAg = new WebBrowser();
        public void CheckAg()
        {
            UserName = CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "UserName", ""));
            PassWord = CryptHelper.DecryptString(Share.wFiles.ReadString("WCO", "PassWord", ""));
            webBrowserAg.ScriptErrorsSuppressed = true;
            webBrowserAg.DocumentCompleted += webBrowserAg_DocumentCompleted;
            try
            {
                Share.SuppressWininetBehavior();
                webBrowserAg.Navigate("http://rd.tencent.com/outsourcing/attendances/add");
            }
            catch (Exception)
            {
               
            }
        }

        public string PassWord { get; set; }

        public string UserName { get; set; }

        private void webBrowserAg_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string urlAdd = "http://rd.tencent.com/top/ptlogin/ptlogins/login?site=";
            string urlAdd2 = "http://tapd.tencent.com/ptlogin/ptlogins/login?";
            if ((webBrowserAg.Url != null && webBrowserAg.Url.ToString().Contains(urlAdd)) || (webBrowserAg.Url != null && webBrowserAg.Url.ToString().Contains(urlAdd2)))
            {
                //判断是否已加载完网页
                if (webBrowserAg.ReadyState == WebBrowserReadyState.Complete)
                {
                    //获取网页文档对象，相当于获取网页的全部源码
                    HtmlDocument htmlDoc = this.webBrowserAg.Document;
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
            string urlCheckOut = "http://om.tencent.com/attendances/check_out";
            string urlCheckIn = "http://rd.tencent.com/outsourcing/attendances/add";
            if (webBrowserAg.Url != null)
            {
                if (webBrowserAg.Url.ToString().Contains(urlCheckOut) || webBrowserAg.Url.ToString().Contains(urlCheckIn))
                {
                    //判断是否已加载完网页
                    if (webBrowserAg.ReadyState == WebBrowserReadyState.Complete)
                    {
                        HtmlDocumentAgilityPack htmlDoc1 = new HtmlDocumentAgilityPack();
                        htmlDoc1.LoadHtml(webBrowserAg.DocumentText);
                        string txt = "";
                        try
                        {
                            txt = htmlDoc1.DocumentNode.SelectSingleNode(@"./html[1]/body[1]/div[3]/div[1]/div[1]/div[4]/div[1]/p[1]/span[1]").InnerText.TrimEnd();
                        }
                        catch (Exception)
                        {
                        }

                        if (txt != string.Empty)
                        {
                            DateTime LastCheckInTime;
                            try
                            {
                                FrmSet.lastCheckInTime= FrmSet.CheckInTime = LastCheckInTime = DateTime.ParseExact(txt, "yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                                Share.wFiles.WriteString("WCO", "LastSignInTime", string.Format("{0:yyyy-MM-dd HH:mm:ss}", LastCheckInTime));
                                webBrowserAg.Dispose();

                            }
                            catch (Exception)
                            {
                                webBrowserAg.Dispose();
                            }

                        }

                    }

                }
            }
        }

        #endregion
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }
        bool disposed = false;
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
                if (webBrowserAg != null)
                {
                    webBrowserAg.Dispose();
                    webBrowserAg = null;
                }
            }
          
            //让类型知道自己已经被释放
            disposed = true;
        }
    }
}
