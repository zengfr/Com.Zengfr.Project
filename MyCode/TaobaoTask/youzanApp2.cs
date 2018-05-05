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
                var result = ProcessItem(consoleApp, restClient, testAlias, true);
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
                CsvMemoryUtil.Instance.Load(dataPath, "alias");
                Console.WriteLine("Load finish totalCount:{0}", CsvMemoryUtil.Instance.TotalCount);
            }
            Console.WriteLine("Please Input Any Key To Continue...{0}", DateTime.Now.ToString("HH:mm:ss fff"));
            Console.ReadKey(); PlayKey();
            var startTime = DateTime.Now;

            consoleApp.InitWindow();
            CsvMemoryUtil.Instance.AddProcessorFor((IList<ResultItem> items) =>
            {
                var fPath = CsvMemoryUtil.Instance.GetLastCreationFile(dataPath, "*.csv");
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
                var data = CSVUtil.EnumerableToDataTable(items);
                CSVUtil.SaveCSV(data, fPath);
            }, saveIntervalMinute, saveIntervalCount);
            foreach (var keyWord in keyWords)
            {
                try
                {
                    var resultItems = ProcessList(consoleApp, restClient, maxDegreeOfParallelism, sleepIntervalMilis, keyWord);
                    CsvMemoryUtil.Instance.SaveToEnqueue(resultItems, (ResultItem item) =>
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
            CsvMemoryUtil.Instance.Process<ResultItem>(false);
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
        protected static List<ResultItem> ProcessList(ConsoleApp consoleApp, IRestClient restClient, int maxDegreeOfParallelism, int sleepIntervalMilis, string keyword)
        {
            var resultItems = new ConcurrentBag<ResultItem>();
            int pageIndex = 1;
            int totalPage = pageIndex;
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = maxDegreeOfParallelism;
            do
            {
                if (sleepIntervalMilis > 0)
                    Thread.Sleep(sleepIntervalMilis);
                var url = string.Format("v2/fenxiao/team/list.json?keyword={0}&p={1}&t={2}", keyword, pageIndex++, RestClientUtil.GetRandomStr(12));
                var resp = RestClientUtil.Get(restClient, url, consoleApp.BottomWriter);
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
                            var data = ProcessItem(consoleApp, restClient, alias, false);
                            var goodslist = ProcessGoodslist(consoleApp, restClient, alias, false);

                            var resultItem = new ResultItem();
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
        protected static TaobaoTask.youzan.supplierteam.Data ProcessItem(ConsoleApp consoleApp, IRestClient restClient, string alias, bool test)
        {
            TaobaoTask.youzan.supplierteam.Data result = null;
            var url = string.Format("v2/fenxiao/supplier/team.json?alias={0}&t={1}", alias, RestClientUtil.GetRandomStr(12));
            var resp = RestClientUtil.Get(restClient, url, consoleApp.BottomWriter);
            if (resp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = RestClientUtil.Unicode2String(resp.Content);
                if (content.IndexOf("登录") != -1 || content.IndexOf("错误") != +-1)
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
        protected static TaobaoTask.youzan.goodslist.Data ProcessGoodslist(ConsoleApp consoleApp, IRestClient restClient, string alias, bool test)
        {
            TaobaoTask.youzan.goodslist.Data result = null;
            var url = string.Format("v2/fenxiao/supplier/goodslist.json?alias={0}&p=1&t={1}", alias, RestClientUtil.GetRandomStr(12));
            var resp = RestClientUtil.Get(restClient, url, consoleApp.BottomWriter);
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
        public class ResultItem
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
