using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amib.Threading;
using UniTask_UI.common;
using UniTask_UI.p1.c;

namespace UniTask_UI.p1
{
    [SerializableAttribute]
    public class MyData
    {

        public static MyData Instance = new MyData();
        public GoalSetting goalSetting = new GoalSetting();
        public Setting setting = new Setting();

        public IList<Account> accounts = new List<Account>();//账号
        public IList<Customer> customers = new List<Customer>();
        public Dictionary<String, ProductDetail> products = new Dictionary<String, ProductDetail>();//可以行程
        public Dictionary<String, PriceItem> prices = new Dictionary<String, PriceItem>();//价格
        public IList<Order> orders = new List<Order>();//订单
        public IList<Head> heads = new List<Head>();//heads
        public IList<UserAgent> userAgents = new List<UserAgent>();// 
        public IList<Proxy> proxys = new List<Proxy>();// 

        public void init()
        {
            MyData d = this;
            if (d.goalSetting == null)
                d.goalSetting = new GoalSetting();
            if (d.prices == null)
                d.prices = new Dictionary<string, PriceItem>();
            if (d.accounts == null)
                d.accounts = new List<Account>();
            if (d.customers == null)
                d.customers = new List<Customer>();
            if (d.setting == null)
                d.setting = new Setting();
            if (d.heads == null)
                d.heads = new List<Head>();
            if (d.proxys == null)
                d.proxys = new List<Proxy>();
            if (d.userAgents == null)
                d.userAgents = new List<UserAgent>();
        }
    }
    [SerializableAttribute]
    public enum Typee 
    {
       
        t0= 0,
        t1=1,
        t2 = 2,
        t3 = 3,
        t4 = 4,
        t5 = 5,
        t6 = 6,
        t7 = 7,
    }

        [SerializableAttribute]
    public abstract class EntityBase
    {
        [Category("基本")]
        [DisplayName("顺序")]
        [Description("数值越大，越靠前")]
        public ushort Sort { get; set; }
        [Category("基本")][DisplayName("启用")][Description("false=无效;true=有效")]
        public bool Enabled { get; set; }
        [Category("基本")]
        [DisplayName("最后更新")][ReadOnlyAttribute(true)]
        public DateTime LastUpdate { get; set; }
        [Category("基本")]
        [DisplayName("最后使用")][ReadOnlyAttribute(true)]
        public DateTime LastUse { get; set; }

        public EntityBase()
        {
            Sort = 10;
            Enabled = true;
            LastUse = DateTime.Now;
            LastUpdate = DateTime.Now;
        }
    }
    [SerializableAttribute]
    public class Proxy : EntityBase
    {
        public ushort Type { get; set; }
        public String Ip { get; set; }
        [DisplayName("端口")]
        public int Port { get; set; }
    }
    [SerializableAttribute]
    public class UserAgent : EntityBase
    {
        public ushort Type { get; set; }
        [EditorAttribute(typeof(PropertyGridRichText), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("浏览器标识")]
        public String Name { get; set; }
    }
    [SerializableAttribute]
    public class Head : EntityBase
    {
        public ushort Type { get; set; }
        [DisplayName("键")]
        public String Key { get; set; }
        [DisplayName("值")]
        [EditorAttribute(typeof(PropertyGridRichText), typeof(System.Drawing.Design.UITypeEditor))]
        public String Value { get; set; }
    }
    [SerializableAttribute]
    public class GoalSetting : EntityBase
    {
        public DateTime regDate = DateTime.Now;
        public int totalOrders = 0;

        public bool init = false;
        public int initThread = 2;
        public WorkItemPriority priority = WorkItemPriority.AboveNormal;
        public int wait = 40;
        public int wait4e = 3012;//error
        public int wait4p = 3012;//price
        public int wait4q = 3012;//quant
        public int wait4s = 3012;//status
        public int wait4o = 3012;//order

        public bool isStart = false;
        public bool isStart4p = false;
        public bool isStart4q = false;
        public bool isStart4s = false;
        public bool isStart4padd = false;
        public bool isStart4o = false;
    }
    [SerializableAttribute]
    public class Setting : EntityBase
    {
        public String line = "";
        public String lineCode = "";
        public String start = "";
        public String end = "";
    }
    [SerializableAttribute]
    public class Customer : EntityBase
    {
        [DisplayName("分组")][Description("0=每个人都独立成一组；其他=按组号组成一组")]
        public ushort Group { get; set; }
       
        [DisplayName("姓名")]
        public String Name { get; set; }
        [DisplayName("手机")]
        public String Tel { get; set; }
        [DisplayName("证件类型")]
        [Description("0=身份证，1=护照，2=港澳通行证")]
        public String IdType { get; set; }
        [DisplayName("证件号码")]
        public String IdCardNum { get; set; }
        [DisplayName("客户类型")]
        [Description("0=成人，1=儿童，2=老人")]
        public ushort Type { get; set; }
        [DisplayName("性别")]
        [Description("0=男，1=女")]
        public byte Sex { get; set; }
        [Editor(typeof(DatePickerEditor), typeof(UITypeEditor))]
        [DisplayName("预定日期")]
        [Description("不指定:请任意选择1个今天以前日期。指定:请选择指定日期。")]
        public DateTime BookDate { get; set; }
        public Customer()
        {
            BookDate = DateTime.Today.AddMonths(-3);
            Name = Tel = IdType = IdCardNum = "";
        }
    }
    [SerializableAttribute]
    public class Account : EntityBase
    {
        [DisplayName("用户名")]
        public String Name { get; set; }
        [DisplayName("邮箱")]
        public String Email { get; set; }
        [DisplayName("手机")]
        public String Tel { get; set; }  
        [DisplayName("密码")]
        public String Pwd { get; set; }
        [DisplayName("类型")]
        [Description("0=通用，1=刷价，2=刷量，3=刷单")]  
        public ushort Type { get; set; }
        [ReadOnlyAttribute(true)]
        [DisplayName("使用次数")]
        public int Use { get; set; }
        [ReadOnlyAttribute(true)]
        [DisplayName("状态")]
        public string Status { get; set; }
        [EditorAttribute(typeof(PropertyGridRichText), typeof(System.Drawing.Design.UITypeEditor))] 
        public String Cookies { get; set; }
        [EditorAttribute(typeof(PropertyGridRichText), typeof(System.Drawing.Design.UITypeEditor))]
        public String Authorization { get; set; }

        public Account()
        {
            Name = Email = Tel = Pwd = Cookies =Authorization=Status= "";
        }
    }
    [SerializableAttribute]
    public class Order : EntityBase
    {
        [ReadOnlyAttribute(true)]
        [DisplayName("订单号")]
        public String OrderNum { get; set; }
        [ReadOnlyAttribute(true)]
        [DisplayName("客户名称")]
        public String CustomerName { get; set; }
        [ReadOnlyAttribute(true)]
        [DisplayName("登录账号")]
        public String AccountName { get; set; }
        [EditorAttribute(typeof(PropertyGridRichText), typeof(System.Drawing.Design.UITypeEditor))]
        [DisplayName("备注说明")]
        public String Comment { get; set; }
    }
    [SerializableAttribute]
    public class Product : EntityBase
    {
        public Product()
        {

        }



    }
    [SerializableAttribute]
    public class ProductDetail : Product
    {
        public ProductDetail()
        {
            feeType = "";
            bookDate = "";
            beginTime = "";
            lineCode = "";
        }
        /// <summary>
        /// 珠海->香港
        /// </summary>
        [DisplayName("路线")]
        public string lineName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("预订日期")]
        public string bookDate { get; set; }
        /// <summary>
        /// 
        /// </summary>  
        [DisplayName("可订数")]
        public int maxPeople { get; set; }
        [DisplayName("价格1")]
        public int price1 { get; set; }
        [DisplayName("价格2")]
        public int price2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string beginTime { get; set; }
      

        public string lineCode { get; set; }
       
        /// <summary>
        /// 
        /// </summary>
        public string maxBookDate { get; set; }
    

        /// <summary>
        /// 
        /// </summary>
        //public string remarks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int saleStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string busStatus { get; set; }

        public DateTime lastUpate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ticketCategory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalPeople { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int checkdedCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string endTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string feeType { get; set; }
    }

}