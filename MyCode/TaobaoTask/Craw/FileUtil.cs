using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace cc
{
   public class FileUtil
    {

        public static void StoreX(IDictionary<string, string> d, string filename)
        {
            if (d.Count > 0)
            {
                new XElement("root", d.Select(kv => new XElement(kv.Key, kv.Value)))
                            .Save(filename, SaveOptions.OmitDuplicateNamespaces);
            }
        }
        public static IDictionary<string, string> RetrieveX(string filename)
        {
            if (!File.Exists(filename))
            {
                File.Create(filename);
                return new Dictionary<string, string>();
            }
            else
            {
                return XElement.Parse(File.ReadAllText(filename))
                               .Elements()
                               .ToDictionary(k => k.Name.ToString(), v => v.Value.ToString());
            }
        }


        public static void Store(IDictionary<string, long> dictionary, string filename,long minValue)
        {
            if (dictionary.Count > 0)
            {
                using (StreamWriter file = new StreamWriter(filename))
                {
                    var keys = new string[dictionary.Count];
                    dictionary.Keys.CopyTo(keys, 0);
                    foreach (var key in keys)
                    {
                        if (dictionary[key] >= minValue)
                            file.WriteLine("{0},{1}", key, dictionary[key]);
                    }
                    keys = null;
                }
            }
        }
        public static IDictionary<string, long> Retrieve(string filename)
        {
            Dictionary<string, long> dict = new Dictionary<string, long>();
            if (!File.Exists(filename))
            {
                File.Create(filename, 12, FileOptions.None);
            }
            else
            {
                using (StreamReader file = new StreamReader(filename))
                {
                    while(!file.EndOfStream){ 
                        var kv = (file.ReadLine() ?? string.Empty).Split(',');
                        if (kv.Length >= 2)
                        {
                            dict.Add(kv[0], long.Parse(kv[1]));
                        }
                        kv = null;
                   }
                }
            }
            return dict;
        }
    }
}
