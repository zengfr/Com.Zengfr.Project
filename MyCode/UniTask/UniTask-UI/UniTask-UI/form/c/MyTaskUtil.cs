using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Newtonsoft.Json;
using RestSharp;
using TaobaoTask;
using UniTask_UI.common;
using UniTask_UI.form.c;

namespace UniTask_UI.p1.c
{
    /**
       * 刷价 只添加
       * 
       * **/
    public class MyTaskThread4padd : MyTaskThread4Pool
    {

        protected override bool enabled()
        {
            return true || goalSetting.isStart && (goalSetting.isStart4padd);
        }

        protected override void runTask(MyThreadPool pool)
        {
            pool.init(goalSetting.initThread);

            MyData d = MyData.Instance;
            var accounts = new List<Account>(d.accounts.Where(t => t.Enabled && t.Cookies != null && (t.Type == 0 || t.Type == 1)));
            int s = accounts.Count;
            if (s > 0)
            {
                DateTime start; DateTime end;
                DateTime.TryParse(d.setting.start, out start);
                DateTime.TryParse(d.setting.end, out end);

                var tt = (end - start).TotalDays;

                for (int i = 0; i <= tt; i++)
                {
                    var enable = enabled();
                    if (!enable)
                    {
                        break;
                    }
                    Application.DoEvents();
                    runTask(pool, accounts, start.AddDays(i));
                }
            }

            else
            {
                InfoFormat(String.Format("有效账号个数:{0}", 0));
            }
        }
        int index = 0;
        private void runTask(MyThreadPool pool, List<Account> accounts, DateTime date)
        {

            MyData d = MyData.Instance;
            int s = accounts.Count;
            var thread = goalSetting.initThread > s ? s : goalSetting.initThread;
            var smart = pool.smart;
            if (smart.InUseThreads < thread || smart.CurrentWorkItemsCount < thread)
            {
                Interlocked.Increment(ref index);
                index = (index % s);
                var account = accounts[index];
                pool.addTask(goalSetting.priority, runTask, account, date, d.setting.lineCode);
            }
        }
        protected void runTask(Account account, DateTime date, String lineCode)
        {
            MyTaskUtil.Instance.req_price(account, date, lineCode);
        }


    }
    /**
     * 按列表刷 基类
     * 
     * **/
    public abstract class MyTaskThread4list : MyTaskThread4Pool
    {
        protected override bool enabled()
        {
            return goalSetting.isStart && (goalSetting.isStart4p);
        }

        protected override void runTask(MyThreadPool pool)
        {
            MyData d = MyData.Instance;
            var accounts = new List<Account>(getAccounts(d));
            int s = accounts.Count;
            if (s > 0)
            {
                var prodGroupd = d.prices.Values.GroupBy(t => t.lineCode).ToList();

                foreach (var prods in prodGroupd)
                {
                    var enable = enabled();
                    if (!enable)
                    {
                        break;
                    }
                    Application.DoEvents();
                    var bookDates = prods.Select(t => t.buyDate).Distinct().ToList();
                    foreach (var bookDate in bookDates)
                    {
                        enable = enabled();
                        if (!enable)
                        {
                            break;
                        }
                        Application.DoEvents();
                        //var date = DateTime.Parse(bookDate);
                        runTask(pool, accounts, bookDate, prods.Key);
                    }
                }
                if (prodGroupd.Count == 0)
                {
                    InfoFormat(String.Format("价格:有效数据{0}条", 0));
                }
            }
            else
            {
                InfoFormat(String.Format("账号:有效{0}个", 0));
            }
        }
        int index = 0;
        protected void runTask(MyThreadPool pool, List<Account> accounts, DateTime date, String lineCode)
        {
            int sleep = getSleep();
            if (sleep > 0)
            {
                DebugFormat("sleep:{0}", sleep);
                Thread.Sleep(sleep);
            }
            MyData d = MyData.Instance;
            int s = accounts.Count;
            var thread = goalSetting.initThread > s + 1 ? s + 1 : goalSetting.initThread;
            var smart = pool.smart;
            if (smart.InUseThreads <= thread || smart.CurrentWorkItemsCount <= thread)
            {
                Interlocked.Increment(ref index);
                index = (index % s);
                var account = accounts[index];
                if (account.Status == null || account.Status.Length == 0 || !account.Status.Equals("执行中"))
                {
                    account.Status = "等待执行";
                    pool.addTask(goalSetting.priority, runTask, account, date, d.setting.lineCode);
                }

            }

        }
        protected abstract int getSleep();
        protected abstract void runTask(Account account, DateTime date, String lineCode);
        protected abstract IEnumerable<Account> getAccounts(MyData d);
    }
    /**
     * 刷价 按列表
     * 
     * **/
    public class MyTaskThread4p : MyTaskThread4list
    {

        protected override IEnumerable<Account> getAccounts(MyData d)
        {
            return d.accounts.Where(t => t.Enabled && t.Cookies != null && (t.Type == 0 || t.Type == 1));
        }

        protected override int getSleep()
        {
            return 60;
        }

        protected override void runInit()
        {
            //MyData d = MyData.Instance;
            //d.prices.Clear();
            base.runInit();
        }
        protected override void runTask(Account account, DateTime date, String lineCode)
        {
            Application.DoEvents();
            account.Status = "执行中";
            MyTaskUtil.Instance.req_price(account, date, lineCode);
            account.Status = "执行完成";
        }
    }
    /**
    * 刷量 立即下单
    * 
    * **/

    public class MyTaskThread4q : MyTaskThread4list
    {
        protected override IEnumerable<Account> getAccounts(MyData d)
        {
            return d.accounts.Where(t => t.Enabled && t.Cookies != null && (t.Type == 0 || t.Type == 2));
        }
        protected override bool enabled()
        {
            return goalSetting.isStart && (goalSetting.isStart4q);
        }

        protected override int getSleep()
        {
            MyData d = MyData.Instance;
            var aCount = d.accounts.Count(t => t.Enabled && t.Cookies != null && (t.Type == 0 || t.Type == 2));

            var s = getSleep2()/ aCount ;

            return s > 100 ? s : s;
        }
        protected int getSleep2()
        {
            MyData d = MyData.Instance;
            var s = d.goalSetting.wait4q ;

            return s;
        }
        protected override void runTask(Account account, DateTime date, string lineCode)
        {
            Application.DoEvents();
            account.Status = "执行中";
            var sleepMs = getSleep2();
            var now = DateTime.Now;
            var m = (now - account.LastUse).TotalMilliseconds;
            var t = sleepMs - m;
            LoggerUtil.Debug("{0:d06} {1} {2} {3} {4:.000} {5:.000}", account.Use, sleepMs, now.ToString("ss.ffff"), account.LastUse.ToString("ss.ffff"), m, t);

            if (t < 0)
            {
                MyData d = MyData.Instance;
                var gs = d.goalSetting;
                InfoFormat(String.Format("开始:{0} {1} {2} 账号:{3}",lineCode, date, account.Cookies.Length, account.Name));
                if (gs.isStart4q)
                {
                    string accept = "application/json,text/plain,text/html,application/xhtml+xml,application/xml,*/*";
                    RestSharp.IRestClient restClient = MyRestUtil.GetRestClient(account, accept, true);
                    RestClientUtil.randomUserAgent(restClient);
                    RestClientUtil.SetCookies(restClient, account.Cookies);

                    MyBookingReq req2 = new MyBookingReq();
                    req2.bookDate = date.ToString("yyyy-MM-dd");
                    req2.lineCode = lineCode;
                    IRestResponse resp2 = null;

                    resp2 = MyRestUtil.getbooking(restClient, req2, account, sleepMs);
                    if (resp2 != null && resp2.IsSuccessful)
                    {
                        MyBookingResp bookingResp = RestClientUtil.DeserializeObject<MyBookingResp>(resp2.Content);
                        if (bookingResp != null)
                        {
                            if (bookingResp.responseData != null)
                            {
                                foreach (ProductDetail r in bookingResp.responseData)
                                {
                                    if (r.bookDate == null || r.bookDate.Length < 1)
                                    {
                                        r.bookDate = req2.bookDate;
                                    }
                                    var k = String.Format("{0},{1},{2}", r.lineCode, r.bookDate, r.beginTime);
                                    d.products[k] = r;

                                    var k2 = String.Format("{0},{1},{2}", r.lineCode, r.bookDate, "" + r.feeType);
                                    var hasPrice = d.prices.ContainsKey(k2);
                                    if (hasPrice)
                                    {
                                        if (d.prices[k2].adultCNY > 0)
                                            r.price1 = d.prices[k2].adultCNY;
                                        if (d.prices[k2].childrenCNY > 0)
                                            r.price2 = d.prices[k2].childrenCNY;
                                        InfoFormat(String.Format("成功:{0} {1} 可订数:{2} 账号:{3}", k2, r.beginTime, r.maxPeople, account.Name));
                                        if (r.maxPeople > 0)
                                        {
                                            InfoFormat(String.Format("可预订赶紧抢单:{0} 可订数:{1}  账号:{2} 自动抢:{3}", k, r.maxPeople, account.Name, d.goalSetting.isStart4o));
                                            if (d.goalSetting.isStart4o)
                                            {
                                                MyTaskUtil.Instance.booking_quick(r);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        WarnFormat(String.Format("无价格或不可订:{0} {1} 可订数:{2}  账号:{3}", k2, r.beginTime, r.maxPeople, account.Name));
                                    }

                                }
                            }
                            if (bookingResp.code == null || bookingResp.code.StartsWith("4") || bookingResp.code.StartsWith("5"))
                            {
                                InfoFormat(String.Format("{0} {1} 账号:{2} 消息:{3}", d.setting.line, req2.bookDate, account.Name, bookingResp == null ? "返回异常" : bookingResp.message));
                                if (bookingResp.code == null)

                                {
                                }
                                else if (bookingResp.message.Contains("操作频繁"))
                                {
                                    InfoFormat(String.Format("操作频繁 账号:{0}", account.Name));

                                }
                                else if (bookingResp.message.Contains("請使用公眾號") || bookingResp.message.Contains("重新打開"))
                                {
                                    InfoFormat(String.Format("cookies过期 账号:{0}", account.Name));


                                }

                            }
                        }
                    }
                }
                else
                {
                    InfoFormat("enable....{0}", gs.isStart4q);
                }
            }
            account.Status = "执行完成";
        }


    }


    public class MyTaskUtil : MyBase
    {
        public static MyTaskUtil Instance = new MyTaskUtil();
        private object lockObj = new object();
        private bool isinit = false;
        public MyTaskUtil()
        {

        }
        private void init()
        {
            if (isinit)
                return;
            MyTaskThread4p t4p = new MyTaskThread4p();
            t4p.Name = "刷价";
            t4p.init();

            MyTaskThread4q t4q = new MyTaskThread4q();
            t4q.Name = "刷量";
            t4q.init();

            isinit = true;
            LoggerUtil.Info("smart engine initialized...");

            try
            {
                var thread3 = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                {
                    LoggerUtil.Info("env checking......");
                    var rtn = RestClientUtil.execCmd("pip install ddddocr", true);
                    if (!rtn.Contains("already"))
                    {
                        LoggerUtil.Error("env error");
                    }
                    else if (!rtn.Contains("ddddocr"))
                    {
                        LoggerUtil.Error("env error:" + rtn);
                    }
                    else
                    {
                        LoggerUtil.Info("env success");
                    }
                    var imgDir = "imgs";
                    if (!Directory.Exists(imgDir))
                    {
                        Directory.CreateDirectory(imgDir);
                    }
                    else
                    {
                        var fs = Directory.GetFiles(imgDir);
                        foreach (var f in fs)
                        {
                            File.Delete(f);
                        }
                    }
                }));
                thread3.IsBackground = true;
                thread3.Start();
            }
            catch (Exception ex)
            {
                LoggerUtil.Error("env", ex);
            }
        }
        public void req_price(Account account, DateTime date, String lineCode)
        {
            MyData d = MyData.Instance;
            var gs = d.goalSetting;

            string accept = "application/json, text/plain, */*";
            RestSharp.IRestClient restClient = MyRestUtil.GetRestClient(account, accept, true);
            RestClientUtil.randomUserAgent(restClient);
            RestClientUtil.SetCookies(restClient, account.Cookies);

            MyPriceReq req = new MyPriceReq();
            req.buyDate = date.ToString("yyyy-MM-dd");
            req.lineCode = d.setting.lineCode;

            IRestResponse resp = null;
            resp = MyRestUtil.getbookingprice(restClient, req, account, gs.wait4p);


            if (resp != null && resp.IsSuccessful)
            {
                MyPriceResp priceResp = RestClientUtil.DeserializeObject<MyPriceResp>(resp.Content);
                if (priceResp != null)
                {
                    if (priceResp.responseData != null)
                    {
                        foreach (PriceItem r in priceResp.responseData)
                        {
                            r.buyDate = DateTime.Parse(req.buyDate);
                            var k = String.Format("{0},{1},{2}", r.lineCode, r.buyDate.ToString("yyyy-MM-dd"), "" + r.feeType);
                            d.prices[k] = r;
                            InfoFormat(String.Format("成功:{0} 价格:{1} {2} 账号:{3}", k, r.adultCNY, r.childrenCNY, account.Name));
                        }
                    }
                    if (priceResp.code == null || priceResp.code.StartsWith("4") || priceResp.code.StartsWith("5"))
                    {

                        InfoFormat(String.Format("{0} {1} 账号:{2} 消息:{3}", d.setting.line, req.buyDate, account.Name,
                          priceResp == null ? "返回异常" : priceResp.message));
                        if (priceResp.code == null)

                        {

                        }
                        else if (priceResp.message.Contains("操作频繁"))
                        {
                            InfoFormat(String.Format("操作频繁 账号:{0}", account.Name));

                        }
                        else if (priceResp.message.Contains("請使用公眾號") || priceResp.message.Contains("重新打開"))
                        {
                            InfoFormat(String.Format("cookies过期 账号:{0}", account.Name));
                        }

                    }
                }
            }
        }

        int cindex = 0;
        int vindex = 0;

        public void booking_quick(ProductDetail r)
        {
            MyData d = MyData.Instance;
            MyThreadPool pool = MyThreadPool.getInstance("订单");
            var accounts = new List<Account>(d.accounts.Where(t => t.Enabled && t.Cookies != null && (t.Type == 0 || t.Type == 3)));
            var size = accounts.Count;
            pool.init(size);
            if (pool.smart.InUseThreads < size
|| pool.smart.CurrentWorkItemsCount < size

                            )
            {
                LoggerUtil.Debug("pool:{0},{0},{0},{0},{0},{0},"
                                   , pool.smart.WaitingCallbacks
                                   , pool.smart.InUseThreads
                                   , pool.smart.ActiveThreads
                                  , pool.smart.CurrentWorkItemsCount
                                  , pool.smart.Concurrency);
                int s = accounts.Count;
                if (s > 0)
                {
                    var customersGroup = (d.customers.Where(t => t.Enabled && !d.orders.Any(o => o.CustomerName == t.Name)).GroupBy(t => t.Group).ToList());
                    int c = customersGroup.Count;
                    if (c > 0)
                    {
                        Interlocked.Increment(ref cindex);
                        cindex = (cindex % c);
                        var customers = customersGroup[cindex];
                        foreach (Account account in accounts)
                        {
                            pool.addTask(d.goalSetting.priority, booking, account, r, customers);
                        }
                    }
                }
            }
        }
        private int booking(Account account, ProductDetail r, IGrouping<ushort, Customer> customers)
        {
            MyData d = MyData.Instance;
            var gs = d.goalSetting;
            try
            {
                var enable = gs.isStart && (gs.isStart4o);
                if (!enable)
                {
                    return -1;//break
                }
                LoggerUtil.Info(String.Format("订单booking:{0} {1} {2} {3} 可订:{4} 价格:{5} {6} 账号:{7}", r.lineCode, r.bookDate, r.beginTime, r.feeType,
                    r.maxPeople, r.price1, r.price2, account.Name));
                if (r.maxPeople < 1 || r.bookDate == null || r.bookDate.Length < 4)
                {
                    return 0;
                }
                if (r.price1 < 1 || r.price2 < 1)
                {
                    return 0;
                }
                var k = String.Format("{0},{1},{2}", r.lineCode, r.bookDate, r.feeType);
                if (!d.prices.ContainsKey(k))
                {
                    LoggerUtil.Warn(String.Format("订单 价格错误:{0} {1} {2} {3} {4} {5}", r.lineCode, r.bookDate, r.feeType, r.beginTime, r.maxPeople, account.Name));
                    return 0;
                }
                if (customers.Any(t => t.BookDate >= DateTime.Today && !t.BookDate.Equals(DateTime.Parse(r.bookDate))))
                {
                    LoggerUtil.Warn(String.Format("订单 客户指定预定日期不符合:{0} {1}", String.Join(";", customers.Select(t => t.Name)), account.Name));
                    return 0;
                }
                if (customers.Any(t => !t.Enabled))
                {
                    LoggerUtil.Warn(String.Format("订单 客户禁用或已下过单:{0} {1}", String.Join(";", customers.Select(t => t.Name)), account.Name));
                    return 0;
                }

                string accept = "image/avif,image/webp,image/wxpic,image/tpg,image/apng,image/*,*/*;q=0.8";
                RestSharp.IRestClient restClient = MyRestUtil.GetRestClient(account, accept, true);
                RestClientUtil.SetCookies(restClient, account.Cookies);
                vv:
                Interlocked.Increment(ref vindex);
                var f = String.Format("imgs/{0}_{1:03d}.{2}", "vc", vindex, "jpg");
                MyRestUtil.getvimg(restClient, f, account, gs.wait4o);


                string vcode = RestClientUtil.execCmd(String.Format("python Newtonsoft.Net.dll {0}", f), true);
                LoggerUtil.Info(String.Format("订单vcode:{0} {1}", vindex, vcode));

                if (vcode == null || vcode.Length < 4)
                {
                    goto vv;
                }
                if ((account.LastUpdate - new DateTime(2023, 1, 30)).TotalDays >= 0)
                {
                    restClient.DefaultParameters.Clear();
                }
                if ((r.lastUpate - new DateTime(2023, 1, 29)).TotalDays >= 0)
                {
                    restClient.DefaultParameters.Clear();
                }
                var pri = d.prices[k];
                OrderReq ordreq = new OrderReq();
                ordreq.bookBeginTime = r.beginTime;
                ordreq.bookEndTime = r.endTime;
                ordreq.captcha = vcode;
                ordreq.currency = "1";
                ordreq.endStationCode = pri.endStationCode;
                ordreq.feeType = int.Parse(r.feeType == null ? "0" : r.feeType);
                ordreq.feeType = 9;
                ordreq.lineCode = r.lineCode;
                ordreq.sessionId = "";
                ordreq.sig = "";
                ordreq.startStationCode = pri.startStationCode;
                ordreq.ticketCategory = r.ticketCategory;
                ordreq.ticketData = r.bookDate;
                ordreq.tickets = new List<TicketsItem>();
                bindUser(account, pri, ordreq, customers);
                if (ordreq.tickets.Count < 1)
                {
                    LoggerUtil.Info("订单 该账号无客户信息或客户都已下单:{0}", account.Name, String.Join(";", customers));
                }
                ordreq.timestamp = 0;
                ordreq.token = "";
                ordreq.totalBalpay = 0;

                ordreq.totalVoucherpay = 0;
                ordreq.voucherNum = 0;
                ordreq.voucherStr = "";
                ordreq.totalBalpay = 0;

                ordreq.boardingPointCode = pri.boardingPointCode;
                ordreq.breakoutPointCode = pri.breakoutPointCode;
                ordreq.timestamp = ConvertDateTimeInt(DateTime.Now);
                //ordreq.totalNeedpay = pri.adultCNY * 100;
                //ordreq.amount = pri.adultCNY * 100;
                var resp = MyRestUtil.getordering(restClient, ordreq, account);

                if (resp != null && resp.IsSuccessful)
                {
                    OrderResp ordResp = RestClientUtil.DeserializeObject<OrderResp>(resp.Content);
                    if (ordResp != null)
                    {
                        if (ordResp.responseData != null)
                        {
                            account.Enabled = false;
                            var ord = ordResp.responseData;
                            var msg = String.Format("订单号:{0} {1} 金额:{2} 账号:{3} {4} 备注:{5}",
                                ord.orderNumber, ord.orderReqno, ord.amount,
                                account.Name, ord.wxAttach, ord.orderDecribe);
                            Order order = new Order();
                            order.AccountName = account.Name;
                            order.OrderNum = ord.orderNumber;
                            order.CustomerName = String.Join(";", customers.Select(t => t.Name));
                            order.Comment = msg;
                            d.orders.Add(order);

                            LoggerUtil.Info(String.Format("订单成功:{0}", msg));
                            customers.Count(t => t.Enabled = false);
                            BeepUtil.MessageBeep((uint)BeepUtil.MessageBeepType.Ok);
                            return 1;
                        }
                        if (ordResp.code == null || ordResp.code.StartsWith("4")
                            && !ordResp.message.Contains("验证")
                            || ordResp.code.StartsWith("5")
                            || ordResp.code.StartsWith("E"))
                        {
                            LoggerUtil.Info(String.Format("订单结果 {0} {1} {2} 账号:{3} 消息:{4}", d.setting.line, r.bookDate, ordreq.bookBeginTime,
                                account.Name, ordResp == null ? "返回异常" : ordResp.message));

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerUtil.Error("run booking", ex);
            }
            return 2;

        }
        private void bindUser(Account account, PriceItem pri, OrderReq ordreq, IGrouping<ushort, Customer> customers)
        {

            foreach (Customer customer in customers)
            {
                var ticket = new TicketsItem();
                var uType = customer.Type;
                if (uType == 0)
                {
                    ticket.ticketType = "00";
                    ordreq.totalNeedpay += pri.adultCNY * 100;
                    ordreq.amount += pri.adultCNY * 100;
                }
                else
                {
                    ticket.ticketType = "01";
                    ordreq.totalNeedpay += pri.childrenCNY * 100;
                    ordreq.amount += pri.childrenCNY * 100;
                }
                ticket.idType = 1;
                ticket.telNum = "";
                ticket.idCard = customer.IdCardNum;
                ticket.userName = customer.Name;
                ordreq.tickets.Add(ticket);

            }


        }
        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"> DateTime时间格式</param>
        /// <returns>Unix时间戳格式</returns>
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        public void stop()
        {
            lock (lockObj)
            {
                LoggerUtil.Info("stop");
                try
                {
                    MyData.Instance.goalSetting.isStart = false;
                    var pools = MyThreadPool.Instances.Values;
                    foreach (MyThreadPool pool in pools)
                        pool.cancel();
                }
                catch (Exception ex)
                {
                    LoggerUtil.Error("stop", ex);
                }
            }

        }
        public void start()
        {
            lock (lockObj)
            {
                LoggerUtil.Info("start");
                try
                {
                    MyData.Instance.goalSetting.isStart = true;
                    init();
                }
                catch (Exception ex)
                {
                    LoggerUtil.Error("start", ex);
                }
            }
        }
    }
}
