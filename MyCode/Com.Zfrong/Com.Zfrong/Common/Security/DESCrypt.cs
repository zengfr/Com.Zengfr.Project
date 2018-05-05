using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
namespace Com.Zfrong.Common.Security
{
    /// <summary>
    /// ���ܽ�����
    /// </summary>
    public class DESCrypt
    {
        /// <summary>
        /// Ԥ����Կ
        /// </summary>
        private const string sKey = "zfrong..";
        /// <summary>
        /// Ĭ�Ϲ���
        /// </summary>
        public DESCrypt()
        {
        }

        #region ��������
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="strText">Ҫ���ܵ��ı�</param>
        /// <returns>���ؼ��ܺ�Ĵ�</returns>
        public static string EncryptText(string strText)
        {
            return EncryptText(strText, sKey);
        }
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="strText">Ҫ���ܵ��ı�</param>
        /// <param name="strKey">����Ϊ8������KEY</param>
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
        /// �����ַ���
        /// </summary>
        /// <param name="strText">Ҫ���ܵ��ı�</param>
        /// <returns>���ؽ��ܺ��ֵ</returns>
        public static string DecryptText(string strText)
        {
            return DecryptText(strText, sKey);
        }
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="strText">Ҫ���ܵ��ı�</param>
        /// <param name="strKey">����Ϊ8��KEY</param>
        /// <returns>���ؽ��ܺ��ֵ</returns>
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

        #region ��������
        /// <summary>
        /// ���ݼ��ܺ���
        /// Function Name:	Encrypt
        /// Parameters	:   string����		Return:	string����
        /// </summary>
        /// <param name="pToEncrypt">����������</param>
        /// <param name="m_strKey">��Կ</param>
        /// <returns>string����</returns>
        private static string Encrypt(string pToEncrypt, String m_strKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //���ַ����ŵ�byte������  
            //ԭ��ʹ�õ�UTF8���룬�Ҹĳ�Unicode�����ˣ�����  
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            //byte[]  inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);  

            //�������ܶ������Կ��ƫ����  
            //ԭ��ʹ��ASCIIEncoding.ASCII������GetBytes����  
            //ʹ�����������������Ӣ���ı�  
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
        /// ���ݽ��ܺ���
        /// Function Name:	Decrypt
        /// Parameters	:   string����		Return:	string����
        /// </summary>
        /// <param name="pToDecrypt">�������ı�</param>
        /// <param name="m_strKey">��Կ</param>
        /// <returns>string����</returns>
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

            //�������ܶ������Կ��ƫ��������ֵ��Ҫ�������޸�  
            des.Key = ASCIIEncoding.ASCII.GetBytes(m_strKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(m_strKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            //Flush  the  data  through  the  crypto  stream  into  the  memory  stream  
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //Get  the  decrypted  data  back  from  the  memory  stream  
            //����StringBuild����CreateDecryptʹ�õ��������󣬱���ѽ��ܺ���ı����������  
            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion
    }
}