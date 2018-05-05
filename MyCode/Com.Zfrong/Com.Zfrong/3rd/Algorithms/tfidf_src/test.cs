using System;
using System.Diagnostics;

namespace Com.Zfrong.Common.Algorithms.TFIDF
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
public	class Class1
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
	public	static void Main2(string[] args)
		{
           Test1();
           // Test2();
            Console.ReadKey();//
		}
       static void Test1()
        {
            string[] doc = new string[20000];
            for (int i = 0; i < 10000; i++)
                doc[i] = "http://www.baidu.com/qwer/ty/"+i+".aspx?yy=" + i;
            for (int i =10000; i < 20000; i++)
                doc[i] = "http://pp.com/" + i + "/pu.aspx?pdi=9&yy=" + i;
           // for (int i =2000; i < 3000; i++)
                //doc[i] = "http://www.yu.kk.com/r.aspx?hj=" + i;
           
            Com.Zfrong.Common.Algorithms.TFIDF.StopWordsHandler stopword = new Com.Zfrong.Common.Algorithms.TFIDF.StopWordsHandler();
            Com.Zfrong.Common.Algorithms.TFIDF.URLTFIDFMeasure tf = new Com.Zfrong.Common.Algorithms.TFIDF.URLTFIDFMeasure(doc);
            tf.DocIndex = 0;//
            tf.Init();//

            tf.GetSimilarityDoc(5);//

            tf.GetIsSimilarity(1005,doc);//
        }
      static  void Test2()
        {
            StopWordsHandler stopword = new StopWordsHandler();
            string[] doc = new string[12]{"test b c c dd d d d d", "a b c c c d dd d d test",
                "c d b f y teyr etre tretr gfgd c","r a e e f n l i f f f f x l",
                "question/3456.html?fr=idnw","question/8789077.html?fr=idnw",
                "http://news.hexun.com/2007-07-20/100066714.html",
                "http://news.hexun.com/2007-09-03/100552950.html",
                "大浪淘沙之后",
                "大浪之后淘沙","abcdefghijklmnxyz","nmlkjihgfedcbaxyz"};

            TFIDFMeasure tf = new TFIDFMeasure(doc);
            Trace.WriteLine((double)System.Math.Log(10000 / 50));
            tf.Init();//
            Trace.WriteLine(tf.GetSimilarity(10, 11));
            Trace.WriteLine(tf.GetSimilarity(0, 1));
            Trace.WriteLine(tf.GetSimilarity(0, 7));
            Trace.WriteLine(tf.GetSimilarity(4, 5));
            Trace.WriteLine(tf.GetSimilarity(6, 7));
            Trace.WriteLine(tf.GetSimilarity(8, 9));
            string[] _grams = NGram.GenerateNGrams("TEXT", 3);
            foreach (string s in _grams)
            {
                Trace.WriteLine(s);//
            }
            //
            // TODO: Add code to start application here
            //
        }
	}
}
