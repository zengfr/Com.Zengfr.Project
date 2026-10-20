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
using TaobaoTask.youzan.gys;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Concurrent;
using ComLib.Configuration;
using ComLib.IO;
using System.IO;
using Com.Zengfr.Proj.Common;

namespace TaobaoTask
{
    public class youzanApp2
    {
        public static void MainT(string[] args)
        {

            var consoleApp = ConsoleApp.Instance;
            consoleApp.InitConsole(true, false);
            consoleApp.InitConfig();
            PlayDingDong();
            Play98();
            var fullScreenMode = consoleApp.GetConfig("fullScreenMode", true);
            if (fullScreenMode)
                ConsoleDisplayModeUtil.SetFullScreenMode2();
            var defUserName = consoleApp.GetConfig("userName", "15692188276");
            string userName = AutoPrompt.PromptForInput("Enter UserName:", defUserName); PlayKey();
            string userPassword = AutoPrompt.GetPassword("Enter Password:"); PlayKey();
            var baseUrl = "https://login.youzan.com/";
            var restClient = RestClientUtil.GetRestClient(baseUrl);
            InitYzm(restClient);
            string userYzm = AutoPrompt.GetPassword("Enter VerificationCode:"); PlayKey();


            baseUrl = "https://www.youzan.com/";
            restClient = RestClientUtil.GetRestClient(baseUrl);
            var defCookies = consoleApp.GetConfig("cookies", string.Empty);
            string cookies = AutoPrompt.PromptForInput("Enter Cookies:", defCookies); PlayKey();
            if (string.IsNullOrWhiteSpace(cookies) || cookies.Length < 10 || userName.IndexOf("188276") < 0 || cookies.IndexOf(userName) < 0)
            {
                PlayError(); Console.ReadKey(); return;
            }
            string testCookies = AutoPrompt.PromptForInput("Test Cookies(Press up/down):", new string[] { "YES", "NO" });
            RestClientUtil.SetCookies(restClient, cookies); PlayKey();
            if (testCookies == "YES")
            {
                var testAlias = consoleApp.GetConfig("testAlias", "bqpxrhnu");
                var result = ProcessTeamItem(consoleApp, restClient, testAlias, true);
            }

            string type = AutoPrompt.PromptForInput("Enter Type(Press up/down):", new string[] { "Suppliers", "Goods" });
            PlayKey();
            var keyWords = consoleApp.GetConfig("keyWords", string.Empty).Split(',').Reverse();
            var maxDegreeOfParallelism = consoleApp.GetConfig("maxDegreeOfParallelism", 2);
            var sleepIntervalMilis = consoleApp.GetConfig("sleepIntervalMilis", 22);
            var storeFileMaxRowsCount = consoleApp.GetConfig("storeFileMaxRowsCount", 60000);

            var saveIntervalMinute = consoleApp.GetConfig("saveIntervalMinute", 30);
            var saveIntervalCount = consoleApp.GetConfig("saveIntervalCount", 1000);
            var playSoundWhenWordFinish = consoleApp.GetConfig("playSoundWhenWordFinish", true);
            var playSoundWhenSaveData = consoleApp.GetConfig("playSoundWhenSaveData", true);
            var loadData = consoleApp.GetConfig("loadData", true);
            if (maxDegreeOfParallelism < 2) maxDegreeOfParallelism = 1;
            if (maxDegreeOfParallelism > 4) maxDegreeOfParallelism = 4;
            if (sleepIntervalMilis < 44) maxDegreeOfParallelism = 44;
            var dataPath = consoleApp.GetPath("data");
            if (loadData)
            {
                if (!Directory.Exists(dataPath))
                    Directory.CreateDirectory(dataPath);
                Console.WriteLine("Loading data...");
                CsvMemoryUtil.Instance.Load(dataPath,type, "alias");
                Console.WriteLine("Load finish totalCount:{0}", CsvMemoryUtil.Instance.TotalCount);
            }
            Console.WriteLine("Please Input Any Key To Continue...{0}", DateTime.Now.ToString("HH:mm:ss fff"));
            Console.ReadKey(); PlayKey();
            var startTime = DateTime.Now;

            consoleApp.InitWindow();
            switch (type) {
                case "Suppliers":
                    
                    AddProcessorFor<SupplierResultItem>(consoleApp, restClient,true,
             dataPath, type, maxDegreeOfParallelism, saveIntervalMinute, saveIntervalCount, storeFileMaxRowsCount,
             sleepIntervalMilis, playSoundWhenSaveData, playSoundWhenWordFinish);
                    ProcessBySupplier(  consoleApp,   restClient,   keyWords,   maxDegreeOfParallelism,
              sleepIntervalMilis,   playSoundWhenWordFinish);
                    break;
                case "Goods":
                    storeFileMaxRowsCount = int.MaxValue - 1;//Goods
                    IDictionary<string, string> fx_priceDict = null;// CsvMemoryUtil.Instance.LoadDictionary(dataPath, type, "alias", "fx_priceString");
                    IDictionary<string, string> sold_num_d30Dict = null;// CsvMemoryUtil.Instance.LoadDictionary(dataPath, type, "alias", "sold_num_d30String");
                    IDictionary<string, string> aliasDict =   CsvMemoryUtil.Instance.LoadDictionary(dataPath, type, "alias", "alias");

                    AddProcessorFor<GoodResultItem>(consoleApp, restClient,false,
             dataPath, type, maxDegreeOfParallelism, saveIntervalMinute, saveIntervalCount, storeFileMaxRowsCount,
             sleepIntervalMilis, playSoundWhenSaveData, playSoundWhenWordFinish);

                    ProcessByGoodAliasDict(consoleApp, restClient, aliasDict.Keys, false);

                    ProcessByGood(consoleApp, restClient, keyWords, maxDegreeOfParallelism,
              sleepIntervalMilis, playSoundWhenWordFinish, fx_priceDict, sold_num_d30Dict);
                    


                    break;
            }
            consoleApp.LeftWriter.WriteLine(string.Format("all Finished! Timespan(ms):{0}", (DateTime.Now - startTime).TotalMilliseconds));
            for (var i = 0; i < 60 / 5 * 5; i++)
            {
                PlayDingDong();
                Thread.Sleep(5 * 1000);
            }
            Console.ReadLine();
        }
        protected static void InitYzm(IRestClient restClient)
        {
            var url = "sso/index";
            RestClientUtil.Get(restClient, url, null);

            var captchaUrl = "sso/index/captcha";
            var resp = RestClientUtil.Get(restClient, captchaUrl, null);
            string path = string.Format("{0}yzm\\{1}", AppDomain.CurrentDomain.BaseDirectory, "yzm.png");
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            File.WriteAllBytes(path, resp.RawBytes);
        }
        protected static void Login(IRestClient restClient)
        {

        }
        protected static void AddProcessorFor<T>(ConsoleApp consoleApp, IRestClient restClient,bool appendFile,
           string dataPath, string type, int maxDegreeOfParallelism, int saveIntervalMinute, int saveIntervalCount, int storeFileMaxRowsCount,
           int sleepIntervalMilis, bool playSoundWhenSaveData,  bool playSoundWhenWordFinish)
        {
            CsvMemoryUtil.Instance.AddProcessorFor((IList<T> items) =>
            {
                var fPath = CsvMemoryUtil.Instance.GetLastCreationFile(dataPath,type+ "*.csv");
                var newfPath = string.Format("{0}\\{1}_{2}.csv", dataPath, type, DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                var isNewFile = false;
                if (string.IsNullOrWhiteSpace(fPath))
                {
                    fPath = newfPath;
                    isNewFile = true;
                }
                else
                {
                    var fLines = File.ReadAllLines(fPath);
                    if (fLines.Length + items.Count > storeFileMaxRowsCount)
                    {
                        fPath = newfPath;
                        isNewFile = true;
                    }
                }
                if (playSoundWhenSaveData)
                {
                    PlayQQ();
                }
                consoleApp.LeftWriter.WriteLine(string.Format("count:{0},isNewFile:{1},saveToFile:{2}", items.Count, isNewFile, fPath));
                var data = CsvFileHelper.EnumerableToDataTable(items);
                if (appendFile)
                {
                    CSVUtil.SaveCSVAppend(data, fPath);
                }
                else
                {
                    data.SetPrimaryKeyColumns<string>("alias");
                    data.AddColumnsByDays<int>("总销量", 0, 1);
                    data.AddColumnsByDays<int>("分销商", 0, 1);

                    data.CopyColumn("fx_count", data.BuildColumnNameByDay("分销商", 0));
                    data.CopyColumn("sold_num", data.BuildColumnNameByDay("总销量", 0));
                    
                    var dt= CsvFileHelper.OpenCSV(fPath);
                    dt.RemoveColumnsByDays<int>("总销量", -22, -15);
                    dt.RemoveColumnsByDays<int>("分销商", -22, -15);
                    
                     
                    dt.SetPrimaryKeyColumns<string>("alias");
                    //20200519 临时注释 dt.MergeTableFrom<string>(data,false);
                    CSVUtil.SaveCSVCreate(dt, fPath);
                }
            }, saveIntervalMinute, saveIntervalCount);
        }
        protected static void ProcessBySupplier(ConsoleApp consoleApp, IRestClient restClient, IEnumerable<string> keyWords,  int maxDegreeOfParallelism, 
            int sleepIntervalMilis, bool playSoundWhenWordFinish)
        {
            
            foreach (var keyWord in keyWords)
            {
                try
                {
                    var resultItems = ProcessSupplierList(consoleApp, restClient, maxDegreeOfParallelism, sleepIntervalMilis, keyWord);
                    CsvMemoryUtil.Instance.SaveToEnqueue(resultItems, (SupplierResultItem item) =>
                    {
                        return item.alias;
                    });
                    if (playSoundWhenWordFinish)
                    {
                        PlayJQ();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            CsvMemoryUtil.Instance.Process<SupplierResultItem>(false);
        }
        protected static void ProcessByGood(ConsoleApp consoleApp, IRestClient restClient, IEnumerable<string> keyWords, int maxDegreeOfParallelism,
            int sleepIntervalMilis, bool playSoundWhenWordFinish, IDictionary<string, string> fx_priceDict, IDictionary<string, string> sold_num_d30Dict )
        {

            foreach (var keyWord in keyWords)
            {
                try
                {
                    var resultItems = ProcessGoodList(consoleApp, restClient, maxDegreeOfParallelism, sleepIntervalMilis, keyWord);
                    resultItems = resultItems.GroupBy(o => o.alias).Select(g => g.First()).ToList();
                    /*foreach (var item in resultItems) {
                        var k = item.alias;
                        if (fx_priceDict.ContainsKey(k))
                        {
                            item.fx_priceString = string.Format("{0},{1}", fx_priceDict[k], item.fx_price);
                        }
                        else {
                            item.fx_priceString = string.Format("{0}",  item.fx_price);
                        }
                        if (sold_num_d30Dict.ContainsKey(k))
                        {
                            item.sold_num_d30String = string.Format("{0},{1}", sold_num_d30Dict[k], item.sold_num_d30);
                        }
                        else
                        {
                            item.sold_num_d30String = string.Format("{0}", item.sold_num_d30);
                        }
                    }*/
                    CsvMemoryUtil.Instance.SaveToEnqueue(resultItems, (GoodResultItem item) =>
                    {
                        return item.alias;
                    });
                    if (playSoundWhenWordFinish)
                    {
                        PlayJQ();
                    }
                }
                catch (Exception ex)
                {
                }
            }
            CsvMemoryUtil.Instance.Process<GoodResultItem>(false);
        }
        protected static void ProcessByGoodAliasDict(ConsoleApp consoleApp, IRestClient restClient,IEnumerable<string> aliass, bool test)
        {
            var resultItems = new ConcurrentBag<GoodResultItem>();
            foreach (var alias in aliass) {
                var resultItem=  new GoodResultItem();
                var data = ProcessGoodDetail(consoleApp, restClient, alias, false);
                if (data != null)
                {
                    resultItem.alias = alias;
                    resultItem.team_name = data.team_info.team_name;
                    if (resultItem.fx_count <= 0)
                        resultItem.fx_count = int.Parse(data.goods_detail.fx_count ?? "0");
                    if (resultItem.sold_num <= 0)
                        resultItem.sold_num = data.goods_detail.sold_num;
                    resultItems.Add(resultItem);
                }
            }
            CsvMemoryUtil.Instance.SaveToEnqueue(resultItems.ToList(), (GoodResultItem item) =>
            {
                return item.alias;
            });
             
        }
        protected static List<GoodResultItem> ProcessGoodList(ConsoleApp consoleApp, IRestClient restClient, int maxDegreeOfParallelism, int sleepIntervalMilis, string keyword)
        {
            var resultItems = new ConcurrentBag<GoodResultItem>();
            int pageIndex = 1;
            int totalPage = pageIndex;
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;
            do
            {
                if (sleepIntervalMilis > 0)
                    Thread.Sleep(sleepIntervalMilis);
                var url = string.Format("v2/fenxiao/goods/list.json?keyword={0}&p={1}&category=all&t={2}", keyword, pageIndex++, RestClientUtil.GetRandomStr(12));
                var resp = RestClientUtil.Get(restClient, url,null, consoleApp.BottomWriter);
                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var root = JsonConvert.DeserializeObject<TaobaoTask.youzan.goodslist.Root>(RestClientUtil.Unicode2String(resp.Content));
                    if (root != null && root.data != null && root.data.pagination != null && root.data.list != null)
                    {
                        consoleApp.LeftWriter.WriteLine("code:{0};msg:{1};keyword:{2};count:{3};{4}", root.code, root.msg, keyword, root.data.list.Count, RestClientUtil.BuildLog(root.data.pagination));
                        totalPage = Math.Max(totalPage, root.data.pagination.perPage);
                        Parallel.ForEach(root.data.list, parallelOptions, item =>
                        {
                            if (sleepIntervalMilis > 0)
                                Thread.Sleep(sleepIntervalMilis);
                            var alias = item.alias;
                            
                            var resultItem = new GoodResultItem();
                            resultItem.word = RestClientUtil.FiterChars(keyword ?? string.Empty);
                            resultItem.page = pageIndex;
                            resultItem.alias = RestClientUtil.FiterChars(alias ?? string.Empty);
                            resultItem.title = RestClientUtil.FiterChars(item.title ?? string.Empty).Replace("\"","");
                            resultItem.fx_price =float.Parse(item.fx_price??"-1");
                            resultItem.sold_num_d30 = item.sold_num_d30;
                            resultItem.sold_num = item.sold_num;
                            resultItem.fx_count = item.fx_count;
                            if (item.Profit_rate != null)
                            {
                                resultItem.min = item.Profit_rate.min;
                                resultItem.max = item.Profit_rate.max;
                            }
                            resultItem.goodsUrl = string.Format("https://www.youzan.com/v2/fenxiao/market/detail/{0}", alias);
                            //resultItem.team_name=item.
                            var data = ProcessGoodDetail(consoleApp, restClient, alias, false);
                            if (data != null) {
                                resultItem.team_name = data.team_info.team_name;
                                if (resultItem.fx_count <= 0)
                                    resultItem.fx_count =int.Parse(data.goods_detail.fx_count??"0");
                                if(resultItem.sold_num<=0)
                                  resultItem.sold_num = data.goods_detail.sold_num;
                                //resultItem.fx_count = data.goods_detail.fx_count;
                            } 
                            resultItems.Add(resultItem);
                        });
                    }
                }

            } while (pageIndex < totalPage);

            return resultItems.ToList();
        }
        protected static List<SupplierResultItem> ProcessSupplierList(ConsoleApp consoleApp, IRestClient restClient, int maxDegreeOfParallelism, int sleepIntervalMilis, string keyword)
        {
            var resultItems = new ConcurrentBag<SupplierResultItem>();
            int pageIndex = 1;
            int totalPage = pageIndex;
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;
            do
            {
                if (sleepIntervalMilis > 0)
                    Thread.Sleep(sleepIntervalMilis);
                var url = string.Format("v2/fenxiao/team/list.json?keyword={0}&p={1}&t={2}", keyword, pageIndex++, RestClientUtil.GetRandomStr(12));
                var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var root = JsonConvert.DeserializeObject<Root>(RestClientUtil.Unicode2String(resp.Content));
                    if (root != null && root.data != null && root.data.pagination != null && root.data.list != null)
                    {
                        consoleApp.LeftWriter.WriteLine("code:{0};msg:{1};keyword:{2};count:{3};{4}", root.code, root.msg, keyword, root.data.list.Count, RestClientUtil.BuildLog(root.data.pagination));
                        totalPage = Math.Max(totalPage, root.data.pagination.perPage);
                        Parallel.ForEach(root.data.list, parallelOptions, item =>
                        {
                            if (sleepIntervalMilis > 0)
                                Thread.Sleep(sleepIntervalMilis);
                            var alias = item.alias;
                            var data = ProcessTeamItem(consoleApp, restClient, alias, false);
                            var goodslist = ProcessSupplierGoodslist(consoleApp, restClient, alias, false);

                            var resultItem = new SupplierResultItem();
                            resultItem.word = RestClientUtil.FiterChars(keyword ?? string.Empty);
                            resultItem.alias = RestClientUtil.FiterChars(alias ?? string.Empty);
                            resultItem.category = RestClientUtil.FiterChars(item.category ?? string.Empty);
                            resultItem.kdt_id = item.kdt_id;
                            resultItem.seller_num = item.seller_num;
                            resultItem.goods_num = item.goods_num;
                            resultItem.team_name = RestClientUtil.FiterChars(item.team_name ?? string.Empty);
                            resultItem.url = string.Format("https://www.youzan.com/v2/fenxiao/market/supplier/{0}", alias);
                            if (goodslist != null && goodslist.list != null && goodslist.list.Any())
                            {
                                resultItem.fxCountMin = goodslist.list.Min(t => t.sold_num_d30);
                                resultItem.fxCountMax = goodslist.list.Max(t => t.sold_num_d30);
                                resultItem.fxCountAvg = (double)goodslist.list.Sum(t => t.sold_num_d30) / goodslist.list.Count;
                            }
                            if (data != null)
                            {
                                resultItem.description = RestClientUtil.FiterChars((data.description ?? string.Empty).Replace("\r", "").Replace("\n", ""));
                                resultItem.mobile = RestClientUtil.FiterChars(data.mobile ?? string.Empty);
                                resultItem.qq = RestClientUtil.FiterChars(data.qq ?? string.Empty);
                                resultItem.wechat = RestClientUtil.FiterChars(data.wechat ?? string.Empty);
                                resultItem.fx_cooperation_weixin = RestClientUtil.FiterChars(data.fx_cooperation_weixin ?? string.Empty);
                                resultItem.follower_num = data.follower_num;
                                resultItem.fx_team_num = data.fx_team_num;
                                resultItem.certification = data.certification;
                                resultItem.is_recommend_supplier = data.is_recommend_supplier;
                                resultItem.is_quit_supplier = data.is_quit_supplier;

                            }

                            resultItems.Add(resultItem);
                        });
                    }
                }

            } while (pageIndex < totalPage);

            return resultItems.ToList();
        }
        protected static TaobaoTask.youzan.supplierteam.Data ProcessTeamItem(ConsoleApp consoleApp, IRestClient restClient, string alias, bool test)
        {
            TaobaoTask.youzan.supplierteam.Data result = null;
            var url = string.Format("v2/fenxiao/supplier/team.json?alias={0}&t={1}", alias, RestClientUtil.GetRandomStr(12));
            var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = RestClientUtil.Unicode2String(resp.Content);
                if ((content.IndexOf("登录") != -1 || content.IndexOf("错误") != -1)&& content.IndexOf("\"msg\":\"\"") <=0)
                {
                    var msg = RestClientUtil.RegexGetString("", content, "\"msg\":\"(.*?)\"", 1);
                    var success = string.IsNullOrWhiteSpace(msg);
                    Console.WriteLine("Test Cookies Result:{0}", success ? "Success" : msg);
                    if (success && test)
                    {
                        PlayOK();
                    }
                    else
                    {
                        PlayError();
                    }
                    if (consoleApp.RightWriter != null)
                        consoleApp.RightWriter.WriteLine("code:{0};msg:{1},alias:{2} Error!", -1, msg, alias);

                }
                else
                {
                    if (test)
                    {
                        Console.WriteLine("Test Cookies Result:{0}", "Success");
                        PlayOK();
                    }
                    try
                    {
                        var root = JsonConvert.DeserializeObject<TaobaoTask.youzan.supplierteam.Root>(content);
                        if (root != null && root.data != null)
                        {
                            if (consoleApp.RightWriter != null)
                                consoleApp.RightWriter.WriteLine("code:{0};msg:{1};alias:{2};{3},{4},{5}", root.code, root.msg, alias, root.data.team_name, root.data.follower_num, root.data.fx_team_num);
                            result = root.data;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return result;
        }
        protected static TaobaoTask.youzan.goodslist.Data ProcessSupplierGoodslist(ConsoleApp consoleApp, IRestClient restClient, string alias, bool test)
        {
            TaobaoTask.youzan.goodslist.Data result = null;
            var url = string.Format("v2/fenxiao/supplier/goodslist.json?alias={0}&p=1&t={1}", alias, RestClientUtil.GetRandomStr(12));
            var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = RestClientUtil.Unicode2String(resp.Content);
                content = content.Replace("‘", "").Replace("’", "");
                try
                {
                    var root = JsonConvert.DeserializeObject<TaobaoTask.youzan.goodslist.Root>(content);
                    if (root != null && root.data != null)
                    {
                        result = root.data;
                    }
                }
                catch (Exception ex)
                {
                    if (consoleApp.RightWriter != null)
                        consoleApp.RightWriter.WriteLine("code:{0};msg:{1};alias:{2}", -1, alias, ex.Message);

                }
            }
            return result;
        }

        protected static TaobaoTask.youzan.marketdetail.Data ProcessGoodDetail(ConsoleApp consoleApp, IRestClient restClient, string alias, bool test)
        {
            TaobaoTask.youzan.marketdetail.Data result = null;
            var url = string.Format("v2/fenxiao/market/detail.json?alias={0}&t={1}", alias, RestClientUtil.GetRandomStr(12));
            var resp = RestClientUtil.Get(restClient, url, null, consoleApp.BottomWriter);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = RestClientUtil.Unicode2String(resp.Content);
                if ((content.IndexOf("登录") != -1 || content.IndexOf("错误") != -1)&& content.IndexOf("\"msg\":\"\"") <=0)
                {
                    var msg = RestClientUtil.RegexGetString("", content, "\"msg\":\"(.*?)\"", 1);
                    var success = string.IsNullOrWhiteSpace(msg);
                    Console.WriteLine("Test Cookies Result:{0}", success ? "Success" : msg);
                    if (success && test)
                    {
                        PlayOK();
                    }
                    else
                    {
                        PlayError();
                    }
                    if (consoleApp.RightWriter != null)
                        consoleApp.RightWriter.WriteLine("code:{0};msg:{1},alias:{2} Error!", -1, msg, alias);

                }
                else
                {
                    if (test)
                    {
                        Console.WriteLine("Test Cookies Result:{0}", "Success");
                        PlayOK();
                    }
                    try
                    {
                        var root = JsonConvert.DeserializeObject<TaobaoTask.youzan.marketdetail.Root>(resp.Content);
                        if (root != null && root.data != null)
                        {
                            root.data.goods_detail.title=RestClientUtil.Unicode2String(root.data.goods_detail.title);
                            if (consoleApp.RightWriter != null)
                                consoleApp.RightWriter.WriteLine("code:{0};msg:{1};alias:{2};{3}", root.code, root.msg, alias, root.data.goods_detail.title.Substring(0,22));
                            result = root.data;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return result;
        }
        
        private static async void PlayOK()
        {
            await Task.Factory.StartNew(() =>
            {
                BeepUtil.MessageBeep((uint)BeepUtil.MessageBeepType.Ok);
                BeepUtil.PlaySoundSync("V06.wav");
                BeepUtil.PlaySoundSync("female.wav");
            });
        }
        private static void PlayDingDong()
        {
            BeepUtil.MessageBeep((uint)BeepUtil.MessageBeepType.Ok);
            BeepUtil.PlaySound("dd.wav");
        }
        private static async void Play98()
        {
            await Task.Factory.StartNew(() =>
            {
                BeepUtil.MessageBeep((uint)BeepUtil.MessageBeepType.Ok);
                BeepUtil.PlaySound("o98.wav");
            });
        }
        private static async void PlayKey()
        {
            await Task.Factory.StartNew(() =>
            {
                //BeepUtil.MessageBeep((uint)BeepUtil.MessageBeepType.Ok);
                BeepUtil.PlaySoundSync("key.wav");
            });
        }
        private static async void PlayQQ()
        {
            await Task.Factory.StartNew(() =>
            {
                //BeepUtil.MessageBeep((uint)BeepUtil.MessageBeepType.Ok);
                BeepUtil.PlaySoundSync("qq.wav");
            });
        }
        private static void PlayError()
        {
            BeepUtil.MessageBeep((uint)BeepUtil.MessageBeepType.Error);
            BeepUtil.PlaySoundSync("wd.wav");
            BeepUtil.PlaySoundSync("male.wav");
        }
        static bool IsPlayingJQ = false;
        private static async void PlayJQ()
        {
            if (!IsPlayingJQ)
            {
                await Task.Factory.StartNew(() =>
                {
                    IsPlayingJQ = true;
                    //BeepUtil.MessageBeep((uint)BeepUtil.MessageBeepType.Ok);
                    //BeepUtil.PlaySoundSync("jqsj2.wav");
                    //BeepUtil.PlaySoundSync("jqsj1.wav");
                    BeepUtil.PlaySoundSync("dbz.wav");
                    IsPlayingJQ = false;
                });
            }
        }
        public class GoodResultItem
        {
            public String word { get; set; }
            public int? page { get; set; }
            public String alias { get; set; }
            public String team_name { get; set; }
            public float? fx_price { get; set; }
            public String min { get; set; }
            public String max{ get; set; }
            /// <summary>
            /// 月销量
            /// </summary>
            public int? sold_num_d30 { get; set; }
            /// <summary>
            /// 总销量
            /// </summary>
            public int? sold_num { get; set; }
            /// <summary>
            /// 分销商 数
            /// </summary>
            public int? fx_count { get; set; }
            public String title { get; set; }
            /// <summary>
            /// https://www.youzan.com/v2/fenxiao/market/detail/36a8czpfplwae
            /// </summary>
            public String goodsUrl { get; set; }
            //public String fx_priceString { get; set; }
            //public String maxString { get; set; }
            //public String sold_num_d30String { get; set; }
        }
            public class SupplierResultItem
        {
            public String word { get; set; }
            public String alias { get; set; }
            public String category { get; set; }
            public int kdt_id { get; set; }
            public int seller_num { get; set; }
            public int goods_num { get; set; }
            public String team_name { get; set; }


            public String mobile { get; set; }
            public String qq { get; set; }
            public String wechat { get; set; }
            public String fx_cooperation_weixin { get; set; }
            public int follower_num { get; set; }
            public int fx_team_num { get; set; }
            public bool certification { get; set; }
            public bool is_recommend_supplier { get; set; }
            public bool is_quit_supplier { get; set; }
            public int fxCountMax { get; set; }
            public int fxCountMin { get; set; }
            public double fxCountAvg { get; set; }
            public string url { get; set; }
            public String description { get; set; }
        }
        public class Statistics
        {
            public int Queue { get; set; }
            public int Process { get; set; }
            public int Finish { get; set; }
            public int Success { get; set; }
            public int Error { get; set; }
        }
    }
}
