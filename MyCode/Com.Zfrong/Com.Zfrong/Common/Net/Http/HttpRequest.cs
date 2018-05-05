using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net;
using System.Xml;
using Sgml;
using System.Text.RegularExpressions;
using System.Drawing;
//using Com.Zfrong.Common.Command;//
using System.Runtime.InteropServices;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System.Threading;
//using NetServ.Net.Json;
using Com.Zfrong.Common.Net.Http;
using Com.Zfrong;
using Com.Zfrong.Common.Extensions;
namespace Com.Zfrong.Common.Net.Http
{
    public interface IRequestPlugin
    {
        string Url { get; set; }
        string Cookies { get; set; }
        string Data { get; set; }
        bool IsMultiPartFormData { get; set; }
        string Encoding { get; set; }
        string Referer { get; set; }
        Dictionary<string,string> KV { get; set; }
        bool Run();
    }
    /// <summary>
    /// 作者：曾繁荣 2008.11
    /// </summary>
    public class Common
    {
        private static RegexOptions Options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;
        private static Regex Regex_IP = new Regex(@"\b(([01]?\d?\d|2[0-4]\d|25[0-5])\.){3}([01]?\d?\d|2[0-4]\d|25[0-5])\b", Options);//
        public static object JsEval(string Expression)
        {
            object result = null;
            try
            {
                Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
                result = Microsoft.JScript.Eval.JScriptEvaluate(Expression, ve);
            }
            catch (Exception e)
            {
                return 0;
                throw new System.Exception("[错误]表达式" + Expression + "错误:" + e.Message);
            }
            return result;
        }
        public static void SetHeaderValue(WebHeaderCollection header, string name, string value)
        {
            var property = typeof(WebHeaderCollection).GetProperty("InnerCollection",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (property != null)
            {
                var collection = property.GetValue(header, null) as NameValueCollection;
                collection[name] = value;
            }
        }
        public static string GetCookies(string defaultCookies, string rtn)
        {
            string cookies = "";
            bool isnew = false;
            if (rtn != null)
            {
                int i1, i2;
                if (rtn.IndexOf("cookies") != -1)
                {
                    i1 = rtn.IndexOf("<cookies>") + 9;
                    i2 = rtn.IndexOf("</cookies>");
                    if (i2 > i1)
                    {
                        cookies += rtn.Substring(i1, i2 - i1);//
                        isnew = true;
                    }
                }
            }
            if (!isnew)
            {
                cookies = string.Format(defaultCookies, new Random(DateTime.Now.Millisecond).Next(100, 999));
            }
            return cookies;
        }
        private static void GetAndSetCookies(ref string header, StringBuilder cookies)
        {
            Match m = Regex.Match(header, @"Set-Cookie: ((\S+)=([^;]*));?");
            Match m2;
            string cookieName;
            while (m.Success)
            {
                while (m.Success)
                {
                    cookieName = m.Groups[2].Value;
                    m2 = Regex.Match(cookies.ToString(), cookieName + "=" + @"([^;]*);?");
                    if (!m2.Success)
                        cookies.Append(";" + m.Groups[1].Value);
                    else if (!m2.Groups[1].Value.Equals(m.Groups[3].Value))
                        cookies.Replace(m2.Groups[1].Value, m.Groups[3].Value);
                    m = m.NextMatch();
                }
            }
            if (cookies.ToString().IndexOf(";") == 0)
                cookies.Remove(0, 1);
            m = null; m2 = null; cookieName = null;
            //return cookies;
        }
        public static System.IO.Stream GetStream(HttpWebResponse HWResp)
        {
            System.IO.Stream stream1 = null;
            if (HWResp.ContentEncoding == "gzip")
            {
                stream1 = new GZipInputStream(HWResp.GetResponseStream());
            }
            else if (HWResp.ContentEncoding == "deflate")
            {
                stream1 = new InflaterInputStream(HWResp.GetResponseStream());
            }
            if (stream1 == null)
            {
                return HWResp.GetResponseStream();
            }
            else
                return stream1;
            //System.IO.MemoryStream stream2 = new System.IO.MemoryStream();
            //int count = 0x800;
            //byte[] buffer = new byte[0x800];
            //goto A;
            //A:
            //count = stream1.Read(buffer, 0, count);
            //if (count > 0)
            //{
            //    stream2.Write(buffer, 0, count);
            //    goto A;
            //}
            //stream2.Seek((long)0, SeekOrigin.Begin);
            //return stream2;
        }
        public static XmlDocument ToXml(string source)
        {
            XmlDocument xmldom = new XmlDocument();
            SgmlReader r = new SgmlReader();
            r.DocType = "HTML";
            r.InputStream = new System.IO.StringReader(source);
            xmldom.Load(r); r.Close(); r = null;
            return xmldom;//
        }
      
        public static string KVToString(IEnumerable<KeyValuePair<string, string>> data)
        {
            string dataStr = "";
            foreach (KeyValuePair<string, string> kv in data)
                dataStr += "&" + kv.Key + "=" + kv.Value;// System.Web.HttpUtility.UrlEncode(kv.Value);
            if (dataStr.Length != 0) dataStr = dataStr.Substring(1);//
            return dataStr;
        }
        public static IEnumerable<KeyValuePair<string, string>> StringToKV(string dataStr, Encoding encoding)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();//
            NameValueCollection nv = System.Web.HttpUtility.ParseQueryString("http://1.2.com/3.aspx?" + dataStr, encoding);//
            foreach (string k in nv.Keys)
            {
                data.Add(k, nv[k]);//
            }
            nv.Clear(); nv = null;
            return data;//
        }

        #region multipart/form-data
        public const string MultiPartFormData_ContentType = "multipart/form-data; boundary=" + BOUNDARY;//
        private const string BOUNDARY = "---------------------------714a6d158c9";//
        public static string Build_MultiPartFormData(string dataStr, Encoding encoding)
        {
            return Build_MultiPartFormData(StringToKV(dataStr, encoding));//
        }
        private static string Build_MultiPartFormData(IEnumerable<KeyValuePair<string, string>> data)
        {
            StringBuilder sb = new StringBuilder();//
            foreach (KeyValuePair<string, string> kv in data)
            {
                sb.Append("--");
                sb.Append(BOUNDARY);
                sb.Append("\r\n");
                sb.Append("Content-Disposition: form-data; name=\"" + kv.Key + "\"\r\n\r\n");
                sb.Append(kv.Value);
                //sb.Append(System.Web.HttpUtility.UrlEncode(kv.Value));
                sb.Append("\r\n");
            }
            sb.Append("--" + BOUNDARY + "--\r\n");
            return sb.ToString();//
        }
        #endregion

        #region ####音效
        public static void Beep()
        {
            System.Media.SystemSounds.Beep.Play();//
        }
        [DllImport("kernel32.dll", EntryPoint = "Beep")]
        public static extern bool Beep(int freq, int dur);
        public enum BeepType
        {
            SimpleBeep = -1,
            IconAsterisk = 0x00000040,
            IconExclamation = 0x00000030,
            IconHand = 0x00000010,
            IconQuestion = 0x00000020,
            Ok = 0x00000000,
        }
        [DllImport("user32.dll", EntryPoint = "MessageBeep")]
        public static extern bool MessageBeep(BeepType beepType);
        #endregion

        public static void Show(string str)
        {
            //Com.Zfrong.Common.Command.CommandBase cmd = CommandManager.GetCommand(CommandFamily.ShowMessage);
            //cmd.Execute(null, str);
            Console.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("yyMMdd HH:mm:ss fff"), str));//
        }

        [DllImport("wininet.dll", EntryPoint = "InternetGetConnectedState",CharSet=CharSet.Auto,SetLastError=true)]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);
        public static bool IsConnected
        {
            get
            {
                int I = 0;
                bool state = InternetGetConnectedState(out I, 0);
                return state;
            }
        }
       
    }
    /// <summary>
    /// 作者：曾繁荣 2008.11
    /// </summary>
    public class Form
    {
        #region ####获取表单提交必需参数 2
        
        public static ICollection<KeyValuePair<string, string>> GetDictionary(string url, string cookieHeader, string encoding, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            return GetDictionary(SyncHttpRequest.Get(url, cookieHeader, System.Text.Encoding.GetEncoding(encoding), strReferer,userAgent,authorization,proxy));
        }
        public static ICollection<KeyValuePair<string, string>> GetDictionary(string url, string cookieHeader, Encoding encoding, string strReferer, string userAgent, string authorization, IWebProxy proxy)
        {
            return GetDictionary(SyncHttpRequest.Get(url, cookieHeader, encoding, strReferer,userAgent,authorization,proxy));
        }
        public static ICollection<KeyValuePair<string, string>> GetDictionary(string source)
        {
            ICollection<KeyValuePair<string, string>> dict = new Dictionary<string, string>();
            RegexOptions op = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;//
            MatchCollection mColl; int index = 0;//
            mColl = Regex.Matches(source, "<form.*?</form>", op); source = null;//
            Common.Show(string.Format("表单-->匹配:{0}个", mColl.Count));//
            string tmp;
            if (mColl.Count != 0)
            {
                if (mColl.Count != 1)
                {
                    index = -1;
                    foreach (Match m in mColl)
                    {
                        index++;
                        tmp = m.Groups[0].Value.ToLower().Replace(" ", "");//
                        if (tmp.IndexOf("method") != -1)
                            if (tmp.IndexOf("action") != -1)
                            {
                                if (tmp.IndexOf("搜索") != -1)
                                    continue;//
                                if (tmp.IndexOf("发表") != -1)
                                    break;//
                                if (tmp.IndexOf("提交") != -1)
                                    break;//
                                if (tmp.IndexOf("添加") != -1)
                                    break;//
                                //if (tmp.IndexOf("login") == -1)
                                //break;//
                            }
                    }
                }
                Common.Show(string.Format("表单-->Index:{0}", index));//
                XmlDocument xml = Common.ToXml(mColl[index].Groups[0].Value);

                XmlNodeList list;
                Common.Show(string.Format("表单-->Action:{0}", xml.SelectSingleNode("//*/@action[.]")));//
                Common.Show(string.Format("表单-->Enctype:{0}", xml.SelectSingleNode("//*/@enctype[.]")));//
                list = xml.SelectNodes("//textarea");//
                GetKV(xml, list, dict);//
                list = xml.SelectNodes("//select");//
                GetKV(xml, list, dict);//
                list = xml.SelectNodes("//input");//
                GetKV(xml, list, dict);//
                xml = null; list = null;
            }
             mColl = null; tmp = null;
            return dict;
        }
        private static void GetKV(XmlDocument xml, XmlNodeList list, ICollection<KeyValuePair<string, string>> dict)
        {
            XmlAttribute t1, t2, t3;
            foreach (XmlNode node in list)
            {
                t1 = GetAttribute(node.Attributes, "name");
                t2 = GetAttribute(node.Attributes, "id");
                t3 = GetAttribute(node.Attributes, "value");
                if (t3 == null)
                    t3 = xml.CreateAttribute("value");//
                if (t1 != null)
                {
                    if (!ContainsKey(dict, t1.Value))
                    {
                        dict.Add(new KeyValuePair<string, string>(t1.Value, t3.Value));
                    }
                }
                else if (t2 != null)
                {
                    if (!ContainsKey(dict, t2.Value))
                    {
                        dict.Add(new KeyValuePair<string, string>(t2.Value, t3.Value));
                    }
                }
            }
            t1 = null; t2 = null; t3 = null;
        }
        private static XmlAttribute GetAttribute(XmlAttributeCollection coll, string name)
        {
            foreach (XmlAttribute att in coll)
            {
                if (att.Name.ToLower() == name)
                    return att;
            }
            return null;//
        }
        private static bool ContainsKey(ICollection<KeyValuePair<string, string>> dict, string key)
        {
            foreach (KeyValuePair<string, string> kv in dict)
            {
                if (kv.Key == key) return true;//
            }
            return false;//
        }
        private static KeyValuePair<string, string> GetKV(ICollection<KeyValuePair<string, string>> dict, string key)
        {
            foreach (KeyValuePair<string, string> kv in dict)
            {
                if (kv.Key == key) return kv;//
            }
            return new KeyValuePair<string, string>();//
        }
        #endregion
    }
    /// <summary>
    /// 作者：曾繁荣 2008.11
    /// </summary>
    public class Proxy
    {
        #region ####Proxy代理
        public static IWebProxy BuildProxy(string host, int port)
        {
            return BuildProxy(host, port, null);//
        }
        public static IWebProxy BuildProxy(string host, int port, ICredentials ic)
        {
            WebProxy proxy = new WebProxy("http://" + host + ":" + port, true, null, ic);//?
            return proxy;
        }
        public static IWebProxy BuildProxy(string host, int port, string userName, string passWord)
        {
            ICredentials ic = new NetworkCredential(userName, passWord);
            //CredentialCache.DefaultCredentials;
            //GlobalProxySelection.Select = _WP;
            return BuildProxy(host, port, ic);//
        }
        public static IWebProxy BuildProxy(string host, int port, string userName, string passWord, string domain)
        {
            ICredentials ic = new NetworkCredential(userName, passWord, domain);
            //CredentialCache.DefaultCredentials;
            //GlobalProxySelection.Select = _WP;
            return BuildProxy(host, port, ic);//
        }
        #endregion
    }
  
    public abstract partial  class HttpRequest
{

}

    
}

