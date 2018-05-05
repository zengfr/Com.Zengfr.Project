using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autofac;
using NCrawler;
using NCrawler.HtmlProcessor;
using NCrawler.Interfaces;
using NCrawler.Services;
using Autofac;
//using Crawler;
using Autofac;
using Autofac.Core.Lifetime;
using NCrawler;
using NCrawler.Extensions;
using NCrawler.Interfaces;
using NCrawler.Services;
using cc;
using ComLib.Configuration;
using ComLib.IO;

namespace Craw
{
    class Program
    {
      public   static void Main2(string[] args)
        {
            var r = DumpDictResult.Split("#3例如， ‘[abc]‘ 可以匹配 plain 中的 ‘a'。");
            Console.WriteLine(string.Join("|", r));
            Console.WriteLine(string.Join("|", DumpDictResult.SplitCut(r, 2)));

            Console.ReadLine();

            DictUtil.Load();
            ConsoleUtil.RegisterCloseConsoleHandle();
            ConsoleUtil.ClosedConsole += (sender, e) =>
            {
                Console.WriteLine("控制台关闭");
                DictUtil.Store();
                Console.ReadLine();
            };

           

            Module[] ms = new Module[] { new CNCrawlerModule() };
            NCrawlerModule.Setup(ms);
            string startUrl = "http://news.hexun.com/";
            // var baseUrl = "http://www.oschina.net/";
            //baseUrl = "http://news.baidu.com/";
            // baseUrl = "http://www.csdn.net/";

            Config.Init(new IniDocument("cfg.ini", true, true));
     
            startUrl = ComLib.Configuration.Config.GetString("cfg","startUrl");
            if (string.IsNullOrWhiteSpace(startUrl))
            {
                startUrl = "http://news.hexun.com/";
            }
            CCrawler c = new CCrawler(new Uri(startUrl),
                new CHtmlDocumentProcessor(),
                //new HtmlDocumentExProcessorPipelineStep(20000000, 1),
                new DumpDictResult(),
                new DumpResultProcessor()
            );
            c.Init();
            c.Crawl();
            Console.ReadLine();
        }

       
    }
}
