using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Qj.Utility
{
    public class AESEncrypt
    {
        #region BASE64 加密解密

        /// <summary>
        /// BASE64 加密
        /// </summary>
        /// <param name="value">待加密字段</param>
        /// <returns></returns>
        public string Base64(string value)
        {
            var btArray = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(btArray, 0, btArray.Length);
        }

        /// <summary>
        /// BASE64 解密
        /// </summary>
        /// <param name="value">待解密字段</param>
        /// <returns></returns>
        public string UnBase64(string value)
        {
            var btArray = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(btArray);
        }

        #endregion BASE64 加密解密

        #region AES 加密解密

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="value">待加密字段</param>
        /// <param name="keyVal">密钥值</param>
        /// <param name="ivVal">加密辅助向量</param>
        /// <returns></returns>
        public string Encrypt(string value, string keyVal, string ivVal)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("未将对象引用设置到对象的实例。");
            }

            var encoding = Encoding.UTF8;

            byte[] btKey = encoding.GetBytes(Base64(keyVal).Substring(0, 16).ToUpper());
            byte[] btIv = encoding.GetBytes(Base64(ivVal).Substring(0, 16).ToUpper());
            byte[] byteArray = encoding.GetBytes(value);
            string encrypt;
            var aes = Rijndael.Create();
            using (var mStream = new MemoryStream())
            {
                using (var cStream = new CryptoStream(mStream, aes.CreateEncryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }
            aes.Clear();
            return encrypt;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="value">待加密字段</param>
        /// <param name="keyVal">密钥值</param>
        /// <param name="ivVal">加密辅助向量</param>
        /// <returns></returns>
        public string Decrypt(string value, string keyVal, string ivVal)
        {
            if (string.IsNullOrEmpty(value)) throw new ArgumentNullException("未将对象引用设置到对象的实例。");

            var encoding = Encoding.UTF8;

            byte[] btKey = encoding.GetBytes(Base64(keyVal).Substring(0, 16).ToUpper());
            byte[] btIv = encoding.GetBytes(Base64(ivVal).Substring(0, 16).ToUpper());
            byte[] byteArray = Convert.FromBase64String(value);
            string decrypt;
            var aes = Rijndael.Create();
            using (var mStream = new MemoryStream())
            {
                using (var cStream = new CryptoStream(mStream, aes.CreateDecryptor(btKey, btIv), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = encoding.GetString(mStream.ToArray());
                }
            }
            aes.Clear();
            return decrypt;
        }

        #endregion AES 加密解密
    }
}