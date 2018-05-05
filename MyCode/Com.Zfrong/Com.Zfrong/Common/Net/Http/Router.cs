using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Zfrong.Common.Net.Http
{
   public interface IRouter
    {
        string IP{set;get;}
        string Authorization{set;get;}
        string Disconnect();
        string Connect();
    }
    public class TP_LINK:IRouter
    {
        string ip;
        string authorization;
        static Encoding encoding = Encoding.GetEncoding("gb2312");
        public string Disconnect()
        {
            string url = "";
            url = string.Format("http://{0}/userRpm/StatusRpm.htm?Disconnect=%B6%CF%20%CF%DF", ip);
            string referer = "";
            referer = string.Format("http://{0}/userRpm/StatusRpm.htm", ip);//
            string rtn = "";
            rtn = SyncHttpRequest.Get(url, null, encoding, referer, null, "Basic " + authorization, null);
            url = null; referer = null;
            return rtn;
        }

        #region IRouter Members

        public string IP
        {
            get
            {
                return ip;
            }
            set
            {
                ip = value;
            }
        }

        public string Authorization
        {
            get
            {
                return authorization;
            }
            set
            {
                authorization = value;
            }
        }

        #endregion

        #region IRouter Members


        public string Connect()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
   public class IPPVHelper
    { 
      public IRouter router;
      static Random random = new Random();
      public  bool doEnd = false;
        /// <summary>
      /// 0=刷PV\t1=换IP+刷PV\t2=换IP
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="min"></param>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="referer"></param>
        /// <returns></returns>
      public string Run(int mode,int min, string url,string encoding, string referer)
      {
          int s = 1;
          do
          {
              switch (mode)
              {
                  case 0: //PV
                      SyncHttpRequest.Get(url, null, encoding, referer,null, null, null);
                      break;
                  case 1://IP,PV
                      router.Disconnect();
                      System.Threading.Thread.Sleep(1000);//
                      SyncHttpRequest.Get(url, null, encoding, referer,null, null, null);
                      break;
                  case 2: //IP
                     router.Disconnect();
                      break;
              }
              s=getRandom(min);
              Console.WriteLine(string.Format("暂停...{0}分钟", s));//
              System.Threading.Thread.Sleep(60000*s);//
          } while (!doEnd);
          return null;
      }
      static int getRandom(int max)
      {
          return random.Next(1, max);//
      }
    }
}
