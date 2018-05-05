/*
Author: Thanh Ngoc Dao
Copyright (c) 2005 by Thanh Ngoc Dao.
*/
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Com.Zfrong.Common.Algorithms.LCS
{
	/// <summary>
	/// Summary description for Tokeniser.
	/// Partition string off into subwords
	/// </summary>
	internal class Tokeniser
	{

		private ArrayList Tokenize(System.String input)
		{
			ArrayList returnVect = new ArrayList(10);
			int nextGapPos;
			for (int curPos = 0; curPos < input.Length; curPos = nextGapPos)
			{
				char ch = input[curPos];
				if (System.Char.IsWhiteSpace(ch))
					curPos++;
				nextGapPos = input.Length;
				for (int i = 0; i < "\r\n\t \x00A0".Length; i++)
				{					
					int testPos = input.IndexOf((Char) "\r\n\t \x00A0"[i], curPos);
					if (testPos < nextGapPos && testPos != - 1)
						nextGapPos = testPos;
				}
				
				System.String term = input.Substring(curPos, (nextGapPos) - (curPos));
				//if (!stopWordHandler.isWord(term))
				returnVect.Add(term);
			}
			
			return returnVect;
		}

		private void Normalize_Casing(ref string input)
		{
			//if it is formed by Pascal/Carmel casing
            //for(int i=0; i < input.Length; i++)
            //{
            //    if (Char.IsSeparator(input[i]))
            //        ;//input=input.Replace(input[i].ToString() , " ") ;//zfr
            //}
			int idx=1;
			while (idx < input.Length - 2)
			{
				++idx;								
				if (
					(Char.IsUpper(input[idx])   
					&& Char.IsLower(input[idx + 1]))
					&& 
					(!Char.IsWhiteSpace(input[idx - 1]) && !Char.IsSeparator(input[idx - 1]) )
					)
				{
					//input=input.Insert(idx, " ") ; //zfr
					++idx;
				}
			}
		}
        /// <summary>
        /// Ä¬ÈÏ partitionRegex=([ =<>])
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partitionRegex"></param>
        /// <returns></returns>
        public string[] Partition(ref string input, string partitionRegex)
        {
            //Regex r=new Regex("([ \\t{}():;.,])");
            if (partitionRegex == null||partitionRegex==string.Empty)
                partitionRegex = "([=<> ])";//
            Regex r = new Regex(partitionRegex);
            Normalize_Casing(ref input);
            //normalization to the lower case
            input = input.ToLower();

            String[] tokens = r.Split(input);
            //ArrayList filter = new ArrayList();
            //zfr×¢ÊÍ
            //for (int i=0; i < tokens.Length ; i++)
            //{
            //    MatchCollection mc=r.Matches(tokens[i]);
            //    if (mc.Count <= 0 && tokens[i].Trim().Length > 0 					 
            //        && !StopWordsHandler.IsStopWord(tokens[i]) )								
            //        filter.Add(tokens[i]) ;


            //}

            //StemmerInterface s=new PorterStemmer() ;
            //tokens=new string[filter.Count] ;
            //for(int i=0; i < filter.Count ; i++)
            //{
            //    tokens[i]=(string) filter[i];
            //    tokens[i]=s.stemTerm(tokens[i]) ;
            //}

            return tokens;
        }
		public string[] Partition(ref string input)
		{
            return Partition(ref input, null);//
		}


		public Tokeniser()
		{
			//
			// TODO: Add constructor logic here
			//
		}
	}
}
