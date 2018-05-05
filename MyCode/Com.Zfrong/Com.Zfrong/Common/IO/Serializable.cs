using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.IO.Compression;
using System.Xml.Serialization;
namespace Com.Zfrong.Common.IO
{
    public class Serialization
    {
        #region 序列化对象
        /// <summary>
        /// 序列化对象XmlSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="objTmp"></param>
        public static void SaveToFile<T>(string fileName, T objTmp)
        {
            SaveToFile<T>(fileName, objTmp, false);//
        }
        public static void SaveToFile<T>(string fileName, T objTmp, bool isCompress)
        {
            XmlSerializer f = new XmlSerializer(typeof(T));
            Stream stream = BuildWriteStream(fileName, isCompress);
            using (stream)
            {
                f.Serialize(stream, objTmp);
            }
        }
        public static void SaveToFile<T>(string fileName, T objTmp, IFormatter f)
        {
            SaveToFile<T>(fileName, objTmp, f, false);//
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="fileName">保存到的文件</param>
        /// <param name="objTmp">对象的实例</param>
        public static void SaveToFile<T>(string fileName, T objTmp, IFormatter f, bool isCompress)
        {
            Stream stream = BuildWriteStream(fileName, isCompress);
            using (stream)
            {
                f.Context = new StreamingContext(StreamingContextStates.File);
                f.Serialize(stream, objTmp);
            }
        }

        #endregion

        #region 反序列化对象
        /// <summary>
        /// 反序列化对象XmlSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T LoadFromFile<T>(string fileName)
        {
            return LoadFromFile<T>(fileName, false);//
        }
        public static T LoadFromFile<T>(string fileName,bool isCompress)
        {
            XmlSerializer f = new XmlSerializer(typeof(T));
            Stream stream = BuildReadStream(fileName, isCompress);
            using (stream)
            {
                return (T)f.Deserialize(stream);
            }
        }
        /// <summary>
        /// 反序列化对象
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="fileName">对象读取的文件</param>
        /// <returns>返回一个对象的实例</returns>
        public static T LoadFromFile<T>(string fileName, IFormatter f)
        {
          return  LoadFromFile<T>(fileName, f, false);//
        }
        public static T LoadFromFile<T>(string fileName, IFormatter f,bool isCompress)
        {
            Stream stream=BuildReadStream(fileName,isCompress);
            using (stream)
            {
                f.Context = new StreamingContext(StreamingContextStates.File);
                return (T)f.Deserialize(stream);
            }
        }
        
        #endregion
        public static Stream BuildReadStream(string fileName, bool isCompress)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            GZipStream zip;
            if (isCompress)
             return   zip = new GZipStream(fs, CompressionMode.Decompress);
         return fs;//
        }
        public static Stream BuildWriteStream(string fileName, bool isCompress)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            GZipStream zip;
            if (isCompress)
                return zip = new GZipStream(fs, CompressionMode.Decompress);
            return fs;//
        }
    }
}
