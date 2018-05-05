using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;
using NHibernate;
using NHibernate.Criterion;
using NUnit.Framework;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Com.Zfrong.Common.Data.AR.Base;
using Castle.ActiveRecord.Framework.Internal;

using Com.Zfrong.Common.Data.AR.Entity;
using System.Net;
using System.IO;
 public class ABC { }
namespace Test
{

    public class TestMAIN
    {
        private static string Chars = "0123456789abcdefghijklmnopqrstuvwxyz";
        private static string GetPassWord(int i,ref int len)
        { 
            List<int> index=new List<int>();
            len = 0;
            int m = i / 36;
            while (m >0)
            {
               len++;
               m = m / 36;
            }
            int n = 0;
            for (int j = len; j >= 1&&i>=0; j--)
            {
                n = i / (int)Math.Pow(36, j);
                index.Add(n);
                i = i - n*(int)Math.Pow(36, j);
            }
            index.Add(i%36);//

            string obj = "";
            foreach(int k in index)
                obj+=Chars[k];
            Console.WriteLine(obj);//
            return obj;
        }
        private static void postrequest(string password)
        {
            string userName = "root";
            //string password = "admin1";
            CookieContainer webCookieContainer = new CookieContainer();
            String url = "http://192.168.1.1/userRpm/index.htm";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0;CIBA)";
            req.CookieContainer = webCookieContainer;
            req.KeepAlive = true;
            req.Accept = "*/*";
            req.PreAuthenticate = true;
            CredentialCache myCache = new CredentialCache();
            myCache.Add(new Uri(url), "Basic", new NetworkCredential(userName, password));//添加Basic认证
            req.Credentials = myCache;
            HttpWebResponse resp; StreamReader sr;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
                resp.Cookies = webCookieContainer.GetCookies(req.RequestUri);
                sr = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.Default);
                String line = sr.ReadToEnd();//这里就是网页内容了。
                sr.Close();
                resp.Close();
                Console.WriteLine(line);//
            }
            catch(Exception ex) { Console.WriteLine(password+":"+ex.Message);}
            finally
            {
                
            }
        }
        public static void Test2()
        {
           
            Type t=typeof(Category);
            //IList<string> strs;

            //strs = PropertyHelper.GetDisplayNames(t, false, null);//
            //strs = PropertyHelper.GetPropertiesName(t, false, null);//
            //strs = PropertyHelper.GetPropertyTypes(t, false);//

            //strs = PropertyHelper.GetDisplayNames(t, true, null);//
            //strs = PropertyHelper.GetPropertiesName(t, true, null);//
            //strs = PropertyHelper.GetPropertyTypes(t, true);//

            IList<PropertySchema> list;
            list =EntityHelper.GetPropertiesSchema(t);//
            list = EntityHelper.GetPropertiesSchema(t, true);//
            //list[0].ToPropertyType(1);//
        }
      
        public static void Start()
        {
            string s; int len1 = 0, len2 = 0;
            System.IO.StreamWriter sw = new StreamWriter("d:\\dict.txt", true, Encoding.UTF8);

            for (int i = 83194618; i < 2000000000; i++)
            {
                len1 = len2;
                s = GetPassWord(i, ref len2);
                if (len2 != len1)
                {
                    sw.Flush();
                    sw.Close();
                    sw = new StreamWriter("d:\\dict" + len2 + ".txt", true, Encoding.UTF8);
                }
                sw.WriteLine(s);//postrequest(s);
            }
            sw.Close();
            Type t = TypeCreator.Creator("ABC", 3);//
            
            //string name = t.AssemblyQualifiedName;
            ////NHibernate.Util.ReflectHelper.ClassForName();//
            //NHibernate.Util.AssemblyQualifiedTypeName parsedName = NHibernate.Util.TypeNameParser.Parse(name);

            //System.Type typeqq = System.Type.GetType(parsedName.ToString());
            //System.Reflection.Assembly assemblyqq = System.Reflection.Assembly.Load(parsedName.Assembly);

            //System.Type result = NHibernate.Util.ReflectHelper.TypeFromAssembly(parsedName, true);

            Test2();//

            log4net.Config.XmlConfigurator.Configure();
            ActiveRecordStarter.ResetInitializationFlag(); 
            ActiveRecordStarter.ModelsCreated += new ModelsDelegate(ActiveRecordStarter_ModelsCreated);
            //ActiveRecordStarter.
            ActiveRecordStarter.Initialize(System.Reflection.Assembly.GetAssembly(typeof(User)), GetConfigSource()
             );//  ,t);
          
           ActiveRecordStarter.DropSchema();
           ActiveRecordStarter.CreateSchema();//
            SlicedOperation2();//

            Order[] orders = new Order[] { Order.Desc("Id") };//
            Post[] post = DB<Post>.SlicedFindAll(30000, 100, orders, Expression.Like("Contents", "contents"));//
            

        }

        static void ActiveRecordStarter_ModelsCreated(ActiveRecordModelCollection models, IConfigurationSource source)
        {
            //new ActiveRecordModelBuilder().Create(TypeCreator.Creator("abcdeefg", 3));//
            foreach (Castle.ActiveRecord.Framework.Internal.ActiveRecordModel model in models)
            {
                if (model.Type.Name == "Blog")
                {
                    model.ActiveRecordAtt.Table = "Blog";
                    //Type t;
                   // model.Properties
                }
            }

        }
        
        protected static IConfigurationSource GetConfigSource()
        {
            return System.Configuration.ConfigurationManager.GetSection("activerecord") as IConfigurationSource;
        }
        public static void SlicedOperation()
        {
            Blog blog =DB<Blog>.Find(1);
            if (blog == null)
            {
                blog = new Blog();
                blog.Name = "hammett's blog";
                blog.Author = "hamilton verissimo";
                DB<Blog>.Save(blog);//
            }
             for (int i = 0; i < 200; i++)
             {
                Post post1 = new Post(blog, "title1", "contents", "category1");
                Post post2 = new Post(blog, "title2", "contents", "category2");

                Category category1 = new Category();
                category1.Name = "Category1";//

                Category category2 = new Category();
                category2.Name = "Category1";//
                category2.Parent = category1;//

                Category category3 = new Category();
                category3.Name = "Category1";//
                category3.Parent = category1;//

                DB<Category>.Save(category1);//            
                DB<Category>.Save(category2);//     
                DB<Category>.Save(category3);//     

                post1.Categories.Add(category1);//
                post1.Categories.Add(category2);//
                post1.Categories.Add(category3);//
                DB<Post>.Save(post1);//
                DB<Post>.SaveAndFlush(post1);//

                DB < Post>.Save(post2);
            }
            Console.WriteLine(blog.Id);//
        }
        public static void SlicedOperation2()
        {
          IList<User> Users = Create<User>();
          IList<Function> Functions = Create<Function>();
          IList<UserRole> UserRoles = Create<UserRole>();
          IList<UserGroup> UserGroups = Create<UserGroup>();
          IList<Department> Departments = Create<Department>();

          Users[0].Functions = Functions;
          Users[0].Departments = Departments;
          Users[0].Friends = Users;
          Users[0].UserGroups = UserGroups;
          Users[0].UserRoles = UserRoles;

          SaveFunctions<User>(Users, Functions);

          SaveFunctions<Department>(Users[0].Departments, Functions);
          SaveFunctions<UserGroup>(Users[0].UserGroups, Functions);
          SaveFunctions<UserRole>(Users[0].UserRoles, Functions);
        }
        private static void SaveFunctions<T>(IList<T> listT, IList<Function> fs) where T :class, IHasFunctions
        {
            foreach(T t in listT)
            {
                t.Functions = fs;
                DB<T>.Save(t);//
            }
        }
        public static IList<T> Create<T>() where T:class,new()
        {
            IList < T > list= new List<T>();
            for (int i = 0; i < 200; i++)
            {
                T f = new T(); 
                DB<T>.Create(f);//
                list.Add(f);
            }
            return list;
        }
    }
    [ActiveRecord("Blog", Lazy = true)]
    public class Blog
    {
        private int _id;
       // [Field]
        private   String _name;
        private String _author;
        private IList<Post> _posts = new List<Post>();
        private IList<Post> _publishedposts;
        private IList<Post> _unpublishedposts;
        private IList<Post> _recentposts;

        [PrimaryKey][Display]//[HasManyToAny(,)]
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Property]
        public virtual String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [Property]
        public virtual String Author
        {
            get { return _author; }
            set { _author = value; }
        }

        [HasMany(Table = "Post", Index = "Post_BlogID", IndexType = "int", ColumnKey = "blogid", Lazy = true)] //[Property(Index = "Blog-PostID")]
        [Display]
        public virtual IList<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }

        [HasMany(typeof(Post), Table = "Post", ColumnKey = "blogid", Where = "published = 1", Lazy = true)]
        public virtual IList<Post> PublishedPosts
        {
            get { return _publishedposts; }
            set { _publishedposts = value; }
        }

        [HasMany(typeof(Post), Table = "Post", ColumnKey = "blogid", Where = "published = 0", Lazy = true)]
        public virtual IList<Post> UnPublishedPosts
        {
            get { return _unpublishedposts; }
            set { _unpublishedposts = value; }
        }

        [HasMany(typeof(Post), Table = "Post", ColumnKey = "blogid", OrderBy = "created desc", Lazy = true)]
        public virtual IList<Post> RecentPosts
        {
            get { return _recentposts; }
            set { _recentposts = value; }
        }
    }
    [ActiveRecord("Post", Lazy = true)]
    public class Post 
    {
        private int _id;
        private String _title;
        private String _contents;
        private String _category;
        private DateTime _created=DateTime.Now;
        private bool _published;
        private Blog _blog;
        private IList<Category> _categories;//
        public Post()
        {
            _categories = new List<Category>();//
        }

        public Post(Blog blog, String title, String contents, String category)
        {
            _blog = blog;
            _title = title;
            _contents = contents;
            _category = category; _categories = new List<Category>();//
        }

        [PrimaryKey]
        [Display]
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Property]
        public virtual String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [Property(ColumnType = "StringClob")]
        public virtual String Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        [Property]
        public virtual String Category
        {
            get { return _category; }
            set { _category = value; }
        }

        [BelongsTo("blogid")]// [Property(Index = "Post-BlogID")]
        [Display]
        public virtual Blog Blog
        {
            get { return _blog; }
            set { _blog = value; }
        }

        [Property("created")]
        public virtual DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        [Property("published")]
        public virtual bool Published
        {
            get { return _published; }
            set { _published = value; }
        }
        [Display]
        [HasAndBelongsToMany(typeof(Category), Index = "Post_CategoryID", IndexType = "int", RelationType = RelationType.Bag, Table = "Post_Category", ColumnKey = "CategoryID", ColumnRef = "PostID")]
        //[Property(Index = "Post_CategoryID")]
        public virtual IList<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }
       
    }
    [ActiveRecord("Category", Lazy = true)]
    public class Category 
    {
        private int _id;
        private String _name;
        private IList<Post> _posts;
        private Category _parent;//
       protected const string ColumnName = "zfr_";
        public Category()
        {
            _posts = new List<Post>();
        }
        public Category(string name)
        {
            _name = name;//
            _posts = new List<Post>();
        }
        [PrimaryKey(PrimaryKeyType.Identity)]//Assigned)]
        [Display]
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Property(ColumnName + "Name")]
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        [BelongsTo("ParentId")]//[Property(Index = "Category-ParentID")]
        [Display]
        public virtual Category Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }
        [HasAndBelongsToMany(typeof(Post), Index = "Post_Category_PostID", IndexType = "int", Lazy = true, RelationType = RelationType.Bag, Table = "Post_Category", ColumnRef = "CategoryID", ColumnKey = "PostID")]
        public virtual IList<Post> Posts
        {
            get { return _posts; }
            set { _posts = value; }
        }
      
    }


}
