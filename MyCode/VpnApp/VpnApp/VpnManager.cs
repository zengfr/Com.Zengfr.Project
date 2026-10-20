using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VpnApp.HttpMiniServer;

namespace VpnApp
{
    public class VpnManager
    {
        private static ILog log = LogManager.GetLogger(typeof(VpnManager));
        private static readonly Lazy<VpnManager> _instance = new(() => new());
        public static VpnManager Instance => _instance.Value;
        static VpnManager()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(@"log4net.config"));
        }

        public void Start()
        {


            Console.WindowWidth = 128;
            Console.BufferWidth = Console.WindowWidth * 2;
            log.Info("start vpn");

            var vpnMan = VpnMan.Instance;
            VpnCfg.Instance.Ip = VpnUtil.GetLocalIP();
            StartServer(vpnMan);
        }
        private void StartServer(VpnMan vpnMan)
        {
            HttpMiniServer ht = new HttpMiniServer();
            //定义打印命令 
            ht.addRouter("/vpn/", new RunCmdCallback((string cmd) =>
            {
                return "" + VpnStat.Instance.GetResult(cmd);
            }));
            ht.runHttpServer(VpnCfg.Instance.ServerPort, VpnCfg.Instance.Ip);
        }
    }
}
