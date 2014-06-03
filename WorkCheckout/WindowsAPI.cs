using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace WorkCheckout
{
    class WindowsAPI
    {
       

        public static bool IsAutoStartup()
        {
            bool isAutoStart = false;

            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            try
            {
                string tmp = (string)key.GetValue("WorkCheckout");
                isAutoStart = !string.IsNullOrEmpty(tmp) ? true : false;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return isAutoStart;
        }

        /// <summary>
        /// Adds the named item and its path to the Current User
        /// startup in registry.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="path"></param>
        public static void AddStartupItem(string name, string path)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            key.SetValue(name, path);
        }

        /// <summary>
        /// Removes the named item from Current User startup in registry.
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveStartupItem(string name)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            key.DeleteValue(name, false);
        }

    }
}
