using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HLSSplit
{
    /// <summary>
    /// 海量分词研究版C#接口
    /// </summary>
    public class HL
    {
        /// <summary>海量分词系统初试化</summary>
        /// <param name="dict"></param>
        /// <returns>bool</returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLSplitInit", SetLastError = true,CharSet=CharSet.Unicode)]
        public static extern bool HLSplitInit(string dict);
        /// <summary>
        /// 海量分词系统初试化
        /// </summary>
        /// <returns></returns>
        public static bool SplitInit()
        {
            return HLSplitInit(null);
        }
        ///<summary>海量分词系统卸载</summary>
        ///<paramref name=""/>
        [DllImport("HLSSplit.dll", EntryPoint = "HLFreeSplit", SetLastError = true)]
        public static extern void HLFreeSplit();
        /// <summary>打开海量分词句柄</summary>
        /// <returns>IntPtr handle</returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLOpenSplit", SetLastError = true)]
        public static extern IntPtr HLOpenSplit();
        /// <summary>关闭海量分词句柄</summary>
        /// <param name="handle"></param>
        [DllImport("HLSSplit.dll", EntryPoint = "HLCloseSplit", SetLastError = true)]
        public static extern void HLCloseSplit(IntPtr handle);
        /// <summary>对一段字符串分词</summary>
        /// <param name="handle">分词句柄</param>
        /// <param name="lpText">要切分文本</param>
        /// <param name="iExtraCalcFlag">分词选项</param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLSplitWord", SetLastError = true)]
        public static extern bool HLSplitWord(IntPtr handle,string lpText, int iExtraCalcFlag);
        /// <summary>
        /// 对一段字符串分词
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool SplitWord(IntPtr handle, string text)
        {
            return HLSplitWord(handle,text, 0);
        }
        /// <summary>
        /// 对一段字符串分词
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="text"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool SplitWord(IntPtr handle,string text, SegOption option)
        {
            return HLSplitWord(handle,text, (int)option);
        }
        /// <summary>
        /// 对一段字符串分词
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="text"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool SplitWord(IntPtr handle,string text, int option)
        {
            return HLSplitWord(handle,text, option);
        }
        /// <summary>获得分词结果个数</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLGetWordCnt", SetLastError = true)]
        public static extern int HLGetWordCnt(IntPtr handle);
        /// <summary>获取指定的分词结果</summary>
        /// <param name="handle"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", SetLastError = true)]
        private static extern IntPtr HLGetWordAt(IntPtr handle, int index);
        /// <summary>
        /// 此处为用户真正的接口，封装了把指针转化为结构的部分
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Seg GetWordAt(IntPtr handle, int index)
        {
            IntPtr pWord = HLGetWordAt(handle, index);
            Seg word = (Seg)Marshal.PtrToStructure((IntPtr)pWord, typeof(Seg));
            //此处需要把高位屏蔽，否则得到的结果会出现错误。所有的词性值在0x4000000000下
            word.POS = (POS)((byte)word.POS & 0x0FFFFFFF);
            return word;
        }
        /// <summary>
        /// 装载用户自定义词典
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLOpenUsrDict", SetLastError = true)]
        public static extern bool HLOpenUsrDict(string dict);
        /// <summary>
        /// 卸载用户自定义词典
        /// </summary>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLFreeUsrDict", SetLastError = true)]
        public static extern bool HLFreeUsrDict();
        /// <summary>
        /// 获取关键词个数
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLGetFileKeyCnt", SetLastError = true)]
        public static extern int HLGetFileKeyCnt(IntPtr handle);
        /// <summary>
        /// 获取指定的关键词
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", SetLastError = true)]
        private static extern IntPtr HLGetFileKeyAt(IntPtr handle, int index);
        /// <summary>
        /// 获取指定的关键词
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Seg GetFileKeyAt(IntPtr handle, int index)
        {
            IntPtr pWord = HLGetFileKeyAt(handle, index);
            Seg word = (Seg)Marshal.PtrToStructure(pWord, typeof(Seg));
            //此处需要把高位屏蔽，否则得到的结果会出现错误。所有的词性值在0x4000000000下
            word.POS = (POS)((uint)word.POS & 0x0FFFFFFF);
            return word;
        }
        /// <summary>
        /// 获得语义指纹
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="rpData"></param>
        /// <param name="rdwLen"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", SetLastError = true)]
        private static extern bool HLGetFingerM(IntPtr handle, out IntPtr rData, out Int32 rLen);
        /// <summary>
        /// 获得语义指纹
        /// </summary>
        public static byte[] GetFingerM(IntPtr handle)
        {
            IntPtr p;
            byte[] data = null;
            Int32 len = 0;
            bool flag = HLGetFingerM(handle, out p, out len);
            if (flag == false || len == 0) return null;
            data = new byte[len];
            for (int i = 0; i < len; i++)
            {
                data[i] = Marshal.ReadByte(p, i);
            }
            return data;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Seg
    {   
        /// <summary>
        /// 字符串
        /// </summary>
        //[MarshalAs(UnmanagedType.BStr)]
        public string word;
        /// <summary>
        /// 词性标志
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public POS POS;
        /// <summary>
        /// 关键词权重，如果不是关键词，权重为0
        /// </summary>
        public float weight;
    };


   /// <summary>
    /// 常量定义部分,分词选项
   /// </summary>
    public enum SegOption : byte
    {
        /// <summary>
        /// 默认选项，仅仅分词
        /// </summary>
        DEFAULT = 0,
        /// <summary>
        /// 计算关键词附加标识
        /// </summary>
        KEYWORD = 0x1,
        /// <summary>
        /// 计算文章语义指纹标识
        /// </summary>
        FINGER = 0x2,   
        /// <summary>
        /// 计算词性标识
        /// </summary>
        POS = 0x4,
        /// <summary>
        /// 输出面向检索的分词结果
        /// </summary>
        SEARCH = 0x8       
    };

    /// <summary>
    /// 词性定义部分
    /// </summary>
    public enum POS : uint
    {
        D_A = 0x40000000,   // 形容词 形语素
        D_B = 0x20000000,   // 区别词 区别语素
        D_C = 0x10000000,   // 连词 连语素
        D_D = 0x08000000,   // 副词 副语素
        D_E = 0x04000000,   // 叹词 叹语素
        D_F = 0x02000000,   // 方位词 方位语素
        D_I = 0x01000000,   // 成语
        D_L = 0x00800000,   // 习语
        A_M = 0x00400000,   // 数词 数语素
        D_MQ = 0x00200000,  // 数量词
        D_N = 0x00100000,   // 名词 名语素
        D_O = 0x00080000,   // 拟声词
        D_P = 0x00040000,   // 介词
        A_Q = 0x00020000,   //0x80020000,// 量词 量语素 一把,一次,一幅,一件,一拳 等
        D_R = 0x00010000,   // 代词 代语素
        D_S = 0x00008000,   // 处所词
        D_T = 0x00004000,   // 时间词
        D_U = 0x00002000,   // 助词 助语素
        D_V = 0x00001000,   // 动词 动语素
        D_W = 0x00000800,   // 0x80000800,//标点符号
        D_X = 0x00000400,   // 非语素字
        D_Y = 0x00000200,   // 语气词 语气语素
        D_Z = 0x00000100,   // 状态词
        A_NR = 0x00000080,  // 0x80000080,//人名
        A_NS = 0x00000040,  // 地名
        A_NT = 0x00000020,  // 机构团体
        A_NX = 0x00000010,  //0x80000010,// 外文字符
        A_NZ = 0x00000008,  // 其他专名
        D_H = 0x00000004,   // 前接成分
        D_K = 0x00000002    // 后接成分
    }

}
 
