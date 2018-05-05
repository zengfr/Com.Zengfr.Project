using System;
using System.Collections.Generic;
using System.Text;

using System.Reflection;
using System.Reflection.Emit;
namespace Com.Zfrong.Common.Data.AR.Base
{
    public class TypeCreator
    {
        public static Type Creator(string ClassName, int PropertiesCount)
        {
            IDictionary<string, Type> Properties = new Dictionary<string, Type>();
            Type t = typeof(string);
            Properties.Add(new KeyValuePair<string, Type>("ID", typeof(int)));
            for (int i = 0; i < PropertiesCount; i++)
            {
                Properties.Add(new KeyValuePair<string, Type>("FF" + i, t));
            }
            return Creator(ClassName, Properties);
        }
        public static Type Creator(string ClassName, IDictionary<string, Type> Properties)
        {
            AppDomain currentDomain = System.Threading.Thread.GetDomain(); //AppDomain.CurrentDomain;
            TypeBuilder typeBuilder = null;
            ModuleBuilder moduleBuilder = null;
            MethodBuilder methodBuilder = null;
            PropertyBuilder propertyBuilder = null;
            FieldBuilder fieldBuilder = null;
            AssemblyBuilder assemblyBuilder = null;
            ILGenerator ilGenerator = null;
            CustomAttributeBuilder cab = null;
            MethodAttributes methodAttrs;

            //Define a Dynamic Assembly
            assemblyBuilder = currentDomain.DefineDynamicAssembly(new AssemblyName("Test2"), AssemblyBuilderAccess.Run);//AssemblyBuilder.GetCallingAssembly().FullName

            //Define a Dynamic Module
            moduleBuilder = assemblyBuilder.DefineDynamicModule("ModuleName", true);

            //Define a runtime class with specified name and attributes.
            typeBuilder = moduleBuilder.DefineType(ClassName, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.BeforeFieldInit | TypeAttributes.Serializable);

            cab = new CustomAttributeBuilder(typeof(Castle.ActiveRecord.ActiveRecordAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
            typeBuilder.SetCustomAttribute(cab);//

            cab = new CustomAttributeBuilder(typeof(Castle.ActiveRecord.PropertyAttribute).GetConstructor(Type.EmptyTypes), new object[0]);

            methodAttrs = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;
            foreach (KeyValuePair<string, Type> kv in Properties)
            {
                // Add the class variable, such as "m_strIPAddress"
                fieldBuilder = typeBuilder.DefineField("field_" + kv.Key, kv.Value, FieldAttributes.Public);

                propertyBuilder = typeBuilder.DefineProperty(kv.Key, System.Reflection.PropertyAttributes.HasDefault, kv.Value, null);
                if (kv.Key != "ID")
                    propertyBuilder.SetCustomAttribute(cab);//
                else
                    propertyBuilder.SetCustomAttribute(new CustomAttributeBuilder(typeof(Castle.ActiveRecord.PrimaryKeyAttribute).GetConstructor(Type.EmptyTypes), new object[0]));//



                methodBuilder = typeBuilder.DefineMethod("get_" + kv.Key, methodAttrs, kv.Value, Type.EmptyTypes);
                ilGenerator = methodBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                ilGenerator.Emit(OpCodes.Ret);
                propertyBuilder.SetGetMethod(methodBuilder);

                methodBuilder = typeBuilder.DefineMethod("set_" + kv.Key, methodAttrs, typeof(void), new Type[] { kv.Value });
                ilGenerator = methodBuilder.GetILGenerator();
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Ldarg_1);
                ilGenerator.Emit(OpCodes.Stfld, fieldBuilder);
                ilGenerator.Emit(OpCodes.Ret);
                propertyBuilder.SetSetMethod(methodBuilder);
            }
            //Create Class 
            return typeBuilder.CreateType();
            return assemblyBuilder.GetType(ClassName);
            return moduleBuilder.GetType(ClassName);

        }
    }
}
