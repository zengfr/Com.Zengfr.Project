using System;
using System.Collections.Generic;
using System.Text;
using CommonPack;
using ParserEngine;//
using DB;
namespace WordBuilder
{
    class Program
    {
        static void ConsoleInit()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WindowWidth = 128;
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight= Console.WindowHeight*3;
            Console.Clear();//
        }
        static void Main(string[] args)
        {
            ConsoleInit();
            WordBuilder.Manager.Init();//
            //Test(); return;//
            Castle.ActiveRecord.Queries.CountQuery q = new Castle.ActiveRecord.Queries.CountQuery(typeof(CnHot),
             new NHibernate.Expression.ICriterion[] { NHibernate.Expression.Expression.Eq("DoCount", 0) });
            int count = (int)CnHot.ExecuteQuery(q);//
            if (count < 20)
            {
                GetCnHot();//
            }
            // return;//
            GetAdvData();
            Console.ReadKey();//
            return;//
        }
        static void GetAdvData()
        {
           // WordBuilder.Manager.Init();//
            AdvDataManager.ThreadStart();//
           // Console.ReadKey();//
        }
        static void GetCnHot()
        {
           // WordBuilder.Manager.Init();//
            WordBuilder.CnHotManager.Start();//
            //Console.ReadKey();//
        }
        static void Test()
        {
            int total = 0;
            XmlSerializableList<Area> list =GoogleSearcher_Blog.Search("美女",0,ref total);
            foreach (Area box in list)
            {
               foreach (Row Row in box.Rows)
                {
                    Console.WriteLine(Row.Cells[0].Value);
                    Console.WriteLine(Row.Cells[1].Value);
               }
            }
        }
        
    }
}
