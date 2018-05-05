using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;//
using System.Runtime.InteropServices;
namespace RSSBuilder
{
    public class RunningInstance
    {
        //1. API函数的声明
        // Uses to active the exist window
        [DllImport("User32.dll")]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        // 0-Hidden, 1-Centered, 2-Minimized, 3-Maximized
        private const int WS_SHOWNORMAL = 3;

        //2. 查找并激活已经存在的程序(仅应用于进程名相同的程序)
        ///
        /// Finds the running instance.
        ///
        /// true if exist a running instace, otherwise false
        public static bool ExistRunningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] procList = Process.GetProcessesByName(currentProcess.ProcessName);

            foreach (Process proc in procList)
            {
                // Found a running instance
                if (proc.Id != currentProcess.Id)
                {
                    // Active the running instance
                    ShowWindowAsync(proc.MainWindowHandle, WS_SHOWNORMAL);
                    SetForegroundWindow(proc.MainWindowHandle);

                    return true;
                }
            }

            return false;
        }
    }
}
//3. 在Main函数入口判断
//[STAThread]
//static void Main()
//{
//// Control only one instance with the same process name can run
//if (ExistRunningInstance())
//{
//Environment.Exit(1); // App is running, exit
//}

//// 程序真正运行的代码
//}