using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.XFactories;
using RestSharp;

namespace TaobaoTask
{
    /**
     * 更新全国统计用区划代码和城乡划分代码
     http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2019/index.html
    **/
    public class regionApp
    {
        private static string baseUrl = "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2019";
        private static string encoding = "GB2312";
        private static ISessionFactory sessionFactory;
        public static void process()
        {
           

            var consoleApp = ConsoleApp.Instance;
            consoleApp.InitConsole(true, true);
            consoleApp.InitConfig();

            initSessionFactory(consoleApp);

            var restClient = RestClientUtil.GetRestClient(baseUrl, true);
            restClient.Encoding = Encoding.GetEncoding(encoding);

            save(consoleApp, "0", "86", "中国", 1, 1);

            var maxDegreeOfParallelism = 4;
            getprovincetr(restClient, maxDegreeOfParallelism, consoleApp, "86");
        }
        public static void initSessionFactory(ConsoleApp consoleApp)
        {
            try
            {
                NHibernate.Cfg.Configuration config = new NHibernate.Cfg.Configuration();
                var file = "~/nhibernate.cfg.xml";
                file = HostingEnvironment.MapPath(file);
                file = "nhibernate.cfg.xml";
                sessionFactory = config.Configure(file, "jenze")
                    //.AddClass(typeof(region))
                    .AddFile("region.hbm.xml")
                    .BuildSessionFactory();
            }
            catch (Exception ex) {
                consoleApp.BottomWriter.WriteLine(ex.Message+" "+ex.StackTrace);
            }
        }
        /**
         * 省份 Parallel
         * **/
        protected static void getprovincetr(IRestClient restClient, int maxDegreeOfParallelism, ConsoleApp consoleApp, string plongcode)
        {

            var url = string.Format("" + "/index.html");
            var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
            if (RestClientUtil.IsSuccess(resp))
            {
                RestClientUtil.FixContentEncoding(resp, encoding);
                string tbl = RestClientUtil.RegexGetString("", resp.Content, "class='provincetr'(.*?)</table>", 0);
                var ms = RestClientUtil.RegexGetMatchs(tbl, "href='(.*?).html'>(.*?)<");
                int sort = 0;
                short level = 2;//;/
                Action<Match> func = (Match m) =>
                {
                    save(consoleApp, plongcode, m.Groups[1].Value, m.Groups[2].Value, level, ++sort);
                    var provinceCode = m.Groups[1].Value;
                   // if (provinceCode != "31")
                        //return;
                    var plongcode2 = "" + provinceCode;
                    getcitytr(restClient, consoleApp, plongcode2, provinceCode);//
                };
                if (maxDegreeOfParallelism > 1)
                {
                    ParallelOptions parallelOptions = new ParallelOptions();
                    parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;

                    var mss = ms.Cast<Match>();

                    Parallel.ForEach(mss, parallelOptions, m =>
                    {
                        func.Invoke(m);
                    });
                }
                else
                {
                    foreach (Match m in ms)
                    {
                        func.Invoke(m);
                    }
                }
            }
        }

        /***
         * 城市
         * ****/
        protected static void getcitytr(IRestClient restClient, ConsoleApp consoleApp, string plongcode, string provinceCode)
        {

            var url = string.Format("" + "/{0}.html", provinceCode);
            var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
            if (RestClientUtil.IsSuccess(resp))
            {
                RestClientUtil.FixContentEncoding(resp, encoding);
                var trs = RestClientUtil.RegexGetMatchs(resp.Content, "class='citytr'(.*?)</tr>");
                foreach (Match tr in trs)
                {
                    int sort = 0;
                    short level = 3;
                    var ms = RestClientUtil.RegexGetMatchs(tr.Value, "href='(.*?).html'>(.*?)</a>(.*?)'>(.*?)</a>");
                    foreach (Match m in ms)
                    {
                        save(consoleApp, plongcode, m.Groups[2].Value, m.Groups[4].Value, level, ++sort);
                        var cityCode = m.Groups[1].Value;
                        var plongcode2 = "" + cityCode.Split('/')[1];
                        getcountytr(restClient, consoleApp, plongcode2, provinceCode, cityCode);
                    }
                }
            }
        }
        /***
         * 区县
         * **/
        protected static void getcountytr(IRestClient restClient, ConsoleApp consoleApp, string plongcode, string provinceCode, string cityCode)
        {

            var url = string.Format("" + "/{1}.html", provinceCode, cityCode);
            var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
            if (RestClientUtil.IsSuccess(resp))
            {
                RestClientUtil.FixContentEncoding(resp, encoding);
                var trs = RestClientUtil.RegexGetMatchs(resp.Content, "class='countytr'(.*?)</tr>");
                foreach (Match tr in trs)
                {
                    int sort = 0;
                    short level = 4;
                    var ms = RestClientUtil.RegexGetMatchs(tr.Value, "href='(.*?).html'>(.*?)</a>(.*?)'>(.*?)</a>");
                    foreach (Match m in ms)
                    {
                        save(consoleApp, plongcode, m.Groups[2].Value, m.Groups[4].Value, level, ++sort);
                        var countyCode = m.Groups[1].Value;
                        var plongcode2 = "" + countyCode.Split('/')[1];
                        getowntr(restClient, consoleApp, plongcode2, provinceCode, cityCode, countyCode);
                    }
                }
            }
        }
        /***
        * 乡镇
        * **/
        protected static void getowntr(IRestClient restClient, ConsoleApp consoleApp, string plongcode, string provinceCode, string cityCode, string countyCode)
        {

            var url = string.Format("" + "/{1}/../{2}.html", provinceCode, cityCode, countyCode);
            var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
            if (RestClientUtil.IsSuccess(resp))
            {
                RestClientUtil.FixContentEncoding(resp, encoding);
                var trs = RestClientUtil.RegexGetMatchs(resp.Content, "class='towntr'(.*?)</tr>");
                foreach (Match tr in trs)
                {
                    int sort = 0;
                    short level = 5;
                    var ms = RestClientUtil.RegexGetMatchs(tr.Value, "href='(.*?).html'>(.*?)</a>(.*?)'>(.*?)</a>");
                    foreach (Match m in ms)
                    {
                        save(consoleApp, plongcode, m.Groups[2].Value, m.Groups[4].Value, level, ++sort);
                        var townCode = m.Groups[1].Value;
                        var plongcode2 = "" + townCode.Split('/')[1];
                        gevillagetr(restClient, consoleApp, plongcode2, provinceCode, cityCode, countyCode, townCode);
                    }
                }
            }
        }
        /***
        * 居委会
        * **/
        protected static void gevillagetr(IRestClient restClient, ConsoleApp consoleApp, string plongcode, string provinceCode, string cityCode, string countyCode, string townCode)
        {

            var url = string.Format("" + "/{1}/../{2}/../{3}.html", provinceCode, cityCode, countyCode, townCode);
            var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
            if (RestClientUtil.IsSuccess(resp))
            {
                RestClientUtil.FixContentEncoding(resp, encoding);
                var trs = RestClientUtil.RegexGetMatchs(resp.Content, "class='villagetr'(.*?)</tr>");
                foreach (Match tr in trs)
                {
                    int sort = 0;
                    short level = 6;
                    var ms = RestClientUtil.RegexGetMatchs(tr.Value, "<td>(.*?)</td><td>(.*?)</td><td>(.*?)</td>");

                    foreach (Match m in ms)
                    {
                        save(consoleApp, plongcode, m.Groups[1].Value, m.Groups[3].Value, level, ++sort);
                    }
                }
            }
        }
        protected static void save(ConsoleApp consoleApp, string plongcode, string longcode, string name, short level, int sort)
        {
            string code = longcode; //
            if (level == 3 ) longcode = longcode.Substring(0,4);
            if (level == 4) longcode = longcode.Substring(0, 6);
            if (level == 5) longcode = longcode.Substring(0, 9);

            if (level == 3 || level == 4) code = longcode.Substring(longcode.Length - 1 - 1);
            if (level == 5 || level == 6) code = longcode.Substring(longcode.Length - 1 - 2);

            consoleApp.LeftWriter.WriteLine(string.Format("{0} {1} {2} {3} {4} {5}", plongcode, level, sort, longcode, code, name));
            save(plongcode, longcode,   name,   code,    level,   sort);
        }
        protected static ThreadLocal<IStatelessSession> threadSession = new ThreadLocal<IStatelessSession>(()=>{
            return sessionFactory.OpenStatelessSession();
          });
        protected static void save(string plongcode, string longcode, string name, string code, short level, int sort)
        {
            IStatelessSession s = threadSession.Value;
            ICriteria c = s.CreateCriteria<region>();
            int pid = 0;
            if (pid <= 0&&level>1)
            {
                c = s.CreateCriteria<region>();
                c.Add(Restrictions.Eq("longcode", plongcode.Trim()));
                region pregion = c.SetMaxResults(1).UniqueResult<region>();
                if (pregion != null)
                {
                    pid = pregion.id;
                }
                else
                {
                    ;//
                    return;
                }
            }
            c = s.CreateCriteria<region>();
            c.Add(Restrictions.Eq("longcode", longcode.Trim()));
            region region = c.SetMaxResults(1).UniqueResult<region>();
            if (region == null || region.id <= 0)
            {
                region = new region();
            }
            region.code = code.Trim();
            region.level = level;
            region.longcode = longcode.Trim();
            region.name = name.Trim();
            region.sort = sort;
            region.pid = pid;
            if (region.id > 0)
                s.Update(region);
            else
                s.Insert(region);
        }

    }
    public class region
    {
        public virtual int id { get; set; }
        public virtual int pid { get; set; }
        public virtual string longcode { get; set; }
        public virtual string name { get; set; }
        public virtual string code { get; set; }
        public virtual short level { get; set; }

        public virtual int sort { get; set; }
    }
}
