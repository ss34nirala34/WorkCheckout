using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using ReadWriteIni;

namespace WorkCheckout
{
    public class Share
    {
        public static IniFiles wFiles = new IniFiles(AppDomain.CurrentDomain.BaseDirectory + "WCOConfig");

        /// <summary>
        /// Ping命令检测网络是否畅通
        /// </summary>
        /// <param name="urls">URL数据</param>
        /// <param name="errorCount">ping时连接失败个数</param>
        /// <returns></returns>
        public static bool MyPing(string[] urls, out int errorCount)
        {
            bool isconn = true;
            Ping ping = new Ping();
            errorCount = 0;
            try
            {
                PingReply pr;
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                for (int i = 0; i < urls.Length; i++)
                {
                    pr = ping.Send(urls[i], 1200, buffer, options);
                    if (pr.Status != IPStatus.Success)
                    {
                        isconn = false;
                        errorCount++;
                    }
                    Console.WriteLine("Ping " + urls[i] + "    " + pr.Status.ToString());
                }
            }
            catch
            {
                isconn = false;
                errorCount = urls.Length;
            }
            //if (errorCount > 0 && errorCount < 3)
            //  isconn = true;
            return isconn;
        }

        public static bool isToday(DateTime dt)
        {
            DateTime today = DateTime.Today;
            DateTime tempToday = new DateTime(dt.Year, dt.Month, dt.Day);
            if (today == tempToday)
                return true;
            else
                return false;
        }

        /// <summary>
        ///     与当前时间差几秒
        /// </summary>
        /// <param name="DateTime1">设置的时间</param>
        /// <returns></returns>
        public static Double DateDiffTotalSecond(DateTime DateTime1)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            Double douLen = ts.TotalSeconds;
            return douLen;
        }

        /// <summary>
        /// 算两个时间段的秒数
        /// </summary>
        /// <param name="DateTime1">时间1（小）</param>
        /// <param name="DateTime2">时间2（大）</param>
        /// <returns></returns>
        public static Double DateTotalSecond(DateTime DateTime1, DateTime DateTime2)
        {
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            Double douLen = ts.TotalSeconds;
            return douLen;
        }
         [System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetOption(int hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);

        public static unsafe void SuppressWininetBehavior()
        {
            /* SOURCE: http://msdn.microsoft.com/en-us/library/windows/desktop/aa385328%28v=vs.85%29.aspx
                * INTERNET_OPTION_SUPPRESS_BEHAVIOR (81):
                *      A general purpose option that is used to suppress behaviors on a process-wide basis. 
                *      The lpBuffer parameter of the function must be a pointer to a DWORD containing the specific behavior to suppress. 
                *      This option cannot be queried with InternetQueryOption. 
                *      
                * INTERNET_SUPPRESS_COOKIE_PERSIST (3):
                *      Suppresses the persistence of cookies, even if the server has specified them as persistent.
                *      Version:  Requires Internet Explorer 8.0 or later.
                */

            int option = (int) 3 /* INTERNET_SUPPRESS_COOKIE_PERSIST*/;
            int* optionPtr = &option;

            bool success = InternetSetOption(0, 81 /*INTERNET_OPTION_SUPPRESS_BEHAVIOR*/, new IntPtr(optionPtr),
                sizeof (int));
            if (!success)
            {
                MessageBox.Show("Something went wrong !>?");
            }
        }

    }
}
