using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using System.Collections;
using System.Security.Cryptography;
using System.IO;
using NHibernate;
using NHibernate.Criterion;
//using NUnit.Framework;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Queries;
using Math = System.Math;
namespace Com.Zfrong.Common.Data.AR.Base
{
    public class DB : ActiveRecordMediator
    {

    }
    public class DB<T> : PowerActiveRecordBase<T> where T : class
    {

    }
    public class DBAccess<T> : PowerActiveRecordBase<T> where T : class
    {

    }
    public class DBService<T> : PowerActiveRecordBase<T> where T : class
    {

    }
    public class DBDAL<T> : PowerActiveRecordBase<T> where T : class
    {

    }
    public class ActiveRecordDAL<T> : PowerActiveRecordBase<T> where T : class
    {

    }
    public class PowerActiveRecordBase<T> : ActiveRecordValidationBase<T> where T : class
    {
        public static ISessionFactoryHolder Holder
        {
            get { return ActiveRecordBase<T>.holder; }
            set { ActiveRecordBase<T>.holder=value; }
        }

        #region Create/Update/Save/Delete/Refresh

        #region Create

        /// <summary>
        /// Creates (Saves) a new instance to the database.
        /// </summary>
        /// <param name="instance">The ActiveRecord instance to be created on the database</param>
        public new static void Create(T instance)
        {
            ActiveRecordValidationBase<T>.Create(instance);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes the instance from the database.
        /// </summary>
        /// <param name="instance">The ActiveRecord instance to be deleted</param>
        public new static void Delete(T instance)
        {
            ActiveRecordValidationBase<T>.Delete(instance);
        }

        #endregion

        #region DeleteAll

        /// <summary>
        /// Deletes all rows for the specified ActiveRecord type
        /// </summary>
        /// <remarks>
        /// This method is usually useful for test cases.
        /// </remarks>
        public new static void DeleteAll()
        {
            ActiveRecordValidationBase<T>.DeleteAll(typeof(T));
        }

        /// <summary>
        /// Deletes all rows for the specified ActiveRecord type that matches
        /// the supplied HQL condition
        /// </summary>
        /// <remarks>
        /// This method is usually useful for test cases.
        /// </remarks>
        /// <param name="where">HQL condition to select the rows to be deleted</param>
        public new static void DeleteAll(String where)
        {
            ActiveRecordValidationBase<T>.DeleteAll(typeof(T), where);
        }

        /// <summary>
        /// Deletes all <typeparamref name="T"/> objects, based on the primary keys
        /// supplied on <paramref name="pkValues" />.
        /// </summary>
        /// <returns>The number of objects deleted</returns>
        public new static int DeleteAll(IEnumerable pkValues)
        {
            return ActiveRecordValidationBase<T>.DeleteAll(typeof(T), pkValues);
        }

        #endregion

        #region Refresh

        /// <summary>
        /// Refresh the instance from the database.
        /// </summary>
        /// <param name="instance">The ActiveRecord instance to be reloaded</param>
        public new static void Refresh(T instance)
        {
            ActiveRecordValidationBase<T>.Refresh(instance);
        }

        #endregion

        #region Update

        /// <summary>
        /// Persists the modification on the instance
        /// state to the database.
        /// </summary>
        /// <param name="instance">The ActiveRecord instance to be updated on the database</param>
        public new static void Update(T instance)
        {
            ActiveRecordValidationBase<T>.Update(instance); 
        }
        public static void UpdateProperty(T instance, string propertyName)
        {
            Type t = typeof(T);
           object v=t.GetProperty(propertyName).GetValue(instance, null);
           ExecuteNonQuery("Update "+GetTableName()+" Set "+GetColumnName(propertyName)+"="+v+" Where ID="+"");//
            //ActiveRecordValidationBase<T>.Update(instance);//zfr
        }
        #endregion

        #region Save

        /// <summary>
        /// Saves the instance to the database. If the primary key is unitialized
        /// it creates the instance on the database. Otherwise it updates it.
        /// <para>
        /// If the primary key is assigned, then you must invoke <see cref="Create"/>
        /// or <see cref="Update"/> instead.
        /// </para>
        /// </summary>
        /// <param name="instance">The ActiveRecord instance to be saved</param>
        public new static void Save(T instance)
        {
            ActiveRecordValidationBase<T>.Save(instance);
        }

        /// <summary>
        /// Saves a copy of the instance to the database. If the primary key is unitialized
        /// it creates the instance on the database. Otherwise it updates it.
        /// <para>
        /// If the primary key is assigned, then you must invoke <see cref="Create"/>
        /// or <see cref="Update"/> instead.
        /// </para>
        /// </summary>
        /// <param name="instance">The transient instance to be saved</param>
        /// <returns>The saved ActiveRecord instance.</returns>
        public new static T SaveCopy(T instance)
        {
            return ActiveRecordValidationBase<T>.SaveCopy(instance);
        }

        #endregion

        #endregion

        #region *AndFlush

        public static void SaveAndFlush(T instance)
        {
            ActiveRecordValidationBase<T>.SaveAndFlush(instance);
        }
        public static void CreateAndFlush(T instance)
        {
            ActiveRecordValidationBase<T>.CreateAndFlush(instance);
        }
        public static void DeleteAndFlush(T instance)
        {
            ActiveRecordValidationBase<T>.DeleteAndFlush(instance);
        }
        //public static void SaveAndFlush(T instance)
        //{
            //ActiveRecordValidationBase<T>.SaveAndFlush(instance);
        //}
        public static void SaveCopyAndFlush(T instance)
        {
            ActiveRecordValidationBase<T>.SaveCopyAndFlush(instance);
        }
        public static void UpdateAndFlush(T instance)
        {
            ActiveRecordValidationBase<T>.UpdateAndFlush(instance);
        }
        #endregion

        #region Count

        /// <summary>
        /// Returns the number of records of <typeparamref name="T"/> in the database
        /// </summary>
        /// <returns>The count query result</returns>
        public new static int Count()
        {
            return ActiveRecordValidationBase<T>.Count();
        }

        /// <summary>
        /// Returns the number of records of <typeparamref name="T"/> in the database
        /// </summary>
        /// <example>
        /// <code escaped="true">
        /// [ActiveRecord]
        /// public class User : ActiveRecordBase&lt;User&gt;
        /// {
        ///   ...
        ///   
        ///   public static int CountAllUsersLocked()
        ///   {
        ///     return Count("IsLocked = ?", true); // Equivalent to: Count(typeof(User), "IsLocked = ?", true);
        ///   }
        /// }
        /// </code>
        /// </example>
        /// <param name="filter">A sql where string i.e. Person=? and DOB &gt; ?</param>
        /// <param name="args">Positional parameters for the filter string</param>
        /// <returns>The count result</returns>
        public new static int Count(String filter, params object[] args)
        {
            return ActiveRecordValidationBase<T>.Count(filter, args);
        }

        /// <summary>
        /// Check if any instance matching the criteria exists in the database.
        /// </summary>
        /// <param name="criteria">The criteria expression</param>		
        /// <returns>The count result</returns>
        public new static int Count(params ICriterion[] criteria)
        {
            return ActiveRecordValidationBase<T>.Count(criteria);
        }

        /// <summary>
        /// Returns the number of records of the specified 
        /// type in the database
        /// </summary>
        /// <param name="detachedCriteria">The criteria expression</param>
        /// <returns>The count result</returns>
        public new static int Count(DetachedCriteria detachedCriteria)
        {
            return ActiveRecordValidationBase<T>.Count(detachedCriteria);
        }

        #endregion

        #region FindByPrimaryKey/Find/TryFind

        /// <summary>
        /// Finds an object instance by an unique ID for <typeparamref name="T"/>
        /// </summary>
        /// <param name="id">ID value</param>
        /// <returns>A <typeparamref name="T"/></returns>
        public new static T FindByPrimaryKey(object id)
        {
            return ActiveRecordValidationBase<T>.FindByPrimaryKey(id);
        }

        /// <summary>
        /// Finds an object instance by a unique ID for <typeparamref name="T"/>
        /// </summary>
        /// <param name="id">ID value</param>
        /// <param name="throwOnNotFound"><c>true</c> if you want to catch an exception 
        /// if the object is not found</param>
        /// <returns>A <typeparamref name="T"/></returns>
        /// <exception cref="ObjectNotFoundException">if <c>throwOnNotFound</c> is set to 
        /// <c>true</c> and the row is not found</exception>
        public new static T FindByPrimaryKey(object id, bool throwOnNotFound)
        {
            return ActiveRecordValidationBase<T>.FindByPrimaryKey(id, throwOnNotFound);
        }
        
        #endregion

        #region 123
        public new static void Replicate(object instance, ReplicationMode replicationMode)
        {
                ActiveRecordValidationBase<T>.Replicate(instance, replicationMode);
        }
        public new static R ExecuteQuery2<R>(IActiveRecordQuery<R> query)
        {
            return ActiveRecordValidationBase<T>.ExecuteQuery2(query);
        }
        public new static IEnumerable EnumerateQuery(IActiveRecordQuery query) 
        {
            return ActiveRecordValidationBase<T>.EnumerateQuery(query);//
        }
        public new static object Execute(NHibernateDelegate call, object instance)
        {
            return ActiveRecordValidationBase<T>.Execute(call, instance);
        }
        //public  static T GroupBy(string[] proNames)
        //{
        //   return GroupBy<T>(proNames);//
        //}
        //public static R GroupBy<R>(string[] proNames)
        //{
        //    IActiveRecordQuery q = new HqlBasedQuery(typeof(R), "");//
        //    return ExecuteQuery2<R>(q);//
        //}
        static void ExecuteNonQuery(string sql)
        {
            ISessionFactoryHolder sessionHolder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = sessionHolder.CreateSession(typeof(T));
            try
            {
                System.Data.IDbCommand command = session.Connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                throw e;
            }

            sessionHolder.ReleaseSession(session);
        }
        static string GetTableName()
        {
            Type t = typeof(T);
            object[] objs=t.GetCustomAttributes(typeof(ActiveRecordAttribute), false);
            if (objs == null || objs.Length == 0)
                return t.Name;
            ActiveRecordAttribute att = objs[0] as ActiveRecordAttribute;
            return att.Table;
        }
        static string GetColumnName(string propertyName)
        {
           Type t = typeof(T);
           System.Reflection.PropertyInfo p = t.GetProperty(propertyName,System.Reflection.BindingFlags.Default);
           object[] objs = p.GetCustomAttributes(typeof(PropertyAttribute), false);
           if (objs == null || objs.Length == 0)
               return p.Name;
           PropertyAttribute att = objs[0] as PropertyAttribute;
            return att.Column;
        }
        #endregion

        #region Session/Transaction
        public static ISession GetSession()
        {
            return GetSession<T>();//
        }
        public static ISession GetSession<K>()
        {
            //ISessionFactoryHolder holder = ActiveRecordMediator.GetSessionFactoryHolder();
            ISession session = Holder.CreateSession(typeof(K));//
            return session;//
        }
        public static IDbTransaction BeginTransaction()
        {
            return BeginTransaction(GetSession());//
        }
        public static IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return BeginTransaction(GetSession(),il);//
        }
        public static IDbTransaction BeginTransaction(ISession session)
        {
            return session.Connection.BeginTransaction();//
        }
        public static IDbTransaction BeginTransaction(ISession session,IsolationLevel il)
        {
            return session.Connection.BeginTransaction(il);//
        }
        #endregion
    }
    /// <summary>
    /// 作者:曾繁荣 2008。8。8
    /// </summary>
    public class PowerCommon
    {
        #region DESC DE/EN MD5
        public static string MD5(string str)
        {
            MD5CryptoServiceProvider p = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            bytes = p.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte num in bytes)
            {
                sb.AppendFormat("{0:x2}", num);
            }
            return sb.ToString();
        }
        private const string DESCKey = ".ZFR0NG.";
        public static string DESCEn(string pToEncrypt)
        {
            return DESCEn(pToEncrypt, DESCKey);
        }
        public static string DESCDe(string pToDecrypt)
        {
            return DESCDe(pToDecrypt, DESCKey);
        }
        public static string DESCEn(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }
        public static string DESCDe(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        #endregion

        #region Is
        public static bool IsNull(object obj)
        {
            return obj == null;//
        }
        public static bool IsNull<T>(T obj)
        {
            return obj == null;//
        }

        public static string IsNullSetDef(string obj)
        {
           return IsNullSetDef<string>(obj);
        }
        public static T IsNullSetDef<T>(T obj)
        {
            if(IsNull<T>(obj))
                return default(T);
            return obj;
        }

        public static bool IsNullOrEmpty(string obj)
        {
            return obj == null || obj.Length == 0;//
        }

        public static bool IsNullOrEmpty(ICollection obj)
        {
            return obj == null || obj.Count == 0;//
        }
        public static bool IsNullOrEmpty<T>(ICollection<T> obj)
        {
            return obj == null || obj.Count == 0;//
        }
        public static bool IsNullOrEmpty(IList obj)
        {
            return obj == null || obj.Count == 0;//System.Collections.Generic.IDictionary<TKey,TValue>
        }
        public static bool IsNullOrEmpty<T>(IList<T> obj)
        {
            return obj == null || obj.Count == 0;//
        }
        public static bool IsNullOrEmpty(IDictionary obj)
        {
            return obj == null || obj.Count == 0;//
        }
        public static bool IsNullOrEmpty<K,V>(IDictionary<K, V> obj)
        {
            return obj == null || obj.Count == 0;//
        }

        public static bool IsInclude(int[] objs, int obj)
        {
            return IsInclude<int>(objs, obj);
        }
        public static bool IsInclude(string[] objs, string obj)
        {
            return IsInclude<string>(objs, obj);
        }
        public static bool IsInclude<T>(T[] objs, T obj)
        {
            bool b = false;
            for (int i = 0; i <objs.Length; i++)
            {
                if (objs[i].Equals(obj))
                {
                    b = true; break;
                }
            }
            return b;
        }
        public static bool IsInclude<T>(ICollection<T> objs, T obj)
        {
            return objs.Contains(obj);
        }
       
        #endregion

        #region Max/Min/Sum
        public static int Div(int i, int r)
        {
            return (int)System.Math.Ceiling((double)i / r);
        }
        public static void Max(int i, ref int r)
        {
            r = System.Math.Max(r, i);
        }
        public static void Min(int i, ref int r)
        {
            r = System.Math.Min(r, i);
        }
        public static void Sum(int i, ref int r)
        {
            r += i;
        }
        public static void Sum(decimal i, ref decimal r)
        {
            r += i;
        }
        #endregion

        #region Max/Min/Sum
        public static int MaxIndex(params int[] param)
        {
            int j =int.MinValue,index=0;
            for (int i = 0; i < param.Length; i++)
            {
                j = System.Math.Max(j, param[i]);
                if (j == param[i]) index = i;
            }
            return index;
        }
        public static int MinIndex(params int[] param)
        {
            int j = int.MaxValue, index = 0;
            for (int i = 0; i < param.Length; i++)
            {
                j = System.Math.Min(j, param[i]);
                if (j == param[i]) index = i;
            }
            return index;
        }
        public static int Max(params int[] param)
        {
            int j = int.MinValue;
            for (int i = 0; i < param.Length; i++)
                j = System.Math.Max(j, param[i]);
            return j;
        }
        public static int Min(params int[] param)
        {
            int j = int.MaxValue;
            for (int i = 0; i < param.Length; i++)
                j = System.Math.Min(j, param[i]);
            return j;
        }
        public static int Sum(params int[] param)
        {
            int j = 0;
            for (int i = 0; i < param.Length; i++)
                j += param[i];
            return j;
        }
        public static decimal Sum(params decimal[] param)
        {
            decimal j = 0;
            for (int i = 0; i < param.Length; i++)
                j += param[i];
            return j;
        }
        #endregion
         
        #region Max/Min Length Str
        public static int Length(string obj)
        {
            if (obj == null) return 0;
            return obj.Length;
        }
        public static int[] Length(string[] objs)
        {
            int[] list = new int[objs.Length];
            for (int i = 0; i < list.Length; i++)
                list[i] = Length(objs[i]);//长度
            return list;//
        }
        public static int MaxLength(string[] objs)
        {
            return Max(Length(objs));
        }
        public static int MinLength(string[] objs)
        {
            return Min(Length(objs));
        }
        public static string MaxLenStr(string[] objs)
        {int index=MaxIndex(Length(objs));
        return objs[index];
        }
        public static string MinLenStr(string[] objs)
        {
            int index = MinIndex(Length(objs));
            return objs[index];
        }
        #endregion

        #region ConvertTo
        public static K ConvertTo<K>(object obj)
        {
            return (K)obj;//
        }
        public static K ConvertToOrDef<K>(object obj)
        {
            try
            {
                return (K)obj;//
            }
            catch {
                return default(K);
            }
        }
        #endregion

        #region BuildCurrentQueryString
        public static string BuildQueryString(System.Collections.Specialized.NameValueCollection QueryString)
        {
            StringBuilder sb = new StringBuilder(); sb.Append("?");
            for (int i = 0; i < QueryString.Count; i++)
            {
                sb.Append("&" + QueryString.GetKey(i) + "=" + UrlEncode(QueryString.Get(i)));//
            }
            return sb.ToString();
        }
        public static string BuildCurrentQueryString(string name, int value)
        {
            return BuildCurrentQueryString(name, value.ToString());
        }
        public static string BuildCurrentQueryString(string name, string value)
        {
            return BuildCurrentQueryString(new string[] { name }, new string[] { value });//
        }
        public static string BuildCurrentQueryString(string[] names,string[] values)
        {
            StringBuilder sb = new StringBuilder(47); sb.Append("?");
            for (int i = 0; i < names.Length; i++)
            {
                sb.Append("&" + names[i] + "=" + UrlEncode(values[i]));//
            }
            string k;
            for (int i = 0; i < System.Web.HttpContext.Current.Request.QueryString.Count; i++)
            {
                k= System.Web.HttpContext.Current.Request.QueryString.GetKey(i);
                if (IsInclude(names,k))
                {
                    sb.Append("&" + k + "=" + UrlEncode(System.Web.HttpContext.Current.Request.QueryString.Get(i)));//
                }
            }
            return sb.ToString();
        }
        #endregion

        #region Encode/Decode
        public static string UrlEncode(string str)
        {
            return System.Web.HttpContext.Current.Server.UrlEncode(str);
        }
        public static string UrlDecode(string str)
        {
            return System.Web.HttpContext.Current.Server.UrlDecode(str);
        }
        public static string HtmlEncode(string str)
        {
            return System.Web.HttpContext.Current.Server.HtmlEncode(str);
        }
        public static string HtmlDecode(string str)
        {
            return System.Web.HttpContext.Current.Server.HtmlDecode(str);
        }
        #endregion

        public class Cached
        {
            #region Cached
            private static Dictionary<int, object> CachedDict = new Dictionary<int, object>();//
            public static K Get<K>(string key)
            {
                return PowerCommon.ConvertTo<K>(Get(key));//
            }
            public static K Get<K>(int key)
            {
                return PowerCommon.ConvertTo<K>(Get(key));//
            }
            public static object Get(string key)
            {
                return Get(key.GetHashCode());//
            }
            public static object Get(int key)
            {
                return CachedDict[key];//
            }
            public static void Set(string key, object value)
            {
                Set<object>(key, value);//
            }
            public static void Set(int key, object value)
            {
                Set<object>(key, value);//
            }
            public static void Set<K>(string key, K value)
            {
                Set<K>(key.GetHashCode(), value);
            }
            public static void Set<K>(int key, K value)
            {
                if(CachedDict.ContainsKey(key))
                    CachedDict[key] = value;//
                else
                    CachedDict.Add(key,value);//
            }
            public static void Remove(string key)
            {
                Remove(key.GetHashCode());//
            }
            public static void Remove(int key)
            {
                CachedDict.Remove(key);//
            }
            public static void ContainsKey(string key)
            {
                ContainsKey(key.GetHashCode());//
            }
            public static void ContainsKey(int key)
            {
                CachedDict.ContainsKey(key);//
            }
            public static void Clear()
            {
                CachedDict.Clear();//
            }
            #endregion
        }
    }
}
