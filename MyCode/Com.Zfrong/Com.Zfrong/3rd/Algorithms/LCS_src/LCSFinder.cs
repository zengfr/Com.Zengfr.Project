/*
Author: Thanh Ngoc Dao
Copyright (c) 2005 by Thanh Ngoc Dao.
*/

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
namespace Com.Zfrong.Common.Algorithms.LCS
{
	/// <summary>
	/// Summary description for LCSFinder.
	/// </summary>
	public class LCSFinder
	{
		public LCSFinder()
		{
		}
		
		public enum BackTracking :byte
        {
			NEITHER,
			UP,
			LEFT,
			UP_AND_LEFT
		}
        
        #region
        public static string GetLCS( string s, string t)
        {
            return GetLCS(ref s, ref t, string.Empty, string.Empty, null, null);
        }
		public static string GetLCS(ref string s,ref string t)
		{
            return GetLCS(ref s,ref t, string.Empty, string.Empty, null, null);
		}
        public static string GetLCS(ref string s, ref string t, IDictionary<int, string> diff1, IDictionary<int, string> diff2)
        {
            return GetLCS(ref s,ref t, string.Empty, string.Empty, diff1, diff2);//
        }
        public static string GetLCS(ref string s,ref string t, string sPartitionRegex, string tPartitionRegex)
        {
            return GetLCS(ref s,ref t, sPartitionRegex, tPartitionRegex, null, null);
        }
        public static string GetLCS(ref string s, ref string t, string sPartitionRegex, string tPartitionRegex, IDictionary<int, string> diff1, IDictionary<int, string> diff2)
        {
            Tokeniser tok = new Tokeniser();
            string[] ss = tok.Partition(ref s, sPartitionRegex);
            string[] tt = tok.Partition(ref t, tPartitionRegex);
            return LCS(ref ss, ref tt, diff1, diff2);
        }
        #endregion
        #region
        private static int ConsecutiveMeasure(int k)
		{
			//f(k)=k*a - b;
			return k*k;
		}

        public static string LCS(ref string[] list1, ref string[] list2, IDictionary<int, string> diff1, IDictionary<int, string> diff2)
        {
            int m = list1.Length;
            int n = list2.Length;

            int[,] lcs = new int[m + 1, n + 1];
            BackTracking[,] backTracer = new BackTracking[m + 1, n + 1];
            int[,] w = new int[m + 1, n + 1];
            int i, j;

            for (i = 0; i <= m; ++i)
            {
                lcs[i, 0] = 0;
                backTracer[i, 0] = BackTracking.UP;
            }
            for (j = 0; j <= n; ++j)
            {
                lcs[0, j] = 0;
                backTracer[0, j] = BackTracking.LEFT;
            }

            for (i = 1; i <= m; ++i)
            {
                for (j = 1; j <= n; ++j)
                {
                    if (list1[i - 1].Equals(list2[j - 1]))
                    {
                        int k = w[i - 1, j - 1];
                        //lcs[i,j] = lcs[i-1,j-1] + 1;
                        lcs[i, j] = lcs[i - 1, j - 1] + ConsecutiveMeasure(k + 1) - ConsecutiveMeasure(k);
                        backTracer[i, j] = BackTracking.UP_AND_LEFT;
                        w[i, j] = k + 1;
                    }
                    else
                    {
                        lcs[i, j] = lcs[i - 1, j - 1];
                        backTracer[i, j] = BackTracking.NEITHER;
                    }

                    if (lcs[i - 1, j] >= lcs[i, j])
                    {
                        lcs[i, j] = lcs[i - 1, j];
                        backTracer[i, j] = BackTracking.UP;
                        w[i, j] = 0;
                    }

                    if (lcs[i, j - 1] >= lcs[i, j])
                    {
                        lcs[i, j] = lcs[i, j - 1];
                        backTracer[i, j] = BackTracking.LEFT;
                        w[i, j] = 0;
                    }
                }
            }
            lcs = null; w = null;
            GC.Collect();////释放

            StringBuilder sbLCS = new StringBuilder();//LCS
            //trace the backtracking matrix.
            i = m; j = n;
            while (i > 0 || j > 0) //注意:backTracer[i,j] 对应 list[1-1,j-1]
            {
                if (backTracer[i, j] == BackTracking.UP_AND_LEFT)
                {
                    i--;
                    j--;
                    sbLCS.Insert(0, list1[i]);//
                    if (backTracer[i, j] != BackTracking.UP_AND_LEFT)
                    {
                        sbLCS.Insert(0, "(*)");//交界
                    }
                }

                else if (backTracer[i, j] == BackTracking.UP)
                {
                    i--;
                    if (diff1 != null)
                        if (diff1.ContainsKey(j))
                            diff1[j]= list1[i] + diff1[j];//合并
                        else
                            diff1.Add(j, list1[i]);
                }

                else if (backTracer[i, j] == BackTracking.LEFT)
                {
                    j--;
                    if (diff2 != null)
                    if (diff2.ContainsKey(i))
                        diff2[i] = list2[j] + diff2[i];//合并
                    else
                        diff2.Add(i, list2[j]);
                }
            }
            backTracer = null;
            GC.Collect();////释放

            string strLCS = sbLCS.ToString();//
            sbLCS.Remove(0, sbLCS.Length);//

            return strLCS;
        }
        #endregion
	}
}
