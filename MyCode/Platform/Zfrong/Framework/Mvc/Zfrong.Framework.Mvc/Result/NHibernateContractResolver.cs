using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using NHibernate.Proxy;
using Newtonsoft.Json;
using System.Diagnostics;

namespace QF.MVC.Result
{
    //自定义Json序列化规则
    public class NHibernateContractResolver : DefaultContractResolver
    {
        private string[] exceptMemberName;//显示指定需要序列化的属性的名称数组

        private static readonly MemberInfo[] NHibernateProxyInterfaceMembers = typeof(INHibernateProxy).GetMembers();

        public NHibernateContractResolver(string[] exceptMemberName)
        {
            this.exceptMemberName = exceptMemberName;
        }
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            var members = new List<PropertyInfo>(objectType.GetProperties());//只出属性
            members.RemoveAll(memberInfo =>
                // (IsMemberNames(memberInfo))||
                (IsMemberPartOfNHibernateProxyInterface(memberInfo)) ||
                (IsMemberDynamicProxyMixin(memberInfo)) ||
                (IsMemberMarkedWithIgnoreAttribute(memberInfo, objectType)) ||
                (IsInheritedISet(memberInfo)) ||
                (IsInheritedEntity(memberInfo))
                );

            var actualMemberInfos = new List<MemberInfo>();
            foreach (var memberInfo in members)
            {
                var infos = memberInfo.DeclaringType.BaseType.GetMember(memberInfo.Name);
                actualMemberInfos.Add(infos.Length == 0 ? memberInfo : infos[0]);
                //Debug.WriteLine(memberInfo.Name);
            }
            return actualMemberInfos;
        }
        private static bool IsMemberDynamicProxyMixin(PropertyInfo memberInfo) //忽略nhib代理
        {
            return memberInfo.Name == "__interceptors";
        }

        private static bool IsMemberPartOfNHibernateProxyInterface(PropertyInfo memberInfo) //忽略nhib代理
        {
            return Array.Exists(NHibernateProxyInterfaceMembers, mi => memberInfo.Name == mi.Name);
        }

        private static bool IsMemberMarkedWithIgnoreAttribute(PropertyInfo memberInfo, Type objectType)//忽略带标记JsonIgnore的属性
        {
            var infos = typeof(INHibernateProxy).IsAssignableFrom(objectType) ?
                objectType.BaseType.GetMember(memberInfo.Name) :
                objectType.GetMember(memberInfo.Name);
            return infos[0].GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Length > 0;
        }

        private bool IsExceptMember(PropertyInfo memberInfo)
        {
            if (exceptMemberName == null)
                return false;
            return Array.Exists(exceptMemberName, i => memberInfo.Name == i);
        }

        private bool IsInheritedISet(PropertyInfo memberInfo)//忽略掉数据类型为ISet<T>的属性
        {
            return (memberInfo.PropertyType.Name == "ISet`1" && !IsExceptMember(memberInfo));
        }

        private bool IsInheritedEntity(PropertyInfo memberInfo)//忽略掉数据类型为Entity的属性，传进来的例外字段名除外
        {
            return (FindBaseType(memberInfo.PropertyType).Name == "Entity" && !IsExceptMember(memberInfo));
        }
        private static Type FindBaseType(Type type)
        {
            if (!type.IsClass)
                return type;
            if (type.Name == "Entity" || type.Name == "Object")
            {
                return type;
            }
            return FindBaseType(type.BaseType);
        }
    }
}
