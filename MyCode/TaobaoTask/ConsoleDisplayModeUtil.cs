using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask
{
    public class ConsoleDisplayModeUtil
    {
        #region Win API
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        const int STD_OUTPUT_HANDLE = -11;
        const int CONSOLE_FULLSCREEN_MODE = 1;
        const int CONSOLE_WINDOWED_MODE = 2;
        #endregion

        public delegate bool SetConsoleDisplayModeFunc(IntPtr hOut, int dwNewMode, out int lpdwOldMode);

        public static void SetFullScreenMode()
        {
            SetConsoleScreenMode(CONSOLE_FULLSCREEN_MODE);
        }
        public static void SetConsoleScreenMode(int mode)
        {
            DllInvoke dll = new DllInvoke("kernel32.dll");
            int dwOldMode;
            //标准输出句柄
            IntPtr hOut = GetStdHandle(STD_OUTPUT_HANDLE);
            //调用Win API,设置屏幕最大化
            SetConsoleDisplayModeFunc s = (SetConsoleDisplayModeFunc)dll.Invoke("SetConsoleDisplayMode", typeof(SetConsoleDisplayModeFunc));
            s(hOut, mode, out dwOldMode);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {

            public short X;
            public short Y;
            public COORD(short x, short y)
            {
                this.X = x;
                this.Y = y;
            }

        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleDisplayMode(
            IntPtr ConsoleOutput
            , uint Flags
            , out COORD NewScreenBufferDimensions
            );
        public static void SetFullScreenMode2()
        {
            IntPtr hConsole = GetStdHandle(-11);   // get console handle
           COORD xy = new COORD(100, 100);
           SetConsoleDisplayMode(hConsole, 1, out xy); // set the console to fullscreen
            //SetConsoleDisplayMode(hConsole, 2);   // set the console to windowed

        }
    }
}
