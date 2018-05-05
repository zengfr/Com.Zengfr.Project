using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Zfrong.Framework.Utils
{
     /// <summary>   
     /// 对称加密算法类   
     /// </summary>   
    public class MySymmetricMethod
    {
        private SymmetricAlgorithm mobjCryptoService;
        private string key;
        /// <summary>   
        /// 对称加密类的构造函数   
        /// </summary>   
        public MySymmetricMethod(string key)
        {
            mobjCryptoService = new RijndaelManaged();
            this.key = key;
        }
        /// <summary>   
        /// 获得密钥   
        /// </summary>   
        /// <returns>密钥</returns>   
        private byte[] GetLegalKey()
        {
            string sTemp = key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (sTemp.Length > KeyLength)
            {
                sTemp = sTemp.Substring(0, KeyLength);
            }
            else if (sTemp.Length < KeyLength)
            {
                sTemp = sTemp.PadRight(KeyLength, ' ');
            }
            bytTemp = null;
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>   
        /// 获得初始向量IV   
        /// </summary>   
        /// <returns>初试向量IV</returns>   
        private byte[] GetLegalIV()
        {
            string sTemp = "E4ghj*Ghg7!rNIfb&95GUY86GfghUb#er57HBh(u%g6HJ($jhWk7&!hg4ui%$hjk";
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (sTemp.Length > IVLength)
            {
                sTemp = sTemp.Substring(0, IVLength);
            }
            else if (sTemp.Length < IVLength)
            {
                sTemp = sTemp.PadRight(IVLength, ' ');
            }
            bytTemp = null;
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }
        /// <summary>   
        /// 加密方法   
        /// </summary>   
        /// <param name="Source">待加密的串</param>   
        /// <returns>经过加密的串</returns>   
        public string Encrypto(string Source)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(Source);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            cs.Close();
            byte[] bytOut = ms.ToArray(); cs = null; ms = null; encrypto = null; bytIn = null;
            return Convert.ToBase64String(bytOut);
        }
        /// <summary>   
        /// 解密方法   
        /// </summary>   
        /// <param name="Source">待解密的串</param>   
        /// <returns>经过解密的串</returns>   
        public string Decrypto(string Source)
        {
            byte[] bytIn = Convert.FromBase64String(Source);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            string strRet = sr.ReadToEnd();
            cs.Close();
            sr.Close();
            cs = null; sr = null; ms = null; encrypto = null; bytIn = null;
            return strRet;
        }
    }
}  


