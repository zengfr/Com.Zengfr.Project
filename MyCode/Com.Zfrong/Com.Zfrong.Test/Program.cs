using System;
using System.Collections;//
using System.Collections.Generic;
using Castle.ActiveRecord.Queries;
using Castle.ActiveRecord.Tests.Model.GenericModel;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Data;//

using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord;//
using Com.Zfrong.Common.Data.AR;//
using IBatisNet.Common;
using IBatisNet.DataAccess;
using IBatisNet.DataMapper;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataMapper.Configuration;
using IBatisNet.DataAccess.Configuration;
//using App.Persistence.MapperDao;
using Test;
using Com.Zfrong.Common.Data.IBatisNet;
namespace Castle.ActiveRecord.Tests.Model.GenericModel
{
    public abstract class Test2ARBase : ActiveRecordBase
    {
        public Test2ARBase()
        {
        }
    }
     class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
         [STAThread]
         static void Main()
         {
             Console.WindowWidth = Console.LargestWindowWidth - 16;
             Console.BufferWidth = Console.WindowWidth;
           
             Com.Zfrong.Common.Http.Async.AsyncHttpRequest r = new Com.Zfrong.Common.Http.Async.AsyncHttpRequest();
             //r.Get("http://bbs.sh.liba.com/member.php","uid=" + userID,"GBK",cookies, null,null,obj);
             r.Beginning += new Com.Zfrong.Common.Http.Async.RequestEventHandler(r_Beginning);
             r.DataComplete += new Com.Zfrong.Common.Http.Async.DataCompleteEventHandler(r_DataComplete);
             r.Get("http://www.baidu.com/s", "wd=美女", "gb2312", null, null, null, null, null, null);
            
             //http://114search.118114.cn/search_web.html?id=3038&kw=123%C3%C0%C5%AE&nid=QukI6wniq3fYqGOxPBx4Tg%3D%3D&st=web&param1=web&param2=
              Console.ReadKey();//
            // STP.DoWork();
             //Test.MAIN.TestICXO();
            // return;
             Test.Form2 f=new Test.Form2();
             f.Start();
             //Com.Zfrong.Common.Common.Http.HttpRequest.TestCSDN();//
             //ibatis();
             return;
           System.Windows.Forms.  Application.EnableVisualStyles();
             System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
             System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;//
            // System.Windows.Forms.Application.Run(new Test.Form2());//
                            //    Com.Zfrong.Common.Common.PostHelper.TestQQGroup2();//
            // Com.Zfrong.Common.Common.PostHelper.TestQQGroupEmail();//
            // Com.Zfrong.Common.Http.Common..PostHelper.TestCSDN();//
             //Com.Zfrong.Common.Common.PostHelper.TestTianya();//
            // Com.Zfrong.Common.Common.PostHelper.TestMop();//
             //Com.Zfrong.Common.Common.PostHelper.TestZhidao();//
             //Start();
             //Test.Test.Start();//
         }

         static void r_Beginning(object sender, System.Net.HttpWebRequest req)
         {
             req.AllowAutoRedirect = false;
         }

         static void r_DataComplete(object sender, Com.Zfrong.Common.Http.Async.DataCompleteEventArgs e)
         {
             Console.WriteLine(e.RecievedData);
             foreach (string k in e.RecievedHeaders)
             {
                 Console.WriteLine(string.Format("{0}\t{1}",k,e.RecievedHeaders[k]));
             }
         }
         static void ibatis()
         {
             DomSqlMapBuilder mapBuilder = new DomSqlMapBuilder();
            // string fileName = "sqlmap" + "_" + ConfigurationManager.AppSettings["database"] + "_" + ConfigurationManager.AppSettings["providerType"] + ".config";
            ISqlMapper sqlMap = mapBuilder.Configure();//fileName); 
          //IList<QQGroup> objs=sqlMap.QueryForList<QQGroup>("ComplexMap", 300,0,10);//
            
            DomDaoManagerBuilder daoBuilder = new DomDaoManagerBuilder();
            daoBuilder.Configure();//"dao" + "_" + ConfigurationManager.AppSettings["database"] + "_"+ ConfigurationManager.AppSettings["providerType"] + ".config"); 
           IDaoManager daoManager = DaoManager.GetInstance("SqlMapDao");
           IDao dao = daoManager.GetDao(typeof(IDao)) as  IDao ;
          // IQQGroupSqlMapDao dao2 = daoManager.GetDao(typeof(IQQGroupSqlMapDao)) as IQQGroupSqlMapDao;
           //IList<QQGroup> objs2 = dao2.ExecutePaginatedForList("ComplexMap", 1000, 10, 10);//
           //objs2 = dao2.ExecutePaginatedForList("ComplexMap", 1000, 10, 10,"IDID desc");//
           //DataTable dt = dao2.ExecutePaginatedQueryForDataTable("ComplexMap", 1000, 10, 10, "IDID desc");//
         }
  //        static void Start()
  //      { 
  //            log4net.Config.XmlConfigurator.Configure();
  //          ActiveRecordStarter.ResetInitializationFlag();

  //          ActiveRecordStarter.Initialize(GetConfigSource(),
  //              typeof(Blog),
  //              typeof(Post),typeof(Category));
  //          //ActiveRecordStarter.CreateSchema();//
  //          //SlicedOperation20000();//
  //          SlicedOperation();

  //          string sql = "select * from posttable where id<1000";//
  //          string hql = "from Post p Where p.Blog.Id < 100";//
  //           IDictionary<string,object> dict=new Dictionary<string, object>();//
  //          dict.Add("@Page",10);//
  //dict.Add("@PageSize",10);//
  //dict.Add("@Table","posttable");//
  //dict.Add("@Field","*");//
  //dict.Add("@OrderBy","ID DESC");//
  //dict.Add("@Filter","1=1");//
  //dict.Add("@MaxPage",10);//
  //dict.Add("@TotalRow",10);//
  //dict.Add("@Descript",10);//
  //          Order[] orders=new Order[]{Order.Desc("Id")};//
  //          Post[] post = Post.SlicedFindAll(30000, 100, orders, Expression.Like("Contents", "contents"));//

  //          IList list ;//= ARHelper.Execute(sql);//
  //          //DataSet ds=ARHelper.ExecuteDS(sql);//
  //         list=ARHelper.ExecuteSP("Pagination",dict);//
           
  //        DataTable dt = ARHelper.ExecuteForDT(sql);//
  //        dt = Com.Zfrong.Common.Data.NH.NHHelper.ExecuteForDT(sql);//
  //        dt = Com.Zfrong.Common.Data.NH.NHHelper.ExecuteForDT(sql);//
         
  //         list = ARHelper.ExecPagedQuery(hql, 1000, 100);//
  //         IList<Post> list4 = ARHelper.ExecPagedQuery<Post>(hql, 1000, 100);//
  //         list = ARHelper.ExecQuery(hql);//
  //         //list3 = ARHelper.ExecSQLPagedQuery(sql, 1000, 100);//
  //        // list4 = ARHelper.ExecSQLPagedQuery<Post>(sql, 1000, 100);//
  //         post = Post.SlicedFindAll(50000,40,Expression.Eq("Title","title1"));//
  //         using (new SessionScope())
  //         {
  //             Blog[] bs = Blog.SlicedFindAll(100, 40,orders, Expression.Gt("Id", 10000));//
  //             int i = 0;//Com.Zfrong.Common.Data.HelperBase.
  //             foreach (Blog b in bs)
  //                 i = b.Posts.Count;//
  //             //post = Post.FindAll();//
  //             //post = Post.FindAll();//
  //         }
  //      }
        //public static void SlicedOperation()
        //{
        //    Blog blog = new Blog();
        //    blog.Name = "hammett's blog";
        //    blog.Author = "hamilton verissimo";
        //    blog.Save();
        //    for (int i = 0; i < 200; i++)
        //    {
        //        Post post1 = new Post(blog, "title1", "contents", "category1");
        //        Post post2 = new Post(blog, "title2", "contents", "category2");

        //        Category category1 = new Category();
        //        category1.Name = "Category1";//

        //        Category category2 = new Category();
        //        category2.Name = "Category1";//
        //        category2.Parent = category1;//

        //        Category category3 = new Category();
        //        category3.Name = "Category1";//
        //        category3.Parent = category1;//

        //        category1.Save();//            
        //        category2.Save();//
        //        category3.Save();//

        //        post1.Categories.Add(category1);//
        //        post1.Categories.Add(category2);//
        //        post1.Categories.Add(category3);//
        //        post1.Save();//
        //        //post1.SaveAndFlush();//

        //        post2.Save();
        //    }
        //    Console.WriteLine(blog.Id);//
        //}

        public static void SlicedOperation20000()
        {
            //for (int i = 0; i < 20000; i++)
                //SlicedOperation();
        }
        protected static IConfigurationSource GetConfigSource()
        {
            return System.Configuration.ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
        }
    }
}