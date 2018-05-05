using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace Zfrong.Framework.Mvc.Result
{
   
    public class JsonNetResult : System.Web.Mvc.JsonResult
    {
        bool jsonp = false;
        public JsonNetResult(bool jsonp)
        {
            this.jsonp = jsonp;
        }
        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }
            if (Data != null)
            {
                HttpRequestBase request = context.HttpContext.Request;
                WriteJson(Data, context.HttpContext, jsonp, true);
              
            }
        }
        public static JsonSerializerSettings DefualtSerializerSettings { get; set; }
        public static Formatting DefualtFormatting { get; set; }
        public static void Write(string Data, HttpContextBase httpContext, bool flush)
        {
            httpContext.Response.Write(Data);
            if (flush)
               httpContext.Response.Flush();
        }
        public static void WriteJson(object Data, HttpContextBase httpContext,bool jsonp,bool flush)
        {
            HttpResponseBase response = httpContext.Response;
            HttpRequestBase request = httpContext.Request;

            JsonTextWriter writer = new JsonTextWriter(response.Output) { Formatting = DefualtFormatting };
            JsonSerializer serializer = JsonSerializer.Create(DefualtSerializerSettings);
            IsoDateTimeConverter converter = new IsoDateTimeConverter();
            converter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            serializer.Converters.Add(converter);
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //serializer.NullValueHandling = NullValueHandling.Ignore;
            
            //var ur = httpContext.Items["ur"];
           // if (ur != null && ur.ToString().IndexOf("view") == -1)
               // serializer.ContractResolver = new NHibernateContractResolver(new string[] { "Contact" });
            if (jsonp)
            {
                response.Write(request.Params["callback"] + "(");//jsoncallback
                serializer.Serialize(writer, Data);
                response.Write(");");
            }
            else
            {
                serializer.Serialize(writer, Data);
            }
            if(flush)
                writer.Flush();
        }
        public static void TestWriteJson(object Data)
        {
            JsonTextWriter writer = new JsonTextWriter(Console.Out) { Formatting = DefualtFormatting };
            JsonSerializer serializer = JsonSerializer.Create(DefualtSerializerSettings);
            IsoDateTimeConverter converter = new IsoDateTimeConverter();
            converter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            serializer.Converters.Add(converter);
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializer.Serialize(writer, Data);
        }
    }
}