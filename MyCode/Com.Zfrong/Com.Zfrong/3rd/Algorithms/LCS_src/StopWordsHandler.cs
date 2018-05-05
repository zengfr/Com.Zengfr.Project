/*
Author: Thanh Ngoc Dao
Copyright (c) 2005 by Thanh Ngoc Dao.
*/

using System;

namespace Com.Zfrong.Common.Algorithms.LCS
{
	/// <summary>
	/// Stop words are frequently occurring, insignificant words words 
	/// Common stop words include
	/// </summary>
	public class StopWordsHandler
	{
		public static string[] stopWordsList=new string[] {
			"after","also","an","a","and","as","at","be","because","before",
			"between","but","before","for","however","from","if","in","into",
			"of","or","other","out","since","such","than","that","the","these",
            "there","this","those","to","under","upon","when","where","whether",
			"which","with","within","without" ,"are","is"

			} ;

		public static bool IsStopWord(string word)
		{
			foreach(string s in stopWordsList)
				if (word.Equals(s) ) return true;
			return false;
		}

		public StopWordsHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
