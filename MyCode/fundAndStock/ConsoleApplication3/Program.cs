using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework.Config;
using ConsoleApplication3.Fund;
using ConsoleApplication3.Stock;
using ConsoleControlSample;
using NHibernate.Criterion;
using ConsoleApplication3.Utils;
using System.Diagnostics;
using System.Text;
using NHibernate.Event;
using SymbolLookup;

namespace ConsoleApplication3
{
    class Program
    {

        static void Main2(string[] args)
        {
            try
            {
             
                InitConfigurationSource();

              //var scheduler=  QuartzUtils.GetScheduler(null);
              //scheduler.Start();
                 args = new string[] { "6" };
                //args = new string[] { "99" };
                if (args != null && args.Length > 0)
                {
                    Console.WindowWidth = 140;
                    Console.BufferWidth = 140 * 3;
                    Command(args[0]);
                    Console.WriteLine("Input Any Key......");
                    Console.ReadLine();
                }
                else
                {
                    RunWinfrom();
                }
            }
            catch (Exception ex)
            {
                logException(ex);
            }
        }
        private static void logException(Exception ex)
        { 
            if(ex != null)
            {
            var sb = new StringBuilder();
            sb.AppendLine(ex.Message);
            sb.AppendLine(ex.Source);
            sb.AppendLine(ex.StackTrace);

            //EventLog log = new EventLog();
            //log.Source = typeof(Program).Assembly.FullName;
            //log.WriteEntry(sb.ToString());

            logException(ex.InnerException);
            }
        }
        private static void Command(string type)
        {
            switch (type)
            {
                case "99":
                    q.execStock0(0);
                    q.execStock1(0);
                    q.execStock2(0);
                    q.execStock3(0);
                    q.execStock4(0);
                    q.execStock5(0);
                    q.execStock6(0);
                    break;
                case "0":
                    MorningstarUtils.GetMorningstarIds(10000);
                    break;
                case "1":
                    HowBuyUtils.GetHowBuyManagerIds(1);
                    break;
                case "2":
                    MorningstarQuicktake();
                    break;
                case "3":
                    Eastmoney();
                    break;
                case "4":
                    HowBuyUtils.UpdateFundManagerAchievements();
                    break;
                
                case "5":
                    StockUtils.process();
                    break;
                case "6":
                    SymbolActorSystem symbolActorSystem = new SymbolActorSystem();
                    symbolActorSystem.Tell("sh600150", DateTime.Today.AddDays(-2));
                    //symbolActorSystem.Tell(DateTime.Today.AddDays(-1));
                    break;
                
                   
                default:
                    break;

            }
        }

        

        private static void MorningstarQuicktake()
        {
            var funds = ActiveRecordMediator<Fund.Fund>.FindAll(new Order[] { Order.Desc("DataChangeLastTime") });

            foreach (var fund in funds)
            {
                if (!string.IsNullOrEmpty(fund.MorningstarId))
                {
                    // MorningstarUtils.quicktake(fund.MorningstarId);
                    MorningstarUtils.quicktake(MorningstarUtils.MorningstarCommand.rating, fund.MorningstarId, fund.FundCode);
                    MorningstarUtils.quicktake(MorningstarUtils.MorningstarCommand.portfolio, fund.MorningstarId, fund.FundCode);
                }

            }
        }
        private static void Eastmoney()
        {
            var funds = ActiveRecordMediator<Fund.Fund>.FindAll(new Order[] { Order.Desc("DataChangeLastTime") });
            foreach (var fund in funds)
            {
                if (!string.IsNullOrEmpty(fund.FundCode))
                {
                    EastmoneyUtils.F10DataApi(fund.FundCode);
                }
            }
        }
        private static void InitConfigurationSource()
        {

            IDictionary<string, string> properties = new Dictionary<string, string>();

            properties.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
            properties.Add("dialect", "NHibernate.Dialect.MsSql2000Dialect");
            properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            //properties.Add("connection.connection_string", "Data Source=testdb.dev.sh.ctriptravel.com,28747;Initial Catalog=zfrdb;Integrated Security=SSPI");
            properties.Add("connection.connection_string", "Data Source=(local);Initial Catalog=fundAndStock;Persist Security Info=True;User ID=sa;Password=1234567;Pooling=False");
            //properties.Add("proxyfactory.factory_class", "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
            properties.Add("hibernate.command_timeout",""+1000*200);
            properties.Add("command_timeout", "" + 1000 * 200);
            InPlaceConfigurationSource source = new InPlaceConfigurationSource();

            source.Add(typeof(ActiveRecordBase), properties);

            ActiveRecordStarter.MappingRegisteredInConfiguration += ActiveRecordStarter_MappingRegisteredInConfiguration;
            ActiveRecordStarter.Initialize(typeof(Fund.Fund).Assembly, source);


            //ActiveRecordStarter.Initialize(source, typeof(Stock.StockTradeHistoryStatistic60));
            //ActiveRecordStarter.DropSchema();
            //ActiveRecordStarter.CreateSchema();


        }

        private static void ActiveRecordStarter_MappingRegisteredInConfiguration(Castle.ActiveRecord.Framework.ISessionFactoryHolder holder)
        {
            NHibernate.Cfg.Configuration configuration =holder.GetConfiguration(typeof(ActiveRecordBase));
            if (configuration != null)
            {
                ConcurrentEventListener concurrentEventListener = new ConcurrentEventListener();
                configuration.SetListener(ListenerType.PreLoad, concurrentEventListener);
                configuration.SetListener(ListenerType.PostLoad, concurrentEventListener);
            }
        }

        private static void RunWinfrom()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var f = new FormConsoleControlSample();
            Application.Run(f);
            

           // var c = new MyCanvas();
          
        
             //Application.Run(c);
            
           // Application.Run(new MyCanvas());
          
        }
    }
}
