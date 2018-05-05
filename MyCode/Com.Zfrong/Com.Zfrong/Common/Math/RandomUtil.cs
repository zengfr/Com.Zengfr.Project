using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Zfrong.Common.Math
{
    public class RandomUtil
    {
        #region ####产生随机数
        private static Random GetRandom()
        {
            return new Random(DateTime.Now.Second * DateTime.Now.Millisecond);
        }
        private static int Pow10(int y)
        {
            //if (y == 0) minLen = 1;
            if (y > 9) y = 9;//
            return (int)System.Math.Pow(10, y);
        }
        private static void Change(ref int min, ref int max)
        {
            int t;
            if (min > max)
            {
                t = min;
                min = max;
                max = t;
            }
        }
        public static int GetRandomInt(int minLen, int maxLen)
        {
            Change(ref minLen, ref maxLen);
            return GetRandom().Next(Pow10(minLen - 1), Pow10(maxLen));
        }
        public static int GetRandomInt(int len)
        {
            return GetRandom().Next(Pow10(len - 1), Pow10(len));
        }
        public static int GetRandomHashCode()
        {
            return Guid.NewGuid().GetHashCode();//
        }
        public static string GetRandomStr(int minLen, int maxLen)
        {
            int len = GetRandom().Next(minLen, maxLen);//
            return GetRandomStr(len);
        }
        public static string GetRandomStr(int len)
        {
            if (len > 32) len = 32;
            string str = Guid.NewGuid().ToString("N");
            int index = 0;
            while (char.IsDigit(str, index)) { index++; }
            if (len > str.Length - index)
                return str.Substring(index);
            else
                return str.Substring(index, len);
            //N 32位：
            //xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            //如：e92b8e30a6e541f6a6b9188230a23dd2

            //D 由连字符分隔的32位数字：                                    
            //xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx
            //如：e92b8e30-a6e5-41f6-a6b9-188230a23dd2

            //B 括在大括号中、由连字符分隔的32位数字：      
            //{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx}
            //如：{e92b8e30-a6e5-41f6-a6b9-188230a23dd2}

            //P 括在圆括号中、由连字符分隔的32位数字：        
            //(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)
        }
        #endregion
    }
}
