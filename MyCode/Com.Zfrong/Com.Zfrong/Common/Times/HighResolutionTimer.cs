using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace Com.Zfrong.Common.Timers
{
    /// <summary>
    /// 一个计时器，来自gotdotnet.com
    /// </summary>
    /// 在此感谢原作者的辛勤劳动
    public class HighResolutionTimer
    {
        private long start;
        private long stop;
        private long frequency;
        /// <summary>
        /// 构造方法
        /// </summary>
        public HighResolutionTimer()
        {
            QueryPerformanceFrequency(ref frequency);
        }
        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            QueryPerformanceCounter(ref start);
        }
        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            QueryPerformanceCounter(ref stop);
        }
        /// <summary>
        /// 输出使用的时间长度 秒
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
