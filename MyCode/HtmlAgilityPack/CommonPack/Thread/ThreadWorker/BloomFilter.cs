using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using CommonPack;
using Com.Zfrong.Configuration;
public class BloomFilterFactory
{
   public static IDictionary<string, BloomFilter> BloomFilters = new Dictionary<string, BloomFilter>();
    static Object SyncObj = new object();

    /// <summary>
    /// 可以保存500万Url 需要 5 * 32M内存
    /// </summary>
    /// <returns></returns>
    public static BloomFilter Get(string key)
    {
        return Get(key, 5 * 32 * 262144);//500万Url
    }
    public static BloomFilter Get(string key, int memoryNum)
    {
        if (!BloomFilters.ContainsKey(key) || BloomFilters[key] == null)
        {
            lock (SyncObj)
            {
                if (!BloomFilters.ContainsKey(key) || BloomFilters[key] == null)
                {
                    BloomFilters.Add(key, new BloomFilter(memoryNum));
                }
                System.Threading.Monitor.PulseAll(SyncObj);//
                //System.Threading.Monitor.Wait(obj);//释放
            }
        }
        return BloomFilters[key];//
    }
    public static void Load(string key, string filePath)
    {
        Load(key, 5 * 32 * 262144, filePath);
    }
    public static void Load(string key,int memoryNum, string filePath)
    {
        if (!Get(key).IsLoad)
        {
            lock (SyncObj)
            {
                if (!Get(key).IsLoad)
                {
                    ConfigX.LoadBitArray(Get(key, memoryNum).MemoryPool, filePath, true);//
                   Get(key).IsLoad = true;//
                }
                System.Threading.Monitor.PulseAll(SyncObj);//
                //System.Threading.Monitor.Wait(obj);//释放
            }
        }
    }
    public static void Save(string key, string filePath)
    {
        if (Get(key).IsLoad)
        {
            lock (SyncObj)
            {
                if (Get(key).IsLoad)
                {
                    ConfigX.SaveBitArray(filePath, Get(key).MemoryPool, true, false);//
                    Get(key).IsLoad = false;//
                }
                System.Threading.Monitor.PulseAll(SyncObj);//
                //System.Threading.Monitor.Wait(obj);//释放
            }
        }
    }
}
public class BloomFilter
{
    #region Field
    /// <summary>
    /// 判断是否保存或加载
    /// </summary>
    public bool IsLoad = false;//
    public BitArray MemoryPool;  //分配int数组原素为262144则整个数组占用1M
    /// <summary>
    /// 已经存储的Url数
    /// </summary>
    public int Count;
    private int MaxNum;                        //平均分配8块每块的元素数32768
    private static MD5CryptoServiceProvider Md5 = new MD5CryptoServiceProvider();
    private static SHA1CryptoServiceProvider Sha1 = new SHA1CryptoServiceProvider();
    private static object SyncObj = new object();//
    #endregion
    #region 构造函数
    /// <summary>
    /// // 构造函数传入参数必须是8的倍数，因为分了8段分别存储4个字节信息，前4段存MD5，后4段存SHA1
    /// //262144正好是1M的内存，50w地址的保存大概在16M左右消耗，如一个站点超过50w的地址，最好增加HASH函数和内存段
    /// 50w，16M： new BloomFilter(262144*16);
    /// </summary>
    /// <param name="MemoryNum"></param>
    public BloomFilter(int MemoryNum)                  //分配内存大小 必须能整除8                 
    {
        if ((MemoryNum % 8) == 0)
        {
            lock (this)
            {
                if (MemoryPool == null || MemoryPool.Count == 0)
                    MemoryPool = new BitArray(MemoryNum);
                MaxNum = MemoryNum / 8;
            }
        }
    }
    ~BloomFilter()
    {
        MemoryPool = null;
        Md5 = null;
        Sha1 = null;
    }
    #endregion
    /// <summary>
    /// FALSE表示无重复，TRUE表示有重复
    ///</summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public bool IsRepeat(string url)
    {
        int wz = 0;
        string Md5Url = Md5Decode(url);
        lock (SyncObj)
        {
            bool Result1 = PushInMemoryMd5(Md5Url, wz);
            Md5Url = Md5Decode(Md5Url);
            bool Result2 = PushInMemorySha1(Md5Url, wz);
            if ((Result1 == false) && (Result2 == false))
            {
                System.Threading.Monitor.PulseAll(SyncObj);//
                return true;                              //存在重复
            }
            System.Threading.Monitor.PulseAll(SyncObj);//
        }
        Count++;//url数
        return false;                                //不重复
    }
    #region PushInMemory
    private bool PushInMemoryMd5(string url, int wz) //对应内存位置置1，如4个对应位置均已经被置1则返回false
    {
        int tmp = 0;
        int k = 0;
        for (int i = 0; i < 16; i = i + 4)
        {
            string str = url.ToString().Substring(i, 4);
            int Hex10 = Convert.ToInt32(str, 16);
            int yy = Hex10 % MaxNum;
            if (MemoryPool[wz + yy] == false)
            {
                MemoryPool[wz + yy] = true;//1
            }
            else
            {
                tmp = tmp + 1;
            }
            k++;
            wz = wz + MaxNum;

        }
        if (tmp == 4)
        {
            return false;
        }
        return true;
    }
    private bool PushInMemorySha1(string url, int wz)    //对应内存位置置1，如4个对应位置均已经被置1则返回false
    {
        int tmp = 0;
        int k = 0;
        for (int i = 0; i < 16; i = i + 4)
        {
            string str = url.ToString().Substring(i, 7);
            int Hex10 = Convert.ToInt32(str, 16);
            int yy = Hex10 % MaxNum;
            if (MemoryPool[wz + yy] == false)
            {
                MemoryPool[wz + yy] = true;
            }
            else
            {
                tmp = tmp + 1;
            }
            k++;
            wz = wz + MaxNum;

        }
        if (tmp == 4)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region Static Method
    private static string Md5Decode(string str)
    {
        return BitConverter.ToString(Md5.ComputeHash(Encoding.Default.GetBytes(str))).Replace("-", "");
    }
    private static string Sha1Decode(string str)
    {
        return BitConverter.ToString(Sha1.ComputeHash(Encoding.Default.GetBytes(str))).Replace("-", "");
    }
    #endregion
}