﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
namespace Com.Zfrong.Common.Win32.API
{
    public class Shudown
    {
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
            
        }
        [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcess", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();
        [DllImport("advapi32.dll", EntryPoint = "OpenProcessToken", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);
        [DllImport("advapi32.dll", EntryPoint = "LookupPrivilegeValue", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);
        [DllImport("advapi32.dll", EntryPoint = "AdjustTokenPrivileges", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,
        ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", EntryPoint = "ExitWindowsEx", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        internal static extern bool ExitWindowsEx(int DoFlag, int rea);
        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        internal const int EWX_LOGOFF = 0x00000000;
        internal const int EWX_SHUTDOWN = 0x00000001;
        internal const int EWX_REBOOT = 0x00000002;
        internal const int EWX_FORCE = 0x00000004;
        internal const int EWX_POWEROFF = 0x00000008;
        internal const int EWX_FORCEIFHUNG = 0x00000010;


        private static void DoExitWin(int DoFlag)
        {
            bool ok;
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ok = ExitWindowsEx(DoFlag, 0);
        }

        public static void Reboot()
        {
            DoExitWin(EWX_FORCE | EWX_REBOOT);
        }
        public static void PowerOff()
        {
            DoExitWin(EWX_FORCE | EWX_POWEROFF);
        }
        public static void LogOff()
        {
            DoExitWin(EWX_FORCE | EWX_LOGOFF);
        }
    }
}
