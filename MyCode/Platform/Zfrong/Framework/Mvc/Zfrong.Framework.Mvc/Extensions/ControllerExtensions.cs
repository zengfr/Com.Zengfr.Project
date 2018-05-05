using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zfrong.Framework.Mvc.Result;
using System.Web.Mvc;
namespace Zfrong.Framework.Mvc.Extensions
{
    public static class ControllerExtensions
    {
        public static JsonNetResult Jsonp(this JsonResult json)
        {
            json.ContentType = "application/javascript";
            return new JsonNetResult(true) { ContentEncoding = json.ContentEncoding, ContentType = json.ContentType, Data = json.Data, JsonRequestBehavior = json.JsonRequestBehavior };
        }
        public static JsonNetResult JsonHTML(this JsonResult json)
        {
            json.ContentType = "text/html";
            return JsonNet(json);
        }
        public static JsonNetResult JsonText(this JsonResult json)
        {
            json.ContentType = "text/plain";
            return JsonNet(json);
        }
        public static JsonNetResult JsonNet(this JsonResult json)
        {
            return new JsonNetResult(false) { ContentEncoding = json.ContentEncoding, ContentType = json.ContentType, Data = json.Data, JsonRequestBehavior = json.JsonRequestBehavior };
        }
    }
}
