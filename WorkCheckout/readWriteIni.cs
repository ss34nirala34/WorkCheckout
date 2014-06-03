using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Specialized;

namespace ReadWriteIni
{
    /**/
    /**/
    /**/
    /// <summary>
    /// IniFiles的类
    /// </summary>
    public class IniFiles
    {
        public string FileName; //INI文件名
        //声明读写INI文件的API函数
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary>
        /// 类的构造函数，传递INI文件名
        /// </summary>
        /// <param name="aFileName">INI文件路径 </param>
        public IniFiles(string aFileName)
        {
            // 判断文件是否存在
            FileInfo fileInfo = new FileInfo(aFileName);
            //Todo:
            if ((!fileInfo.Exists))
            { //|| (FileAttributes.Directory in fileInfo.Attributes))
                //文件不存在，建立文件
                System.IO.StreamWriter sw = new System.IO.StreamWriter(aFileName, false, System.Text.Encoding.Default);
                try
                {
                    sw.Write("#配置，请不要随意改动");
                    sw.Close();
                }
                catch
                {
                    throw (new ApplicationException("Ini文件不存在"));
                }
            }
            //必须是完全路径，不能是相对路径
            FileName = fileInfo.FullName;
        }
        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">项目名称(如 [TypeName] )</param>
        /// <param name="ident">键</param>
        /// <param name="value">值</param>
        public void WriteString(string section, string ident, string value)
        {
            if (!WritePrivateProfileString(section, ident, value, FileName))
            {

                throw (new ApplicationException("写Ini文件出错"));
            }
        }

        /// <summary>
        /// 写入INI文件
        /// </summary>
        /// <param name="section">项目名称(如 [TypeName] )</param>
        /// <param name="ident">键</param>
        /// <param name="value">值</param>
        /// <param name="encryencryption"> 是否启动加密写入</param>
        public void WriteString(string section, string ident, string value, bool encryencryption)
        {
            if (encryencryption)
            {

                if (!WritePrivateProfileString(section,ident, EncryptionHelper.EncryptString(value), FileName))
                {

                    throw (new ApplicationException("写Ini文件出错"));
                }
            }
            else
            {
                if (!WritePrivateProfileString(section, ident, value, FileName))
                {

                    throw (new ApplicationException("写Ini文件出错"));
                }
                
            }
            
        }
        //读取INI文件指定
       // public string ReadString(string Section, string Ident, string Default)
        //{
           // Byte[] Buffer = new Byte[65535];
           // int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), FileName);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文
           // string s = Encoding.GetEncoding(0).GetString(Buffer);
           // s = s.Substring(0, bufLen);
            //return s.Trim();
       // }
        //
        /// <summary>
        /// 读取INI字符串
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
        /// <param name="Default">Default</param>
        /// <param name="n">读取的字符串限制长度</param>
        /// <returns></returns>
        public string ReadString(string Section, string Ident, string Default,int n)
        {
            StringBuilder Buffer = new StringBuilder(n);
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, n, FileName);

            return Buffer.ToString();
        }

        /// <summary>
        /// 读取INI字符串
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
        /// <param name="Default">Default</param>
        /// <param name="n">读取的字符串限制长度</param>
        /// <param name="encryencryption">是否启动解密读取</param>
        /// <returns></returns>
        public string ReadString(string Section, string Ident, string Default, int n, bool encryencryption)
        {
            StringBuilder Buffer = new StringBuilder(n);
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, n, FileName);
            if (!encryencryption)
            {
                return Buffer.ToString();
            }

            return EncryptionHelper.DecryptString(Buffer.ToString());
        }
        /// <summary>
        /// 读取INI字符串，限制读取的长度最大值为10000(已重载)
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
        /// <param name="Default">Default</param>
        /// <returns></returns>
        public string ReadString(string Section, string Ident, string Default)
        {
            StringBuilder Buffer = new StringBuilder(10000);
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, 10000, FileName);
            return Buffer.ToString();
        }
        /// <summary>
        /// 读取INI字符串，限制读取的长度最大值为10000(已重载)
        /// </summary>
        /// <param name="Section">Section</param>
        /// <param name="Ident">Ident</param>
        /// <param name="Default">Default</param>
        /// <param name="n">读取的字符串限制长度</param>
        /// <param name="encryencryption">是否启动解密读取</param>
        /// <returns></returns>
        public string ReadString(string Section, string Ident, string Default, bool encryencryption)
        {
            StringBuilder Buffer = new StringBuilder(10000);
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, 10000, FileName);
            if (!encryencryption)
            {
                return Buffer.ToString();
            }

            return EncryptionHelper.DecryptString(Buffer.ToString());
        }
        //读整数
        public int ReadInteger(string Section, string Ident, int Default)
        {
            string intStr = ReadString(Section, Ident, Convert.ToString(Default));
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Default;
            }
        }

        //写整数
        public void WriteInteger(string Section, string Ident, int Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        //读布尔
        public bool ReadBool(string Section, string Ident, bool Default)
        {
            try
            {
                return Convert.ToBoolean(ReadString(Section, Ident, Convert.ToString(Default)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Default;
            }
        }

        //写Bool
        public void WriteBool(string Section, string Ident, bool Value)
        {
            WriteString(Section, Ident, Convert.ToString(Value));
        }

        //从Ini文件中，将指定的Section名称中的所有Ident添加到列表中
        public void ReadSection(string Section, StringCollection Idents)
        {
            Byte[] Buffer = new Byte[16384];
            //Idents.Clear();

            int bufLen = GetPrivateProfileString(Section, null, null, Buffer, Buffer.GetUpperBound(0),
              FileName);
            //对Section进行解析
            GetStringsFromBuffer(Buffer, bufLen, Idents);
        }

        private void GetStringsFromBuffer(Byte[] Buffer, int bufLen, StringCollection Strings)
        {
            Strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((Buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }
        //从Ini文件中，读取所有的Sections的名称
        public void ReadSections(StringCollection SectionList)
        {
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个Section
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer,
              Buffer.GetUpperBound(0), FileName);
            GetStringsFromBuffer(Buffer, bufLen, SectionList);
        }
        //读取指定的Section的所有Value到列表中
        public void ReadSectionValues(string Section, NameValueCollection Values)
        {
            StringCollection KeyList = new StringCollection();
            ReadSection(Section, KeyList);
            Values.Clear();
            foreach (string key in KeyList)
            {
                Values.Add(key, ReadString(Section, key, ""));

            }
        }
        /**/
        ////读取指定的Section的所有Value到列表中，
        //public void ReadSectionValues(string Section, NameValueCollection Values,char splitString)
        //{   string sectionValue;
        //    string[] sectionValueSplit;
        //    StringCollection KeyList = new StringCollection();
        //    ReadSection(Section, KeyList);
        //    Values.Clear();
        //    foreach (string key in KeyList)
        //    {  
        //        sectionValue=ReadString(Section, key, "");
        //        sectionValueSplit=sectionValue.Split(splitString);
        //        Values.Add(key, sectionValueSplit[0].ToString(),sectionValueSplit[1].ToString());

        //    }
        //}
        //清除某个Section
        public void EraseSection(string Section)
        {
            //
            if (!WritePrivateProfileString(Section, null, null, FileName))
            {
                throw (new ApplicationException("无法清除Ini文件中的Section"));
            }
        }
        //删除某个Section下的键
        public void DeleteKey(string Section, string Ident)
        {
            WritePrivateProfileString(Section, Ident, null, FileName);
        }
        //Note:对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件
        //在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile
        //执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。
        public void UpdateFile()
        {
            WritePrivateProfileString(null, null, null, FileName);
        }

        //检查某个Section下的某个键值是否存在
        public bool ValueExists(string Section, string Ident)
        {
            //
            StringCollection Idents = new StringCollection();
            ReadSection(Section, Idents);
            return Idents.IndexOf(Ident) > -1;
        }
        //确保资源的释放
        ~IniFiles()
        {
            UpdateFile();
        }
    }
    class EncryptionHelper
    {
        #region 成员变量

        private static readonly byte[] ArrKey = { 0x63, 0x28, 0x65, 0xAE, 0x45, 0x55, 0x24, 0x6E };

        private static readonly byte[] ArrIv = { 0xFE, 0xDC, 0x3A, 0x87, 0x16, 0x54, 0x32, 0x2F };

        #endregion

        #region 字符串解密
        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="sString">需要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        public static string DecryptString(string sString)
        {
            if (sString == null || string.Empty.Equals(sString))
            {
                return string.Empty;
            }
            byte[] arrByteArray = new byte[sString.Length];
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            arrByteArray = Convert.FromBase64String(sString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(ArrKey, ArrIv), CryptoStreamMode.Write);
            cs.Write(arrByteArray, 0, arrByteArray.Length);
            cs.FlushFinalBlock();
            cs.Close();
            ms.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }
        #endregion

        #region 字符串加密
        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="sString">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string EncryptString(string sString)
        {
            if (sString.Equals(string.Empty))
            {
                return sString;
            }
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputbyteArray = Encoding.UTF8.GetBytes(sString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(ArrKey, ArrIv), CryptoStreamMode.Write);
            cs.Write(inputbyteArray, 0, inputbyteArray.Length);
            cs.FlushFinalBlock();
            cs.Close();
            ms.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion
    }
}