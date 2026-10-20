using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using v2rayN.Base;
using v2rayN.Handler;
using v2rayN.Resx;
using v2rayN;
using v2rayN.Mode;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Concurrent;
using log4net;
using static VpnApp.VpnStat;
using System.Net.Http.Headers;

namespace VpnApp
{
    internal class VpnMan
    {
        class TimerState
        {
            public int Counter;
        }

        private static ILog log = LogManager.GetLogger(typeof(VpnMan));
        private static readonly Lazy<VpnMan> _instance = new(() => new());
        public static VpnMan Instance => _instance.Value;

        private static string basePath = AppDomain.CurrentDomain.BaseDirectory;
        private static string lineSplit = Environment.NewLine;
        private static Encoding encoding = Encoding.UTF8;

        private VpnStat stat = VpnStat.Instance;
        private VpnCfg vpnCfg = VpnCfg.Instance;

        private Semaphore semaphore1;
        private System.Threading.Timer timer;
        public VpnMan()
        {
            Init();
            AddAllSubs();
        }
        private void Init()
        {
            lineSplit = "\n";
            var concurrency = vpnCfg.Concurrency;
            semaphore1 = new Semaphore(concurrency, concurrency);

            var dir = String.Format("{0}/results/", basePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);


            var timerState = new TimerState { Counter = 0 };
            timer = new Timer(callback: new TimerCallback(TimerTask), state: timerState, dueTime: 3000, period: 1000 * vpnCfg.Period);
        }
        private void AddAllSubs()
        {
            AddSubs(vpnCfg.CfgFile1);
            AddSubs(vpnCfg.CfgFile2);
        }
        private void AddSubs(string file)
        {
            if (File.Exists(basePath + file))
            {
                var lines = File.ReadAllLines(basePath + file);
                foreach (var line in lines)
                {
                    var l = line.Trim().Trim('"');
                    if (!Utils.IsNullOrEmpty(l))
                    {
                        AddSub(l);
                        if (l.IndexOf("https://raw.githubusercontent.com") < 10)
                            AddSub("https://ghproxy.com/" + l);
                    }
                }
            }
        }
        private void AddSub(string url)
        {
            stat.Subs[url.GetHashCode().ToString()] = url;
        }
        private void TimerTask(object? state)
        {
            var dir = String.Format("{0}/results/history/{1}", basePath, DateTime.Today.ToString("yyyyMMdd"));
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);


            var statK = "parse";
            try
            {

                if (stat.GetStat(statK) <= 0)
                {
                    stat.SetStat(statK, 1);
                    ParseAll();
                    stat.SetStat(statK, 0);
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                stat.SetStat(statK, 0);
            }
            statK = "test";
            try
            {
                log.Info(String.Format("left:{0}", stat.Profiles.Count(t => t.Value.delay < -1)));
                if (stat.GetStat("test") <= 0)
                {
                    stat.SetStat(statK, 1);
                    TestAll();
                    stat.SetStat(statK, 0);
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
                stat.SetStat(statK, 0);
            }
            if ((DateTime.Now - stat.Last) > TimeSpan.FromMinutes(60))
            {
                stat.Save();
            }
        }

        public void ParseAll()
        {
            log.Info(String.Format("parseAll...{0}", ""));
            var tasks = new List<Task>();
            foreach (var subUrl in stat.Subs.Values)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        semaphore1.WaitOne();

                        var d = DownloaderHelper.Instance.DownloadStringAsync(null, subUrl, null, 6000);
                        string msg = "";
                        await d;
                        await d.ContinueWith((r) =>
                        {

                            var clipboardData = r.Result;
                            if (Utils.IsNullOrEmpty(clipboardData))
                                return;
                            if (!Utils.IsNullOrEmpty(clipboardData) && Utils.IsBase64String(clipboardData))
                            {
                                clipboardData = Utils.Base64Decode(clipboardData);
                            }
                            var arrData = clipboardData.Split(lineSplit.ToCharArray()).Where(t => !string.IsNullOrEmpty(t)).Distinct();
                            log.Info(String.Format("parse...{0}", arrData.Count()));
                            foreach (string str in arrData)
                            {
                                var p = ShareHandler.ImportFromClipboardConfig(str, out msg);
                                if (p != null)
                                {
                                    var k = String.Format("{0},{1}", p.address, p.port);
                                    if (!stat.Profiles.ContainsKey(k))
                                    {
                                        stat.Profiles[k] = new TestItem() { Item = p, delay = -2, sub = subUrl };
                                    }
                                    else
                                    {
                                        stat.Profiles[k].Item = p;
                                        stat.Profiles[k].sub = subUrl;
                                    }
                                }
                            }
                        }
                        );
                    }
                    catch (Exception ex)
                    {
                        log.Error(String.Format("{0,-64} {1}", subUrl, ex.Message));
                    }
                    semaphore1.Release();
                }));

            }
            var count = tasks.Count;
            log.Info(String.Format("parse...waitAll...{0}", count));
            Task.WaitAll(tasks.ToArray());
            log.Info(String.Format("parse...waitAll...{0}...end", count));
        }

        public void TestAll()
        {
            log.Info(String.Format("testAll...{0}", ""));
            var tests = new List<TestItem>();
            var tasks = new List<Task>();

            var ad = " github.com/zengfr/free-vpn-subscribe";
            foreach (var profile in stat.Profiles)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        //semaphore2.WaitOne();
                        if (stat.GetStat(profile.Key) < vpnCfg.Retry)
                        {
                            var pv = profile.Value.Item;
                            int responseTime = GetTcpingTime(pv.address, pv.port, profile.Key);

                            profile.Value.delay = responseTime;
                            if (!string.IsNullOrEmpty(pv.remarks))
                            {
                                pv.remarks = VpnUtil.ReplaceUrl(pv.remarks).Trim();
                                if (pv.remarks.IndexOf(ad) < 0)
                                    pv.remarks += ad;
                            }

                            tests.Add(profile.Value);
                        }

                    }
                    catch (Exception ex)
                    {
                        log.Error("test", ex);
                        var key = profile.Key;
                        stat.SetStat(key, stat.GetStat(key) + 1);
                    }
                    //semaphore2.Release();
                }));


            }
            try
            {
                var count = tasks.Count;
                log.Info(String.Format("test...waitAll...{0}", count));
                Task.WaitAll(tasks.ToArray());
                log.Info(String.Format("test...waitAll...{0}...end", count));
                if (count > 0)
                {
                    var testsOrdered = tests.Where(t => t.delay >= 0 && t.delay < vpnCfg.DelayFilter).OrderBy(t => t.delay);
                    ToResults("", testsOrdered);
                    ToResults(EConfigType.VMess.ToString(), testsOrdered);
                    ToResults(EConfigType.VLESS.ToString(), testsOrdered);
                    ToResults(EConfigType.Shadowsocks.ToString(), testsOrdered);
                    ToResults(EConfigType.Socks.ToString(), testsOrdered);
                    ToResults(EConfigType.Trojan.ToString(), testsOrdered);
                    log.Info(String.Format("saveAll...{0}", count));

                    SaveNodes();
                    VpnUtil.ExecProcess("auto_commit_push.cmd", false, new string[] { });
                    stat.Save();

                    log.Info(String.Format("saveAll...{0}...end", count));
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }

        }
        private void SaveNodes()
        {
            var txt = string.Join(Environment.NewLine, stat.GetStatsKeys());
            File.WriteAllTextAsync(String.Format("{0}/results/{1}.txt", basePath, "bad_nodes"), txt, encoding);
        }
        private void ToResults(string type, IEnumerable<TestItem> results)
        {
            type = type.ToLower();
            var rs = results.Where(t => string.IsNullOrEmpty(type) || t.Item.configType.ToString().ToLower() == type);

            var rs2 = new List<string>();
            foreach (var t in rs)
            {
                if (string.IsNullOrEmpty(type))
                {
                    log.Info(String.Format("\t{0,24},{1,6},{2,6},{3,36},{4,64}", t.delay, t.Item.configType, t.Item.coreType, t.Item.address + ":" + t.Item.port, t.sub));
                }
                var r = ShareHandler.GetShareUrl(t.Item);
                if (r != null)
                {
                    rs2.Add(r.Trim());
                }
            }

            StringBuilder result = new StringBuilder();
            int j = 0;
            for (int i = 0; i < rs2.Count; i++)
            {
                var r = rs2[i];
                if (!string.IsNullOrEmpty(r))
                {
                    if (j > 0)
                    {
                        result.Append(lineSplit.ToCharArray());
                    }
                    result.Append(r);
                    //if (j < vpnCfg.Export)
                    //    File.WriteAllTextAsync(String.Format("{0}/results/top_{1}_{2}.txt", basePath, type, j.ToString("00")), r, encoding);
                    j++;
                }
            }
            log.Info(String.Format("results:{0,12} {1,6} {2,6}", type, rs2.Count,j));
            if (result.Length > 9 && j > 0)
            {

                var e = result.ToString();
                var ee = Utils.Base64Encode(e);
                stat.Results[type + "/raw"] = e;
                stat.Results[type] = ee;
                File.WriteAllTextAsync(String.Format("{0}/results/vpn_sub_raw_{1}.txt", basePath, type), e, encoding);
                File.WriteAllTextAsync(String.Format("{0}/results/vpn_sub_{1}.txt", basePath, type), ee, encoding);
                File.WriteAllTextAsync(String.Format("{0}/results/history/{2}/vpn_sub_raw_{1}.txt", basePath, type, DateTime.Today.ToString("yyyyMMdd")), e, encoding);
                File.WriteAllTextAsync(String.Format("{0}/results/history/{2}/vpn_sub_{1}.txt", basePath, type, DateTime.Today.ToString("yyyyMMdd")), ee, encoding);
            }
        }
        private int GetTcpingTime(string url, int port, string key)
        {
            int responseTime = -1;

            try
            {
                if (!IPAddress.TryParse(url, out IPAddress ipAddress))
                {
                    IPHostEntry ipHostInfo = System.Net.Dns.GetHostEntry(url);
                    ipAddress = ipHostInfo.AddressList[0];
                }

                Stopwatch timer = new();
                timer.Start();

                IPEndPoint endPoint = new(ipAddress, port);
                using Socket clientSocket = new(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                IAsyncResult result = clientSocket.BeginConnect(endPoint, null, null);
                var tcpingWait = vpnCfg.TcpingWait;
                if (!result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(tcpingWait)))
                {
                    stat.SetStat(key, stat.GetStat(key) + 1);
                    log.Debug(String.Format("connect timeout {0}s:{1,5} {3,5} {2,56}", tcpingWait, stat.GetStat(key), url, port));
                }
                clientSocket.EndConnect(result);

                timer.Stop();
                responseTime = timer.Elapsed.Milliseconds;
            }
            catch (Exception ex)
            {
                log.Debug(String.Format("tcping:{1,5} {0,56} {2}", url, port, ex.Message));
            }
            log.Info(String.Format("tcping:{0,2} {1,5} {3,5} {2,56}", stat.GetStat(key), responseTime, url, port));
            return responseTime;
        }

    }
}
