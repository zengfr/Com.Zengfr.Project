using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;
using Konsole.Forms;

namespace TaobaoTask.KC
{
    public static class BoxExtensions
    {
        public static void WritBox(this IConsole console)
        {
            WritBox(console, console.WindowWidth-2, console.WindowHeight-8, new ThinBoxStyle());
        }
        public static void WritBox(this IConsole console, IBoxStyle _boxStyle )
        {
            WritBox(console, console.WindowWidth-2, console.WindowHeight-4,   _boxStyle);
        }
        public static void WritBox(this IConsole console, int width, int height)
        {
            WritBox(console, width, height, new ThinBoxStyle());
        }
        public static void WritBox(this IConsole console, int width, int height, IBoxStyle _boxStyle)
        {
            var box = new BoxWriterEx(_boxStyle, width, 0, 1);
            string header = box.Header("");
            var footer = box.Footer;
            console.WriteLine(header.Replace(_boxStyle.T, '-'));
            for (int i = 0; i < height; i++)
            {
                console.WriteLine(box.Write(""));
            }
            console.WriteLine(footer.Replace(_boxStyle.T, '-'));
        }
    }
}
