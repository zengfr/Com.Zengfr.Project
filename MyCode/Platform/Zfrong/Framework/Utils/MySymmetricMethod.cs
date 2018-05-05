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
     /// �ԳƼ����㷨��   
     /// </summary>   
    public class MySymmetricMethod
    {
        private SymmetricAlgorithm mobjCryptoService;
        private string key;
        /// <summary>   
        /// �ԳƼ�����Ĺ��캯��   
        /// </summary>   
        public MySymmetricMethod(string key)
        {
            mobjCryptoService = new RijndaelManaged();
            this.key = key;
        }
        /// <summary>   
        /// �����Կ   
        /// </summary>   
        /// <returns>��Կ</returns>   
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
        /// ��ó�ʼ����IV   
        /// </summary>   
        /// <returns>��������IV</returns>   
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
        /// ���ܷ���   
        /// </summary>   
        /// <param name="Source">�����ܵĴ�</param>   
        /// <returns>�������ܵĴ�</returns>   
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
        /// ���ܷ���   
        /// </summary>   
        /// <param name="Source">�����ܵĴ�</param>   
        /// <returns>�������ܵĴ�</returns>   
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


