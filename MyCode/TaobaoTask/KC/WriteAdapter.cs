using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;

namespace TaobaoTask.KC
{
    public class WriteAdapter : IWrite
    {
        private IWrite write;
        public WriteAdapter(IWrite w) {
            write = w;
        }
        protected string Buid(string format, params object[] args)
        {
            var str = string.Format(format, args);
            return string.Format("{0} {1}",DateTime.Now.ToString("HH:mm:ss fff"), str);
        }
        public void Write(string format, params object[] args)
        {
            write.Write("{0}", Buid(format, args));
        }

        public void Write(ConsoleColor color, string format, params object[] args)
        {
            write.Write(color, "{0}", Buid(format, args));
        }

        public void WriteLine(string format, params object[] args)
        {
            write.WriteLine("{0}",Buid(format, args));
        }

        public void WriteLine(ConsoleColor color, string format, params object[] args)
        {
            write.WriteLine(color, "{0}", Buid(format, args));
        }
    }
}
