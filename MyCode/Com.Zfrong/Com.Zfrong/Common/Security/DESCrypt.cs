using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace Com.Zfrong.Common.Security
{
    /// <summary>
    /// 加密解密类
    /// </summary>
    public class DESCrypt
    {
        /// <summary>
        /// 预定密钥
        /// </summary>
        private const string sKey = "zfrong..";
        /// <summary>
        /// 默认构造
        /// </summary>
        public DESCrypt()
        {
        }

        #region 公共方法
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="strText">要加密的文本</param>
        /// <returns>返回加密后的串</returns>
        public static string EncryptText(string strText)
        {
            return EncryptText(strText, sKey);
        }
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="strText">要加密的文本</param>
        /// <param name="strKey">长度为8的种子KEY</param>
        /// <returns></returns>
        public static string EncryptText(string strText, string strKey)
        {
            if (strText != null && strKey != null)
            {
                if (strText.Trim() != "" && strKey.Trim().Length == 8)
                    return Encrypt(strText, strKey);
            }
            return "";
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="strText">要解密的文本</param>
        /// <returns>返回解密后的值</returns>
        public static string DecryptText(string strText)
        {
            return DecryptText(strText, sKey);
        }
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="strText">要解密的文本</param>
        /// <param name="strKey">长度为8的KEY</param>
        /// <returns>返回解密后的值</returns>
        public static string DecryptText(string strText, string strKey)
        {
            if (strText != null && strKey != null)
            {
                if (strText.Trim() != "" && strKey.Trim().Length == 8)
                    return Decrypt(strText, strKey);
            }
            return "";
        }
        #endregion

        #region 保护方法
        /// <summary>
        /// 数据加密函数
        /// Function Name:	Encrypt
        /// Parameters	:   string类型		Return:	string类型
        /// </summary>
        /// <param name="pToEncrypt">待加密明文</param>
        /// <param name="m_strKey">密钥</param>
        /// <returns>string类型</returns>
        private static string Encrypt(string pToEncrypt, String m_strKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //把字符串放到byte数组中  
            //原来使用的UTF8编码，我改成Unicode编码了，不行  
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[]  inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);  

            //建立加密对象的密钥和偏移量  
            //原文使用ASCIIEncoding.ASCII方法的GetBytes方法  
            //使得输入密码必须输入英文文本  
            des.Key = ASCIIEncoding.ASCII.GetBytes(m_strKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(m_strKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            //Write  the  byte  array  into  the  crypto  stream  
            //(It  will  end  up  in  the  memory  stream)  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            //Get  the  data  back  from  the  memory  stream,  and  into  a  string  
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                //Format  as  hex  
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// 数据解密函数
        /// Function Name:	Decrypt
        /// Parameters	:   string类型		Return:	string类型
        /// </summary>
        /// <param name="pToDecrypt">待解密文本</param>
        /// <param name="m_strKey">密钥</param>
        /// <returns>string类型</returns>
        private static string Decrypt(string pToDecrypt, String m_strKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            //Put  the  input  string  into  the  byte  array  
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改  
            des.Key = ASCIIEncoding.ASCII.GetBytes(m_strKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(m_strKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //Get  the  decrypted  data  back  from  the  memory  stream  
            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象  
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion
    }
}