using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using EeekSoft.Text;

namespace AhoCorasickSearch
{
	


	/// <summary>
	/// Testing application
	/// </summary>
	class TestApp
	{
		/// <summary>
		/// Minimal and maximal word length
		/// </summary>
		const int MaxWordLength = 10;
		const int MinWordLength = 3;
		
		/// <summary>
		/// Allowed letters in word
		/// </summary>
		const string AllowedLetters="abcdefghijklmnopqrstuvwxyz";

		#region Utility methods

		static Random rnd=new Random(12345);

		/// <summary>
		/// Generate random word
		/// </summary>
		public static string GetRandomWord()
		{
			StringBuilder sb=new StringBuilder();
      for(int i=0; i<rnd.Next(MaxWordLength-MinWordLength)+MinWordLength; i++)
				sb.Append(AllowedLetters[rnd.Next(AllowedLetters.Length)]);
			return sb.ToString();
		}


		/// <summary>
		/// Generate list of random keywords
		/// </summary>
		public static string[] GetRandomKeywords(int count)
		{
			string[] ret=new string[count];
			for(int i=0; i<count; i++)
				ret[i]=GetRandomWord();
			return ret;
		}


		/// <summary>
		/// Generate random text
		/// </summary>
		public static string GetRandomText(int count)
		{
			StringBuilder sb=new StringBuilder();
			while(sb.Length<count) { sb.Append(GetRandomWord()); sb.Append(" "); }
			return sb.ToString();
		}

		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main2(string[] args)
		{
			// set language to english
			// (because in czech "ch" is one letter and "abch".IndexOf("bc") returns -1)
			System.Threading.Thread.CurrentThread.CurrentCulture=System.Globalization.CultureInfo.CreateSpecificCulture("en");

			// algorithms
			IStringSearchAlgorithm[] algorithms=new IStringSearchAlgorithm[3];
			algorithms[0]=new StringSearch();
			algorithms[1]=new IndexOfSearch();
			algorithms[2]=new RegexSearch();

			double[] results=new double[3];
			for(int j=0; j<100; j++)
			{
				// generate random keywords and random text
				string[] keywords=GetRandomKeywords(80);
				string text=GetRandomText(2000);

				// insert keyword into text
				text=text.Insert(rnd.Next(text.Length),keywords[rnd.Next(keywords.Length)]);
				
				// for each algorithm...
				double first=0;
				for(int algIndex=0; algIndex<algorithms.Length; algIndex++)
				{
					IStringSearchAlgorithm alg=algorithms[algIndex];
					alg.Keywords=keywords;

					// search for keywords and measure performance
					HiPerfTimer tmr=new HiPerfTimer();
					tmr.Start();
					for(int i=0; i<10; i++)
					{
						if (!alg.ContainsAny(text)) 
						{ Console.WriteLine(" Invalid result!"); return; }
					}
					tmr.Stop();

					// write results
					if (first==0) first=tmr.Duration;
					results[algIndex]+=tmr.Duration;
					Console.WriteLine("{0}: {1:0.00000}ms  ({2:0.00}%)",alg.GetType().Name.PadRight(15),tmr.Duration,
						100.0*tmr.Duration/first);
				}
				Console.WriteLine();
			}


			// Write summary results for all test
			Console.WriteLine("\n===SUMMARY===");
			double finalFirst=results[0];
			for(int j=0; j<algorithms.Length; j++)
			{
				IStringSearchAlgorithm alg=algorithms[j];
				Console.WriteLine("{0}: {1:0.00000}ms  ({2:0.00}%)",alg.GetType().Name.PadRight(15),results[j],
					100.0*results[j]/finalFirst);
			}

			Console.WriteLine();
		}
	}
}
