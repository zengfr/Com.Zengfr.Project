using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using FluentNHibernate;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;

namespace Com.Zengfr.CodeGenerator.Web
{
    public class NHibernateUtil
    {
        public enum CurrentSessionContextClass
        {
            managed_web,
            call,
            thread_static,
            web
        }
        public  static ISessionFactory SessionFactory { get; set; }
        public static ISession Session {
            get {
                BindContext();
                return SessionFactory.GetCurrentSession();
            }
        }
        private static void BindContext()
        {
            if (!CurrentSessionContext.HasBind(SessionFactory))
            {
                CurrentSessionContext.Bind(SessionFactory.OpenSession());
            }
        }

        private static void UnBindContext()
        {
            if (CurrentSessionContext.HasBind(SessionFactory))
            {
                CurrentSessionContext.Unbind(SessionFactory);
            }
        }
        static NHibernateUtil() {
            //Init();
        }

        private static PersistenceModel getFluentNhibernatePersistenceModel()
        {
            var model = new PersistenceModel();
           // model.Conventions.GetForeignKeyName = prop => prop.Name + "Id"; 
            //model.Conventions.GetForeignKeyNameOfParent = type => type.Name + "Id";
            model.AddMappingsFromAssembly(Assembly.GetExecutingAssembly());
            model.Conventions.Add<MyClassConvention>();
            model.Conventions.Add<PrimaryKeyConvention>();
            //model.Conventions.Add<MyClassConvention>();
            return model;
        }
        public static void Init() {
            var cfgFile = GetRootPath() + "hibernate.cfg.xml";
            var cfg = new NHibernate.Cfg.Configuration().Configure(cfgFile);
            var model = getFluentNhibernatePersistenceModel();
            model.Configure(cfg);
            cfg.SetProperty("current_session_context_class", ""+CurrentSessionContextClass.thread_static);
            SessionFactory =Fluently.Configure(cfg).ExposeConfiguration(BuildSchema).BuildSessionFactory();
           // SessionFactory = cfg.BuildSessionFactory();

            
        }
        public static string GetRootPath() {
            return AppDomain.CurrentDomain.BaseDirectory + "\\";
        }
        private static void BuildSchema(Configuration config)

        {
            var file = GetRootPath() +"Schema.sql";
            if (File.Exists(""+ file))
                File.Delete(""+ file);
            var se = new NHibernate.Tool.hbm2ddl.SchemaExport(config);
            var sb = new StringBuilder();
            TextWriter output = new StringWriter(sb);
            se.SetDelimiter(";");
            se.Execute(true, false, false, null, output);

            var sql = sb.ToString();
            var regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;
            sql = Regex.Replace(sql, "add constraint(.*?);", "--c--", regexOptions);
            sql = Regex.Replace(sql, "alter table(.*?)--c--", "--", RegexOptions.RightToLeft | regexOptions);

            File.WriteAllText(file, sql);
        }
    }
}