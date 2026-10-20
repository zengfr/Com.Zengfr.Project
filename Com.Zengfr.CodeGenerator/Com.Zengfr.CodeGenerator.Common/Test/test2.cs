using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Test2
{
    using System;
    class A
    {
        public static int X;
        static A()
        {
            X = B.Y + 1;
        }
    }
    class B
    {
        public static int Y = A.X + 1;
        static B() { }
      public  static void Main2()
        {
            Console.WriteLine("X={0},Y={1}", A.X, B.Y);
        }
    }
    //产生的输出结果是什么？
   /// 答：x=1,y=2

}
