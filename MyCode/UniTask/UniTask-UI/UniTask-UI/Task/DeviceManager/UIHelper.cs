using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniTask_UI.Task.DeviceManager
{
    /// <summary>
    /// <code>
    /// if (_Setting.DarkMode)
    ///         {
    ///          tabBackColor = Color.FromArgb(24, 24, 24);
    ///              foreColor = Color.FromArgb(192, 192, 192);
} ///
  ///       else
  ///{
  ///tabBackColor = Color.FromArgb(242, 242, 244);
  ///foreColor = Color.FromArgb(48, 48, 48);
  ///}</ code >
  /// </summary>
public class UIHelper
{
    [DllImport("user32.dll")]
    static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    private static void MoveToPanel(IntPtr hWndDocked, System.Windows.Forms.Panel Panel)
    {
        MoveWindow(hWndDocked, 0, 0, Panel.Width, Panel.Height, true);
    }
    public static IntPtr SetParentPanel(IntPtr hWnd, System.Windows.Forms.Panel panel)
    {
        var hWndOriginalParent = SetParent(hWnd, panel.Handle);
        MoveToPanel(hWnd, panel);
        return hWndOriginalParent;
    }
    public static IntPtr SetParentPanel(Process process, System.Windows.Forms.Panel panel)
    {
        IntPtr hWndDocked = IntPtr.Zero;
        while (hWndDocked == IntPtr.Zero)
        {
            if (process.HasExited)
            {
                return hWndDocked;
            }
            hWndDocked = process.MainWindowHandle;
            if (hWndDocked == IntPtr.Zero)
            {
                //process.WaitForInputIdle(1000); //wait for the window to be ready for input;
                process.Refresh();
                Thread.Sleep(25);
            }
        }
        return SetParentPanel(hWndDocked, panel);
    }
}
}
