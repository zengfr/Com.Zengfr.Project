using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;
using Konsole.Forms;
using System;
using System.Data;
using Konsole.Internal;
namespace TaobaoTask.KC
{
    public class FormEx
    {
        private int _width;
        private readonly IBoxStyle _boxStyle;
        private readonly IConsole _console;

        public FormEx(IConsole console = null) : this(console, console?.WindowWidth - 1 ?? 80, null) { }
        public FormEx(int width) : this(null, width, null) { }
        public FormEx() : this(null, 80, null) { }

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
            }
        }

        public FormEx(int width, IBoxStyle boxStyle) : this(null, width, boxStyle)
        { }

        public FormEx(IConsole console, int width) : this(console, width, new ThinBoxStyle())
        {

        }

        public FormEx(IConsole console, int width, IBoxStyle boxStyle)
        {
            _width = width;
            _boxStyle = boxStyle ?? new ThinBoxStyle();
            _console = console ?? new Writer();
        }

        /// <summary>
        /// form is written (inline) at current cursor position, and cursor is updated to next line below form, with left=0
        /// </summary>
        public void Write<T>(T item, string title = null)
        {
            var t = typeof(T);
            var boxtitle = title ?? t.Name;
            var fl = new FieldReader(item).ReadFieldList();
            var box = new BoxWriterEx(_boxStyle, _width, fl.CaptionWidth, 1);
            string header = box.Header(boxtitle);
            var footer = box.Footer;
            _console.WriteLine(header.Replace(_boxStyle.T,'-'));
            foreach (var f in fl.Fields)
            {
                var text = string.Format("{0} : {1}", f.Caption.FixLeft(fl.CaptionWidth), f.Value);
                _console.WriteLine(box.Write(text));
            }
            _console.WriteLine(footer.Replace(_boxStyle.T, '-'));
        }
    }
}
