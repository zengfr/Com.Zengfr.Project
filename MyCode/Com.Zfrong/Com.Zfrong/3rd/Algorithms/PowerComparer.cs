using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace Com.Zfrong.Common.Algorithms
{
    ////作者：曾繁荣
    //public class SortItem<TKey> : IComparer, IComparer<SortItem<TKey>>
    //    where TKey : IComparable<TKey>
    //{
    //    public TKey Key;
    //    public object Tag;
    //    #region   IComparer   成员
    //    public SortItem()
    //    {
    //        this.Key = default(TKey);
    //        this.Tag = null;//
    //    }
    //    public SortItem(TKey key)
    //    {
    //        this.Key = key;
    //        this.Tag = null;//
    //    }
    //    public SortItem(TKey key, object tag)
    //    {
    //        this.Key = key;
    //        this.Tag = tag;//
    //    }
    //    public int Compare(object x, object y)
    //    {
    //        SortItem<TKey> objX = x as SortItem<TKey>;
    //        SortItem<TKey> objY = y as SortItem<TKey>;

    //        return Compare(objX, objY);//
    //    }
    //    public int Compare(SortItem<TKey> x, SortItem<TKey> y)
    //    {

    //        int v=0;
    //        switch (SortList<TKey>.SortType)
    //        {
    //            case SortType.Key:
    //                v = x.Key.CompareTo(y.Key);//
    //                break;//
    //            //case SortType.Value:
    //            //    v = x.Value.CompareTo(y.Value);//
    //            //    break;//
    //            case SortType.KeyLength:
    //                v = Compare(x.Key.ToString().Length, y.Key.ToString().Length);//
    //                break;//
    //            //case SortType.ValueLength:
    //            //    v = Compare(x.Value.ToString().Length, y.Value.ToString().Length);//
    //            //    break;//
    //            case SortType.None:
    //                v = 0;//
    //                break;//
    //            default:
    //                v = 0;//
    //                break;
    //        }
    //        return v;
    //    }
    //    private int Compare(int x, int y)
    //    {
    //        if (x > y) return 1;
    //        if (x < y) return -1;
    //        return 0;//
    //    }
    //    #endregion
    //}
    //public class SortList<T> : List<SortItem<T>>
    //     where T : IComparable<T>
    //{
    //    public static SortType SortType = SortType.None;
    //    public SortList(SortType value)
    //    {
    //        SortType = value;//
    //    }
    //    public SortList()
    //        : base()
    //    {

    //    }
    //    public SortType SortedType
    //    {
    //        set
    //        {
    //            SortType = value;//
    //        }
    //        get
    //        {
    //            return SortType;//
    //        }
    //    }
    //}

    //public class SortItem<TKey, TValue> : IComparer,IComparer<SortItem<TKey,TValue>>
    //    where TKey : IComparable<TKey>
    //    where TValue : IComparable<TValue>
    //{
    //    public TKey Key;
    //    public TValue Value;
    //    public object Tag;
    //    #region   IComparer   成员
    //    public SortItem()
    //    {
    //        this.Key = default(TKey);
    //        this.Value = default(TValue);
    //        this.Tag = null;//
    //    }
    //    public SortItem(TKey key, TValue value)
    //    {
    //        this.Key = key;
    //        this.Value = value;
    //        this.Tag=null;//
    //    }
    //    public SortItem(TKey key, TValue value,object tag)
    //    {
    //        this.Key = key;
    //        this.Value = value;
    //        this.Tag = tag;//
    //    }
    //    public int Compare(object x, object y)
    //    {
    //        SortItem<TKey, TValue> objX = x as SortItem<TKey, TValue>;
    //        SortItem<TKey, TValue> objY = y as SortItem<TKey, TValue>;

    //        return Compare(objX, objY);//
    //    }
    //    public int Compare(SortItem<TKey, TValue> x, SortItem<TKey, TValue> y)
    //    {

    //        int v = 0;
    //        switch (SortList<TKey, TValue>.SortType)
    //        {
    //            case SortType.Key:
    //                v = x.Key.CompareTo(y.Key);//
    //                break;//
    //            case SortType.Value:
    //                v = x.Value.CompareTo(y.Value);//
    //                break;//
    //            case SortType.KeyLength:
    //                v = Compare(x.Key.ToString().Length, y.Key.ToString().Length);//
    //                break;//
    //            case SortType.ValueLength:
    //                v = Compare(x.Value.ToString().Length, y.Value.ToString().Length);//
    //                break;//
    //            case SortType.None:
    //                v = 0;//
    //                break;//
    //            default:
    //                v = 0;//
    //                break;
    //        }
    //        return v;
    //    }
    //    public int Compare(int x, int y)
    //    {
    //        if (x > y) return 1;
    //        if (x < y) return -1;
    //        return 0;//
    //    }
    //    #endregion
    //}


    //public class SortList<TKey, TValue> : List<SortItem<TKey, TValue>>
    //    where TKey : IComparable<TKey>
    //    where TValue : IComparable<TValue>
    //{
    //    public static SortType SortType = SortType.None;
    //    public SortList(SortType value)
    //    {
    //        SortType = value;//
    //    }
    //    public SortList()
    //        : base()
    //    {

    //    }
    //    public SortType SortedType
    //    {
    //        set
    //        {
    //            SortType = value;//
    //        }
    //        get
    //        {
    //            return SortType;//
    //        }
    //    }
    //}

    /// <summary>
    /// 比较类型
    /// </summary>
    public enum PowerComparerType
    {
        None,
        Key,
        KeyLength,
        Value,
        ValueLength,

        ToString,
        TypeGUID,
        HashCode
    }
    public class PowerComparer<TKey, TValue> : IComparer<KeyValuePair<TKey, TValue>>
        where TKey : IComparable<TKey>
        where TValue : IComparable<TValue>
    {
        private PowerComparerType Type;
        private bool IsDesc = false;

        public PowerComparer(PowerComparerType type)
        {
            Type = type;//
        }
        public PowerComparer(PowerComparerType type, bool isDesc)
        {
            Type = type;//
            IsDesc = isDesc;//
        }
        public int Compare(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            int v;
            switch (Type)
            {
                case PowerComparerType.Key:
                    v = x.Key.CompareTo(y.Key);//
                    break;//
                case PowerComparerType.Value:
                    v = x.Value.CompareTo(y.Value);//
                    break;//
                case PowerComparerType.KeyLength:
                    v = Compare(x.Key.ToString().Length, y.Key.ToString().Length);//
                    break;//
                case PowerComparerType.ValueLength:
                    v = Compare(x.Value.ToString().Length, y.Value.ToString().Length);//
                    break;//

                case PowerComparerType.ToString:
                    v = x.ToString().CompareTo(y.ToString());//
                    break;//
                case PowerComparerType.TypeGUID:
                    v = x.GetType().GUID.CompareTo(y.GetType().GUID);//
                    break;//
                case PowerComparerType.HashCode:
                    v = x.GetHashCode().CompareTo(y.GetHashCode());//
                    break;//
                case PowerComparerType.None:
                    v = 0;//
                    break;//
                default:
                    v = 0;//
                    break;
            }
            if (IsDesc)
                v = -v;//
            return v;
        }
        public int Compare(int x, int y)
        {
            if (x > y) return 1;
            if (x < y) return -1;
            return 0;//
        }
    }
}