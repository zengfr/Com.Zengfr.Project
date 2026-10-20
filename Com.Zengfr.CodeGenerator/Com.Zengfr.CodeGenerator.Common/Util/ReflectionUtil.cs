using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Util
{
    public class ReflectionUtil
    {
        private static ReflectionUtil instance;
        public static ReflectionUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ReflectionUtil();
                }
                return instance;
            }
        }
        public virtual List<PropertyInfo> GetProperties(Type type)
        {
            var ps = type.GetProperties(BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance);
            return ps.Where(p => (!p.IsSpecialName && p.Name != "Item")).ToList();
        }
        public virtual List<String> GetPropertiesName(Type type)
        {
            return GetProperties(type).Select(p=>p.Name).ToList();
        }
        public virtual List<MethodInfo> GetMethods(Type type)
        {
            var ms = type.GetMethods(BindingFlags.Default | BindingFlags.Public | BindingFlags.Instance);
            return ms.Where(p => (!p.IsSpecialName && p.Name != "Item")).ToList();
        }
        public virtual List<String> GetMethodsName(Type type)
        {
            return GetProperties(type).Select(m => m.Name).ToList();
        }
       
    }
}
