using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Tuple.Entity;
using System.Collections;
using NHibernate.Engine;
using NHibernate.Mapping;

namespace Com.Zengfr.Proj.Common.NH
{
    /// <summary>
    ///    在 NHibernate.Engine.ForeignKeys.GetEntityIdentifierIfNotUnsaved(String entityName, Object entity, ISessionImplementor session)
    //在 NHibernate.Type.EntityType.GetIdentifier(Object value, ISessionImplementor session)
    //在 NHibernate.Type.ManyToOneType.NullSafeSet(IDbCommand st, Object value, Int32 index, Boolean[] settable, ISessionImplementor session)
    //在 NHibernate.Persister.Entity.AbstractEntityPersister.Dehydrate(Object id, Object[] fields, Object rowId, Boolean[] includeProperty, Boolean[][] includeColumns, Int32 table, IDbCommand statement, ISessionImplementor session, Int32 index)
    // /// </summary>
    public class NullableTuplizer : PocoEntityTuplizer
    {
        //http://nhibernate.info/blog/2011/01/28/how-to-use-0-instead-of-null-for-foreign-keys.html
        //http://stackoverflow.com/questions/4776618/nhibernate-0-in-foreign-key-column
        public NullableTuplizer(EntityMetamodel entityMetamodel, PersistentClass mappedEntity)
            : base(entityMetamodel, mappedEntity)
        {

        }

        public override object[] GetPropertyValuesToInsert(
            object entity, IDictionary mergeMap, ISessionImplementor session)
        {
            object[] values = base.GetPropertyValuesToInsert(entity, mergeMap, session);
            //dirty hack 1
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null && IsEntityType(getters[i].ReturnType))
                {
                    values[i] = ProxyFactory.GetProxy(0, null);
                }
            }
            return values;
        }

        public override object[] GetPropertyValues(object entity)
        {
            object[] values = base.GetPropertyValues(entity);
            //dirty hack 2
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null && IsEntityType(getters[i].ReturnType))
                {
                    values[i] = ProxyFactory.GetProxy(0, null);
                }
            }
            return values;
        }


        public override void SetPropertyValues(object entity, object[] values)
        {
            //dirty hack 3.
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null && IsEntityType(getters[i].ReturnType))
                {
                    values[i] = Activator.CreateInstance(getters[i].ReturnType, null);
                }
                //if (IsEntityType(getters[i].ReturnType)
                //    && values[i] != null)
                //{
                //    if (GetPropertyValue<long>(values[i], getters[i].ReturnType.Name + "ID") == 0)
                //    {
                //        values[i] = null;
                //    }
                //}
            }
            base.SetPropertyValues(entity, values);
        }
        private static T GetPropertyValue<T>(object obj, string property)
        {
            return (T)obj.GetType().GetProperty(property).GetValue(obj, null);
        }
        private static bool IsEntityType(Type type)
        {
            if (type.FullName.StartsWith("System") || type.Name.EndsWith("Object"))
            {
                return false;
            }
            if (type.Name.EndsWith("AbstractEntity") || type.Name.EndsWith("AbstractModel"))
            {
                return true;
            }
            return IsEntityType(type.BaseType);
        }
    }
}
