using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Security.Cryptography;
namespace Zfrong.Framework.Utils
{
    /// <summary>
    /// 数据加密类
    /// </summary>
    public class MyEncryptionHelper
    {
        public static string EncryptoData(string key, string data)
        {
            MySymmetricMethod sm = new MySymmetricMethod(MyEncryptionHelper.GenerateDataKey(key, "Q", "Q"));
            string rtn = sm.Encrypto(data); sm = null;
            return rtn;
        }
        public static string DecryptoData(string key, string data)
        {
            MySymmetricMethod sm = new MySymmetricMethod(MyEncryptionHelper.GenerateDataKey(key, "Q", "Q"));
            string rtn = sm.Decrypto(data); sm = null;
            return rtn;
        }

        public static string MD5(string strPwd)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(strPwd);//将字符编码为一个字节序列 
            byte[] md5data = md5.ComputeHash(data);//计算data字节数组的哈希值 
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length - 1; i++)
            {
                str += md5data[i].ToString("x").PadLeft(2, '0');
            }
            md5 = null; data = null; md5data = null;
            return str;
        }

        public static string GenerateDataKey(string name, string pwd, string company)
        {
            return GenerateDataKey(string.Format("{0}_{1}_{2}",name, pwd, company));
        }
        private static string GenerateDataKey(string data)
        {
            System.Security.Cryptography.HMACSHA1 sha = new System.Security.Cryptography.HMACSHA1();
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(data);
            MemoryStream ms = new MemoryStream();
            sha.Key = UTF8Encoding.UTF8.GetBytes("SOAKeyGenerate2012-08-02");
            byte[] bytOut = sha.ComputeHash(bytIn);
            string result = Convert.ToBase64String(bytOut);
            bytIn = null; bytOut = null; ms.Close(); ms = null;
            return result;
        }
    }
}
