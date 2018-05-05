using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zfrong.Amib.Threading
{
    public class EventManagerFactory:DictionaryManagerBase<string,EventManager>
    {
        public static EventManagerFactory Instance = new EventManagerFactory();
        public virtual EventManager Create(string poolName)
        {
            return Create<EventManager>(poolName);
        }
        public virtual EventManager Create<T>(string name) where T : EventManager, new()
        {
            if (!this.Exist(name))
            {
                T item = new T();
                item.Name = name;
                this.Items[item.Name] = item;
            }
            return this.Items[name];
        }
    }
}
