using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Threading.Tasks;

namespace Qj.Utility
{
    public class BaseEncrypt
    {
        private const string EncryptKey = "keydnrelhvfbkjejjhufa1";

        //private const string EncryptKey = "Web";

        #region MD5 - 32

        /// <summary>
        /// 加密
        /// </summary>
        public static string MD5Encrypt(string input)
        {
            return MD5EncryptX(input, EncryptKey);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string MD5EncryptX(string input, string sKey)
        {
            string strData = input + sKey;
            var md5Provider = new MD5CryptoServiceProvider();
            byte[] inBytes = Encoding.Default.GetBytes(strData);

            byte[] outBytes = md5Provider.ComputeHash(inBytes);
            StringBuilder outString = new StringBuilder();
            foreach (var data in outBytes)
            {
                outString.AppendFormat("{0:X2}", data);
            }
            return outString.ToString();
        }

        public static string MD5EncryptY(string input, string sKey)
        {
            string strData = input + sKey;

            MD5 md5Provider = MD5.Create();
            byte[] data = md5Provider.ComputeHash(Encoding.UTF8.GetBytes(strData));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        #endregion MD5 - 32

        #region DES

        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private const string desKey = "viewblob";   //DES秘钥

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString">源字符串</param>
        /// <param name="encryptKey">加密秘钥，要求8位</param>
        /// <returns>加密成功，返回加密字符串，失败返回源串</returns>
        public static string DESEncrypt(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;

                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);

                var dcsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dcsp.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return System.Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString">源字符串</param>
        /// <returns>加密成功，返回加密字符串，失败返回源串</returns>
        public static string DESEncrypt(string encryptString)
        {
            return DESEncrypt(encryptString, desKey);
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">加密字符串</param>
        /// <param name="decryptKey">解密秘钥，要求8位和加密秘钥相同</param>
        /// <returns>解密成功后返回解密字符串，失败返回源串</returns>
        public static string DESDecrypt(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = System.Convert.FromBase64String(decryptString);

                var dcsp = new DESCryptoServiceProvider();
                var mStream = new MemoryStream();
                var cStream = new CryptoStream(mStream, dcsp.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">加密字符串</param>
        /// <returns>解密成功后返回解密字符串，失败返回源串</returns>
        public static string DESDecrypt(string decryptString)
        {
            return DESDecrypt(decryptString, desKey);
        }

        #endregion DES
    }
}