using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
namespace Zfrong.Framework.Mvc.Binder
{
   public class JsonBinder:DefaultModelBinder, IModelBinder
  {
      
      public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
      {
          if (!IsJSONRequest(controllerContext))
          {
              return base.BindModel(controllerContext, bindingContext);
          }
          if(controllerContext.HttpContext.Request.InputStream.CanSeek)
              controllerContext.HttpContext.Request.InputStream.Seek(0,SeekOrigin.Begin);
          StreamReader reader = new StreamReader(controllerContext.HttpContext.Request.InputStream);
          string json = reader.ReadToEnd(); reader.Close(); reader = null;
  
          if (string.IsNullOrEmpty(json))
              return json;

          JavaScriptSerializer serializer = new JavaScriptSerializer();
         //object Result = serializer.DeserializeObject(json);
         //Result<List<User>> d = serializer.Deserialize<Result<List<User>>>(json);
         //return serializer.DeserializeObject(json);
         return serializer.Deserialize(json, bindingContext.ModelMetadata.ModelType);
         //return serializer.Deserialize<T>(json);
     }
      private static bool IsJSONRequest(ControllerContext controllerContext)
      {
          var contentType = controllerContext.HttpContext.Request.ContentType;
          return contentType.Contains("application/json");
      }
 }
}