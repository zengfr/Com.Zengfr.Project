using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hookInject.core
{
   public class HistoryQueue<T>
    {
        T[] arr;
        int start;
        int end;
        int size;

        public HistoryQueue(int capacity)
        {
            arr = new T[capacity];
        }
        /*
         * 0为当前 -1前回朔
         */
        public T get(int his)
        {
            int idx=end-1 + his;
            if(idx<0)
            {
                idx = arr.Length + idx;
            }

            return arr[idx];
        }
        public int push(T obj)
        {
            arr[end] = obj;
            // 两种方法都可以
            // end = end == arr.length -1 ? 0 : end +1 ;
            end = (end + 1) % arr.Length;
            return end;
        }

        public T poll()
        {
            T res = arr[start];
            start = (start + 1) % arr.Length;
            return res;
        }
    }
}
