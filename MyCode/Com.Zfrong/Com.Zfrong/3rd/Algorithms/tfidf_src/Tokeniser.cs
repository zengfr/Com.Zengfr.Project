/*
Tokenization
Author: Thanh Ngoc Dao - Thanh.dao@gmx.net
Copyright (c) 2005 by Thanh Ngoc Dao.
*/

using System;
using System.Collections;
using System.Text.RegularExpressions;


namespace Com.Zfrong.Common.Algorithms.TFIDF
{
	/// <summary>
	/// Summary description for Tokeniser.
	/// Partition string into SUBwords
	/// </summary>
	internal class Tokeniser
	{

		public static string[] ArrayListToArray(ArrayList arraylist)
		{
			string[] array=new string[arraylist.Count] ;
			for(int i=0; i < arraylist.Count ; i++) array[i]=(string) arraylist[i];
			return array;
		}
        //public string[] Partition(string input)
        //{
        //    return Partition(input, "([ \\t{}(). \n,/-_?#+])");
        //}
        public string[] Partition(string input)
        {
            return Partition(input, "([ \\t{}(). \n,;:|/-_?#=&\\+])");
        }
        //public string[] Partition(string input)
        //{
        //    return Partition(input, "([ \\t{}():;. \n])");
        //}
		public string[] Partition(string input,string regex)
		{
			Regex r;
            //r==new Regex("([ \\t{}():;. \n])");
            r = new Regex(regex);//zfr
            //r = new Regex("([ ;,/=-_?#\\+])");//zfr url
			input=input.ToLower() ;

			String [] tokens=r.Split(input); 									

			ArrayList filter=new ArrayList() ;

            for (int i = 0; i < tokens.Length; i++)
            {
                MatchCollection mc = r.Matches(tokens[i]);
                if (mc.Count <= 0 && tokens[i].Trim().Length > 0
                    && !StopWordsHandler.IsStopword(tokens[i])
                    )
                    filter.Add(tokens[i]);
            }

          
			return ArrayListToArray (filter);
		}
        /// <summary>
        /// zfr
        /// </summary>
        /// <param name="input"></param>
        /// <param name="regex"></param>
        /// <returns></returns>
        public string[] Partition2(string input, string regex)
        {
            ArrayList filter = new ArrayList();
           Regex r = new Regex(regex);//zfr
            input = input.ToLower();
            //zfr添加
            MatchCollection mColl = r.Matches(input);
            string tmp; int start, index;

            for (int i = 0; i < mColl.Count; i++)
            {
                if (i == 0) start = 0;
                else
                    start = mColl[i - 1].Index + mColl[i - 1].Length;
                tmp = input.Substring(start, mColl[i].Index - start);
                if (tmp.Length != 0)
                    filter.Add(tmp);

                tmp = mColl[i].Value;
                filter.Add(mColl[i].Value);

                if (i == mColl.Count - 1)
                {
                    tmp = input.Substring(mColl[i].Index + mColl[i].Length);
                    if (tmp.Length != 0)
                        filter.Add(tmp);
                }
            }
            return ArrayListToArray(filter);
        }

		public Tokeniser()
		{
		}
	}
}
