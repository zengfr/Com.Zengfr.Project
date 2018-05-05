using System;
using System.Collections.Generic;
using System.Text;

using BlackHen.Threading;//
using System.Threading;
using CommonPack;

using System.Collections;
namespace WordBuilder
{
    class EnHotManager:Manager
    {
        public static WorkQueue WorkQueue = new WorkQueue();
        
        static public void TheadStart()
        {
            
            ThreadStart ts = new ThreadStart(Start);
            Thread t = new Thread(ts);//
            t.Start();//
        }
        static public void Start()
        {
            WorkQueue.Add(new EnHotWorkItem("http://www.google.com/trends/hottrends"));//
        }
        
    }
}
