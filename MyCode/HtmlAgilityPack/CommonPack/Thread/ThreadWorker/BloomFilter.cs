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
    /// ���Ա���500��Url ��Ҫ 5 * 32M�ڴ�
    /// </summary>
    /// <returns></returns>
    public static BloomFilter Get(string key)
    {
        return Get(key, 5 * 32 * 262144);//500��Url
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
                //System.Threading.Monitor.Wait(obj);//�ͷ�
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
                //System.Threading.Monitor.Wait(obj);//�ͷ�
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
                //System.Threading.Monitor.Wait(obj);//�ͷ�
            }
        }
    }
}
public class BloomFilter
{
    #region Field
    /// <summary>
    /// �ж��Ƿ񱣴�����
    /// </summary>
    public bool IsLoad = false;//
    public BitArray MemoryPool;  //����int����ԭ��Ϊ262144����������ռ��1M
    /// <summary>
    /// �Ѿ��洢��Url��
    /// </summary>
    public int Count;
    private int MaxNum;                        //ƽ������8��ÿ���Ԫ����32768
    private static MD5CryptoServiceProvider Md5 = new MD5CryptoServiceProvider();
    private static SHA1CryptoServiceProvider Sha1 = new SHA1CryptoServiceProvider();
    private static object SyncObj = new object();//
    #endregion
    #region ���캯��
    /// <summary>
    /// // ���캯���������������8�ı�������Ϊ����8�ηֱ�洢4���ֽ���Ϣ��ǰ4�δ�MD5����4�δ�SHA1
    /// //262144������1M���ڴ棬50w��ַ�ı�������16M�������ģ���һ��վ�㳬��50w�ĵ�ַ���������HASH�������ڴ��
    /// 50w��16M�� new BloomFilter(262144*16);
    /// </summary>
    /// <param name="MemoryNum"></param>
    public BloomFilter(int MemoryNum)                  //�����ڴ��С ����������8                 
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
    /// FALSE��ʾ���ظ���TRUE��ʾ���ظ�
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
                return true;                              //�����ظ�
            }
            System.Threading.Monitor.PulseAll(SyncObj);//
        }
        Count++;//url��
        return false;                                //���ظ�
    }
    #region PushInMemory
    private bool PushInMemoryMd5(string url, int wz) //��Ӧ�ڴ�λ����1����4����Ӧλ�þ��Ѿ�����1�򷵻�false
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
    private bool PushInMemorySha1(string url, int wz)    //��Ӧ�ڴ�λ����1����4����Ӧλ�þ��Ѿ�����1�򷵻�false
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