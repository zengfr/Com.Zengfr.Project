using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace hookInject.core
{
    public static class ObjectExtension
    {
        //获取私有字段的值
        public static T GetPrivateField<T>(this object instance, string fieldName)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo info = type.GetField(fieldName, flags);
            return (T)info.GetValue(instance);
        }

        //设置私有字段的值
        public static void SetPrivateField(this object instance, string fieldName, object value)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            FieldInfo info = type.GetField(fieldName, flags);
            info.SetValue(instance, value);
        }

        //获取私有属性的值
        public static T GetPrivateProperty<T>(this object instance, string propertyName)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo info = type.GetProperty(propertyName, flags);
            return (T)info.GetValue(instance, null);
        }

        //设置私有属性的值
        public static void SetPrivateProperty<T>(this object instance, string propertyName, object value)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            PropertyInfo info = type.GetProperty(propertyName, flags);
            info.SetValue(instance, value, null);
        }

        //调用私有方法
        public static object CallPrivateMethod(this object instance, string methodName, params object[] param)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = instance.GetType();
            MethodInfo info = type.GetMethod(methodName, flags);
            return info.Invoke(instance, param);
        }
    }
}
