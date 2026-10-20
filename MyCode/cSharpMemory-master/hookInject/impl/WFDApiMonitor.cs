using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using hookInject.api;
using hookInject.core;
using hookInject.impl;

namespace hookInject
{
    public class WFDApiMonitor : ApiMonitor
    {
        public const string processesName = "mame64";//"hbmameui64";// mame64";
        EasyHook.LocalHook writeFileHook;
        EasyHook.LocalHook fopenHook;
        EasyHook.LocalHook ffHook;
        EasyHook.LocalHook fcloseHook;

        EasyHook.LocalHook fpfHook;
        EasyHook.LocalHook fpf2Hook;
        EasyHook.LocalHook vfpfHook;

        EasyHook.LocalHook flbHook;
        public WFDApiMonitor(MainInjection inject) : base(inject)
        {

        }
        public override void InstallHook()
        {
            Injector.initq("error.log");
            Injector.initq("m.log");
            Injector.initq("t.log");
            writeFileHook = Injector.createHook("kernel32.dll", "WriteFile", new WriteFile_Delegate(WriteFile_Hook));
            fopenHook = Injector.createHook("msvcrt.dll", "fopen", new fopen_Delegate(fopen_Hook));
            ffHook = Injector.createHook("msvcrt.dll", "fflush", new ff_Delegate(ff_Hook));
            fcloseHook = Injector.createHook("msvcrt.dll", "fclose", new fclose_Delegate(fclose_Hook));

            fpfHook = Injector.createHook("msvcrt.dll", "fprintf", new fpf_Delegate(fpf_Hook));
            //fpf2Hook = Injector.createHook("msvcrt.dll", "fprintf", new fpf2_Delegate(fpf2_Hook));
            //vfpfHook = Injector.createHook("msvcrt.dll", "vfprintf", new vfpf_Delegate(vfpf_Hook));

            flbHook = Injector.createHook("msvcrt.dll", "_flsbuf", new flb_Delegate(flb_Hook));

            //https://stackoverflow.com/questions/26304864/marshaling-variable-arguments-arglist-or-alternative
            //https://stackoverflow.com/questions/2479153/using-pinvoke-in-c-sharp-to-call-sprintf-and-friends-on-64-bit
            // var f = msvcrt.fopen("test.txt", "w");
            // msvcrt.fprintf(f, "My name is %s %s %s\n", __arglist("Bart11", "Bart2", "Bart33"));
            //msvcrt.fprintf(f, "My name is %s %s %s\n", __arglist("1", "2", "3"));
            //msvcrt.vfprintf(f, "My name is %s %s %s\n", __arglist("4", "5", "6"));
            // msvcrt.fprintf(f, "My name is %s %s\n", "7", "8");
            //fprintf(f, "My name is %s %s\n", "1", "2");
            // msvcrt.fclose(f);
        }

        private int flb_Hook(int c, IntPtr f)
        {
            return 0;
            StringBuilder filename = new StringBuilder(256);
            winapi.GetFinalPathNameByHandle(f, filename, 256, 0);
            String fname = filename.ToString();
            Injector.Ipc.ReportMessage(FormatMessage("_flsbuf " + "", fname));
            bool original = (!fname.Contains(".dup")) && (fname.Contains("t.log") || fname.Contains("error.log"));
            if(!original)
               return msvcrt._flsbuf(c, f);
            
            return 0;
        }

        private void fprintf(IntPtr file, string format, params VariableArgument[] args)
        {
            using (var combinedVariables = new CombinedVariables(args))
            {
                msvcrt.fprintf(file, format, combinedVariables.GetPtr());
            }
        }

        public override void UninstallHook()
        {
            writeFileHook.Dispose();
            fopenHook.Dispose();
            ffHook.Dispose();
            fcloseHook.Dispose();

            fpfHook.Dispose();
            //fpf2Hook.Dispose();
            //vfpfHook.Dispose();
            flbHook.Dispose();

        }
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int fclose_Delegate(IntPtr stream);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        delegate IntPtr fopen_Delegate(string filename, string mode);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, SetLastError = true)]
        delegate int ff_Delegate(IntPtr stream);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        delegate int fpf_Delegate(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)]string fmt, IntPtr args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        delegate int fpf2_Delegate(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)]string fmt, string arg1, string arg2);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        delegate int vfpf_Delegate(IntPtr stream, [MarshalAs(UnmanagedType.LPStr)]string fmt, IntPtr args);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        delegate bool WriteFile_Delegate(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.I4)]
        delegate int flb_Delegate(int c, IntPtr args);
        bool WriteFile_Hook(IntPtr hFile, IntPtr lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped)
        {
            bool result = false;
            lpNumberOfBytesWritten = 1;
            try
            {

                StringBuilder filename = new StringBuilder(255);
                winapi.GetFinalPathNameByHandle(hFile, filename, 255, 0);
                String fname = filename.ToString();
                bool original = (!fname.Contains(".dup")) && (fname.Contains("t.log") || fname.Contains("error.log"));
                // Call original first so we get lpNumberOfBytesWritten
                if (!original)
                    result = FileActivities.WriteFile(hFile, lpBuffer, nNumberOfBytesToWrite, out lpNumberOfBytesWritten, lpOverlapped);

                if (original)
                {
                    string str = Marshal.PtrToStringAnsi(lpBuffer, (int)nNumberOfBytesToWrite);
                    Injector.enqueue(Path.GetFileName(fname), str);//logerror
                }
            }
            catch (Exception ex)
            {
                Injector.Ipc.ReportException(ex);
            }

            return result;
        }
        private int vfpf_Hook(IntPtr hFile, string fmt, IntPtr args)
        {
            string msg = getContent(fmt, args);
            Injector.Ipc.ReportMessage(FormatMessage("vfprintf", msg));
            return msvcrt.vfprintf(hFile, fmt, args);
        }

        private int fpf_Hook(IntPtr hFile, string fmt, IntPtr args)
        {
            string msg = getContent(fmt, args);
            Injector.Ipc.ReportMessage(FormatMessage("fprintf", msg));

            /*ArgIterator args2 = new ArgIterator(args);
            while (args2.GetRemainingCount() > 0)
            {

                Injector.Ipc.ReportMessage(string.Format("{0}:{1}", 
                    Type.GetTypeFromHandle(args2.GetNextArgType()),
                    TypedReference.ToObject(args2.GetNextArg())));

            }*/

            return msvcrt.fprintf(hFile, fmt, args);
        }

        private int fpf2_Hook(IntPtr hFile, string fmt, string arg1, string arg2)
        {
            string msg = string.Format(fmt, arg1, arg2);
            Injector.Ipc.ReportMessage(FormatMessage("fprintf 2", msg));

            return msvcrt.fprintf(hFile, fmt, arg1, arg2);
        }
        private string getContent(string fmt, IntPtr args)
        {
            var bufferCapacity = msvcrt._scprintf(fmt, args);
            StringBuilder sb = new StringBuilder(bufferCapacity + 2);
            msvcrt.sprintf(sb, fmt, args);
            return sb.ToString();

        }

        private IntPtr fopen_Hook(string fname, string mode)
        {
            Injector.Ipc.ReportMessage(FormatMessage("fopen " + mode, fname));
             return msvcrt.fopen(fname, mode);
             
        }
        HistoryQueue<String> lastlog = new HistoryQueue<string>(5);
        bool lastIsOld  = true;
        private int ff_Hook(IntPtr stream)
        {
            //var ptr = reg.getregsinfo5();
            try
            {
                //Int64[] bRawData = new Int64[0x8];
                //Marshal.Copy(ptr, bRawData, 0, bRawData.Length);
               
                String log;
                if (processesName.StartsWith("hbmame"))
                {
                    String pc = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(new IntPtr(0x2481a0), 0));
                    var lp = Marshal.ReadIntPtr(new IntPtr(0x248180), 0);
                    var asm = Marshal.PtrToStringAnsi(lp);
                    log = pc + ":" + asm;
                }
                else
                {
                    String pc = Marshal.PtrToStringAnsi(new IntPtr(0x248ae0));
                    var lp = Marshal.ReadIntPtr(new IntPtr(0x248ab0), 0);
                    var asm = Marshal.PtrToStringAnsi(lp);
                    log = pc + ":" + asm;
                }
                lastlog.push(log);
                bool isNew= Injector.enqueue("t.log",log);//trace log
                if (isNew)
                {
                    if (lastIsOld)
                    {
                        Injector.Ipc.ReportMessage(FormatMessage(" :", lastlog.get(-2)));
                        Injector.Ipc.ReportMessage(FormatMessage("F:", lastlog.get(-1)));
                    }
                    lastIsOld = false;
                    Injector.Ipc.ReportMessage(FormatMessage("T:", log));
                }
                else {
                    lastIsOld = true;
                }
            }
            catch (Exception ex)
            {
                Injector.Ipc.ReportException(ex);
            }
            return -1;// msvcrt.fflush(stream);
        }
        private int fclose_Hook(IntPtr file)
        {
            Injector.Ipc.ReportMessage(FormatMessage("fclose", "" + file.ToInt64()));
            return msvcrt.fclose(file);
        }
    }



}
