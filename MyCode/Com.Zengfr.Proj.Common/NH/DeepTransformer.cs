
//
// 创建标识:Copyright (C) 2014-2015 zengfr 版权所有
// 创建描述:
// 创建时间:2015-9-18 21:26:05
// 功能描述:
// 修改标识: 无
// 修改描述: 无
//



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Transform;


namespace Com.Zengfr.Proj.Common.NH
{
    public class DeepTransformer<TEntity> : IResultTransformer
     where TEntity : class
    {
        // rows iterator
        public object TransformTuple(object[] tuple, string[] aliases)
        {
            var list = new List<string>(aliases);

            var propertyAliases = new List<string>(list);
            var complexAliases = new List<string>();

            for (var i = 0; i < list.Count; i++)
            {
                var aliase = list[i];
                // Aliase with the '.' represents complex IPersistentEntity chain
                if ((!string.IsNullOrWhiteSpace(aliase)) && (aliase.Contains('.') || aliase.Contains('_')))
                {
                    complexAliases.Add(aliase);
                    propertyAliases[i] = null;
                }
            }

            // be smart use what is already available
            // the standard properties string, valueTypes
            var result = Transformers
                 .AliasToBean<TEntity>()
                 .TransformTuple(tuple, propertyAliases.ToArray());

            TransformPersistentChain(tuple, complexAliases, result, list);

            return result;
        }

        /// <summary>Iterates the Path Client.Address.City.Code </summary>
        protected virtual void TransformPersistentChain(object[] tuple
              , List<string> complexAliases, object result, List<string> list)
        {
            var entity = result as TEntity;

            foreach (var aliase in complexAliases)
            {
                // the value in a tuple by index of current Aliase
                var index = list.IndexOf(aliase);
                var value = tuple[index];
                SetValue(entity, aliase, value);
            }
        }
        public static void SetValue(object entity, string complexAlias, object value)
        {
            if (value != null)
            {
                var parts = complexAlias.Split('.', '_');
                var name = parts[0];
                var propertyInfo = entity.GetType().GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                object currentObject = entity;
                var current = 1;
                object instance;
                while (current < parts.Length)
                {
                    name = parts[current];
                    instance = propertyInfo.GetValue(currentObject, null);
                    if (instance == null)
                    {
                        instance = Activator.CreateInstance(propertyInfo.PropertyType);
                        propertyInfo.SetValue(currentObject, instance, null);
                    }
                    propertyInfo = propertyInfo.PropertyType.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    currentObject = instance;
                    current++;
                }
                var dictionary = currentObject as IDictionary;
                if (dictionary != null)
                {
                    dictionary[name] = value;
                }
                else
                {
                    propertyInfo.SetValue(currentObject, value, null);
                }
            }
        }
        // convert to DISTINCT list with populated Fields
        public System.Collections.IList TransformList(System.Collections.IList collection)
        {
            var results = Transformers.AliasToBean<TEntity>().TransformList(collection);
            return results;
        }
    }
}

