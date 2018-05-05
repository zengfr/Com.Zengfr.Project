﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Newtonsoft.Json.Linq;

using RestSharp.Extensions;
using System.Globalization;
using RestSharp;
using RestSharp.Deserializers;
namespace Com.Zengfr.Proj.Common
{
    public class RestSharpJsonDeserializer : RestSharp.Deserializers.IDeserializer
    {
        //public string RootElement { get; set; }
        //public string Namespace { get; set; }
        //public string DateFormat { get; set; }
        //public CultureInfo Culture { get; set; }

        //public RestSharpJsonDeserializer()
        //{
        //    Culture = CultureInfo.InvariantCulture;
        //}

        //public T Deserialize<T>(IRestResponse response) where T : new()
        //{
        //    var target = new T();

        //    if (target is IList)
        //    {
        //        var objType = target.GetType();

        //        if (RootElement.HasValue())
        //        {
        //            var root = FindRoot(response.Content);
        //            target = (T)BuildList(objType, root.Children());
        //        }
        //        else
        //        {
        //            JArray json = JArray.Parse(response.Content);
        //            target = (T)BuildList(objType, json.Root.Children());
        //        }
        //    }
        //    else if (target is IDictionary)
        //    {
        //        var root = FindRoot(response.Content);
        //        target = (T)BuildDictionary(target.GetType(), root.Children());
        //    }
        //    else
        //    {
        //        var root = FindRoot(response.Content);
        //        Map(target, root);
        //    }

        //    return target;
        //}

        //private JToken FindRoot(string content)
        //{
        //    JObject json = JObject.Parse(content);
        //    JToken root = json.Root;

        //    if (RootElement.HasValue())
        //        root = json.SelectToken(RootElement);

        //    return root;
        //}

        //private void Map(object x, JToken json)
        //{
        //    var objType = x.GetType();
        //    var props = objType.GetProperties().Where(p => p.CanWrite).ToList();

        //    foreach (var prop in props)
        //    {
        //        var type = prop.PropertyType;

        //        var name = prop.Name;
        //        var actualName = name.GetNameVariants(Culture).FirstOrDefault(n => json[n] != null);
        //        var value = actualName != null ? json[actualName] : null;

        //        if (value == null || value.Type == JTokenType.Null)
        //        {
        //            continue;
        //        }

        //        // check for nullable and extract underlying type
        //        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        //        {
        //            type = type.GetGenericArguments()[0];
        //        }

        //        if (type.IsPrimitive)
        //        {
        //            // no primitives can contain quotes so we can safely remove them
        //            // allows converting a json value like {"index": "1"} to an int
        //            var tmpVal = value.AsString().Replace("\"", string.Empty);
        //            prop.SetValue(x, tmpVal.ChangeType(type, Culture), null);
        //        }
        //        else if (type.IsEnum)
        //        {
        //            var converted = type.FindEnumValue(value.AsString(), Culture);
        //            prop.SetValue(x, converted, null);
        //        }
        //        else if (type == typeof(Uri))
        //        {
        //            string raw = value.AsString();
        //            var uri = new Uri(raw, UriKind.RelativeOrAbsolute);
        //            prop.SetValue(x, uri, null);
        //        }
        //        else if (type == typeof(string))
        //        {
        //            string raw = value.AsString();
        //            prop.SetValue(x, raw, null);
        //        }
        //        else if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
        //        {
        //            DateTime dt;
        //            if (DateFormat.HasValue())
        //            {
        //                var clean = value.AsString();
        //                dt = DateTime.ParseExact(clean, DateFormat, Culture);
        //            }
        //            else if (value.Type == JTokenType.Date)
        //            {
        //                dt = value.Value<DateTime>().ToUniversalTime();
        //            }
        //            else
        //            {
        //                // try parsing instead
        //                dt = value.AsString().ParseJsonDate(Culture);
        //            }

        //            if (type == typeof(DateTime))
        //                prop.SetValue(x, dt, null);
        //            else if (type == typeof(DateTimeOffset))
        //                prop.SetValue(x, (DateTimeOffset)dt, null);
        //        }
        //        else if (type == typeof(Decimal))
        //        {
        //            var dec = Decimal.Parse(value.AsString(Culture), Culture);
        //            prop.SetValue(x, dec, null);
        //        }
        //        else if (type == typeof(Guid))
        //        {
        //            string raw = value.AsString();
        //            var guid = string.IsNullOrEmpty(raw) ? Guid.Empty : new Guid(raw);
        //            prop.SetValue(x, guid, null);
        //        }
        //        else if (type.IsGenericType)
        //        {
        //            var genericTypeDef = type.GetGenericTypeDefinition();
        //            if (genericTypeDef == typeof(List<>))
        //            {
        //                var list = BuildList(type, value.Children());
        //                prop.SetValue(x, list, null);
        //            }
        //            else if (genericTypeDef == typeof(Dictionary<,>))
        //            {
        //                var keyType = type.GetGenericArguments()[0];

        //                // only supports Dict<string, T>()
        //                if (keyType == typeof(string))
        //                {
        //                    var dict = BuildDictionary(type, value.Children());
        //                    prop.SetValue(x, dict, null);
        //                }
        //            }
        //            else
        //            {
        //                // nested property classes
        //                var item = CreateAndMap(type, json[actualName]);
        //                prop.SetValue(x, item, null);
        //            }
        //        }
        //        else
        //        {
        //            // nested property classes
        //            var item = CreateAndMap(type, json[actualName]);
        //            prop.SetValue(x, item, null);
        //        }
        //    }
        //}

        //private object CreateAndMap(Type type, JToken element)
        //{
        //    object instance = null;
        //    if (type.IsGenericType)
        //    {
        //        var genericTypeDef = type.GetGenericTypeDefinition();
        //        if (genericTypeDef == typeof(Dictionary<,>))
        //        {
        //            instance = BuildDictionary(type, element.Children());
        //        }
        //        else if (genericTypeDef == typeof(List<>))
        //        {
        //            instance = BuildList(type, element.Children());
        //        }
        //        else if (type == typeof(string))
        //        {
        //            instance = (string)element;
        //        }
        //        else
        //        {
        //            instance = Activator.CreateInstance(type);
        //            Map(instance, element);
        //        }
        //    }
        //    else if (type == typeof(string))
        //    {
        //        instance = (string)element;
        //    }
        //    else
        //    {
        //        instance = Activator.CreateInstance(type);
        //        Map(instance, element);
        //    }
        //    return instance;
        //}

        //private IDictionary BuildDictionary(Type type, JEnumerable<JToken> elements)
        //{
        //    var dict = (IDictionary)Activator.CreateInstance(type);
        //    var valueType = type.GetGenericArguments()[1];
        //    foreach (JProperty child in elements)
        //    {
        //        var key = child.Name;
        //        var item = CreateAndMap(valueType, child.Value);
        //        dict.Add(key, item);
        //    }

        //    return dict;
        //}

        //private IList BuildList(Type type, JEnumerable<JToken> elements)
        //{
        //    var list = (IList)Activator.CreateInstance(type);
        //    var itemType = type.GetGenericArguments()[0];

        //    foreach (var element in elements)
        //    {
        //        if (itemType.IsPrimitive)
        //        {
        //            var value = element as JValue;
        //            if (value != null)
        //            {
        //                list.Add(value.Value.ChangeType(itemType, Culture));
        //            }
        //        }
        //        else if (itemType == typeof(string))
        //        {
        //            list.Add(element.AsString());
        //        }
        //        else
        //        {
        //            var item = CreateAndMap(itemType, element);
        //            list.Add(item);
        //        }
        //    }
        //    return list;
        //}
        public string DateFormat
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public T Deserialize<T>(IRestResponse response)
        {
            throw new System.NotImplementedException();
        }

        public string Namespace
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public string RootElement
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
    }

}
