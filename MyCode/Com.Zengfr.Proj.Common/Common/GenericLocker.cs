using System.Collections.Generic;

namespace Com.Zengfr.Proj.Common
{
    public class GenericLocker
    {
        private static object lockObj = new object();

        private static Dictionary<string, Dictionary<string, object>> lockObjs = new Dictionary<string, Dictionary<string, object>>();

        private static Dictionary<string, object> GetGroupDictionary(string group)
        {
            if (!lockObjs.ContainsKey(group))
            {
                lock (lockObj)
                {
                    if (!lockObjs.ContainsKey(group))
                    {
                        lockObjs.Add(group, new Dictionary<string, object>());
                    }
                }
            }
            return lockObjs[group];
        }
        private static object GetLockObj(string group, string key)
        {
            var groupDictionary = GetGroupDictionary(group);
            if (!groupDictionary.ContainsKey(key))
            {
                lock (lockObj)
                {
                    if (!groupDictionary.ContainsKey(key))
                    {
                        groupDictionary.Add(key, new object());
                    }
                }
            }
            return groupDictionary[key];
        }
        public static object GetLockObjForMarketer(long key)
        {
            return GetLockObj("Marketer", "" + key);
        }
        public static object GetLockObjForLawyer(long key)
        {
            return GetLockObj("Lawyer", "" + key);
        }
        public static object GetLockObjForHandler(long key)
        {
            return GetLockObj("Handler", "" + key);
        }
        public static object GetLockObjForHandlerCompany(long key)
        {
            return GetLockObj("HandlerCompany", "" + key);
        }
        public static object GetLockObjForItemOrder(long key)
        {
            return GetLockObj("ItemOrder", "" + key);
        }
        public static object GetLockObjForItemOrder(long itemOrderId, long handlerId, long handlerCompanyId, long lawyerId)
        {
            var group = string.Format("{0}", itemOrderId, handlerId, handlerCompanyId, lawyerId);
            var key = string.Format("{0},{1},{2},{3}", itemOrderId, handlerId, handlerCompanyId, lawyerId);
            return GetLockObj(group, "" + key);
        }

        public static object GetLockObjForMarketerPayBank(long key)
        {
            return GetLockObj("MarketerPayBank", "" + key);
        }

        public static object GetLockObjForHandlerPayBank(long key)
        {
            return GetLockObj("HandlerPayBank", "" + key);
        }

    }
}
