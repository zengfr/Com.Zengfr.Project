//中文分词是中文搜索引擎的基础，主要应用在信息检索、信息挖掘、中外文对译、中文校对、自动聚类、自动分类等很多方面.

//这个是我参照VC的例子修改的C#版本。^  ^

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace HLSSplit
{
    /// <summary>
    /// HLParse 的摘要说明。
    /// </summary>
    public class HLParse
    {
        private  Dictionary<string, float> m_strKeyWords;
        private Dictionary<string, POS> m_strWords;
        private TimeSpan ts;
        private byte iExtraCalcFlag=0;//
        private Dictionary<string, byte> m_strFinger;

        public Dictionary<string, float> KeyWords
        {
            get { return m_strKeyWords; }
        }
        public Dictionary<string, POS> Words
        {
            get { return m_strWords; }
        }
        public TimeSpan DoTime
        { 
            get { return ts; } 
        }
        public byte ExtraCalcFlag
        {
            set {
                iExtraCalcFlag = value;//
            }
        }
        public Dictionary<string, byte> Finger
        {
            get { return m_strFinger; }
        }
        public byte ParseAll(string text)
        {
            iExtraCalcFlag = 0; //附加计算标志，不进行附加计算
            //获得附加计算标识
            iExtraCalcFlag |= (byte)SegOption.POS;//
            iExtraCalcFlag |= (byte)SegOption.KEYWORD;
            iExtraCalcFlag |= (byte)SegOption.SEARCH;
            iExtraCalcFlag |= (byte)SegOption.FINGER;
            return Parse(text);//
        }
        public byte Parse(string text)
        {
            return ParseWord(text, iExtraCalcFlag, out m_strKeyWords, out m_strWords, out m_strFinger, out ts);
        }

        public static byte ParseWord(string text, byte iExtraCalcFlag, out Dictionary<string, float> m_strKeyWords,
        out Dictionary<string, POS> m_strWords,
        out Dictionary<string, byte> m_strFinger, out TimeSpan ts)
        {
            bool isOutKeyword=((iExtraCalcFlag|(byte)SegOption.KEYWORD)==iExtraCalcFlag);
            bool isOutFinger = ((iExtraCalcFlag | (byte)SegOption.FINGER) == iExtraCalcFlag);
            return ParseWord(text, iExtraCalcFlag, out m_strKeyWords, out m_strWords, out m_strFinger, out ts, isOutKeyword, isOutFinger);
        }

        public static byte ParseWord(string text, out Dictionary<string, float> m_strKeyWords,
        out Dictionary<string, POS> m_strWords,
        out Dictionary<string, byte> m_strFinger, out TimeSpan ts)
        {
            byte iExtraCalcFlag = 0; //附加计算标志，不进行附加计算
            //获得附加计算标识
            iExtraCalcFlag |= (byte)SegOption.POS;//
            iExtraCalcFlag |= (byte)SegOption.KEYWORD;
            iExtraCalcFlag |= (byte)SegOption.SEARCH;
            iExtraCalcFlag |= (byte)SegOption.FINGER;

            return ParseWord(text, iExtraCalcFlag, out m_strKeyWords, out m_strWords, out m_strFinger, out ts, true, true);
        }


        private static byte ParseWord(string text, byte iExtraCalcFlag, out Dictionary<string, float> m_strKeyWords,
        out Dictionary<string, POS> m_strWords,
        out Dictionary<string, byte> m_strFinger, out TimeSpan ts, bool isOutKeyword, bool isOutFinger)
        {
            byte isError = 0;
            m_strFinger = new Dictionary<string, byte>();
            m_strKeyWords = new Dictionary<string, float>();//
            m_strWords = new Dictionary<string, POS>();//
            ts = TimeSpan.Zero;//

             if (!HL.SplitInit())
            {
                isError = 1;
                return isError;
            }
            IntPtr hHandle = HL.HLOpenSplit(); //创建分词句柄
            if (hHandle ==IntPtr.Zero)
            {
                isError=2;
                HL.HLFreeSplit();//卸载分词字典
                return isError;
            }

            DateTime bgdt = DateTime.Now;
            bool bSuccess = HL.SplitWord(hHandle,text, iExtraCalcFlag);
            ts = DateTime.Now - bgdt;

            if (bSuccess)
            {
                //分词成功
                int nResultCnt = HL.HLGetWordCnt(hHandle);//取得分词个数
                for (int i = 0; i < nResultCnt; i++)
                {
                    Seg pWord = HL.GetWordAt(hHandle, i);//取得一个分词结果
                    m_strWords[pWord.word] = pWord.POS;
                }
                if (isOutKeyword)
                {
                    //获取关键词
                    int nKeyCnt = HL.HLGetFileKeyCnt(hHandle);//获得关键词个数
                    for (int j = 0; j < nKeyCnt; j++)
                    {
                        Seg pKey = HL.GetFileKeyAt(hHandle, j);//获得指定的关键词
                        if (pKey.word == null || pKey.word == "")
                            continue;
                        m_strKeyWords[pKey.word] = pKey.weight;
                    }
                }
                if (isOutFinger)
                {
                    byte[] fs = HL.GetFingerM(hHandle);//获得语义指纹
                    foreach (byte f in fs)
                    {
                        string strU=string.Format("{0:x}", f);
                        m_strFinger[strU] = f;
                    }
                }
            }
            else
            {
                isError = 2;
            }
            HL.HLCloseSplit(hHandle);//关闭分词句柄
            HL.HLFreeSplit(); //卸载分词词典
            hHandle =IntPtr.Zero;//
            return isError;//
        }
    }
}