using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EncryptionHelper

{
    public static class CryptHelper
    {
        #region 成员变量

        private static byte[] arrKey = { 0x62, 0x28, 0x45, 0xAE, 0x49, 0x75, 0x24, 0x6E };

        private static byte[] arrIV = { 0xFE, 0xDC, 0x3A, 0x88, 0x16, 0x54, 0x32, 0x1F };

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
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(arrKey, arrIV), CryptoStreamMode.Write);
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
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(arrKey, arrIV), CryptoStreamMode.Write);
            cs.Write(inputbyteArray, 0, inputbyteArray.Length);
            cs.FlushFinalBlock();
            cs.Close();
            ms.Close();
            return Convert.ToBase64String(ms.ToArray());
        }
        #endregion

        #region 对字符串进行MD5加密
        /// <summary>
        /// 对字符串进行MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <example></example>
        public static string MD5(string str)
        {
            byte[] b = Encoding.ASCII.GetBytes(str);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            b = md5.ComputeHash(b);
            //释放md5的资源
            md5.Clear();

            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X2"));
            }

            return sb.ToString();
        }
        #endregion

        #region 对字节数组进行MD5加密
        /// <summary>
        /// 对字节数组进行MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <example></example>
        public static string MD5(byte[] bytes)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] b = md5.ComputeHash(bytes);
            //释放md5的资源
            md5.Clear();

            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X2"));
            }

            return sb.ToString();
        }
        #endregion

        #region 对字节数流进行MD5加密
        /// <summary>
        /// 对字节数组进行MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <example></example>
        public static string MD5(Stream stream)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.Initialize();
            md5.ComputeHash(stream);

            byte[] b = md5.Hash;
            md5.Clear();

            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X2"));
            }

            return sb.ToString();
        }
        #endregion

        #region 对大文件进行MD5加密
        /// <summary>
        /// 对大文件进行MD5加密
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <example></example>
        public static string MD5File(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.Initialize();
            md5.ComputeHash(fs);
            fs.Close();

            byte[] b = md5.Hash;
            md5.Clear();

            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X2"));
            }
            return sb.ToString();
        }
        #endregion

        #region 使用密码加密
        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，长度至少为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            if (sKey == null || sKey.Length < 8)
            {
                throw new ArgumentException("sKey");
            }
            else if (sKey.Length > 8)
            {
                sKey = sKey.Substring(sKey.Length - 8);
            }

            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = des.Key;
                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        #endregion

        #region 使用密码解密
        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，长度至少为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            if (sKey == null || sKey.Length < 8)
            {
                throw new ArgumentException("sKey");
            }
            else if (sKey.Length > 8)
            {
                sKey = sKey.Substring(sKey.Length - 8);
            }

            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = des.Key;
                MemoryStream ms = new MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        #endregion

        #region 获取广众通mac码的hash值
        /// <summary>
        /// 获取广众通mac码的hash值
        /// </summary>
        /// <param name="strMac"></param>
        /// <returns></returns>
        public static UInt32 HashGZTMac(string strMac)
        {
            if (string.IsNullOrEmpty(strMac))
            {
                return 0;
            }
            UInt32 h = 0;
            foreach (var ch in strMac)
            {
                h *= 21257619;
                h ^= Convert.ToUInt16(ch);
            }
            return h;
        }
        #endregion
    }

    #region RSACryptoHelper
    /// <summary>
    /// RSA算法辅助类
    /// </summary>
    public sealed class RSACryptoHelper
    {
        #region 成员变量
        /// <summary>
        /// RSA密钥根路径
        /// </summary>
        private static readonly string RsaKeyPath = AppDomain.CurrentDomain.BaseDirectory;
        /// <summary>
        /// 
        /// </summary>
        private static RSACryptoServiceProvider _encryptRSA = null;
        /// <summary>
        /// 
        /// </summary>
        private static RSACryptoServiceProvider _decryptRSA = null;

        /// <summary>
        /// 用于加密的RSA
        /// </summary>
        private static RSACryptoServiceProvider EncryptRSA
        {
            get
            {
                if (_encryptRSA == null)
                {
                    _encryptRSA = new RSACryptoServiceProvider();
                    string pubKey = File.ReadAllText(RsaKeyPath + "cnitpub.xml");
                    _encryptRSA.FromXmlString(pubKey);
                }
                return _encryptRSA;
            }
        }

        /// <summary>
        /// 用于解密的RSA
        /// </summary>
        private static RSACryptoServiceProvider DecryptRSA
        {
            get
            {
                if (_decryptRSA == null)
                {
                    _decryptRSA = new RSACryptoServiceProvider();
                    string priKey = File.ReadAllText(RsaKeyPath + "cnitpri.xml");
                    _decryptRSA.FromXmlString(priKey);
                }
                return _decryptRSA;
            }
        }
        #endregion

        /// <summary>
        /// 生成Key
        /// </summary>
        public static void CreateKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            File.WriteAllText(RsaKeyPath + "cnitpri.xml", rsa.ToXmlString(true));
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "cnitpub.xml", rsa.ToXmlString(false));
        }

        #region 加密
        /// <summary>
        /// 加密
        /// </summary>
        public static byte[] Encrypt(byte[] bytes)
        {
            return EncryptRSA.Encrypt(bytes, false);
        }

        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string EncryptToBase64(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(Encrypt(bytes));
        }
        #endregion

        #region 解密
        /// <summary>
        /// 解密
        /// </summary>
        public static byte[] Decrypt(byte[] bytes)
        {
            return DecryptRSA.Decrypt(bytes, false);
        }

        /// <summary>
        /// 解密Base64String
        /// </summary>
        public static string DecryptBase64(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            byte[] data = Decrypt(bytes);
            return Convert.ToBase64String(data);
        }
        #endregion

        #region 签名
        /// <summary>
        /// 签名
        /// </summary>
        public static byte[] SignData(byte[] bytes)
        {
            return EncryptRSA.SignData(bytes, new SHA1CryptoServiceProvider());
        }

        /// <summary>
        /// 签名Base64String字符串
        /// </summary>
        public static string SignDataBase64(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            byte[] data = SignData(bytes);
            return Convert.ToBase64String(data);
        }
        #endregion
    }
    #endregion

    #region AESCryptoHelper
    /// <summary>
    /// AES加密辅助类
    /// </summary>
    public class AESCryptoHelper
    {
        #region 成员变量
        /// <summary>
        /// 缓冲长度
        /// </summary>
        private static int _buffSize = 4096;
        /// <summary>
        /// 运算模式
        /// </summary>
        private static CipherMode _cipherMode = CipherMode.ECB;
        /// <summary>
        /// 填充模式(不足128bit补0)
        /// </summary>
        private static PaddingMode _paddingMode = PaddingMode.Zeros;
        /// <summary>
        /// 字符串采用的编码
        /// </summary>
        private static Encoding _encoding = Encoding.UTF8;
        #endregion

        #region 获取32byte密钥数据
        /// <summary>
        /// 获取32byte密钥数据
        /// </summary>
        /// <param name="password">密码</param>
        /// <returns></returns>
        private static byte[] GetKeyArray(string password)
        {
            if (password == null)
            {
                // throw new ArgumentNullException("AES密钥不能为空！");
                password = string.Empty;
            }

            if (password.Length < 32)
            {
                password = password.PadLeft(32, '0');
            }
            else if (password.Length > 32)
            {
                password = password.Substring(0, 32);
            }

            return _encoding.GetBytes(password);
        }
        #endregion

        #region 生成key
        /// <summary>
        /// 生成key
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string CreateKey(string password)
        {
            byte[] b = _encoding.GetBytes(password);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            b = md5.ComputeHash(b);
            //释放md5的资源
            md5.Clear();

            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X2"));
            }

            //byte[] keyData = UTF8_encoding.GetBytes(sb.ToString());
            //string base64Key = Convert.ToBase64String(keyData);
            //return base64Key;

            return sb.ToString();
        }
        #endregion

        #region 加密
        /// <summary>
        /// 加密字节数据
        /// </summary>
        /// <param name="inputData">要加密的字节数据</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] inputData, string password)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = GetKeyArray(password);
            aes.Mode = _cipherMode;
            aes.Padding = _paddingMode;
            ICryptoTransform transform = aes.CreateEncryptor();
            byte[] data = transform.TransformFinalBlock(inputData, 0, inputData.Length);
            aes.Clear();
            return data;

            //MemoryStream ms = new MemoryStream();
            //CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            //cs.Write(input, 0, input.Length);
            //cs.Close();
            //byte[] data = ms.ToArray();
            //ms.Close();

            //return data;
        }

        /// <summary>
        /// 加密字符串(加密为base64)
        /// </summary>
        /// <param name="inputString">要加密的字符串</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string Encrypt(string inputString, string password)
        {
            byte[] toEncryptArray = _encoding.GetBytes(inputString);
            return Convert.ToBase64String(Encrypt(toEncryptArray, password));
        }

        /// <summary>
        /// 加密文件(二进制)
        /// </summary>
        /// <param name="inputFile">要加密的文件名</param>
        /// <param name="outputFile">加密后的文件名</param>
        /// <param name="password">密码</param>
        public static void Encrypt(string inputFile, string outputFile, string password)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = GetKeyArray(password);
            aes.Mode = _cipherMode;
            aes.Padding = _paddingMode;

            FileStream fsIn = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
            CryptoStream cs = new CryptoStream(fsOut, aes.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] buffer = new byte[_buffSize];
            int bytesRead;

            do
            {
                bytesRead = fsIn.Read(buffer, 0, _buffSize);
                cs.Write(buffer, 0, bytesRead);
            }
            while (bytesRead > 0);

            cs.Close();
            fsIn.Close();
            fsOut.Close();
        }

        /// <summary>
        /// 加密文件(base64格式)
        /// </summary>
        /// <param name="inputFile">要加密的文件名</param>
        /// <param name="outputFile">加密后的文件名</param>
        /// <param name="password">密码</param>
        public static void EncryptBase64(string inputFile, string outputFile, string password)
        {
            FileStream fs = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            string result = Convert.ToBase64String(Encrypt(data, password));

            File.WriteAllText(outputFile, result);
        }

        /// <summary>
        /// 加密字符串并保存到文件中(二进制)
        /// </summary>
        /// <param name="inputString">要加密的字符串</param>
        /// <param name="outputFile">输出文件名</param>
        /// <param name="password">密码</param>
        public static void EncrytToFile(string inputString, string outputFile, string password)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = GetKeyArray(password);
            aes.Mode = _cipherMode;
            aes.Padding = _paddingMode;

            FileStream fsOut = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
            CryptoStream cs = new CryptoStream(fsOut, aes.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] data = _encoding.GetBytes(inputString);
            cs.Write(data, 0, data.Length);
            cs.Close();
            fsOut.Close();
        }

        /// <summary>
        /// 加密字符串并保存到文件中(base64格式)
        /// </summary>
        /// <param name="inputString">要加密的字符串</param>
        /// <param name="outputFile">输出文件名</param>
        /// <param name="password">密码</param>
        public static void EncrytToFileBase64(string inputString, string outputFile, string password)
        {
            byte[] data = _encoding.GetBytes(inputString);
            string result = Convert.ToBase64String(Encrypt(data, password));
            File.WriteAllText(outputFile, result);
        }
        #endregion

        #region 解密
        /// <summary>
        /// 解密字节数组
        /// </summary>
        /// <param name="inputData">要解密的字节数据</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] inputData, string password)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = GetKeyArray(password);
            aes.Mode = _cipherMode;
            aes.Padding = _paddingMode;
            ICryptoTransform transform = aes.CreateDecryptor();
            byte[] data = transform.TransformFinalBlock(inputData, 0, inputData.Length);
            aes.Clear();
            return data;



        }

        /// <summary>
        /// 解密base64编码的字符串为字符串
        /// </summary>
        /// <param name="inputString">要解密的字符串</param>
        /// <param name="password">密码</param>
        /// <returns>字符串</returns>
        public static string Decrypt(string inputString, string password)
        {
            byte[] toDecryptArray = Convert.FromBase64String(inputString);
            string decryptString = _encoding.GetString(Decrypt(toDecryptArray, password));
            return decryptString.TrimEnd('\0');
        }

        /// <summary>
        /// 解密文件(将AES加密的二进制数据解密成二进制)
        /// </summary>
        /// <param name="inputFile">要解密的文件名</param>
        /// <param name="outputFile">解密后的文件名</param>
        /// <param name="password">密码</param>
        public static void Decrypt(string inputFile, string outputFile, string password)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = GetKeyArray(password);
            aes.Mode = _cipherMode;
            aes.Padding = _paddingMode;

            FileStream fsIn = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
            CryptoStream cs = new CryptoStream(fsOut, aes.CreateDecryptor(), CryptoStreamMode.Write);

            byte[] buffer = new byte[_buffSize];
            int bytesRead;

            do
            {
                bytesRead = fsIn.Read(buffer, 0, _buffSize);
                cs.Write(buffer, 0, bytesRead);
            }
            while (bytesRead > 0);

            cs.Close();
            fsIn.Close();
            fsOut.Close();
        }

        /// <summary>
        /// 解密文件(将base64编码的数据解密成文本文件)
        /// </summary>
        /// <param name="inputFile">要解密的文件名</param>
        /// <param name="outputFile">解密后的文件名</param>
        /// <param name="password">密码</param>
        public static void DecryptBase64(string inputFile, string outputFile, string password)
        {
            string inputString = File.ReadAllText(inputFile);
            byte[] data = Convert.FromBase64String(inputString);
            data = Decrypt(data, password);
            //string outputString = _encoding.GetString(data);
            //File.WriteAllText(outputFile, outputString.TrimEnd('\0'));

            FileStream fs = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(data, 0, data.Length);
            if (_paddingMode == PaddingMode.Zeros) // 去掉后面为0的字节(AES加密有可能会有后面补0)
            {
                int validLen = data.Length;
                while (validLen > 0 && data[validLen - 1] == 0)
                {
                    --validLen;
                }
                fs.SetLength(validLen);
            }
            fs.Close();
        }
        #endregion
    }
    #endregion

    #region Xxtea
    /// <summary>
    /// Autor:Tecky
    /// Date:2008-06-03
    /// Desc:xxtea 算法的C#加密实现
    /// e-mail:likui318@163.com
    /// </summary>
    public static class Xxtea
    {
        public static string Encrypt(string source, string key)
        {
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            //UTF8==>BASE64==>XXTEA==>BASE64
            byte[] bytData = encoder.GetBytes(base64Encode(source));
            byte[] bytKey = encoder.GetBytes(key);
            if (bytData.Length == 0)
            {
                return "";
            }
            return System.Convert.ToBase64String(ToByteArray(Encrypt(ToUInt32Array(bytData, true), ToUInt32Array(bytKey, false)), false));
        }
        public static string Decrypt(string source, string key)
        {
            if (source.Length == 0)
            {
                return "";
            }
            // reverse
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            byte[] bytData = System.Convert.FromBase64String(source);
            byte[] bytKey = encoder.GetBytes(key);

            return base64Decode(encoder.GetString(ToByteArray(Decrypt(ToUInt32Array(bytData, false), ToUInt32Array(bytKey, false)), true)));
        }

        private static UInt32[] Encrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum = 0, e;
            Int32 p, q = 6 + 52 / (n + 1);
            while (q-- > 0)
            {
                sum = unchecked(sum + delta);
                e = sum >> 2 & 3;
                for (p = 0; p < n; p++)
                {
                    y = v[p + 1];
                    z = unchecked(v[p] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                y = v[0];
                z = unchecked(v[n] += (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
            }
            return v;
        }

        private static UInt32[] Decrypt(UInt32[] v, UInt32[] k)
        {
            Int32 n = v.Length - 1;
            if (n < 1)
            {
                return v;
            }
            if (k.Length < 4)
            {
                UInt32[] Key = new UInt32[4];
                k.CopyTo(Key, 0);
                k = Key;
            }
            UInt32 z = v[n], y = v[0], delta = 0x9E3779B9, sum, e;
            Int32 p, q = 6 + 52 / (n + 1);
            sum = unchecked((UInt32)(q * delta));
            while (sum != 0)
            {
                e = sum >> 2 & 3;
                for (p = n; p > 0; p--)
                {
                    z = v[p - 1];
                    y = unchecked(v[p] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                }
                z = v[n];
                y = unchecked(v[0] -= (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[p & 3 ^ e] ^ z));
                sum = unchecked(sum - delta);
            }
            return v;
        }

        private static UInt32[] ToUInt32Array(Byte[] Data, Boolean IncludeLength)
        {
            Int32 n = (((Data.Length & 3) == 0) ? (Data.Length >> 2) : ((Data.Length >> 2) + 1));
            UInt32[] Result;
            if (IncludeLength)
            {
                Result = new UInt32[n + 1];
                Result[n] = (UInt32)Data.Length;
            }
            else
            {
                Result = new UInt32[n];
            }
            n = Data.Length;
            for (Int32 i = 0; i < n; i++)
            {
                Result[i >> 2] |= (UInt32)Data[i] << ((i & 3) << 3);
            }
            return Result;
        }

        private static Byte[] ToByteArray(UInt32[] Data, Boolean IncludeLength)
        {
            Int32 n;
            if (IncludeLength)
            {
                n = (Int32)Data[Data.Length - 1];
            }
            else
            {
                n = Data.Length << 2;
            }
            Byte[] Result = new Byte[n];
            for (Int32 i = 0; i < n; i++)
            {
                Result[i] = (Byte)(Data[i >> 2] >> ((i & 3) << 3));
            }
            return Result;
        }

        public static string base64Decode(string data)
        {
            try
            {
                //System.Text.UTF8Encoding encoder = System.Text.Encoding.UTF8;

                byte[] todecode_byte = Convert.FromBase64String(data);
                return System.Text.Encoding.UTF8.GetString(todecode_byte);
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Decode" + e.Message);
            }
        }

        public static string base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64Encode" + e.Message);
            }
        }
    }
    #endregion


}
