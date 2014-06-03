using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Common;
using EncryptionHelper;
using NetDimension.Weibo;
using SHDocVw;
using WebBrowser = System.Windows.Forms.WebBrowser;


namespace WorkCheckout
{
    public class WeiboRemote
    {
        static string AppKey = "2202783286";
        static string AppSecret = "c0bd7699bb222ec2408288d28f739310";
        private static string CallbackUrl = "https://api.weibo.com/oauth2/default.html";
        private static bool isPing;
        static OAuth oauth = null;

        private static NetDimension.Weibo.Client Sina;
        void GetWeibo()
        {
            LogInWeibo();//登录验证
            Sina = new Client(oauth);
            var json = Sina.API.Entity.Statuses.UserTimeline(count: 1);
            if (json.Statuses == null) return;
            foreach (var status in json.Statuses)
            {
                if (status.User == null)
                    continue;
                Console.WriteLine(status.Text);
            }


        }
        public OAuth LogInWeibo()
        {
            int error;
            string[] urls = { "www.weibo.com" };
            isPing = Share.MyPing(urls, out error);
            while (!isPing)
            {
                isPing = Share.MyPing(urls, out error);
                Console.WriteLine("未连接网络....");
                Thread.Sleep(1000);
            }
            Console.WriteLine("成功连网络....");
            var token = Share.wFiles.ReadString("WCO", "AcessToken", "");
            string accessToken = CryptHelper.DecryptString(token);
            //string accessToken = "2.00OSWZREzoSwED929b3de75dapfDMB";
            if (string.IsNullOrEmpty(accessToken))	//判断配置文件中有没有保存到AccessToken，如果没有就进入授权流程
            {
                oauth = Authorize();


                if (!string.IsNullOrEmpty(oauth.AccessToken))
                {

                    Share.wFiles.WriteString("WCO", "AcessToken", CryptHelper.EncryptString(oauth.AccessToken));
                }

                return oauth;
            }
            else//如果配置文件中保存了AccesssToken
            {
                Console.WriteLine("获取到已保存的AccessToken{{{0}}}！", accessToken);
                oauth = new OAuth(AppKey, AppSecret, accessToken, "");	//用Token实例化OAuth无需再次进入验证流程
                Console.WriteLine("验证Token有效性...");
                TokenResult result = oauth.VerifierAccessToken();	//测试保存的AccessToken的有效性
                if (result == TokenResult.Success)
                {
                    return oauth;
                }
                else
                {
                    Console.WriteLine("AccessToken无效！因为：{0}", result);
                    Share.wFiles.WriteString("WCO", "AcessToken", CryptHelper.EncryptString(""));
                    Console.WriteLine("已从配置文件移除AccessToken值，重新运行程序获取有效的AccessToken");
                    oauth = Authorize();
                    return oauth;
                }
            }
        }

        private string Code = null;
        private OAuth o;
        OAuth Authorize()
        {
            o = new OAuth(AppKey, AppSecret, CallbackUrl);
            try
            {
               
                string authorizeUrl = o.GetAuthorizeURL();
                Code = null;
                webBrowser = new WebBrowser();
                GetoAuthA(authorizeUrl);
               
            }
            catch (Exception ex)
            {
                LogUtil.WriteError(ex); 
            }
            return o;
        }

        //static OAuth Authorize()
        //{
        //    OAuth o = new OAuth(AppKey, AppSecret, CallbackUrl);
        //    while (ClientLogin(o)!=null)	//使用模拟方法
        //    {
        //        Console.WriteLine("授权登录失败，请重试。");
        //    }

        //    return o;
        //}

        //private static AccessToken ClientLogin(OAuth o)
        //{
        //    Console.Write("微博账号:");
        //    string passport = FrmSet.WeiboUserName;
        //    Console.Write("登录密码:");

        //    string password = FrmSet.WeiboPassword;

        //    var result = o.GetAccessTokenByPassword(passport, password);
        //    return result;
        //}
        private string passport;


        private string password;
        static SHDocVw.WebBrowser wb;
        static mshtml.IHTMLDocument2 doc;
        private static WebBrowser webBrowser;
        public void GetoAuthA(string url)
        {

            webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
            passport = FrmSet.WeiboUserName;
            password = FrmSet.WeiboPassword;
            try
            {

                //Share.SuppressWininetBehavior();
                webBrowser.Navigate(url);
                wb = webBrowser.ActiveXInstance as SHDocVw.WebBrowser;
                wb.NavigateComplete2 += new DWebBrowserEvents2_NavigateComplete2EventHandler(wb_NavigateComplete2);
                //waitwebCompleted();
                //SHDocVw.WebBrowser wb = this.webBrowser1.ActiveXInstance as SHDocVw.WebBrowser;
                //wb.NavigateComplete2 += new SHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(wb_NavigateComplete2);

            }
            catch (Exception ex)
            {
                LogUtil.WriteError(ex);
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
        private System.Threading.Thread submitT;
        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                string urlAdd = "https://api.weibo.com/oauth2/authorize";

                if ((webBrowser.Url != null && webBrowser.Url.ToString().Contains(urlAdd)))
                {
                    //判断是否已加载完网页
                    if (webBrowser.ReadyState == WebBrowserReadyState.Complete)
                    {
                        //获取网页文档对象，相当于获取网页的全部源码
                        HtmlDocument htmlDoc = webBrowser.Document;
                        //设置帐号
                        HtmlElement id = htmlDoc.GetElementById("userId");
                        id.SetAttribute("value", passport);
                        //设置密码
                        HtmlElement pwd = htmlDoc.GetElementById("passwd");
                        pwd.SetAttribute("value", password);
                        //登录

                        HtmlDocument cd = webBrowser.Document;
                        HtmlElementCollection dhl = cd.GetElementsByTagName("a");

                        foreach (HtmlElement item in dhl)
                        {

                            if (item.GetAttribute("action-type").Equals("submit"))
                            {
                                item.InvokeMember("click");
                                break;
                            }
                        }
                        HtmlElement btn = htmlDoc.GetElementById("submit");
                        if (btn != null)
                        {
                            btn.InvokeMember("click");
                        }
                    }
                }
                // 根据id找到对应的元素  
                string oauth2Url = "https://api.weibo.com/oauth2/default.html?code=";

                if ((webBrowser.Url != null && webBrowser.Url.ToString().Contains(oauth2Url)))
                {
                    //MessageBox.Show(webBrowser.Url.ToString());
                    var url = webBrowser.Url.ToString();
                    char[] cha = { '=' };
                    string[] strs = url.Split(cha, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Count() == 2)
                    {
                        Code = strs[1];
                        try
                        {
                            AccessToken accessToken = o.GetAccessTokenByAuthorizationCode(strs[1]); //请注意这里返回的是AccessToken对象，不是string
                            Share.wFiles.WriteString("WCO", "AcessToken", CryptHelper.EncryptString(oauth.AccessToken));
                            if (submitT != null && submitT.IsAlive)
                            {
                                submitT.Abort();
                            }
                            webBrowser.Dispose();
                        }
                        catch (WeiboException ex)
                        {
                            Console.WriteLine(ex.Message);
                            if (submitT != null && submitT.IsAlive)
                            {
                                submitT.Abort();
                            }
                            webBrowser.Dispose();
                        }
                    }

                }
                string authorizeUrl = "https://api.weibo.com/oauth2/authorize";
                submitT = new Thread(() =>
                {
                    Thread.Sleep(1000);
                    bool search = true;
                    while (search)
                    {

                        if (!webBrowser.IsHandleCreated)
                        {
                            return;
                        }
                        webBrowser.Invoke(new Action(() =>
                        {

                            if (webBrowser.Url != null && webBrowser.Url.ToString() == authorizeUrl)
                            {

                                HtmlDocument cd = webBrowser.Document;
                                HtmlElementCollection dhl = cd.GetElementsByTagName("a");

                                foreach (HtmlElement item in dhl)
                                {

                                    if (item.GetAttribute("action-type").Equals("submit"))
                                    {
                                        item.InvokeMember("click");
                                        search = false;
                                        break;
                                    }
                                }

                            }

                        }));

                        Thread.Sleep(500);
                    }
                });
                submitT.Start();
              

            }
            catch (Exception ex)
            {

                LogUtil.WriteError(ex);
            }


        }
    }
}

