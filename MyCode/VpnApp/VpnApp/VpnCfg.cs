using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpnApp
{
    public class VpnCfg
    {
        private static readonly Lazy<VpnCfg> _instance = new(() => new());
        public static VpnCfg Instance => _instance.Value;
        public uint ServerPort { get; set; } = 8080;
        public string CfgFile1 { get; set; } = "sub1.txt";
        public string CfgFile2 { get; set; } = "sub2.txt";
        /// <summary>
        ///6
        /// </summary>
        public int Concurrency { get; set; } = 15;

        /// <summary>
        /// 60*3
        /// </summary>
        public int Period { get; set; } = 60*30;

        public int Export { get; set; } = 12;

        public int DelayFilter { get; set; } = 1000;
        public int Retry { get; set; } = 5;
        public int TcpingWait { get; set; } = 3;
        public string Ip { get; set; } = "127.0.0.1";
    }
}
