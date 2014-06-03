using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using RunCmd;

namespace WorkCheckout
{
    public partial class FrmAfterWork : Form
    {
        //禁用关闭按钮
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hwnd, bool bRevert);

        [DllImport("user32.dll")]
        private static extern bool DeleteMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        [DllImport("user32.dll")]
        private static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, uint uEnable);

        private const uint SC_MOVE = 0xF010; //移动    
        private const uint SC_CLOSE = 0xF060; //关闭    

        private const uint MF_BYCOMMAND = 0x00; //按命令方式    
        private const uint MF_GRAYED = 0x01; //灰掉    
        private const uint MF_DISABLED = 0x02; //不可用  

        public FrmAfterWork()
        {
            InitializeComponent();

            Version ver = System.Environment.OSVersion.Version;
            if (ver.Major >= 6)
            {
                ShutdownBlockReasonCreate(this.Handle, "您今天签出了吗？点击取消可返回！！！！！！！！！");
            }
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

        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            lblTime.Text = string.Format("{0}{1}", "现在的时间是：", string.Format("{0:yyyy-MM-dd ddd HH:mm:ss}", date));
        }

        private void btnAW_Click(object sender, EventArgs e)
        {
            bool ownBrowser = Share.wFiles.ReadBool("WCO", "OwnBrowser", true);
            if (ownBrowser)
            {
                FrmBrowserAW frm = new FrmBrowserAW();
                frm.TopMost = true;
                frm.Show();
                this.Close();
                FrmSet.blocked = true;

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
                FrmSet.blocked = true;
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



        [DllImport("user32.dll")]
        public extern static bool ShutdownBlockReasonCreate(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] string pwszReason);


        [DllImport("user32.dll")]
        public extern static bool ShutdownBlockReasonDestroy(IntPtr hWnd);

        Cmd cmd = new Cmd();
        private void btnShutdown_Click(object sender, EventArgs e)
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
