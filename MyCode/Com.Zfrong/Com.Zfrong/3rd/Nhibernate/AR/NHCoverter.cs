using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Web.Script.Serialization;
using NHibernate.Proxy;
namespace Com.Zfrong.Data.AR
{
        public class NHibernateConverter : JavaScriptConverter
    {
        private const BindingFlags getProperty = BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance;
        private const BindingFlags setProperty = BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty;
        public Dictionary<Type, string> idBag = new Dictionary<Type, string>();
        private IList<Type> supportedTypes;

        public NHibernateConverter()
        {
        }

        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                if (supportedTypes == null)
                {
                    BurrowFramework bf = new BurrowFramework();
                    ISessionFactory s = bf.GetSession().SessionFactory;

                    supportedTypes = new List<Type>();
                    //supportedTypes.Add(typeof(User));
                    IDictionary classf = s.GetAllClassMetadata();
                    foreach (Type o in classf.Keys)
                    {
                        AbstractEntityPersister cla = (AbstractEntityPersister) classf[o];
                        supportedTypes.Add(o);
                        idBag.Add(o, cla.IdentifierPropertyName);
                    }
                }
                return supportedTypes;
            }
        }

        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                           JavaScriptSerializer serializer)
        {
            //只是对Public的并且又Set的方法赋值。
            BurrowFramework bf = new BurrowFramework();
            object id = dictionary[idBag[type]];

            object result = bf.GetSession(type).Get(type, id);

            foreach (PropertyInfo info in type.GetProperties(setProperty))
            {
                if (dictionary.ContainsKey(info.Name))
                {
                    object value = dictionary[info.Name];
                    info.SetValue(result, value, null);
                }
            }
            return result;
        }

        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            BurrowFramework bf = new BurrowFramework();
            //从缓冲中删除。哪么在设定过程中就不会触发LazyLoad。
            bf.GetSession().Evict(obj);
            //已经处理过一次的对象。
            ArrayList hadConverObjects = new ArrayList();
            hadConverObjects.Add(obj);

            Dictionary<string, object> result = new Dictionary<string, object>();
            CreateResult(result, obj, hadConverObjects);
            return result;
        }

        private void CreateResult(Dictionary<string, object> result, object obj, IList hadConverObjects)
        {
            Type type = obj.GetType();
            foreach (PropertyInfo proertyInfo in type.GetProperties(getProperty))
            {
                object value;
                //HibernateLazyInitializer是懒加载对象独有属性。
                if ("HibernateLazyInitializer" != proertyInfo.Name &&
                    GetObject(proertyInfo, obj, hadConverObjects, out value))
                {
                    if (!result.ContainsKey(proertyInfo.Name))
                        result.Add(proertyInfo.Name, value);
                }
            }
        }


        private bool GetObject(PropertyInfo propertyInfo, Object obj, IList hadConverObjects, out object value)
        {
            PropertyInfo info = obj.GetType().GetProperty("HibernateLazyInitializer");
            value = null;
            if (info != null)
            { 
                CastleLazyInitializer lazyObject = (CastleLazyInitializer) info.GetValue(obj, null);
                //这个lazyLoad属性并没有初始化。
                if (lazyObject.IsUninitialized)
                {
                    return false;
                }
            }

            if (IsValueType(propertyInfo.PropertyType))
            {
                value = propertyInfo.GetValue(obj, null);
                return true;
            }
            else
            {
                value = null;
                //不知道怎样处理Index属性。？？？
                if (propertyInfo.GetIndexParameters().Length != 0)
                    return false;
                
                if (supportedTypes.Contains(propertyInfo.PropertyType))
                {
                    value = propertyInfo.GetValue(obj, null);
                    //如果已经处理过就不再处理，跳过。
                    if (hadConverObjects.Contains(value))
                        return false;
                    else
                    {
                        hadConverObjects.Add(value);
                        return true;
                    }
                }
                else if (IsCollectionWithNHibernateObject(propertyInfo.PropertyType))
                {
                    value = propertyInfo.GetValue(obj, null);
                    return true;
                }

                return false;
            }
        }

        private static bool IsValueType(Type t)
        {
            if (t == typeof (ValueType) || t == typeof (string))
                return true;
            else
            {
                if (t.BaseType != null)
                    return IsValueType(t.BaseType);
                else return false;
            }
        }

        private bool IsCollectionWithNHibernateObject(Type t)
        {
            if (t == typeof (IEnumerable))
            {
                foreach (Type type in t.GetGenericArguments())
                {
                    return supportedTypes.Contains(type);
                }
                return false;
            }
            else
            {
                if (t.BaseType != null)
                {
                    return IsCollectionWithNHibernateObject(t.BaseType);
                }
                else
                    return false;
            }
        }
    }
}
