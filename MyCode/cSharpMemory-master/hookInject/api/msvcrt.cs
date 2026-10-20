using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace hookInject
{
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class ConstAttribute : Attribute { }

    [Serializable, Flags, CLSCompliant(false)]
    public enum size_t : uint { }

    public class msvcrt
    {
        [DllImport(ModuleName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int _flsbuf(int c, IntPtr ptr);

        [DllImport(ModuleName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int _scprintf(string format, IntPtr ptr);

        [DllImport(ModuleName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf([In, Out]StringBuilder buffer, string format, IntPtr args);

        [DllImport(ModuleName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf([In, Out]StringBuilder buffer, string format, __arglist);
        [DllImport(ModuleName, CharSet = CharSet.Ansi,CallingConvention = CallingConvention.Cdecl)]
        public static extern int sprintf([In, Out]StringBuilder buffer,string format,  RuntimeArgumentHandle args);

        [DllImport(ModuleName, CharSet = CharSet.Unicode,CallingConvention = CallingConvention.Cdecl)]
       public  static extern int swprintf([In, Out] StringBuilder buffer, string format,__arglist);
        [DllImport(ModuleName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int swprintf([In, Out] StringBuilder buffer, string format, RuntimeArgumentHandle args);

        [DllImport(ModuleName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern int fclose(IntPtr stream);

        [DllImport(ModuleName,CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern IntPtr fopen([Const] IntPtr filename, [Const] IntPtr mode);

        [DllImport(ModuleName, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern IntPtr fopen(string filename, string mode);
        [DllImport(ModuleName)]
        public static extern int fflush(IntPtr stream);

        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int fprintf(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)]string format, string arg1, string arg2);

        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int fprintf(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)]string format, IntPtr args);
        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int fprintf(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)]string format, __arglist);

        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int fprintf(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)] string format, RuntimeArgumentHandle args);

        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int vfprintf(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)]string format, IntPtr args);
        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int vfprintf(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)]string format, __arglist);

        [DllImport("msvcrt.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        public static extern int vfprintf(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)] string format, RuntimeArgumentHandle args);

        [NonSerialized]
        public   const string ModuleName = "msvcrt.dll";

        [DllImport(ModuleName), CLSCompliant(false)]
        public static extern IntPtr _memccpy(IntPtr dest, [Const] IntPtr src, int c, uint count);

        [DllImport(ModuleName), CLSCompliant(false)]
        public static extern IntPtr memchr(IntPtr buf, int c, size_t count);

        [DllImport(ModuleName), CLSCompliant(false)]
        public static extern int memcmp([Const] IntPtr buf1, [Const] IntPtr buf2, size_t count);

        [DllImport(ModuleName), CLSCompliant(false)]
        public static extern IntPtr memcpy(IntPtr dest, [Const] IntPtr src, size_t count);

        [DllImport(ModuleName), CLSCompliant(false)]
        public static extern int _memicmp([Const] IntPtr buf1, [Const] IntPtr buf2, uint count);

        [DllImport(ModuleName), CLSCompliant(false)]
        public static extern IntPtr memmove(IntPtr dest, [Const] IntPtr src, size_t count);

        [DllImport(ModuleName), CLSCompliant(false)]
        public static extern IntPtr memset(IntPtr dest, int c, size_t count);

        [DllImport(ModuleName), CLSCompliant(false)]
        public static extern void _swab(IntPtr src, IntPtr dest, int n);
    }
}
