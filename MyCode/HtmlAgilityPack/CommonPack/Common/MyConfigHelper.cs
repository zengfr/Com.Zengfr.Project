using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using HtmlAgilityPack;
namespace CommonPack
{
    public class MyConfigHelper
    {
        public static IDictionary<string, MyConfig> ConfigDictionary = new Dictionary<string, MyConfig>();//
        private static object SyncObj = new object();//
        public static MyConfig Get(string key)
        {
            lock(SyncObj)
            {
                if (!ConfigDictionary.ContainsKey(key) || ConfigDictionary[key] == null)
                {
                    MyConfig myConfig = new MyConfig(key);
                    myConfig.Read();
                    ConfigDictionary.Add(key, myConfig);//
                }
                System.Threading.Monitor.PulseAll(SyncObj);//
            }
            return ConfigDictionary[key];//
        }
        public static MyConfig Get(string dir,string fileName)
        {
            return Get(dir,fileName,".xml");
        }
        public static MyConfig Get(string dir,string fileName,string fileExt)
        {
            return Get(System.AppDomain.CurrentDomain.BaseDirectory + @"\"+dir + @"\"+fileName + fileExt);//
        }
        public static void CopyTo(MyConfig source,MyConfig target)
        {
            target.FilePath = source.FilePath;//
            target.xmlDoc = source.xmlDoc;//
            CopyTo(source.Dictionary, target.Dictionary);
        }
        public static void CopyTo(IDictionary<string, IDictionary<string, string>> source, IDictionary<string, IDictionary<string, string>> target)
        {
            foreach (KeyValuePair<string, IDictionary<string, string>> kv in source)
            {
                if (!target.ContainsKey(kv.Key))
                    target.Add(kv.Key, kv.Value);
                else if (target[kv.Key] == null)
                    CopyTo(source[kv.Key], target[kv.Key]);
            }

        }
        public static void CopyTo(IDictionary<string, string>  source,IDictionary<string, string> target)
        {
            foreach (KeyValuePair<string, string> kv in source)
            {
                if (!target.ContainsKey(kv.Key))
                    target.Add(kv.Key, kv.Value);
                else if (target[kv.Key] == null)
                    target[kv.Key] = kv.Value;
            }
        }
    }
    public class MyConfig
    {
        public IDictionary<string, IDictionary<string, string>> Dictionary = new Dictionary<string, IDictionary<string, string>>();
        public string FilePath;
        public XmlDocument xmlDoc=new XmlDocument();
        public MyConfig()
        {
            ;//
        }
        public MyConfig(string filePath)
        {
            this.FilePath = filePath;//

            if (!File.Exists(FilePath))
            {
                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(FilePath)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(FilePath));//

                this.xmlDoc.AppendChild(this.xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null));
                this.xmlDoc.AppendChild(this.xmlDoc.CreateElement("config"));//
                this.xmlDoc.Save(this.FilePath);
            }
        }
        public void Read()
        {
            this.xmlDoc.PreserveWhitespace = true;
            this.xmlDoc.Load(this.FilePath); 
            XmlNodeList nl = xmlDoc.SelectNodes("/config/*");//
            foreach (XmlNode xn in nl)
            {
                Read(xn.Name);//
            }
        }
        private void Read(string parentNodeName)
        {
            try
            {
                Dictionary<string, string> dictionary = new Dictionary<string, string>();
                ReadNode(this.xmlDoc, "/config/"+parentNodeName+"/add", dictionary);
                if(this.Dictionary.ContainsKey(parentNodeName))
                    this.Dictionary[parentNodeName]= dictionary;
                    else
                this.Dictionary.Add(parentNodeName, dictionary);
            }
            catch
            {
                throw new Exception("≈‰÷√Œƒº˛¥ÌŒÛ£°");//
            }
        }
        private static void ReadNode(XmlDocument xmlDoc,string xPath, IDictionary<string, string> dictionary)
        {
            string key;
            XmlNodeList nl = xmlDoc.SelectNodes(xPath);//
            foreach (XmlNode xn in nl)
            {
                if (xn.Attributes["key"] != null && xn.Attributes["value"] != null)
                {
                    key = xn.Attributes["key"].Value;
                    if (dictionary.ContainsKey(key))
                        dictionary[key] = xn.Attributes["value"].Value;
                    else
                        dictionary.Add(key, xn.Attributes["value"].Value);
                }
            }
        }
        public void Clear()
        {
            this.Dictionary.Clear();
        }
        public void Save()
        {
            this.Save(this.FilePath);
        }
        public void Save(string file)
        {
            xmlDoc.DocumentElement.RemoveAll();

            foreach (KeyValuePair<string, IDictionary<string,string>> kv in this.Dictionary)
            {
                WriteDictionary(xmlDoc,kv.Key, kv.Value);
            }
            xmlDoc.Normalize();
            xmlDoc.Save(file);//
        }
        private void WriteDictionary(XmlDocument xmlDoc, string parentNodeName, IDictionary<string, string> dictionary)
        {
            xmlDoc.SelectSingleNode("/config").AppendChild(xmlDoc.CreateElement(parentNodeName));
            Write(xmlDoc, "/config/" + parentNodeName, dictionary);
        }
        private static void Write(XmlDocument xml, string parentNodeXPath, IDictionary<string, string> dictionary)
        {
            
            foreach (KeyValuePair<string, string> kv in dictionary)
            {
                Write(xml, parentNodeXPath, kv.Key, kv.Value);
            }
        }
        private static void Write(XmlDocument xml, string parentNodeXPath, string attName,string attValue)
        {
            XmlNode parentNode = xml.SelectSingleNode(parentNodeXPath);
            XmlNode xmlNode = xml.CreateElement("add");

            XmlAttribute xmlAttribute;

            xmlAttribute = xml.CreateAttribute("key");
            xmlAttribute.Value = attName;
            xmlNode.Attributes.Append(xmlAttribute);

            xmlAttribute = xml.CreateAttribute("value");
            xmlAttribute.Value = attValue;
            xmlNode.Attributes.Append(xmlAttribute);

            parentNode.AppendChild(xmlNode);
        }
        public IDictionary<string, string> Get(string name)
        {
            if (!Dictionary.ContainsKey(name))
            Dictionary.Add(name,new Dictionary<string,string>());
            return Dictionary[name];//
        }
        public T GetParamValue<T>(string name)
        {
            T t = default(T);
            if (this.Get("param").ContainsKey(name))
                t =(T)((object)Dictionary["param"][name]);//
            else
                Dictionary["param"].Add(name, t.ToString());
            return t;
        }
        public string GetParamValue(string name)
        {
            string str = null;
            GetParamValue(name, ref  str);
            return str;
        }
        public int GetParamValue_Int(string name)
        {
            int i = 0;
            GetParamValue(name, ref  i);
            return i;
        }
        public bool GetParamValue_Bool(string name)
        {
            bool b =false;
            GetParamValue(name, ref  b);
            return b;
        }
        public void GetParamValue(string name, ref string rtn)
        {
            if (this.Get("param").ContainsKey(name))
                rtn = Dictionary["param"][name];//
            else
                Dictionary["param"].Add(name, rtn);
        }
        public void GetParamValue(string name, ref int rtn)
        {
            if (this.Get("param").ContainsKey(name))
                int.TryParse(Dictionary["param"][name], out rtn);//
            else
                Dictionary["param"].Add(name, rtn.ToString());
        }
        public void GetParamValue(string name, ref bool rtn)
        {
            if (this.Get("param").ContainsKey(name))
                bool.TryParse(Dictionary["param"][name], out rtn);//
            else
                Dictionary["param"].Add(name, rtn.ToString());
        }

        public void SetParamValue<T>(string name, T value)
        {
            SetParamValue(name,value.ToString());
        }
        public void SetParamValue(string name, int value)
        { 
            SetParamValue(name, value.ToString()); 
        }
        public void SetParamValue(string name, bool value)
        {
            SetParamValue(name, value.ToString());
        }
        public void SetParamValue(string name, string value)
        {
            if (!Dictionary.ContainsKey("param"))
                Dictionary.Add("param", new Dictionary<string,string>());
             //if(!Dictionary["param"].ContainsKey(name))
             //   Dictionary["param"].Add(name, value);
            Dictionary["param"][name] = value;//
        }
        public void Set(string name,IDictionary<string, string> value)
        {
            if (!Dictionary.ContainsKey(name))
                Dictionary.Add(name, new Dictionary<string, string>());
             Dictionary[name]=value;//
        }
        public static void Replace(IDictionary<string, string> dictionary,ref string str)
        {
            foreach (KeyValuePair<string,string> kv in dictionary)
            {
                str = Regex.Replace(str, kv.Key, kv.Value, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            }
        }
        public static void Replace(IDictionary<string, string> dictionary,StringBuilder sb)
        {
            Regex r;
            MatchCollection ms;
            foreach (KeyValuePair<string, string> kv in dictionary)
            {
                r = new Regex(kv.Key, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                ms = r.Matches(sb.ToString());
                foreach (Match ma in ms)
                {
                    sb.Replace(ma.Value, kv.Value);
                }
            }
        }
        public static void RemoveAllChildrenNodeFromXPath(IDictionary<string, string> dictionary, HtmlDocument doc)
        {
             HtmlNode node; int code = 0;
             foreach (KeyValuePair<string, string> kv in dictionary)
            {
               node= doc.DocumentNode.SelectSingleNode(kv.Key);//
               code =kv.Key.GetHashCode();
               node.RemoveAllChildren();
               node.AppendChild(HtmlNode.CreateNode(ScriptFormat.Replace("{0}","Remove/"+code)));
            }
        }
        public static void AppendChildNodeFromXPath(IDictionary<string, string> dictionary, HtmlDocument doc)
        {
            HtmlNode node; int code = 0;
            foreach (KeyValuePair<string, string> kv in dictionary)
            {
                node = doc.DocumentNode.SelectSingleNode(kv.Key);//
                code = kv.Key.GetHashCode();
                node.AppendChild(HtmlNode.CreateNode(ScriptFormat.Replace("{0}", "Append/" + code)));
            }
        }
        public static void PrependChildNodeFromXPath(IDictionary<string, string> dictionary, HtmlDocument doc)
        {
            HtmlNode node; int code = 0;
            foreach (KeyValuePair<string, string> kv in dictionary)
            {
                node = doc.DocumentNode.SelectSingleNode(kv.Key);//
                code = kv.Key.GetHashCode();
                node.PrependChild(HtmlNode.CreateNode(ScriptFormat.Replace("{0}", "Prepend/" + code)));
            }
        }
        private static string ScriptFormat = "<script language=\"JavaScript\" type=\"text/javascript\" src=\"/zfrong/js/{0}.js\"></script>";
    }
  
}
