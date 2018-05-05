using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zfrong.Amib.Threading
{
    public class DictionaryManagerBase<K, V>
    {
        protected IDictionary<K, V> Items = new Dictionary<K, V>();
        protected object lockobj = new object();
        public virtual int ItemsCount
        {
            get { return Items.Count; } 
        }
        public virtual V Get(K key)
        {
            lock (lockobj)
            {
                if (this.Exist(key))
                {
                    return this.Items[key];
                }
            }
            return default(V);
        }
        public virtual void Add(K key,V value)
        {
            lock (lockobj)
            {
                this.Items.Add(key, value);
            }
        }
        public virtual void Remove(K key)
        {
            lock (lockobj)
            {
                this.Items.Remove(key);
            }
        }
        public virtual  bool Exist(K key)
        {
            lock (lockobj)
            {
               return this.Items.ContainsKey(key);
            }
        }
        public virtual void Clear()
        {
            lock (lockobj)
            {
                this.Items.Clear();
            }
        }
    }
}
