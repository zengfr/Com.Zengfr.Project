using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;  
using System.Threading.Tasks;

namespace TaobaoTask
{
    public class RandomComparer<T> : IComparer<T>
    {
        static Random random = new Random(DateTime.Now.Millisecond);
        public int Compare(T x, T y)
        {
            return random.Next(-1, 1); 
        }
    }
     
}
