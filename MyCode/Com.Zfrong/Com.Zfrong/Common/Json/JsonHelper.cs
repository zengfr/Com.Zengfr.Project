using System;
using System.Collections.Generic;
using System.Text;

using System.Web.Script.Serialization;
using System.Collections;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data;
namespace Com.Zfrong.Common.Json
{

    /**/
    /// <summary>
    /// JSONHelper 的摘要说明
    /// </summary>
    public class JSONHelper
    {
        /// <summary>
        /// JavaScriptSerializer
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
        public static string ToJSON(object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }
        /// <summary>
        /// DataContractJsonSerializer
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonObject(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                StringBuilder sb = new StringBuilder();
                sb.Append(Encoding.UTF8.GetString(ms.ToArray()));
                return sb.ToString();
            }
        }

        public static string ToJson(string jsonName, DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            Json.Append("{\"" + jsonName + "\":[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]}");
            return Json.ToString();
        }
    }
}
