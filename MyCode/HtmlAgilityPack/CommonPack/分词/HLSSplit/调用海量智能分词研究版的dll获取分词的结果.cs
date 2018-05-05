//���ķִ���������������Ļ�������ҪӦ������Ϣ��������Ϣ�ھ������Ķ��롢����У�ԡ��Զ����ࡢ�Զ�����Ⱥܶ෽��.

//������Ҳ���VC�������޸ĵ�C#�汾��^  ^

using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
namespace HLSSplit
{
    /// <summary>
    /// HLParse ��ժҪ˵����
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
            iExtraCalcFlag = 0; //���Ӽ����־�������и��Ӽ���
            //��ø��Ӽ����ʶ
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
            byte iExtraCalcFlag = 0; //���Ӽ����־�������и��Ӽ���
            //��ø��Ӽ����ʶ
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
            IntPtr hHandle = HL.HLOpenSplit(); //�����ִʾ��
            if (hHandle ==IntPtr.Zero)
            {
                isError=2;
                HL.HLFreeSplit();//ж�طִ��ֵ�
                return isError;
            }

            DateTime bgdt = DateTime.Now;
            bool bSuccess = HL.SplitWord(hHandle,text, iExtraCalcFlag);
            ts = DateTime.Now - bgdt;

            if (bSuccess)
            {
                //�ִʳɹ�
                int nResultCnt = HL.HLGetWordCnt(hHandle);//ȡ�÷ִʸ���
                for (int i = 0; i < nResultCnt; i++)
                {
                    Seg pWord = HL.GetWordAt(hHandle, i);//ȡ��һ���ִʽ��
                    m_strWords[pWord.word] = pWord.POS;
                }
                if (isOutKeyword)
                {
                    //��ȡ�ؼ���
                    int nKeyCnt = HL.HLGetFileKeyCnt(hHandle);//��ùؼ��ʸ���
                    for (int j = 0; j < nKeyCnt; j++)
                    {
                        Seg pKey = HL.GetFileKeyAt(hHandle, j);//���ָ���Ĺؼ���
                        if (pKey.word == null || pKey.word == "")
                            continue;
                        m_strKeyWords[pKey.word] = pKey.weight;
                    }
                }
                if (isOutFinger)
                {
                    byte[] fs = HL.GetFingerM(hHandle);//�������ָ��
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
            HL.HLCloseSplit(hHandle);//�رշִʾ��
            HL.HLFreeSplit(); //ж�طִʴʵ�
            hHandle =IntPtr.Zero;//
            return isError;//
        }
    }
}