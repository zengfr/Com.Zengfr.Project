using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System;
using System.Drawing;

namespace Com.Zfrong.Common.Win32.API
{
    public class AppbarHelper
    {
        [StructLayout(LayoutKind.Sequential)]
        struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct APPBARDATA
        {
            public int cbSize;
            public IntPtr hWnd;
            public int uCallbackMessage;
            public int uEdge;
            public RECT rc;
            public IntPtr lParam;
        }
        enum ABMsg : int
        {
            ABM_NEW = 0,
            ABM_REMOVE,
            ABM_QUERYPOS,
            ABM_SETPOS,
            ABM_GETSTATE,
            ABM_GETTASKBARPOS,
            ABM_ACTIVATE,
            ABM_GETAUTOHIDEBAR,
            ABM_SETAUTOHIDEBAR,
            ABM_WINDOWPOSCHANGED,
            ABM_SETSTATE
        }
        enum ABNotify : int
        {
            ABN_STATECHANGE = 0,
            ABN_POSCHANGED,
            ABN_FULLSCREENAPP,
            ABN_WINDOWARRANGE
        }
      public  enum ABEdge : int
        {
            ABE_LEFT = 0,
            ABE_TOP,
            ABE_RIGHT,
            ABE_BOTTOM
        }
        [DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
      private static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);
        [DllImport("USER32")]
        private static extern int GetSystemMetrics(int Index);
        [DllImport("User32.dll", ExactSpelling = true,
            CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool MoveWindow
            (IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int RegisterWindowMessage(string msg);
        public static void RegisterBar(int handle, ABEdge edge, int widthorheight)
        {
            RegisterBar(new IntPtr(handle), edge, widthorheight, widthorheight);
        }
        public static void RegisterBar(IntPtr handle, ABEdge edge, int widthorheight)
        {
            RegisterBar(handle, edge, widthorheight, widthorheight);
        }
        public static void RegisterBar(int handle, ABEdge edge, int width, int height)
        {
            RegisterBar(new IntPtr(handle), edge, width, height);
        }
        public static void RegisterBar(IntPtr handle, ABEdge edge, int width, int height)
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = handle;

            int uCallBack = RegisterWindowMessage("AppBarMessage");
            abd.uCallbackMessage = uCallBack;

            uint ret = SHAppBarMessage((int)ABMsg.ABM_NEW, ref abd);
            ABSetPos(handle, edge,width, height);
        }
        public static void UnRegisterBar(int handle)
         {
             UnRegisterBar(new IntPtr(handle));
         }
        public static void UnRegisterBar(IntPtr handle)
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = handle;
          
            SHAppBarMessage((int)ABMsg.ABM_REMOVE, ref abd);
        }
        private static void ABSetPos(IntPtr handle, ABEdge edge, int width, int height)
        { 
            ABSetPos(handle, edge,new Size(width,height));
        }
        private static void ABSetPos(IntPtr handle, ABEdge edge, Size size)
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd =handle;
            abd.uEdge = (int)edge;// (int)ABEdge.ABE_TOP;

            if (abd.uEdge == (int)ABEdge.ABE_LEFT || abd.uEdge == (int)ABEdge.ABE_RIGHT)
            {
                abd.rc.top = 0;
                abd.rc.bottom = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
                if (abd.uEdge == (int)ABEdge.ABE_LEFT)
                {
                    abd.rc.left = 0;
                    abd.rc.right = size.Width;
                }
                else
                {
                    abd.rc.right = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
                    abd.rc.left = abd.rc.right - size.Width;
                }

            }
            else
            {
                abd.rc.left = 0;
                abd.rc.right = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Width;
                if (abd.uEdge == (int)ABEdge.ABE_TOP)
                {
                    abd.rc.top = 0;
                    abd.rc.bottom = size.Height;
                }
                else
                {
                    abd.rc.bottom = System.Windows.Forms.SystemInformation.PrimaryMonitorSize.Height;
                    abd.rc.top = abd.rc.bottom - size.Height;
                }
            }

            SHAppBarMessage((int)ABMsg.ABM_QUERYPOS, ref abd);

            switch (abd.uEdge)
            {
                case (int)ABEdge.ABE_LEFT:
                    abd.rc.right = abd.rc.left + size.Width;
                    break;
                case (int)ABEdge.ABE_RIGHT:
                    abd.rc.left = abd.rc.right - size.Width;
                    break;
                case (int)ABEdge.ABE_TOP:
                    abd.rc.bottom = abd.rc.top + size.Height;
                    break;
                case (int)ABEdge.ABE_BOTTOM:
                    abd.rc.top = abd.rc.bottom -size.Height;
                    break;
            }

            SHAppBarMessage((int)ABMsg.ABM_SETPOS, ref abd);
            MoveWindow(abd.hWnd, abd.rc.left, abd.rc.top,
                    abd.rc.right - abd.rc.left, abd.rc.bottom - abd.rc.top, true);
        }
    }

   public partial class FindWindow
    {
       public static IntPtr FindChildHandle(IntPtr hwndParent, string classname, string caption, int timeout)
       {
           FindWindow f = new FindWindow(hwndParent, classname, caption, timeout);
           return f.FoundHandle;
       }

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        //IMPORTANT : LPARAM  must be a pointer (InterPtr) in VS2005, otherwise an exception will be thrown
        private static extern bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);
        //the callback function for the EnumChildWindows
        private delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        //if found  return the handle , otherwise return IntPtr.Zero
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        private string m_classname; // class name to look for
        private string m_caption; // caption name to look for

        private DateTime start;
        private int m_timeout;//If exceed the time. Indicate no windows found.

        private IntPtr m_hWnd; // HWND if found
        public IntPtr FoundHandle
        {
            get { return m_hWnd; }
        }

        private bool m_IsTimeOut;
        public bool IsTimeOut
        {
            get { return m_IsTimeOut; }
            set { m_IsTimeOut = value; }
        }

        // ctor does the work--just instantiate and go
        public FindWindow(IntPtr hwndParent, string classname, string caption, int timeout)
        {
            m_hWnd = IntPtr.Zero;
            m_classname = classname;
            m_caption = caption;
            m_timeout = timeout;
            start = DateTime.Now;
            FindChildClassHwnd(hwndParent, IntPtr.Zero);
        }

        /**/
        /// <summary>
        /// Find the child window, if found m_classname will be assigned 
        /// </summary>
        /// <param name="hwndParent">parent's handle</param>
        /// <param name="lParam">the application value, nonuse</param>
        /// <returns>found or not found</returns>
        //The C++ code is that  lParam is the instance of FindWindow class , if found assign the instance's m_hWnd
        private bool FindChildClassHwnd(IntPtr hwndParent, IntPtr lParam)
        {
            EnumWindowProc childProc = new EnumWindowProc(FindChildClassHwnd);
            IntPtr hwnd = FindWindowEx(hwndParent, IntPtr.Zero, m_classname, m_caption);
            if (hwnd != IntPtr.Zero)
            {
                this.m_hWnd = hwnd; // found: save it
                m_IsTimeOut = false;
                return false; // stop enumerating
            }

            DateTime end = DateTime.Now;

            if (start.AddSeconds(m_timeout) > end)
            {
                m_IsTimeOut = true;
                return false;
            }

            EnumChildWindows(hwndParent, childProc, IntPtr.Zero); // recurse  redo FindChildClassHwnd
            return true;// keep looking
        }
    }
   public partial class FindWindow
   {
       [DllImport("user32.dll", EntryPoint = "FindWindow")]
       public static extern IntPtr FindWindowHandle(string 类名, string 标题); 
   }
}
