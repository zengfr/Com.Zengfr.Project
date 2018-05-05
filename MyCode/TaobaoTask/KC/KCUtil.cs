using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTableExt;
using Konsole;
using Konsole.Forms;

namespace TaobaoTask.KC
{
   public  class KCUtil
    {
        public static void Write<T>(T item, string title = null)
        {
            /* var t = typeof(T);
             var boxtitle = title ?? t.Name;
             var fl = new FieldReader(item).ReadFieldList();
             var box = new BoxWriter(_boxStyle, _width, fl.CaptionWidth, 1);
             _console.WriteLine(box.Header(boxtitle));
             foreach (var f in fl.Fields)
             {
                 var text = string.Format("{0} : {1}", f.Caption.FixLeft(fl.CaptionWidth), f.Value);
                 _console.WriteLine(box.Write(text));
             }
             _console.WriteLine(box.Footer);*/
        }
        public static void WriteFields<T>(IWrite console,IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                WriteFields<T>( console, item);
            }
        }
        public static void WriteFields<T>(IWrite console, T item )
        {
             var t = typeof(T);
             var fl = new FieldReader(item).ReadFieldList();
             foreach (var f in fl.Fields)
             {
                 var text = string.Format("{0} : {1}", f.Caption, f.Value);
                 console.WriteLine(text);
             }
            
        }
        public static void WriteTable<T>(IWrite console, T item)
        {
            var rs = ConsoleTableBuilder.From(DataTableUtil.ToDataTable(item)).WithFormat(ConsoleTableBuilderFormat.Alternative).Export().ToString().Split('\r','\n');
            foreach (var r in rs)
            {
                console.WriteLine(r);
            }
        }
        public static void WriteTable<T>(IWrite console, IEnumerable<T> items)
        {
            var rs = ConsoleTableBuilder.From(DataTableUtil.ToDataTable(items)).WithFormat(ConsoleTableBuilderFormat.Alternative).Export().ToString().Split('\r', '\n');
            foreach (var r in rs)
            {
                console.WriteLine(r);
            }
               
        }
    }
}
