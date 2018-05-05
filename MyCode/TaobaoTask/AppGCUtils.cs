using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask
{
   public class AppGCUtils
    {
        #region 内存回收

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        protected static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        [DllImport("psapi.dll")]
        protected static extern int EmptyWorkingSet(IntPtr hwProc);
        /// <summary>

        /// 释放内存

        /// </summary>

        public static void ClearMemory()

        {

            GC.Collect();

            GC.WaitForPendingFinalizers();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT
                || Environment.OSVersion.Platform == PlatformID.Win32Windows
                   || Environment.OSVersion.Platform == PlatformID.Win32S)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, 205);
               // EmptyWorkingSet(Process.GetCurrentProcess().Handle);
                System.Diagnostics.Process.GetCurrentProcess().MinWorkingSet= new System.IntPtr(5);
            }

        }
        #endregion
    }
}
