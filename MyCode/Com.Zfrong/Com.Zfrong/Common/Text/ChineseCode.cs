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
            //调用函数产生1个随机中文汉字编码 
            byte[] bytes = CreateRegionCode();
            //根据汉字编码的字节数组解码出中文汉字 
           return gb.GetString(bytes);
        }
        
       /// <summary>
        /// //获取GB2312编码页（表） 
       /// </summary>
      static  Encoding gb = Encoding.GetEncoding("gb2312");
       /// <summary>
        /// //定义一个字符串数组储存汉字编码的组成元素 
       /// </summary>
      static string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" }; 
        /// <summary>
        /// 此函数在汉字编码范围内随机创建含两个元素的十六进制字节数组，每个字节数组代表一个汉字，并将 
        ///四个字节数组存储在object数组中。 
        /// 参数：strlength，代表需要产生的汉字个数 
        /// </summary>
        /// <param name="strlength"></param>
        /// <returns></returns>
       private static byte[] CreateRegionCode()
       {
           Random rnd;
           /*  每个汉字有四个区位码组成 
            区位码第1位和区位码第2位作为字节数组第一个元素 
            区位码第3位和区位码第4位作为字节数组第二个元素 */
           //区位码第1位 
           rnd = new Random(GetRandomSeed((int)DateTime.Now.Ticks));
           int r1 = rnd.Next(11, 14);
           //区位码第2位 
           rnd = new Random(GetRandomSeed(r1));
           int r2;
           switch (r1)
           {
               case 13:
                   r2 = rnd.Next(0, 7); break;
               default:
                   r2 = rnd.Next(0, 16); break;
           }
           //区位码第3位 
           rnd = new Random(GetRandomSeed(r2));
           int r3 = rnd.Next(10, 16);
           //区位码第4位 
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
           //定义两个字节变量存储产生的随机汉字区位码 
           byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
           byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
           //将两个字节变量存储在字节数组中 
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
