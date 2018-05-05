using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaobaoTask.KC;

namespace TaobaoTask
{
    class Program
    {
        static void Main(string[] args)
        {
            youzanApp2.MainT(args);
            return;
            // KonsoleSample.MainT(args);
            //return;
            //Craw.Program.Main2(args);
            youzanApp.process();
            //TaobaoKeyword TaobaoKeyword = new TaobaoKeyword();
            //    TaobaoKeyword.Run();
            Console.ReadLine();
        }
    }
}
