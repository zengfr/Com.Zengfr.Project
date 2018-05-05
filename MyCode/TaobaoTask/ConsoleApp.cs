using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;
using rohankapoor.AutoPrompt;
using Konsole.Internal;
using Konsole.Layouts;
using Konsole.Forms;
using Konsole.Drawing;
using System.Threading;
using TaobaoTask.KC;
using ConsoleTableExt;
using ComLib.Configuration;
using ComLib.IO;
using Newtonsoft.Json;

namespace TaobaoTask
{
    public class ConsoleApp
    {
        static ConsoleApp consoleApp;
        public static ConsoleApp Instance
        {
             get{
                if (consoleApp == null)
                    consoleApp = new ConsoleApp();
                return consoleApp;
            }
            }
        public IWrite LeftWriter { get; set; }
        public IWrite RightWriter { get; set; }
        public IWrite BottomWriter { get; set; }
        public IWrite ConsleWriter { get; set; }
        public IConsole Left { get; set; }
        public IConsole Right { get; set; }
        public IConsole Top { get; set; }
        public IConsole Bottom { get; set; }
        public Window Window { get; set; }
        public void InitConsole(bool clear,bool initWindow) {
            if (clear)
            {
                Console.Clear();
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WindowLeft = 0;
            Console.WindowTop = 0;
            Console.WindowWidth = Console.LargestWindowWidth - 10;
            Console.WindowHeight = Console.LargestWindowHeight * 4/5;
            Console.BufferWidth = Console.WindowWidth * 3;
            Console.BufferHeight = Console.WindowHeight * 4;
           
            if (initWindow)
            {
                InitWindow();
            }
        }
        public void InitConfig()
        {
            log4net.Config.XmlConfigurator.Configure();
            Config.Init(new IniDocument("config.ini", true, true));
           
        }
        public T GetConfig<T>( string key, T defaultValue) {
          return GetConfig("app",   key,  defaultValue);
        }
        public T GetConfig<T>(string section, string key, T defaultValue)
        {
            return ComLib.Configuration.Config.GetDefault(section, key, defaultValue);
        }
        public void InitWindow()
        {
            int w = Console.WindowWidth * 4;
            int h = Console.WindowHeight;
            Window = new Window(w, h, K.Clipping);//, K.FullScreen);
            Window.BackgroundColor = ConsoleColor.DarkBlue;
            SplitConsole(Window);
        }
        private void SplitConsole(IConsole w)
        {
            var fColor = ConsoleColor.Gray;
            Top = w.SplitTop(fColor);
            Bottom = w.SplitBottom(fColor);
           
            Left = Top.SplitLeft(fColor);
            Right = Top.SplitRight(fColor);

            
            BottomWriter= new WriteAdapter(Bottom);
            LeftWriter = new WriteAdapter(Left);
            RightWriter = new WriteAdapter(Right);
            ConsleWriter = new WriteAdapter(new ConsoleWriter());
        }
        public string GetPath(string p)
        {
            return string.Format("{0}{1}",AppDomain.CurrentDomain.BaseDirectory,p);
        }

        
    }
}
