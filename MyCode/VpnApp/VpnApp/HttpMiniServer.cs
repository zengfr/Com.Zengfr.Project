using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VpnApp
{
     public class HttpMiniServer
    {
        public delegate String RunCmdCallback(string cmd);
        private Dictionary<string, RunCmdCallback> routers = new Dictionary<string, RunCmdCallback>();
        public void addRouter(String router, RunCmdCallback cb = null)
        {
            if (!router.EndsWith('/'))
            {
                this.routers.Add(router + '/', cb);
            }
            else
            {
                this.routers.Add(router, cb);
            }
        }
        public void runHttpServer(UInt32 port, String ip = "")
        {
            if (!System.Net.HttpListener.IsSupported)
            {
                Console.WriteLine("Windows XP SP2 or Server 2003 is required to use the HttpListener class.");
                return;
            }
            if (ip == String.Empty)
            {
                ip = "127.0.0.1";
            }
            // Create a listener.
            HttpListener listener = new HttpListener();
            foreach (KeyValuePair<string, RunCmdCallback> kp in this.routers)
            {
                listener.Prefixes.Add($"http://{ip}:{port}{kp.Key}");
            }
            listener.Start();
            Console.WriteLine($"Http Server Listening At http://{ip}:{port}...");
            while (true)
            {
                S: HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                // Obtain a response object.
                HttpListenerResponse response = context.Response;
                //response.Headers = request.Headers;
                // Construct a response.
                foreach (KeyValuePair<string, RunCmdCallback> kp in this.routers)
                {
                    if (request.RawUrl.IndexOf(kp.Key) == 0)
                    {
                        String prms = request.RawUrl.Substring(kp.Key.Length);
                        string ret = kp.Value.Invoke(prms);
                        byte[] buf = System.Text.Encoding.UTF8.GetBytes(ret);
                        response.ContentLength64 = buf.Length;
                        response.StatusCode = 200;
                        response.ContentType = "text/plain;charset=utf-8";
                        //response.KeepAlive = false;
                       // response.ContentEncoding = "";
                        System.IO.Stream ot = response.OutputStream;
                        ot.Write(buf, 0, buf.Length);
                        ot.Close();
                        goto S;
                    }

                }
                string responseString = "error";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            listener.Stop();
        }

        static void Main()
        {
            HttpMiniServer ht = new HttpMiniServer();
            //定义打印命令 
            ht.addRouter("/print/", new RunCmdCallback((string cmd) => {
                //参数
                //http://127.0.0.1:8080/print/{打印机ID或者其他参数}
                Console.WriteLine(cmd);
                return cmd;
            }));
            //关闭打印机 路由 
            ht.addRouter("/close/", new RunCmdCallback((string cmd) => {

                //参数
                //http://127.0.0.1:8080/close/{打印机ID或者其他参数}
                Console.WriteLine(cmd);
                return cmd;
            }));
            ht.runHttpServer(8080, "127.0.0.1");
        }
 

    }
}
