using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Com.Zfrong.Common.Command
{
    /// <summary>
    /// NotifyDicionary���ݸ���ʱ�Ļص�����
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="value"></param>
    public delegate void OnDataSetEventHandler(CoreDataType Key, object value);
    /// <summary>
    /// һ��֧��INotifyPropertyChanged�ӿڵĹ����������洢����ȫ�ֵ�
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
        /// һ��ͬ����������
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
        /// ����ָ���������ݵĸ�����Ϣ
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
        /// �ⲿ�¼��ҽӽӿڡ��κ��ⲿ�¼�������ͨ����̵������������
        /// ��������������ú�
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="paras"></param>
        public void OnExternalMethodsInvoked(CoreDataType Type, params object[] paras)
        { 
        
        }

        #region INotifyPropertyChanged ��Ա

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    ///// <summary>
    ///// ����������������
    ///// </summary>
    //public enum CoreDataType
    //{
    // 
    //}
}
