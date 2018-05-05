using System;
using NHibernate;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
namespace Com.Zengfr.Proj.Common.NH
{
    public static class HintOptionExtensions
    {
        public static ICriteria HintOption(this ICriteria query, string option)
        {
            string field = query.GetField<string>("comment");
            return query.SetComment(field + string.Format("{0}{{{1}}}", " hint-option:", option));
        }
        public static IQuery HintOption(this IQuery query, string option)
        {
            string field = query.GetField<string>("comment");
            return query.SetComment(field + string.Format("{0}{{{1}}} ", " hint-option:", option));
        }
        public static ICriteria HintOptionRecompile(this ICriteria query)
        {
            return query.HintOption("RECOMPILE");
        }
        public static IQuery HintOptionRecompile(this IQuery query)
        {
            return query.HintOption("RECOMPILE");
        }
        public static ICriteria HintSQL(this ICriteria query, string sql)
        {
            string field = query.GetField<string>("comment");
            return query.SetComment(field + string.Format("{0}{{{1}}} ", " hint-sql:", sql));
        }
        public static IQuery HintSQL(this IQuery query, string sql)
        {
            string field = query.GetField<string>("comment");
            return query.SetComment(field + string.Format("{0}{{{1}}} ", " hint-sql:", sql));
        }
        public static ICriteria HintTable(this ICriteria query, string table, string content)
        {
            string field = query.GetField<string>("comment");
            return query.SetComment(field + string.Format("{0}{1}{{{2}}} ", " hint-table:", table, content));
        }
        public static IQuery HintTable(this IQuery query, string table, string content)
        {
            string field = query.GetField<string>("comment");
            return query.SetComment(field + string.Format("{0}{1}{{{2}}} ", " hint-table:", table, content));
        }
        public static IQuery HintNoLock(this IQuery query, string table)
        {
            return query.HintTable(table, "With(NOLOCK)");
        }
        public static ICriteria HintNoLock(this ICriteria query, string table)
        {
            return query.HintTable(table, "With(NOLOCK)");
        }
        public static IQuery HintNoLock<T>(this IQuery query, ISession session)
        {
            string dBTableName = session.GetDBTableName<T>();
            return query.HintNoLock(dBTableName);
        }
        public static ICriteria HintNoLock<T>(this ICriteria query, ISession session)
        {
            string dBTableName = session.GetDBTableName<T>();
            return query.HintNoLock(dBTableName);
        }
        public static IQuery HintNoLock<T>(this IQuery query, ISessionFactory sessionFactory)
        {
            string dBTableName = sessionFactory.GetDBTableName<T>();
            return query.HintNoLock(dBTableName);
        }
        public static ICriteria HintNoLock<T>(this ICriteria query, ISessionFactory sessionFactory)
        {
            string dBTableName = sessionFactory.GetDBTableName<T>();
            return query.HintNoLock(dBTableName);
        }
        public static ICriteria HintNoLock(this ICriteria query, ISessionFactory sessionFactory, Type persistentClass)
        {
            string dBTableName = sessionFactory.GetDBTableName(persistentClass);
            return query.HintNoLock(dBTableName);
        }
        public static T GetField<T>(this object obj, string name)
        {
            System.Type type = obj.GetType();
            while (type != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name, HintOptionExtensions.GetBindingFlags());
                if (field != null)
                {
                    break;
                }
                type = type.BaseType;
            }
            return (T)((object)type.InvokeMember(name, HintOptionExtensions.GetBindingFlags() | System.Reflection.BindingFlags.GetField, null, obj, null));
        }
        public static void SetField(this object obj, string name, object value)
        {
            System.Type type = obj.GetType();
            while (type != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name, HintOptionExtensions.GetBindingFlags());
                if (field != null)
                {
                    break;
                }
                type = type.BaseType;
            }
            type.InvokeMember(name, HintOptionExtensions.GetBindingFlags() | System.Reflection.BindingFlags.SetField, null, obj, new object[]
			{
				value
			});
        }
        private static System.Reflection.BindingFlags GetBindingFlags()
        {
            return System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.ExactBinding;
        }
        public static string GetDBTableName<T>(this ISession session)
        {
            IClassMetadata classMetadata = session.SessionFactory.GetClassMetadata(typeof(T));
            return ((SingleTableEntityPersister)classMetadata).TableName;
        }
        public static string[] GetDBColumnNames<T>(this ISession session, string property)
        {
            IClassMetadata classMetadata = session.SessionFactory.GetClassMetadata(typeof(T));
            return ((SingleTableEntityPersister)classMetadata).GetPropertyColumnNames(property);
        }
        public static string GetDBTableName<T>(this ISessionFactory sessionFactory)
        {
            IClassMetadata classMetadata = sessionFactory.GetClassMetadata(typeof(T));
            return ((SingleTableEntityPersister)classMetadata).TableName;
        }
        public static string[] GetDBColumnNames<T>(ISessionFactory sessionFactory, string property)
        {
            IClassMetadata classMetadata = sessionFactory.GetClassMetadata(typeof(T));
            return ((SingleTableEntityPersister)classMetadata).GetPropertyColumnNames(property);
        }
        public static string GetDBTableName(this ISessionFactory sessionFactory, Type persistentClass)
        {
            IClassMetadata classMetadata = sessionFactory.GetClassMetadata(persistentClass);
            return ((SingleTableEntityPersister)classMetadata).TableName;
        }
    }
}