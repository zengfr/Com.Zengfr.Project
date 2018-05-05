using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace Com.Zfrong.Common.Xml.Serialization
{
    public class XMLSerialization
    {/// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        public static void ReadXmlForList<T>(IList<T> obj, XmlReader reader)
        {
            if (reader.IsEmptyElement || !reader.Read())
            {
                return;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                //reader.ReadStartElement("Item");
                T key = (T)serializer.Deserialize(reader);
                //reader.ReadEndElement();//
                reader.MoveToContent();
                obj.Add(key);
            }
            reader.ReadEndElement();
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        public static void WriteXmlForList<T>(IList<T> obj, XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            foreach (T key in obj)
            {
               // writer.WriteStartElement("Item");
                serializer.Serialize(writer, key);
                //writer.WriteEndElement();
            }

        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        public static void ReadXmlForKV<TKey, TValue>(Dictionary<TKey, TValue> obj, XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            if (reader.IsEmptyElement || !reader.Read())
            {
                return;
            }
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("Item");

                reader.ReadStartElement("Key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement("Value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadEndElement();
                reader.MoveToContent();

                obj.Add(key, value);
            }
            reader.ReadEndElement();
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        public static void WriteXmlForKV<TKey, TValue>(Dictionary<TKey, TValue> obj, XmlWriter writer)
        {
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));

            foreach (TKey key in obj.Keys)
            {
                writer.WriteStartElement("Item");

                writer.WriteStartElement("Key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();

                writer.WriteStartElement("Value");
                valueSerializer.Serialize(writer, obj[key]);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
    }
    [Serializable]
    //[System.Xml.Serialization.XmlInclude(typeof(XmlSerializableList<T>))]
    public class XmlSerializableDictionary<TKey, TValue>
        : Dictionary<TKey, TValue>, IXmlSerializable
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            XMLSerialization.ReadXmlForKV<TKey, TValue>(this, reader);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            XMLSerialization.WriteXmlForKV<TKey, TValue>(this, writer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return new System.Xml.Schema.XmlSchema();//
        }
    }
    [Serializable]
    //[System.Xml.Serialization.XmlInclude(typeof(XmlSerializableList<T>))]
    public class XmlSerializableList<T>
        : List<T>, IXmlSerializable
    {
        public XmlSerializableList()
            : base()
        {

        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            XMLSerialization.ReadXmlForList<T>(this, reader);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            XMLSerialization.WriteXmlForList<T>(this, writer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return new System.Xml.Schema.XmlSchema();//
        }
    }
    [Serializable]
    public class XmlSerializableQueue<T>
            : Queue<T>, IXmlSerializable
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(XmlReader reader)
        {
            if (reader.IsEmptyElement || !reader.Read())
            {
                return;
            }
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                //reader.ReadStartElement("Item");
                T key = (T)serializer.Deserialize(reader);
                //reader.ReadEndElement();//
                reader.MoveToContent();
                this.Enqueue(key);
            }
            reader.ReadEndElement();
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            foreach (T key in this)
            {
                //writer.WriteStartElement("Item");
                serializer.Serialize(writer, key);
                //writer.WriteEndElement();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return new System.Xml.Schema.XmlSchema();//
        }
    }
    [Serializable]
    public class XmlSerializable
    {

    }
}