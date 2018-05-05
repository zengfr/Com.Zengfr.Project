using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace Com.Zfrong.Common.Timers
{
    /// <summary>
    /// һ����ʱ��������gotdotnet.com
    /// </summary>
    /// �ڴ˸�лԭ���ߵ������Ͷ�
    public class HighResolutionTimer
    {
        private long start;
        private long stop;
        private long frequency;
        /// <summary>
        /// ���췽��
        /// </summary>
        public HighResolutionTimer()
        {
            QueryPerformanceFrequency(ref frequency);
        }
        /// <summary>
        /// ��ʼ��ʱ
        /// </summary>
        public void Start()
        {
            QueryPerformanceCounter(ref start);
        }
        /// <summary>
        /// ֹͣ��ʱ
        /// </summary>
        public void Stop()
        {
            QueryPerformanceCounter(ref stop);
        }
        /// <summary>
        /// ���ʹ�õ�ʱ�䳤�� ��
        /// </summary>
        public float ElapsedTime
        {
            get
            {
                float elapsed = (((float)(stop - start)) / ((float)frequency));
                return elapsed;
            }
        }
        public float GetElapsedTime()
        {
            if (start == 0)
                return 0;//
            Stop();//
            return this.ElapsedTime;//
        }
        [System.Runtime.InteropServices.DllImport("KERNEL32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool QueryPerformanceCounter([In, Out] ref long performanceCount);
        [System.Runtime.InteropServices.DllImport("KERNEL32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool QueryPerformanceFrequency([In, Out] ref long frequency);
    }
}
