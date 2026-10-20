using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VpnApp
{
    public class VpnUtil
    {
        private static ILog log = LogManager.GetLogger(typeof(VpnUtil));
        public static string GetLocalIP()
        {
            string localIP = string.Empty;
            try
            {

                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localIP = endPoint.Address.ToString();
                }
            }
            catch (Exception ex)
            {
                log.Error("", ex);
            }
            return localIP;
        }
        public static string ReplaceUrl(string s)
        {
            Regex re = new Regex(@"(?<url>http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?)");
            MatchCollection mc = re.Matches(s);
            foreach (Match m in mc)
            {
                s = s.Replace(m.Result("${url}"), String.Format("<a href='{0}'>{0}</a>", m.Result("${url}")));
            }
            return s;
        }
        public static void ExecProcess(String cmd, bool shell, String[] args)
        {
            try
            {

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var arguments = string.Join(" ", args.Where(x => !string.IsNullOrWhiteSpace(x)));
                var processStartInfo = new ProcessStartInfo(cmd, arguments)
                {
                    CreateNoWindow = !shell,
                    WindowStyle = shell ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = !shell,
                    UseShellExecute = shell,
                    WorkingDirectory = basePath
                };
                log.Info(String.Format("cmd:{0} args:{1}", cmd, arguments));
                var process = Process.Start(processStartInfo);

                if (process != null)
                {
                    process.Exited += (sender, eventArgs) =>
                    {
                        //// only restart once, do not go crazy if device cannot be viewed
                        //if (bitrate == null)
                        //{
                        //    if (process.ExitCode != 0)
                        //    {
                        //        ViewDevice(device, mirror, bitrate: 80000);
                        //    }
                        //}
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error("cmd:" + cmd, ex);
            }
        }
    }
}
