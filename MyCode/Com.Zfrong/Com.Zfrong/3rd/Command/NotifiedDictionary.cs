using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Com.Zfrong.Common.Command
{
    /// <summary>
    /// NotifyDicionary数据更新时的回调方法
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="value"></param>
    public delegate void OnDataSetEventHandler(CoreDataType Key, object value);
    /// <summary>
    /// 一个支持INotifyPropertyChanged接口的哈西表，用来存储各种全局的
    /// </summary>
    public class NotifiedDictionary:Dictionary<CoreDataType,object>,INotifyPropertyChanged
    {

        Dictionary<CoreDataType, PropertyChangedEventArgs> properties =
            new Dictionary<CoreDataType, PropertyChangedEventArgs>();

        [MethodImpl(MethodImplOptions.Synchronized)]
        public new void Add(CoreDataType key, object value)
        {
            base.Add(key, value);
            if (properties.ContainsKey(key))
            {
                return;
            }
            else
            {
                properties.Add(key, new PropertyChangedEventArgs(key.ToString()));
            }
        }

        /// <summary>
        /// 一个同步的索引器
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        
        public new object this[CoreDataType Key]
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return base[Key];
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set 
            {
                if (base[Key] != value)
                {
                    base[Key] = value;
                    this.InvokeCoreDataTypeNotification(Key);
                }
            }
        }
        /// <summary>
        /// zfr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <returns></returns>
        public T Get<T>(CoreDataType Key)
        {
           return (T) this[Key];
        }
        /// <summary>
        /// zfr
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public T Get<T>(CoreDataType Key,int index)
        {
            object[] objs = this[Key] as object[];
            
            return (T)objs[index];
        }
        /// <summary>
        /// 触发指定类型数据的更新信息
        /// </summary>
        /// <param name="Type"></param>
        public void InvokeCoreDataTypeNotification(CoreDataType Type)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, properties[Type]);
            }
        }

        /// <summary>
        /// 外部事件挂接接口。任何外部事件，可以通过编程调用这个方法。
        /// 当这个方法被调用后，
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="paras"></param>
        public void OnExternalMethodsInvoked(CoreDataType Type, params object[] paras)
        { 
        
        }

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    ///// <summary>
    ///// 描述核心数据类型
    ///// </summary>
    //public enum CoreDataType
    //{
    // 
    //}
}
