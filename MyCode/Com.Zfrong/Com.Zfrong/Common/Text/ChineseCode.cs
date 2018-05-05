using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Zfrong.Common.Text
{
   public class ChineseCode
    {
       public static string GetRegionChineseString(int length)
       {
           StringBuilder sb = new StringBuilder();
           for (int i = 0; i < length; i++)
               sb.Append(GetRegionChineseWord());
           return sb.ToString();//
       }
       public static string GetRegionChineseWord()
        {
            //���ú�������1��������ĺ��ֱ��� 
            byte[] bytes = CreateRegionCode();
            //���ݺ��ֱ�����ֽ������������ĺ��� 
           return gb.GetString(bytes);
        }
        
       /// <summary>
        /// //��ȡGB2312����ҳ���� 
       /// </summary>
      static  Encoding gb = Encoding.GetEncoding("gb2312");
       /// <summary>
        /// //����һ���ַ������鴢�溺�ֱ�������Ԫ�� 
       /// </summary>
      static string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" }; 
        /// <summary>
        /// �˺����ں��ֱ��뷶Χ���������������Ԫ�ص�ʮ�������ֽ����飬ÿ���ֽ��������һ�����֣����� 
        ///�ĸ��ֽ�����洢��object�����С� 
        /// ������strlength��������Ҫ�����ĺ��ָ��� 
        /// </summary>
        /// <param name="strlength"></param>
        /// <returns></returns>
       private static byte[] CreateRegionCode()
       {
           Random rnd;
           /*  ÿ���������ĸ���λ����� 
            ��λ���1λ����λ���2λ��Ϊ�ֽ������һ��Ԫ�� 
            ��λ���3λ����λ���4λ��Ϊ�ֽ�����ڶ���Ԫ�� */
           //��λ���1λ 
           rnd = new Random(GetRandomSeed((int)DateTime.Now.Ticks));
           int r1 = rnd.Next(11, 14);
           //��λ���2λ 
           rnd = new Random(GetRandomSeed(r1));
           int r2;
           switch (r1)
           {
               case 13:
                   r2 = rnd.Next(0, 7); break;
               default:
                   r2 = rnd.Next(0, 16); break;
           }
           //��λ���3λ 
           rnd = new Random(GetRandomSeed(r2));
           int r3 = rnd.Next(10, 16);
           //��λ���4λ 
           rnd = new Random(GetRandomSeed(r3));
           int r4;
           switch (r3)
           {
               case 10:
                   r4 = rnd.Next(1, 16); break;
               case 15:
                   r4 = rnd.Next(0, 15); break;
               default:
                   r4 = rnd.Next(0, 16); break;
           }
           return CreateCode(rBase[r1], rBase[r2], rBase[r3], rBase[r4]);//
       }
       private static byte[] CreateCode(string str_r1,string str_r2,string str_r3,string str_r4)
       {
           //���������ֽڱ����洢���������������λ�� 
           byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
           byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
           //�������ֽڱ����洢���ֽ������� 
           byte[] str_r = new byte[] { byte1, byte2 };
           return str_r;//
       }
       private static int GetRandomSeed(int seed)
       {
           return unchecked(seed * (int)DateTime.Now.Ticks)+i++;//
       }
       private static int i = 1;//
    } 

}
