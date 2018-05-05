using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Com.Zengfr.Proj.Common
{
    public class DESCryptoServiceProviderUtils
    {
        static string Key = "szds@1r";
        public static string Encrypt(string stringToDecrypt)
        {
            return Encrypt(stringToDecrypt, Key);
        }
        public static string Decrypt(string stringToDecrypt)
        {
            return Decrypt(stringToDecrypt, Key);
        }
        static string initEncryptString(string stringToEncrypt)
        {
            if (string.IsNullOrWhiteSpace(stringToEncrypt)) stringToEncrypt = string.Empty;
            if (!stringToEncrypt.StartsWith("$"))
            {
                stringToEncrypt = string.Format("${0}$", stringToEncrypt);
            }
            return stringToEncrypt;
        }
        public static string Encrypt(string stringToEncrypt, string sKey)
        {
            stringToEncrypt = initEncryptString(stringToEncrypt);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
            des.Key = ASCIIEncoding.UTF8.GetBytes(string.Format("K{0}", sKey));
            des.IV = ASCIIEncoding.UTF8.GetBytes(string.Format("I{0}", sKey));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            var result = ret.ToString(); ret.Clear(); ret = null;
            return result;
        }

        public static string Decrypt(string stringToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[stringToDecrypt.Length / 2];
            for (int x = 0; x < stringToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(stringToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = Encoding.UTF8.GetBytes(string.Format("K{0}", sKey));
            des.IV = Encoding.UTF8.GetBytes(string.Format("I{0}", sKey));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //StringBuilder ret = new StringBuilder();
            var result = System.Text.Encoding.UTF8.GetString(ms.ToArray());
            return result.Trim('$');
        }
    }
}
