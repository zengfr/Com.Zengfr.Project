using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HLSSplit
{
    /// <summary>
    /// �����ִ��о���C#�ӿ�
    /// </summary>
    public class HL
    {
        /// <summary>�����ִ�ϵͳ���Ի�</summary>
        /// <param name="dict"></param>
        /// <returns>bool</returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLSplitInit", SetLastError = true,CharSet=CharSet.Unicode)]
        public static extern bool HLSplitInit(string dict);
        /// <summary>
        /// �����ִ�ϵͳ���Ի�
        /// </summary>
        /// <returns></returns>
        public static bool SplitInit()
        {
            return HLSplitInit(null);
        }
        ///<summary>�����ִ�ϵͳж��</summary>
        ///<paramref name=""/>
        [DllImport("HLSSplit.dll", EntryPoint = "HLFreeSplit", SetLastError = true)]
        public static extern void HLFreeSplit();
        /// <summary>�򿪺����ִʾ��</summary>
        /// <returns>IntPtr handle</returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLOpenSplit", SetLastError = true)]
        public static extern IntPtr HLOpenSplit();
        /// <summary>�رպ����ִʾ��</summary>
        /// <param name="handle"></param>
        [DllImport("HLSSplit.dll", EntryPoint = "HLCloseSplit", SetLastError = true)]
        public static extern void HLCloseSplit(IntPtr handle);
        /// <summary>��һ���ַ����ִ�</summary>
        /// <param name="handle">�ִʾ��</param>
        /// <param name="lpText">Ҫ�з��ı�</param>
        /// <param name="iExtraCalcFlag">�ִ�ѡ��</param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLSplitWord", SetLastError = true)]
        public static extern bool HLSplitWord(IntPtr handle,string lpText, int iExtraCalcFlag);
        /// <summary>
        /// ��һ���ַ����ִ�
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool SplitWord(IntPtr handle, string text)
        {
            return HLSplitWord(handle,text, 0);
        }
        /// <summary>
        /// ��һ���ַ����ִ�
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
        /// ��һ���ַ����ִ�
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="text"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public static bool SplitWord(IntPtr handle,string text, int option)
        {
            return HLSplitWord(handle,text, option);
        }
        /// <summary>��÷ִʽ������</summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLGetWordCnt", SetLastError = true)]
        public static extern int HLGetWordCnt(IntPtr handle);
        /// <summary>��ȡָ���ķִʽ��</summary>
        /// <param name="handle"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", SetLastError = true)]
        private static extern IntPtr HLGetWordAt(IntPtr handle, int index);
        /// <summary>
        /// �˴�Ϊ�û������Ľӿڣ���װ�˰�ָ��ת��Ϊ�ṹ�Ĳ���
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Seg GetWordAt(IntPtr handle, int index)
        {
            IntPtr pWord = HLGetWordAt(handle, index);
            Seg word = (Seg)Marshal.PtrToStructure((IntPtr)pWord, typeof(Seg));
            //�˴���Ҫ�Ѹ�λ���Σ�����õ��Ľ������ִ������еĴ���ֵ��0x4000000000��
            word.POS = (POS)((byte)word.POS & 0x0FFFFFFF);
            return word;
        }
        /// <summary>
        /// װ���û��Զ���ʵ�
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLOpenUsrDict", SetLastError = true)]
        public static extern bool HLOpenUsrDict(string dict);
        /// <summary>
        /// ж���û��Զ���ʵ�
        /// </summary>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLFreeUsrDict", SetLastError = true)]
        public static extern bool HLFreeUsrDict();
        /// <summary>
        /// ��ȡ�ؼ��ʸ���
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", EntryPoint = "HLGetFileKeyCnt", SetLastError = true)]
        public static extern int HLGetFileKeyCnt(IntPtr handle);
        /// <summary>
        /// ��ȡָ���Ĺؼ���
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", SetLastError = true)]
        private static extern IntPtr HLGetFileKeyAt(IntPtr handle, int index);
        /// <summary>
        /// ��ȡָ���Ĺؼ���
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Seg GetFileKeyAt(IntPtr handle, int index)
        {
            IntPtr pWord = HLGetFileKeyAt(handle, index);
            Seg word = (Seg)Marshal.PtrToStructure(pWord, typeof(Seg));
            //�˴���Ҫ�Ѹ�λ���Σ�����õ��Ľ������ִ������еĴ���ֵ��0x4000000000��
            word.POS = (POS)((uint)word.POS & 0x0FFFFFFF);
            return word;
        }
        /// <summary>
        /// �������ָ��
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="rpData"></param>
        /// <param name="rdwLen"></param>
        /// <returns></returns>
        [DllImport("HLSSplit.dll", SetLastError = true)]
        private static extern bool HLGetFingerM(IntPtr handle, out IntPtr rData, out Int32 rLen);
        /// <summary>
        /// �������ָ��
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
        /// �ַ���
        /// </summary>
        //[MarshalAs(UnmanagedType.BStr)]
        public string word;
        /// <summary>
        /// ���Ա�־
        /// </summary>
        [MarshalAs(UnmanagedType.U4)]
        public POS POS;
        /// <summary>
        /// �ؼ���Ȩ�أ�������ǹؼ��ʣ�Ȩ��Ϊ0
        /// </summary>
        public float weight;
    };


   /// <summary>
    /// �������岿��,�ִ�ѡ��
   /// </summary>
    public enum SegOption : byte
    {
        /// <summary>
        /// Ĭ��ѡ������ִ�
        /// </summary>
        DEFAULT = 0,
        /// <summary>
        /// ����ؼ��ʸ��ӱ�ʶ
        /// </summary>
        KEYWORD = 0x1,
        /// <summary>
        /// ������������ָ�Ʊ�ʶ
        /// </summary>
        FINGER = 0x2,   
        /// <summary>
        /// ������Ա�ʶ
        /// </summary>
        POS = 0x4,
        /// <summary>
        /// �����������ķִʽ��
        /// </summary>
        SEARCH = 0x8       
    };

    /// <summary>
    /// ���Զ��岿��
    /// </summary>
    public enum POS : uint
    {
        D_A = 0x40000000,   // ���ݴ� ������
        D_B = 0x20000000,   // ����� ��������
        D_C = 0x10000000,   // ���� ������
        D_D = 0x08000000,   // ���� ������
        D_E = 0x04000000,   // ̾�� ̾����
        D_F = 0x02000000,   // ��λ�� ��λ����
        D_I = 0x01000000,   // ����
        D_L = 0x00800000,   // ϰ��
        A_M = 0x00400000,   // ���� ������
        D_MQ = 0x00200000,  // ������
        D_N = 0x00100000,   // ���� ������
        D_O = 0x00080000,   // ������
        D_P = 0x00040000,   // ���
        A_Q = 0x00020000,   //0x80020000,// ���� ������ һ��,һ��,һ��,һ��,һȭ ��
        D_R = 0x00010000,   // ���� ������
        D_S = 0x00008000,   // ������
        D_T = 0x00004000,   // ʱ���
        D_U = 0x00002000,   // ���� ������
        D_V = 0x00001000,   // ���� ������
        D_W = 0x00000800,   // 0x80000800,//������
        D_X = 0x00000400,   // ��������
        D_Y = 0x00000200,   // ������ ��������
        D_Z = 0x00000100,   // ״̬��
        A_NR = 0x00000080,  // 0x80000080,//����
        A_NS = 0x00000040,  // ����
        A_NT = 0x00000020,  // ��������
        A_NX = 0x00000010,  //0x80000010,// �����ַ�
        A_NZ = 0x00000008,  // ����ר��
        D_H = 0x00000004,   // ǰ�ӳɷ�
        D_K = 0x00000002    // ��ӳɷ�
    }

}
 
