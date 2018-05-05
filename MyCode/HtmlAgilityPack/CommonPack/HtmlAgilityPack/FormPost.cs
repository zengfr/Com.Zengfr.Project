using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Web;
namespace CommonPack.HtmlAgilityPack
{
    /// <summary>
    /// 
    /// </summary>
    public class ParamValue
    {
        /// <summary>
        /// 原值
        /// </summary>
        public string SValue="";
        /// <summary>
        /// 目标值
        /// </summary>
        public string TValue="";
        /// <summary>
        /// 原值，目标值
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public ParamValue(string v1, string v2)
        {
            SValue = v1;
            TValue = v2;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public struct EncodingType
    {
     public const string  GB2312="gb2312";
     public const string  UTF8="utf-8";
    }
    /// <summary>
    /// form提交类
    /// </summary>
   public class FormPost
    {
        string _url;
        public string Url
        {
            set
            {
                _url = value;
            }
        }
        string _referer;
        public string Referer
        {
            set
            {
                _referer = value;
            }
        }
       Dictionary<string, ParamValue> _params = new Dictionary<string, ParamValue>();
        public void SetParam(string key, string value)
        {
            if (!_params.ContainsKey(key))
            {
                _params.Add(key, new ParamValue(null,value));//
            }
            else
            {
                _params[key].TValue = value;
            }
        }
        string encodingType;
        public string EncodingType
        {
            set
            {
                encodingType = value;//
            }
        }
       public void GetParams(int formIndex)
       {
           DocumentWithForm f = new DocumentWithForm(this._referer,formIndex);//
           f.GetParams();//
           this._url = f.PostUrl;//
           foreach (KeyValuePair<string, string> k in f.Params)
           {
               this._params.Add(k.Key, new ParamValue(k.Value,k.Value));//
           }
           f = null;//
       }
       public void AutoSetParamValue(string name,string content)
       {
           IEnumerator<string> e=this._params.Keys.GetEnumerator();//
           while(e.MoveNext())
           {
               string k =e.Current.ToLower();
               if (this._params[e.Current].SValue == "" || this._params[e.Current].SValue == null)
               {
                   if (k.IndexOf("content") != -1)
                       this._params[e.Current].TValue = content;
                   if (k.IndexOf("size") != -1)
                       this._params[e.Current].TValue = "12";
                   //if (k.IndexOf("name") != -1)
                       //this._params[e.Current].TValue = name;
                   if (k.IndexOf("title") != -1)
                       this._params[e.Current].TValue = name;
               }
           }
       }
        public void PostDate()
        {
            //建立登录检查地址 
            //建立request对象  
            HttpWebRequest req = (HttpWebRequest)System.Net.HttpWebRequest.Create(this._url);
            //这个新建的Cookie集合不知道有什么用？？  
            req.CookieContainer = new System.Net.CookieContainer();
            req.Method = "POST";//POST方式请求  
            req.ContentType = "application/x-www-form-urlencoded";//内容类型  
            req.Referer = this._referer;
           
            req.Headers["HTTP_REFERER"] = this._referer;
            //参数经过URL编码  
            string paraUrlCoded = "";
            
            Encoding encoding=Encoding.Default;
            if(encodingType!=null)
            {
               encoding = Encoding.GetEncoding(encodingType.ToString()); 
            }

            foreach (KeyValuePair<string,ParamValue> k in this._params)
            {
                paraUrlCoded += "&" + k.Key + "=" + HttpUtility.UrlEncode(k.Value.TValue,encoding);
            }
            //paraUrlCoded = System.Web.HttpUtility.UrlEncode(paraUrlCoded);//
            //将URL编码后的字符串转化为字节  
            try
            {
                byte[] payload=new byte[0];
                payload = encoding.GetBytes(paraUrlCoded); 
                   
                req.ContentLength = payload.Length;   //设置请求的ContentLength    
                System.IO.Stream writer = req.GetRequestStream();//获得请求流    
                writer.Write(payload, 0, payload.Length);//将请求参数写入流  
                writer.Close();//关闭请求流
                writer.Dispose();//

                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;//'以下获取服务器返回信息
                
                StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.Default, true);//
                string respHTML = reader.ReadToEnd();//
                reader.Close(); reader.Dispose();//

                Console.Write(resp.StatusCode.ToString());//
                resp.Close();//　'关闭
                encoding = null;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);//
            }
            //获得响应流  
        }   
    }
}
