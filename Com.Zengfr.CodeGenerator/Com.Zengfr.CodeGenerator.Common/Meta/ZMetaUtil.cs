using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Meta
{
    public class ZMetaUtil
    {
        private static Dictionary<string, string> IgnoreTypes = new Dictionary<string, string>();
        private static ZMetaUtil instance;
        public static ZMetaUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ZMetaUtil();
                }
                return instance;
            }
        }
        static ZMetaUtil()
        {
            IgnoreTypes.Clear();
            IgnoreTypes.Add("Entity", string.Empty);
            IgnoreTypes.Add("Model", string.Empty);
            IgnoreTypes.Add("Contract", string.Empty);
            IgnoreTypes.Add("Schema", string.Empty);
            IgnoreTypes.Add("Base", string.Empty);
            IgnoreTypes.Add("Mapping", string.Empty);
            IgnoreTypes.Add("Mapper", string.Empty);
            IgnoreTypes.Add("Properties", string.Empty);
        }
        public bool IsIgnoreType(Type t)
        {
            var ignore = false;
            var dict = new Dictionary<string, string>();
            foreach (var key in IgnoreTypes)
            {
                ignore = t.FullName.IndexOf(key.Key) > 0;
            }
            return ignore;
        }
        public bool IsModelType(Type t)
        {
            var isModelType = false;
            if (t != null && t.IsClass)
            {
                isModelType = (t.FullName.IndexOf("Entity") != -1 ||
                               t.FullName.IndexOf("Model") != -1 ||
                        // t.FullName.IndexOf("Domain") != -1 ||
                        t.FullName.IndexOf("Contract") != -1)

                       && t.FullName.IndexOf("Schema") == -1
                       && t.Name.StartsWith("Base") == false
                       && t.Name.EndsWith("Base") == false
                       && t.FullName.IndexOf("Mapping") == -1
                       && t.Name != "Properties"
                       ;
            }
            return isModelType;
        }
        public bool IsAttrIndexd(ZMetaAttribute attr) {
            return attr.IsID || attr.IsIndex || attr.IsKey || attr.IsUnique;
        }
        public ZMetaAttribute GetModelAttr4ID(ZMetaModel model)
        {
            foreach (var attr in model.Attributes)
            { 
                if(attr.IsID)
                    return attr;
            }
            return null;
        }
        public IEnumerable<ZMetaAttribute> GetModelAttr4Indexd(ZMetaModel model)
        {
            foreach (var attr in model.Attributes)
            {
                if (IsAttrIndexd(attr))
                  yield  return attr;
            }
            yield break;
        }
        public IEnumerable<String> GetModelAttrNames(ZMetaModel model)
        {
            foreach (var attr in model.Attributes)
            {
                if (IsAttrIndexd(attr))
                    yield return attr.Name;
            }
            yield break;
        }
        public bool IsComponentType(Type type)
        {
            if (type != null && type.IsClass)
            {
                var t = type.BaseType;
                if (t != null && (t.Name.IndexOf("Entity") >= 0 || t.Name.IndexOf("Model") >= 0))
                {
                    return false;
                }
                if (t == null)
                {
                    return true;
                }
                return IsComponentType(t);
            }
            return false;
        }
       
    }
}
