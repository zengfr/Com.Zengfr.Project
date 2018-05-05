using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;

namespace TaobaoTask.KC
{
    public class ConsoleWriter : IWrite
    {
        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            Console.Write(format, args);
        }

        public void Write(string format, params object[] args)
        {
            Console.Write(format, args);
        }
    }
}
