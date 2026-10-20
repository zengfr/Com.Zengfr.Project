using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace hookInject.api
{
   public class reg
    {
        [DllImport("regdbg.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getregsinfo();
        [DllImport("regdbg.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getregsinfo2();
        [DllImport("regdbg.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getregsinfo3();

        [DllImport("regdbg.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getregsinfo4();
        [DllImport("regdbg.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr getregsinfo5();
    }
}
