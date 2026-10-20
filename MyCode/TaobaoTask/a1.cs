using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTask
{
    public class a1
    {
        public static void gg()
        {
            sss("40397\\/A2PA31473");
            sss(@"40397\/A2PA31473");
            //sss("40397/A2PA31473");
           // sss("40397A2PA31473");
            Console.ReadLine();
        }
        private static void sss(String s)
        {
            String t = ""; var tt = "";
            string tag = "m";
           var m= showQ(tag, s);

            tag = "mm";
            var mm = showQ(tag, m);
            //"A6EF8A52D2EA46F3""8800DE9C003C7BF2" A6EF8A52 D2EA46F3 8800DE9C 003C7BF2
            //d2f02314fe132ba9b2565f1067a58dbe  d2f02314 fe132ba9 b2565f10 67a58dbe

            //A6EF8A52+d2f02314=1 79DFAD66
            //003C7BF2+d2f02314=D32C 9F06
            //A6EF8A52+67a58dbe=1 0E95 1810
            //003C7BF2+67a58dbe=67E2 09B0

            //A6EF8A52D2EA46F3+d2f02314fe132ba9=79DF AD67 D0FD 729C
            //A6EF8A52D2EA46F3+b2565f1067a58dbe=5945 E963 3A8F D4B1
            //8800DE9C003C7BF2+d2f02314fe132ba9=5AF1 01B0 FE4F A79B
            //8800DE9C003C7BF2+b2565f1067a58dbe=3A57 3DAC 67E2 09B0
            tag = "t1";
            t= "79DFAD66" + "fe132ba9b2565f1067a58dbe";
            tt= showQ(tag, t);

            tag = "t2";
            t = "D32C9F06" + "fe132ba9b2565f1067a58dbe";
            tt = showQ(tag, t);

            tag = "t3";
            t =  "d2f02314fe132ba9b2565f10"+ "0E951810";
            tt = showQ(tag, t);


            tag = "t4";
            t = "d2f02314fe132ba9b2565f10" + "67E209B0";
            tt = showQ(tag, t);


            tag = "t5";
            t = "79DFAD66" + "D2EA46F38800DE9C003C7BF2";
            tt = showQ(tag, t);

            tag = "t6";
            t = "D32C9F06" + "D2EA46F38800DE9C003C7BF2";
            tt = showQ(tag, t);

            tag = "t7";
            t = "A6EF8A52D2EA46F38800DE9C" + "0E951810";
            tt = showQ(tag, t);


            tag = "t8";
            t = "A6EF8A52D2EA46F38800DE9C" + "67E209B0";
            tt = showQ(tag, t);




            tag = "t55";
            t = "79DFAD67D0FD729C" + "8800DE9C003C7BF2";
            tt = showQ(tag, t);

            tag = "t66";
            t = "5945E9633A8FD4B1" + "8800DE9C003C7BF2";
            tt = showQ(tag, t);

            tag = "t77";
            t = "A6EF8A52D2EA46F3" + "5AF101B0FE4FA79B";
            tt = showQ(tag, t);


            tag = "t88";
            t = "A6EF8A52D2EA46F3" + "3A573DAC67E209B0";
            tt = showQ(tag, t);

            //var a1 = hexStrToBytes("A6EF8A52D2EA46F38800DE9C003C7BF2");// d2f02314fe132ba9b2565f1067a58dbe
            //var a2 = hexStrToBytes(m);
            // var m3 = byteadd(  a1,   a2);



        }
        private static String showQ(string tag,String s)
        {
            string m = MD5Encrypt32(s);
            string mm = hexStrToStr(m);
            Console.WriteLine("  {0} {1} {2} {3}",tag, s, m, mm);
            return m;
        }
            static byte[] byteadd(byte[] a1, byte[] a2) {
            byte[] a = new byte[a1.Length];

            for (int i = 0; i < a.Length; i += 8)
            { 

            }
            return a;
        }
        public static byte[] hexStrToBytes(String s)
        {
            byte[] a = new byte[s.Length / 2];
            for (int i = 0, h = 0; h < s.Length; i++, h += 2)
            {
                a[i] = (byte)Int32.Parse(s.Substring(h, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return a;
        }

    public static string MD5Encrypt64(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);

        }
        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            return MD5Encrypt32(Encoding.UTF8.GetBytes(cl));//.GetEncoding("utf-8")
        }
            public static string MD5Encrypt32(byte[] buf)
        {
            //Swap(buf);
           // FlipInt32(buf);
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(buf);
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x02");
            }
            return pwd;
        }
        // 16位md5加密
        public static string GetMD5_16(string s)
        {
            MD5 md5 = MD5.Create();
            byte[] t = md5.ComputeHash(UTF8Encoding.Default.GetBytes(s));
            string t2 = BitConverter.ToString(t,4, 8);
            t2 = t2.Replace("-", "");
            return t2;
        }
        public static void Swap(byte[] data)
        {
            for (int i = 0; i < data.Length-1; i += 2)
            {
                byte b = data[i];
                data[i] = data[i + 1];
                data[i + 1] = b;
            }
        }
        static void Swap(ref byte a, ref byte b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        static byte[] FlipInt16(byte[] rawData)
        {
            for (var i = 0; i < rawData.Length; i += 2) // Step two for 2x8 bits=16
                Swap(ref rawData[i], ref rawData[i + 1]);
            return rawData;
        }

        static byte[] FlipInt32(byte[] rawData)
        {
            for (var i = 0; i < rawData.Length; i += 4)
            {// Step four for 4x8 bits=32
                Swap(ref rawData[i + 0], ref rawData[i + 2]);
                Swap(ref rawData[i + 1], ref rawData[i + 3]);
            }
            return rawData;
        }

        private static string hexStrToStr(string str)
        {
            //去除字符串中的空格
            string[] strT = str.Split(' ');
            string strA = "";
            foreach (string strB in strT)
            {
                strA += strB;
            }

            char[] chars = strA.ToCharArray();
            string returnstr = "";
            string[] str1 = new string[chars.Length / 2];

            for (int i = 0; i < chars.Length / 2; i++)
            {
                string str111 = chars[2 * i].ToString() + chars[2 * i + 1].ToString();
                uint num = uint.Parse(str111, System.Globalization.NumberStyles.AllowHexSpecifier);
                char charr = (char)num;
                returnstr = returnstr + charr;
            }
            return returnstr;
        }
    }
}
