using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
using Newtonsoft.Json.Serialization;
namespace Com.Zengfr.Proj.Common.Web
{

    public class NewtonJsonResult : JsonResult
    {
        public JsonSerializerSettings JsonSerializerSettings { get; set; }
        public NewtonJsonResult()
        {
            // this.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }
        public NewtonJsonResult(object obj)
        {
            //this.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            this.Data = obj;
        }
        public NewtonJsonResult(object obj, JsonSerializerSettings jsonSerializerSettings)
        {
            // this.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            this.Data = obj;
            this.JsonSerializerSettings = jsonSerializerSettings;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            //if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) && (string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase)))
            //{
            //    throw new InvalidOperationException("改方法当前不允许使用Get");
            //}
            HttpResponseBase response = context.HttpContext.Response;
            if (context.HttpContext.Request.HttpMethod != "GET")
            {
                response.Expires = -1;
                response.CacheControl = "no-cache";
            }
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                string strJson = JsonConvert.SerializeObject(this.Data, JsonSerializerSettings);
                response.Write(strJson);
                response.End();
            }
        }
    }


    public static class ControllerExtend
    {
        public static JsonResult NewtonJson(this Controller controller, object obj)
        {
            return NewtonJson(controller, null, null, JsonRequestBehavior.DenyGet, obj, null);
        }
        public static JsonResult NewtonJson(this Controller controller, object obj, IContractResolver contractResolver)
        {
            return NewtonJson(controller, null, null, JsonRequestBehavior.DenyGet, obj, contractResolver);
        }
        public static JsonResult NewtonJson(this Controller controller, Encoding encoding, string contentType, JsonRequestBehavior jsonRequestBehavior, object obj, IContractResolver contractResolver)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                MaxDepth = 10,
                DateFormatString = "yyyy-MM-dd HH:mm",
                //NullValueHandling = NullValueHandling.Ignore,
                //DefaultValueHandling = DefaultValueHandling.Ignore,
                Converters = new List<JsonConverter> { new DecimalConverter() }
            };
            if (contractResolver != null)
            {
                jsonSerializerSettings.ContractResolver = contractResolver;
            }
            return new NewtonJsonResult()
            {
                JsonSerializerSettings = jsonSerializerSettings,
                ContentEncoding = encoding,
                ContentType = contentType,
                JsonRequestBehavior = jsonRequestBehavior,
                Data = obj
            };
        }
        public static JsonResult NewtonJson(this Controller controller, object obj, JsonRequestBehavior jsonRequestBehavior)
        {
            return NewtonJson(controller, null, null, jsonRequestBehavior, obj, null);
        }
        public static JsonResult NewtonJson(this Controller controller, object obj, Encoding encoding, JsonRequestBehavior jsonRequestBehavior)
        {
            return NewtonJson(controller, encoding, null, jsonRequestBehavior, obj, null);
        }
        public static JsonResult NewtonJson(this Controller controller, object obj, string contentType)
        {
            return NewtonJson(controller, null, contentType, JsonRequestBehavior.DenyGet, obj, null);
        }
        public static JsonResult NewtonJson(this Controller controller, object obj, Encoding encoding)
        {
            return NewtonJson(controller, encoding, null, JsonRequestBehavior.DenyGet, obj, null);
        }
        public static JsonResult NewtonJson(this Controller controller, object obj, Encoding encoding, string contentType)
        {
            return NewtonJson(controller, encoding, contentType, JsonRequestBehavior.DenyGet, obj, null);
        }
        public static JsonResult NewtonJson(this Controller controller, object obj, Encoding encoding, string contentType, JsonRequestBehavior jsonRequestBehavior)
        {
            return NewtonJson(controller, encoding, contentType, jsonRequestBehavior, obj, null);
        }
    }
}