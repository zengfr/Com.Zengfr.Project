using log4net;
using Microsoft.VisualBasic;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using v2rayN.Mode;
using static VpnApp.VpnMan;

namespace VpnApp
{
    [Serializable]
    public class VpnStat
    {
        [Serializable]
        public class TestItem
        {
            public string sub = "";
            public ProfileItem Item;
            public int delay = -2;
        }

        private static Lazy<VpnStat> _instance = new(() => Load());
        public static VpnStat Instance => _instance.Value;

        public static string file = "result.bin";

        public Dictionary<string, string> Subs { get; private set; } = new Dictionary<string, string>();
        public Dictionary<string, TestItem> Profiles = new Dictionary<string, TestItem>();
        public Dictionary<string, string> Results = new Dictionary<string, string>();
        private Dictionary<string, int> Stats = new Dictionary<string, int>();
        public DateTime Last { get; private set; } = DateTime.Now;
     
        
        public static VpnStat Load()
        {
            VpnStat obj = null;
            //DataContractSerializer bi = new DataContractSerializer(typeof(VpnStat));
            if (File.Exists(file))
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (FileStream ff = new FileStream(file, FileMode.Open))
                {
                    object o = bf.Deserialize(ff);

                    // object o = bi.ReadObject(ff);
                    var oo = o as VpnStat;
                    if (oo != null)
                    {
                        var keys = oo.Stats.Where(t => t.Key.Length < 10).Select(t => t.Key);
                        foreach (var key in keys)
                            oo.Stats.Remove(key, out int v);
                        if (oo.Profiles.Count(t => t.Value.delay >= 0) <= 0)
                        {
                            oo.Stats.Clear();
                        }
                    }
                    obj = oo;
                }
            }
            if (obj == null)
            {
                obj = new VpnStat();
                obj.Save();
            }
            return obj;
        }
        public string GetResult(String key)
        {
            lock (Results)
            {
                return Results.GetValueOrDefault(key, "");
            }
        }
        public int GetStat(String key)
        {
            lock (Stats)
            {
                return Stats.GetValueOrDefault(key, 0);
            }
        }
        public int SetStat(String key,int v)
        {
            lock (Stats)
            {
                return Stats[key]=v;
            }
        }
        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            // DataContractSerializer bi = new DataContractSerializer(typeof(VpnStat));
            using (FileStream fi = new FileStream(file, FileMode.Create))
            {
                this.Last = DateTime.Now;
                bf.Serialize(fi, this);
                // this.Last = DateTime.Now;
                //  bi.WriteObject(fi, this);
            }
        }

        internal Dictionary<string, int>.KeyCollection GetStatsKeys()
        {
            return Stats.Keys;
        }
    }
}
