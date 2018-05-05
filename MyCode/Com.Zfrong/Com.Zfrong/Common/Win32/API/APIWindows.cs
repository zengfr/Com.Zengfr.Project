using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Com.Zfrong.Common.Win32.API
{
   public  class APIWindows
    {
        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);
        //[DllImport("user32.dll")]
        //private static extern IntPtr FindWindowW(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);

        public struct WindowInfo
        {
            public IntPtr hWnd;
            public string szWindowName;
            public string szClassName;
        }
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        const int WM_CLOSE = 0x0010;

        public static void Run()
        {
            WindowInfo[]  ww=GetAllDesktopWindows();
            foreach (WindowInfo w in ww)
            {
                if (w.szWindowName.ToLower().IndexOf("http") != -1 
                    || w.szWindowName.ToLower().IndexOf("analyzer") != -1
                    || w.szWindowName.ToLower().IndexOf("monitor") != -1
                    ||w.szWindowName.ToLower().IndexOf("sniff") != -1
                    ||w.szWindowName.ToLower().IndexOf("shark") != -1
                    || w.szWindowName.ToLower().IndexOf("network") != -1
                    || w.szWindowName.ToLower().IndexOf("protocol") != -1)
                {
                    SendMessage(w.hWnd, WM_CLOSE, 0, 0);
                    //Console.WriteLine(w.szWindowName);//
                }
            }
        }

        static WindowInfo[] GetAllDesktopWindows()
        {
            List<WindowInfo> wndList = new List<WindowInfo>();

            //enum all desktop windows
            EnumWindows(delegate(IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                //get hwnd
                wnd.hWnd = hWnd;
                //get window name
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                //get window class
                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                //add it into list
                wndList.Add(wnd);
                return true;
            }, 0);

            return wndList.ToArray();
        }
    }
}
