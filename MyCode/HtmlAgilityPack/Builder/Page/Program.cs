using System;
using System.Text;
using System.Collections.Generic;
using SubSonic;
using DB.DataDB;
using System.Threading;
using CommonPack;//
namespace Builder.Page
{
    class Program
    {
      public  static void  Start(string[] args)
        {
            //JS.Build();//
            //Start(new ThreadStart(TagBox.Build));//
            Start(new ThreadStart(ItemBox.Build));//
           // Start(new ThreadStart(ItemList.Build));//

            //Start(new ThreadStart(TagList.Build));//
            //Start(new ThreadStart(TagItemList.Build));//
            //Start(new ThreadStart(Item.Build));//

        }
        static void Start(ThreadStart ts)
        {
            Thread m_thread = new Thread(ts);
            m_thread.Start();
        }

        public static string make(int Id)
        {
            string str = DESCrypt.EncryptText(Id.ToString());
            for (int i = 3; i < str.Length - 1; i += 4)
            {
                str = str.Insert(i, "/");//
            }
            return str;//
        }
    }
}