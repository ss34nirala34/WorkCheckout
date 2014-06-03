using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using ReadWriteIni;
using Timer = System.Windows.Forms.Timer;

namespace WorkCheckout
{
    public partial class FrmStartWork : Form
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
        IniFiles wFiles = new IniFiles(AppDomain.CurrentDomain.BaseDirectory + "WCOConfig");
        public FrmStartWork()
        {
            InitializeComponent();
            var icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.Icon = icon;
            IntPtr hMenu = GetSystemMenu(this.Handle, false); //获取程序窗体的句柄    
          
            if (hMenu != IntPtr.Zero)
            {
                DeleteMenu(hMenu, SC_MOVE, MF_BYCOMMAND); //删除移动菜单，禁用移动功能    
                EnableMenuItem(hMenu, SC_CLOSE, MF_BYCOMMAND | MF_GRAYED | MF_DISABLED); //禁用关闭功能    
            }
            DateTime date = DateTime.Now;
            lblTime.Text = string.Format("{0}{1}", "现在的时间是：", string.Format("{0:yyyy-MM-dd ddd HH:mm:ss}", date));
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Tick += timer_Tick;
           
        }

        void timer_Tick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            lblTime.Text = string.Format("{0}{1}", "现在的时间是：", string.Format("{0:yyyy-MM-dd ddd HH:mm:ss}", date));
        }

        private void btnSW_Click(object sender, EventArgs e)
        {
            bool ownBrowser = wFiles.ReadBool("WCO", "OwnBrowser", true);
            if (ownBrowser)
            {
                FrmBrowser frm=new FrmBrowser();
                frm.TopMost = true;
                frm.Show();
               this.Close();
            }
            else
            {
                string target = "http://rd.tencent.com/outsourcing/attendances/add";
                try
                {
                    System.Diagnostics.Process.Start(target);

                }
                catch (System.ComponentModel.Win32Exception noBrowser)
                {
                    if (noBrowser.ErrorCode == -2147467259)
                        MessageBox.Show(noBrowser.Message);
                }
                catch (System.Exception other)
                {
                    MessageBox.Show(other.Message);
                }
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}
