using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WorkCheckout
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new FrmSet());
        }
        #region 托盘图标

        public static System.Windows.Forms.NotifyIcon notifyIcon = null;
        private static void InitialTray()
        {

            //设置托盘的各个属性
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            //notifyIcon.BalloonTipText = "程序开始运行";
            notifyIcon.Text = "XinBSWallPaper";
            //notifyIcon.Icon = new System.Drawing.Icon(System.Windows.Forms.Application.StartupPath + \"\\\\wp.ico\");
            notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            notifyIcon.Visible = true;
            //notifyIcon.ShowBalloonTip(2000);
            notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(notifyIcon_MouseClick);

            //设置菜单项
            //System.Windows.Forms.MenuItem menu1 = new System.Windows.Forms.MenuItem("显示");
            //System.Windows.Forms.MenuItem menu2 = new System.Windows.Forms.MenuItem("菜单项2");
            //System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("菜单", new System.Windows.Forms.MenuItem[] { menu1 , menu2 });
            System.Windows.Forms.MenuItem menu = new System.Windows.Forms.MenuItem("显示");
            menu.Click += menu_Click;
            //退出菜单项
            System.Windows.Forms.MenuItem exit = new System.Windows.Forms.MenuItem("退出");
            exit.Click += new EventHandler(exit_Click);

            //关联托盘控件
            System.Windows.Forms.MenuItem[] childen = new System.Windows.Forms.MenuItem[] { menu, exit };
            notifyIcon.ContextMenu = new System.Windows.Forms.ContextMenu(childen);

            //窗体状态改变时候触发
            //this.StateChanged += new EventHandler(SysTray_StateChanged);
        }

        static void menu_Click(object sender, EventArgs e)
        {


            //this.Visibility = Visibility.Visible;
            //this.WindowState = WindowState.Normal;
            //this.Activate();
        }
        ///
        /// 窗体状态改变时候触发
        ///
        ///

        ///

        private static void SysTray_StateChanged(object sender, EventArgs e)
        {
            //if (this.WindowState == WindowState.Minimized)
            //{


            //    this.Visibility = Visibility.Hidden;
            //}
        }

        ///
        /// 退出选项
        ///
        ///

        ///

        private static void exit_Click(object sender, EventArgs e)
        {
            //if (System.Windows.MessageBox.Show("确定要关闭[XinBSWallPaper]吗?",
            //                                   "退出",
            //                                    MessageBoxButton.YesNo,
            //                                    MessageBoxImage.Question,
            //                                    MessageBoxResult.No) == MessageBoxResult.Yes)
            //{
            //    notifyIcon.Dispose();
            //    Environment.Exit(0);
            //    System.Windows.Application.Current.Shutdown();
            //}
        }

        ///
        /// 鼠标单击
        ///
        ///

        ///

        private static void notifyIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if (e.Button == System.Windows.Forms.MouseButtons.Left)
            //{
            //    if (this.Visibility == Visibility.Visible)
            //    {
            //        this.Visibility = Visibility.Hidden;
            //    }
            //    else
            //    {

            //        jianbian();
            //        this.WindowState = WindowState.Maximized;

            //        this.Visibility = Visibility.Visible;
            //        Thread normalT = new Thread(() =>
            //        {
            //            //Thread.Sleep(500);
            //            this.Dispatcher.Invoke(new Action(() => this.WindowState = WindowState.Normal));
            //        });
            //        normalT.Start();
            //        //this.WindowState = WindowState.Normal;
            //        this.Activate();
            //    }

            //}

        }
       
        #endregion
        private static void StartWork()
        {
            
        }
    }
}
