using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleTask.proj
{
    internal class sps
    {
        public static void Main2(string[] args)
        {
            sps s = new sps();
            s.t1();
            s.t2();
            Console.ReadLine();
        }
        public void t2()
        {
            Console.WriteLine();
            string mid = "30mm250h2i6j106mi8m4m910kii64h4j";
            string lic = "";
            string uid2 = gen_uid(mid);
            string uid3 = to_uidf(uid2);
            string m = s1 + uid2 + s2;
            lic = SHA256EncryptString(m);
            Console.WriteLine(mid);
            Console.WriteLine(uid2);
            Console.WriteLine(uid3);
            Console.WriteLine(string.Format("{0} {1}", lic.Length, lic));
        }
        string to_uidf(string uid)
        {
            string s = "-";
            string nuid = uid;
            for (int i = 0; i < 4; i++)
            {
                nuid = nuid.Insert(8 + i + 4 * i, s);
            }
            return nuid;////30mm250h 2i6j 106m i8m4 m910 kii6 4h4j
        }
        string gen_uid(string mid)
        {
            int ss = 7;
            StringBuilder sb = new StringBuilder();
            foreach (char c in mid)
            {
                if (Char.IsDigit(c))
                {
                    var i = ((int)c - 48 - ss + 10) % 10 + 48;
                    sb.Append((char)i);
                }
                else if (Char.IsLetter(c))
                {
                    int baseInt = 65;
                    if (Char.IsLower(c))
                    {
                        baseInt = 97;
                    }
                    var i = baseInt + ((int)c - baseInt - ss + 26) % 26;
                    sb.Append((char)i);
                }
            }
            return sb.ToString();
        }
        public static string SHA256EncryptString(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            byte[] hash = SHA256Managed.Create().ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }
            return builder.ToString();
        }
        static string s1 = "catarc";
        static string s2 = "shenlanv2pro";
        public void t1()
        {
            string uid = "63ff583a-5b9c-439f-b1f7-f243dbb97a7c";
            string mid = "30mm250h2i6j106mi8m4m910kii64h4j";
            string mid2 = gen_mid(uid);
            Console.WriteLine(uid);
            Console.WriteLine(mid);
            Console.WriteLine(mid2);
        }
        string gen_mid(string id)
        {
            int ss = 7;
            StringBuilder sb = new StringBuilder();
            foreach (char c in id)
            {
                if (Char.IsDigit(c))
                {
                    //(v8 - 48 + shift + 10) % 10 + 48);
                    var i = ((int)c - 48 + 7 + 10) % 10 + 48;
                    sb.Append((char)i);
                }
                else if (Char.IsLetter(c))
                {
                    int baseInt = 65;
                    if (Char.IsLower(c))
                    {
                        baseInt = 97;
                        // a =b  +shift +  26) % 26;
                    }
                    var i = baseInt + ((int)c - baseInt + 7 + 26) % 26;
                    sb.Append((char)i);
                }
            }
            return sb.ToString();
        }
    }
}