using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Event;
using System.Threading;

namespace ConsoleApplication3.Stock
{
    public class ConcurrentEventListener : 
        IPreLoadEventListener, IPostLoadEventListener,
        IPreUpdateEventListener,IPostUpdateEventListener,
        IPreInsertEventListener,IPostInsertEventListener
    {
        public static int MaxConcurrent=25;
        public static int CurrentConcurrent=0;
       
        void Increment()
        {
            while (CurrentConcurrent > MaxConcurrent)
            {
#if DEBUG
                Console.WriteLine("ConcurrentEventListener:{0},{1}", CurrentConcurrent, MaxConcurrent);
#endif
                Thread.Sleep(500);
            }
            Interlocked.Increment(ref CurrentConcurrent);
        }
        void Decrement()
        {
            Interlocked.Decrement(ref CurrentConcurrent);
        }
        public void OnPostLoad(PostLoadEvent @event)
        {

            Decrement();
        }
        public void OnPreLoad(PreLoadEvent @event)
        {
            Increment();
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            Increment();
            return true;
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            Decrement();
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            Increment();
            return true;
        }

        public void OnPostInsert(PostInsertEvent @event)
        {
            Decrement();
        }
    }
}
