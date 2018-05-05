using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.Xml.Serialization;
namespace Com.Zfrong.Common.Collections
{
    [XmlRoot("list")]
    [Serializable]
    public class SList<T> : List<T>, IXmlSerializable
    {
        #region  Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "item";
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);
            bool wasEmpty = reader.IsEmptyElement;

            reader.Read();

            if (wasEmpty)

                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                //reader.ReadStartElement("item");
                T item = (T)serializer.Deserialize(reader);
                //reader.ReadEndElement();
                this.Add(item);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
            serializer = null;

        }
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "item";
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            foreach (T item in this)
            {
               // writer.WriteStartElement("item");
                serializer.Serialize(writer, item);
               // writer.WriteEndElement();
            }
            serializer = null;
        }
        #endregion
    }
    [XmlRoot("dict")]
    [Serializable]
  
    public class SDictionary<TKey, TValue> : Dictionary<string,string>, IXmlSerializable {
        #region  Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "";
            XmlSerializer valueSerializer = new XmlSerializer(typeof(string), root);


            bool wasEmpty = reader.IsEmptyElement;

            reader.Read();

            if (wasEmpty)

                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {

                //reader.ReadStartElement("item");
                string key = "";
                if (reader.HasAttributes)
                {
                    reader.MoveToAttribute("k");

                    key = reader.Value;

                    reader.Read();
                }

                //reader.ReadStartElement("v");
               // TValue value = (TValue)valueSerializer.Deserialize(reader);
                //reader.ReadEndElement();
                if (!reader.IsEmptyElement)
                {
                    this.Add(key, reader.ReadContentAsString());

                    //this.Add(key, value);

                    reader.ReadEndElement();
                }
                reader.MoveToContent();

            }

            reader.ReadEndElement();
            valueSerializer = null;

        }
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "";
            XmlSerializer valueSerializer = new XmlSerializer(typeof(string), root);

            foreach (string key in this.Keys)
            {

                writer.WriteStartElement("kv");


               writer.WriteStartAttribute("k");

               writer.WriteString(key);

                writer.WriteEndAttribute();


                 writer.WriteString(this[key]);
                //writer.WriteStartElement("v");
               // TValue value = this[key];
                //valueSerializer.Serialize(writer, value);

                //writer.WriteEndElement();

                writer.WriteEndElement();
            }
             valueSerializer = null;
        }
        #endregion

    }
    [XmlRoot("dictionary")]
    [Serializable]
    public class ZDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region  Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(System.Xml.XmlReader reader)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "Q";
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey),root);

            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue),root);


            bool wasEmpty = reader.IsEmptyElement;

            reader.Read();

            if (wasEmpty)

                return;

            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {

                reader.ReadStartElement("item");


                reader.ReadStartElement("k");

                TKey key = (TKey)keySerializer.Deserialize(reader);

                reader.ReadEndElement();


                reader.ReadStartElement("v");

                TValue value = (TValue)valueSerializer.Deserialize(reader);

                reader.ReadEndElement();


                this.Add(key, value);


                reader.ReadEndElement();

                reader.MoveToContent();

            }

            reader.ReadEndElement();
            keySerializer = null; valueSerializer = null;

        }
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlRootAttribute root = new XmlRootAttribute();
            root.ElementName = "Q";
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey),root);

            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue), root);

            foreach (TKey key in this.Keys)
            {

                writer.WriteStartElement("item");


                writer.WriteStartElement("k");

                keySerializer.Serialize(writer, key);

                writer.WriteEndElement();
               

                writer.WriteStartElement("v");

                TValue value = this[key];

                valueSerializer.Serialize(writer, value);

                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            keySerializer = null; valueSerializer = null;
        }
        #endregion

    }
}
