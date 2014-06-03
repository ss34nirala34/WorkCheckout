using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace RunCmd
{
    
    public class Cmd
    {
        public bool CmdError;
        public string RunCmd(string command, string rootpath)
        {
            CmdError = false;
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = command;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;

            try
            {
                p.Start();
                p.StandardInput.WriteLine(command);
                p.StandardInput.AutoFlush = true;
                p.StandardInput.WriteLine("exit");
                p.WaitForExit(500);
                StreamReader reader = p.StandardOutput;
                StreamReader errorstr = p.StandardError;
                
                string text = reader.ReadToEnd().Trim();
                
                if (text.IndexOf(command) != -1)
                {
                    int start = text.IndexOf(command) + command.Length;
                    string endstring = rootpath + ">exit";
                    int end = text.IndexOf(endstring);
                    text = text.Substring(start, text.Length - start - endstring.Length + 0).Trim();
                    if (text == string.Empty)
                    {
                        text = errorstr.ReadToEnd().Trim();
                        if (text!=string.Empty)
                        {
                            CmdError = true;
                        }
                        
                        return text;
                    }
                    else
                    {
                        return text;
                    }
                }
                return "";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ex.Message;
            }
            finally
            {
                p.Close();
            }
        }
        /// <summary>
        /// 管道方式获得cmd返回值
        /// </summary>
        /// <param name="command">cmd命令</param>
        /// <param name="rootpath">当前程序调用cmd运行的目录</param>
        /// <param name="errorStr">接收cmd的错误提示</param>
        /// <returns></returns>
        public static string RunCmd(string command, string rootpath, out string errorStr)
        {
            errorStr = "";
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = command;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;

            try
            {
                p.Start();
                p.StandardInput.WriteLine(command);
                p.StandardInput.AutoFlush = true;
                p.StandardInput.WriteLine("exit");
                p.WaitForExit(500);
                StreamReader reader = p.StandardOutput;
                StreamReader errorstr = p.StandardError;

                string text = reader.ReadToEnd().Trim();

                if (text.IndexOf(command) != -1)
                {
                    int start = text.IndexOf(command) + command.Length;
                    string endstring = rootpath + ">exit";
                    int end = text.IndexOf(endstring);
                    text = text.Substring(start, text.Length - start - endstring.Length + 0).Trim();
                   
                }

                errorStr = errorstr.ReadToEnd().Trim();

                return text;

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ex.Message;
            }
            finally
            {
                p.Close();
            }
        }
    }
}