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
using SHDocVw;
using WebBrowser = System.Windows.Forms.WebBrowser;



namespace WorkCheckout
{
   public class HidWebBrowser
    {
       SHDocVw.WebBrowser wb;
        mshtml.IHTMLDocument2 doc;
       WebBrowser webBrowser=new WebBrowser();
       bool search = true;
       public FrmSet.ShowTipsDelegate showTipsDelegate;
       public FrmSet.ShowWindowsDele _showWindows;
       public void HidWebBrowserRun()
       {
           webBrowser.DocumentCompleted += webBrowser_DocumentCompleted;
          
           try
           {
               search = true;
               Share.SuppressWininetBehavior();
               webBrowser.Navigate("http://om.tencent.com/attendances/check_out/");
               wb = webBrowser.ActiveXInstance as SHDocVw.WebBrowser;
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
           HtmlElement htmlEle = webBrowser.Document.GetElementById("checkout_btn");

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
                           webBrowser.Invoke(new Action(() =>
                           {

                               HtmlDocument cd = webBrowser.Document;
                               HtmlElementCollection dhl = cd.GetElementsByTagName("BUTTON");

                               foreach (HtmlElement item in dhl)
                               {
                                   if (item.InnerText == "提交" || item.InnerText == "Submit")
                                   {
                                       item.InvokeMember("click");
                                       search = false;
                                       Share.wFiles.WriteString("WCO", "LastCheckOutTime", string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));

                                       showTipsDelegate();

                                       webBrowser.Dispose();

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
               if (webBrowser != null)
               {
                   webBrowser.Dispose();
                   webBrowser = null;
               }
           }
           // 清理非托管资源
           if (wb != null)
           {
               wb = null;
           }
           //让类型知道自己已经被释放
           disposed = true;
       }
    }
}
