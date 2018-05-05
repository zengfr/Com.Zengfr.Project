using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization;
namespace Com.Zfrong.Common.IO
{
   public class SerializeHelper
    {
       static Encoding Encoding = new UTF8Encoding(false);
         #region XmlSerializer 
       public static string BytesToString(byte[] item)
       {
           return Encoding.GetString(item);
       }
       public static byte[] StringToBytes(String str)
       {
           return Encoding.GetBytes(str);
       }
       public static byte[] ToXmlBytesEncrypt<T>(T item)
       {
           string xml =Com.Zfrong.Common.Security.SecurityHelper.Encrypt(ToXml<T>(item));
           return StringToBytes(xml);
       }
       public static byte[] ToXmlBytes<T>(T item)
       {
           string xml = ToXml<T>(item);
           return StringToBytes(xml);
       }
       public static string ToXmlEncrypt<T>(T item)
       {
          return Com.Zfrong.Common.Security.SecurityHelper.Encrypt(ToXml<T>(item));
       }
       public static string ToXml<T>(T item)
       {
           return ToXml<T>(item,null) ;
       }
       public static string ToXml<T>(T item, XmlAttributeOverrides attrOver)
       {
           XmlSerializer serializer = new XmlSerializer(item.GetType(), attrOver);
           XmlWriterSettings setting = new XmlWriterSettings();
           setting.Encoding = new UTF8Encoding(false);
           setting.Indent = false;// true;
           setting.CloseOutput = true;
           setting.OmitXmlDeclaration = true;//去掉xml版本声明
           XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
           xmlns.Add(string.Empty, string.Empty);

           MemoryStream stream = new MemoryStream();
           using (XmlWriter writer =XmlTextWriter.Create(stream, setting))
           {
               serializer.Serialize(writer, item, xmlns);
               string s=Encoding.UTF8.GetString(stream.ToArray());
               return s;
           }
       }
      static  XmlAttributeOverrides GetXmlAttributeOverrides(Type t)
       {
          //XmlRootAttribute[]  a=t.GetCustomAttributes(typeof(XmlRootAttribute),true) as XmlRootAttribute[];
          XmlAttributeOverrides overrides = new XmlAttributeOverrides();
          // if(a!=null&&a.Length!=0){
          //XmlAttributes attrs = new XmlAttributes();
          //attrs.XmlRoot = new XmlRootAttribute("list");
          //overrides.Add(typeof(Dictionary<string, string>), attrs);
          //overrides.Add(typeof(List<SDictionary<string, string>>), attrs);

          //overrides.Add(typeof(SDictionary<string, string>),attrs);
          
           return overrides;
       }
       public static void Save<T>(string path, T item)
       {

           try
           {
              // lock (this)
               {
                  XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
                  xmlns.Add(string.Empty, string.Empty);

                  XmlSerializer serializer = new XmlSerializer(typeof(T), GetXmlAttributeOverrides(typeof(T)));
                   StreamWriter w = new StreamWriter(path);
                   serializer.Serialize(w, item,xmlns);
                   w.Close();
                   serializer = null; w = null;
               }
           }
           catch(Exception ex) 
           {

           }
       }

       public static T Load<T>(string path) where T : class
       {
           T item = null;
           try
           {
              // lock (SerializeHelper)
               {
               if (File.Exists(path))
               {
                   XmlSerializer serializer = new XmlSerializer(typeof(T), GetXmlAttributeOverrides(typeof(T)));
                       StreamReader r = new StreamReader(path);
                       item = serializer.Deserialize(r) as T;
                       r.Close();
                       r = null; serializer = null;
               }
               }
           }
           catch { }
           return item;
       }
       public static string FromXmlBytesDecrypt(byte[] item)
       {
           return Com.Zfrong.Common.Security.SecurityHelper.Decrypt(BytesToString(item));
       }
       public static string FromXmlBytes(byte[] item)
       {
           return BytesToString(item);
       }
       public static T FromXmlDecrypt<T>(string str)
       {
           return FromXml<T>(Com.Zfrong.Common.Security.SecurityHelper.Decrypt(str));
       }
           public static T FromXml<T>(string str)   
          {  
               return FromXml<T>(str,null)  ;
           }

         public static T FromXml<T>(string str, XmlAttributeOverrides attrOver)
           {
              XmlReaderSettings setting = new XmlReaderSettings();
              setting.IgnoreComments = true;
              setting.CloseInput = true;
              setting.IgnoreWhitespace = true;
              
             XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
             xmlns.Add(string.Empty, string.Empty);

              XmlSerializer serializer = new XmlSerializer(typeof(T),attrOver);
              T obj = default(T);
             using (XmlReader reader = XmlTextReader.Create(new StringReader(str),setting))   
              {
                  try
                  {
                      obj = (T)serializer.Deserialize(reader);
                  }
                  catch { }
              }
             return obj;
          }
#endregion

         # region BinaryFormatter   
         public static string ToBinary<T>(T item)   
          {   
              BinaryFormatter formatter = new BinaryFormatter();   
             using (MemoryStream ms = new MemoryStream())   
              {   
                  formatter.Serialize(ms, item);   
                  ms.Position = 0;   
                 byte[] bytes = ms.ToArray();   
                  StringBuilder sb = new StringBuilder();   
                 foreach (byte bt in bytes)   
                  {   
                      sb.Append(string.Format("{0:X2}", bt));   
                  }   
                 return sb.ToString();   
              }   
          }

         public static T FromBinary<T>(string str)   
          {   
             int intLen = str.Length / 2;   
             byte[] bytes = new byte[intLen];   
             for (int i = 0; i < intLen; i++)   
              {   
                 int ibyte = Convert.ToInt32(str.Substring(i * 2, 2), 16);   
                  bytes[i] = (byte)ibyte;   
              }   
              BinaryFormatter formatter = new BinaryFormatter();   
             using (MemoryStream ms = new MemoryStream(bytes))   
              {   
                 return (T)formatter.Deserialize(ms);   
              }   
          }
# endregion

         #region SoapFormatter   
         public static string ToSoap<T>(T item)   
          {   
              SoapFormatter formatter = new SoapFormatter();   
             using (MemoryStream ms = new MemoryStream())   
              {   
                  formatter.Serialize(ms, item);   
                  ms.Position = 0;   
                  XmlDocument xmlDoc = new XmlDocument();   
                  xmlDoc.Load(ms);   
                 return xmlDoc.InnerXml;   
              }   
          }

         public static T FromSoap<T>(string str)   
          {   
              XmlDocument xmlDoc = new XmlDocument();   
              xmlDoc.LoadXml(str);   
              SoapFormatter formatter = new SoapFormatter();   
             using (MemoryStream ms = new MemoryStream())   
              {   
                  xmlDoc.Save(ms);   
                  ms.Position = 0;   
                 return (T)formatter.Deserialize(ms);   
              }   
          }
#endregion

         # region JsonSerializer   
         //public string ToJson<T>(T item)   
         // {   
         //    DataContractJsonSerializer serializer = new DataContractJsonSerializer(item.GetType());   
         //   using (MemoryStream ms = new MemoryStream())   
         //     {   
         //        serializer.WriteObject(ms, item);   
         //        return Encoding.UTF8.GetString(ms.ToArray());   
         //     }   
         // }   
   
         //public T FromJson<T>(string str) where T : class  
         // {   
         //     DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));   
         //    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(str)))   
         //     {   
         //        return serializer.ReadObject(ms) as T;   
         //     }   
         // }
#endregion   
  
    }
}
