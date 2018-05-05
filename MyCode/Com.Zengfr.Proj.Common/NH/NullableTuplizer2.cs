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
    public class NullableTuplizer2 : PocoEntityTuplizer
    {
        //http://nhibernate.info/blog/2011/01/28/how-to-use-0-instead-of-null-for-foreign-keys.html
        //http://stackoverflow.com/questions/4776618/nhibernate-0-in-foreign-key-column
        public NullableTuplizer2(EntityMetamodel entityMetamodel, PersistentClass mappedEntity)
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
                if (IsEntityType(getters[i].ReturnType)
                     && values[i] != null)
                {
                    if (GetPropertyValue<long>(values[i], getters[i].ReturnType.Name + "ID") == 0)
                    {
                        values[i] = null;
                    }
                }
                else if (getters[i].ReturnType.IsClass && !getters[i].ReturnType.Name.EndsWith("String"))
                {
                    SetEntityTypeToNULL(values[i]);
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
                if (IsEntityType(getters[i].ReturnType)
                     && values[i] != null)
                {
                    if (GetPropertyValue<long>(values[i], getters[i].ReturnType.Name + "ID") == 0)
                    {
                        values[i] = null;
                    }
                }
                else if (getters[i].ReturnType.IsClass && !getters[i].ReturnType.Name.EndsWith("String"))
                {
                    SetEntityTypeToNULL(values[i]);
                }
            }
            return values;
        }


        public override void SetPropertyValues(object entity, object[] values)
        {
            //dirty hack 3.
            for (int i = 0; i < values.Length; i++)
            {
                if (IsEntityType(getters[i].ReturnType)
                    && values[i] != null)
                {
                    if (GetPropertyValue<long>(values[i], getters[i].ReturnType.Name + "ID") == 0)
                    {
                        values[i] = null;
                    }
                }
                else if (getters[i].ReturnType.IsClass && !getters[i].ReturnType.Name.EndsWith("String"))
                {
                    SetEntityTypeToNULL(values[i]);
                }
            }
            base.SetPropertyValues(entity, values);
        }
        private static void SetEntityTypeToNULL(object obj)
        {
            if (obj != null)
            {
                var type = obj.GetType();
                var ps = type.GetProperties();
                foreach (var p in ps)
                {
                    var pv = p.GetValue(obj, null);
                    if (pv != null && p.PropertyType.IsClass)
                    {
                        if (IsEntityType(p.PropertyType))
                        {

                            if (GetPropertyValue<long>(pv, p.PropertyType.Name + "ID") == 0)
                            {
                                p.SetValue(obj, null, null);
                            }
                        }
                        else if (!p.PropertyType.Name.EndsWith("String"))
                        {
                            SetEntityTypeToNULL(pv);
                        }
                    }
                }
            }
        }
        private static T GetPropertyValue<T>(object obj, string property)
        {
            return (T)obj.GetType().GetProperty(property).GetValue(obj, null);
        }
        private static bool IsEntityType(Type type)
        {
            if (type.FullName.StartsWith("System") || type.Name.EndsWith("Object") || !type.IsClass)
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
