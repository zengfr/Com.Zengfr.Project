using System;
using System.Collections.Generic;
using System.Text;

using BlackHen.Threading;//
using System.Threading;
using CommonPack;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord;//
using System.Collections;
namespace WordBuilder
{
    class CnHotManager : Manager
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
            WorkQueue.Add(new CnHotWorkItem("http://top.baidu.com/index.html"));//
        }
        
       
    }
}
