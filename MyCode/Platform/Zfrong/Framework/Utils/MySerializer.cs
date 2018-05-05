using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
namespace Zfrong.Framework.Utils
{
    /// <summary>
    /// 常用方法
    /// </summary>
    public class MySerializer
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="o">实体</param>
        /// <returns>xml</returns>
        public static string XmlSerialize(object o)
        {
            string result = string.Empty;
            System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(o.GetType());
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(stream, Encoding.UTF8);
                sz.Serialize(writer, o);
                writer.Close();
                result = Encoding.UTF8.GetString(stream.ToArray());
                stream.Close();
            }
            return result;
        }
        /// <summary>
        /// 反序列化实体
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="requestXml">请求xml，解密的</param>
        /// <returns>实体</returns>
        public static T XmlDeserialize<T>(string requestXml)
        {
            T result = default(T);
            System.Xml.Serialization.XmlSerializer sz = new System.Xml.Serialization.XmlSerializer(typeof(T));
using (System.IO.StringReader rdr = new System.IO.StringReader(requestXml))
                {
                    result = (T)sz.Deserialize(rdr);
                    rdr.Close();
                }
                return result;
        }

        public static string DataSerialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            string ret = "";
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                using (XmlDictionaryWriter binaryDictionaryWriter = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8))
                {
                    serializer.WriteObject(binaryDictionaryWriter, obj);
                    binaryDictionaryWriter.Flush();
                }
                ret = Encoding.UTF8.GetString(stream.ToArray());
            }
            return ret;
        }

        public static T DataDeserialize<T>(string xml, IList<Type> knownTypes)
        {
             T ret = default(T);
            if (string.IsNullOrEmpty(xml))
            {
                return ret;
            }
            using (MemoryStream stream = new MemoryStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0L;
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                using (XmlDictionaryReader binaryDictionaryReader = XmlDictionaryReader.CreateTextReader(stream, XmlDictionaryReaderQuotas.Max))
                {
                    ret = (T)serializer.ReadObject(binaryDictionaryReader);
                }
            }
            return ret;
        }

    }
}
