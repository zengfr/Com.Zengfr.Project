using System;
using System.Text.RegularExpressions;
 
using Spring.Http.Client;
using Spring.Http.Converters;
using Spring.Http.Converters.Json;
using Spring.Rest.Client;
using Spring.Http;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;

namespace Spring.Rest.Utils
{
    public static class StringUtils
    {
        public static int? ToInt(this string value)
        {
            int v = 0;
            if (!string.IsNullOrEmpty(value))
            {
                int.TryParse(value, out v);
                return v;
            }
            return null;
        }
        public static decimal? ToDecimal(this string value)
        {
            decimal v = 0;
            if(!string.IsNullOrEmpty(value))
            {
                decimal.TryParse(value,out v);
                return v;
            }
            return null;
        }
        public static float? ToFloat(this string value)
        {
            float v = 0;
            if (!string.IsNullOrEmpty(value))
            {
                float.TryParse(value, out v);
                return v;
            }
            return null;
        }
        public static double? ToDouble(this string value)
        {
            double v = 0;
            if (!string.IsNullOrEmpty(value))
            {
                double.TryParse(value, out v);
                return v;
            }
            return null;
        }
        public static DateTime? ToDateTime(this string value)
        {
            DateTime v = new DateTime(1900,01,01);
            if (!string.IsNullOrEmpty(value))
            {
                DateTime.TryParse(value, out v);
                return v;
            }
            return null;
        }
    }
    public class RestTemplateUtils
    {
        public static RegexOptions RegexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase |
                                                 RegexOptions.Singleline;
        protected static Random Random = new Random();
        public class RestWebClientHttpRequestFactory : WebClientHttpRequestFactory
        {
            public override IClientHttpRequest CreateRequest(Uri uri, Spring.Http.HttpMethod method)
            {
               var webClientHttpRequest = base.CreateRequest(uri, method) as Spring.Http.Client.WebClientHttpRequest;
               webClientHttpRequest.HttpWebRequest.Proxy = null;
               webClientHttpRequest.HttpWebRequest.ServicePoint.ConnectionLimit = 65534;
               webClientHttpRequest.HttpWebRequest.ConnectionGroupName = "Rest";
               webClientHttpRequest.HttpWebRequest.Timeout = 1000 * 40;
                webClientHttpRequest.HttpWebRequest.ReadWriteTimeout = 1000 * 40;
                while (webClientHttpRequest.HttpWebRequest.ServicePoint.CurrentConnections > 25)
                {
                    Thread.Sleep(1000);
                }
                return webClientHttpRequest;
            }
        }
        public class StringExHttpMessageConverter : StringHttpMessageConverter
        {
            protected override Encoding GetContentTypeCharset(MediaType contentType, Encoding defaultEncoding)
            {
                if (contentType != null && contentType.CharSet != null)
                {
                    return contentType.CharSet;
                }
                if (contentType != null&& this.SupportedMediaTypes!=null)
                {
                    var contentType2 = this.SupportedMediaTypes.FirstOrDefault(
                        t => t.Type == contentType.Type && t.Subtype == contentType.Subtype);
                    if (contentType2 != null && contentType2.CharSet != null)
                        return contentType2.CharSet;
                }
                return base.GetContentTypeCharset(contentType, defaultEncoding);
            }   
        }
        public static RestTemplate BuildRestTemplate(string url)
        {
            return BuildRestTemplate(url,null);
        }
        public static RestTemplate BuildRestTemplate(string url, string encoding)
        {
            var requestFactory = new RestWebClientHttpRequestFactory();
             
            RestTemplate template = new RestTemplate(url);
            template.MessageConverters.Add(new FormHttpMessageConverter());
            template.MessageConverters.Add(new StringExHttpMessageConverter());
            template.MessageConverters.Add(new DataContractJsonHttpMessageConverter());

            template.RequestInterceptors.Add(new PerfRequestSyncInterceptor());
            template.RequestInterceptors.Add(new PerfRequestAsyncInterceptor());
            template.RequestInterceptors.Add(new PerfRequestBeforeInterceptor());
            template.RequestFactory = requestFactory;

            if (!string.IsNullOrEmpty(encoding))
            {
                var converter = new StringExHttpMessageConverter();
                converter.SupportedMediaTypes.Clear();
                converter.SupportedMediaTypes.Add(new MediaType("text", "html", System.Text.Encoding.GetEncoding(encoding)));
                converter.SupportedMediaTypes.Add(new MediaType("application", "vnd.ms-excel", System.Text.Encoding.GetEncoding(encoding)));
                 
                template.MessageConverters.Clear();
                template.MessageConverters.Add(converter);
            }
            return template;
        }
    }
}
