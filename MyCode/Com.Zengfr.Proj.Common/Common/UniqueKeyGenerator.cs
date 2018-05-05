using System;
using System.Security.Cryptography;

namespace Com.Zengfr.Proj.Common
{
    /// <summary>
    ///  
    /// </summary>
    public class UniqueKeyGenerator
    {
        static RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
        /// <summary>
        /// 1 个人 2单位公司
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string RandomDCMCode(int type)
        {
            //return string.Format("DCM{0}{1}{2}", "CN", type, RandomDCMCode13());
            return string.Format("{0}{1}{2}", "CN", type, RandomDCMCode13());
        }
        protected static string RandomDCMCode13()
        {
            Random r = new Random(RandomInt());
            var result = string.Format("{0}{1}{2}", r.Next(0, 10), r.Next(0, int.MaxValue).ToString("d10"), r.Next(10, 100));
            return result;
        }
        public static ulong RandomULong()
        {
            var buffer = new byte[8];
            ulong result = 0;
            do
            {
                rNGCryptoServiceProvider.GetBytes(buffer);
                result = BitConverter.ToUInt64(buffer, 0);
            } while (result == 0);
            return result;
        }
        public static uint RandomUInt()
        {
            var buffer = new byte[4];
            uint result = 0;
            do
            {
                rNGCryptoServiceProvider.GetBytes(buffer);
                result = BitConverter.ToUInt32(buffer, 0);
            } while (result == 0);
            return result;
        }
        public static int RandomInt()
        {
            var buffer = new byte[4];
            int result = 0;
            do
            {
                rNGCryptoServiceProvider.GetBytes(buffer);
                result = BitConverter.ToInt32(buffer, 0);
            } while (result == 0);
            return result;
        }
        public static Guid NewGuid()
        {
            return Guid.NewGuid();
        }
        public static string GuidToString(string format)
        {
            return NewGuid().ToString(format);
        }
        /// <summary>  
        /// 根据GUID获取16位的唯一字符串  
        /// </summary>  
        /// <param name=\"guid\"></param>  
        /// <returns></returns>  
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        /// <summary>  
        /// 根据GUID获取19位的唯一数字序列  
        /// </summary>  
        /// <returns></returns>  
        public static long GuidToLongID()
        {
            byte[] buffer = NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <summary>  
        /// 生成22位唯一的数字 并发可用  
        /// </summary>  
        /// <returns></returns>  
        public static string GenerateUniqueID()
        {
            System.Threading.Thread.Sleep(1); //保证yyyyMMddHHmmssffff唯一  
            Random d = new Random(BitConverter.ToInt32(NewGuid().ToByteArray(), 0));
            string strUnique = DateTime.Now.ToString("yyyyMMddHHmmssffff") + d.Next(1000, 9999);
            return strUnique;
        }
    }
}
