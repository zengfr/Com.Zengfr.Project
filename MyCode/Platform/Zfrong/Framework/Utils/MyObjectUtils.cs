using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Zfrong.Framework.Core.DataContract;
namespace Zfrong.Framework.Utils
{
   public class MyObjectUtils
    {
        #region
        public static void BindProperties<S>(S targetObj, S sourceObj)
        {
            BindProperties<S, S>(targetObj, sourceObj);
        }
        public static void BindProperties<S, R>(R targetObj, S sourceObj)
        {
            Type t = typeof(R);
            PropertyInfo[] properties = t.GetProperties();
            object v;
            foreach (PropertyInfo p in properties)
            {
                if (p.CanWrite)
                {
                    v = p.GetValue(sourceObj, null);
                    if (v != null)
                    {
                        p.SetValue(targetObj, v, null);
                    }
                }
            }
        }
        public static void BindDictionaryProperties(object targetObj, IDictionary<string, object> data)
        {
            BindDictionaryProperties(targetObj, data, null);
        }
        public static void BindDictionaryProperties(object targetObj, IDictionary<string, object> data, string[] exceptReys)
        {
            if (targetObj == null) return;
            Type t = targetObj.GetType();
            PropertyInfo p; object value; TypeConverter conv;
            foreach (KeyValuePair<string, object> kv in data)
            {
                if (kv.Key.IndexOf('.') != -1) continue;
                p = t.GetProperty(kv.Key);

                if (p != null && p.CanWrite)
                {
                    conv = TypeDescriptor.GetConverter(p.PropertyType,true);
                    if (kv.Value is IDictionary<string, object>)
                    {
                        value = p.GetValue(targetObj, null);
                        if(value==null)
                        { 
                            System.Reflection.Assembly a = System.Reflection.Assembly.GetAssembly(p.PropertyType); 
                            value=a.CreateInstance(p.PropertyType.FullName);
                        }
                        BindDictionaryProperties(value, kv.Value as Dictionary<string, object>);
                        p.SetValue(targetObj, value, null);
                    }
                    else  if(kv.Value==null)
                    {
                        p.SetValue(targetObj, kv.Value, null);
                    }
                    else
                    {
                        if (exceptReys != null && exceptReys.Contains(kv.Key))
                            continue;
                        if (conv.CanConvertFrom(kv.Value.GetType()))
                        {
                            value = conv.ConvertFrom(kv.Value);
                            p.SetValue(targetObj, value, null);
                        }
                        else
                        {
                           switch(p.PropertyType.ToString()){
                               case "System.Byte": p.SetValue(targetObj,byte.Parse(""+kv.Value), null); break;
                               case "System.Int32": p.SetValue(targetObj, int.Parse("" + kv.Value), null); break;
                               case "System.Int64": p.SetValue(targetObj, long.Parse("" + kv.Value), null); break;
                               case "System.DateTime": p.SetValue(targetObj, DateTime.Parse("" + kv.Value), null); break;
                               default: p.SetValue(targetObj, kv.Value, null); break;
                           }
                        }
                    }
                }
            }
        }
        #endregion
        public static void QueryStringToFilter<T>(List<ExtFilterItem> items, string name)
        {
            string v = System.Web.HttpContext.Current.Request.QueryString[name];
            if (!string.IsNullOrEmpty(v))
            {
                ExtFilterItem filter = new ExtFilterItem();
                filter.Field = name;
                filter.Data.Value = v;
                filter.Data.Type = typeof(T).Name;
                items.Add(filter);
            }
        }
    }
}
