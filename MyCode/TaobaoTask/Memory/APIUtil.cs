using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask
{
    /// <summary>
    /// 
    /// <code>IntPtr hh=GetHandleByProcessName("notepad.exe");
    ///   if(hh!=IntPtr.Zero)
    ///   {
    ///    SendMessage((int)hh,0x000C,0,"A");  //打开记事本，发送字母A
    ///  }</code>
    /// </summary>
    public class APIUtil
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
      uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
          uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress,
          int dwSize, uint flNewProtect, out uint lpflOldProtect);
        public const int
    PAGE_READWRITE = 0x40,
            PAGE_GUARD=0x100,
    PROCESS_VM_OPERATION = 0x0008,
    PROCESS_VM_READ = 0x0010,
    PROCESS_VM_WRITE = 0x0020;

        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessEntry32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;


            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        }
        private static Hashtable processWnd = null;
        public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
        static APIUtil()
        {
            if (processWnd == null)
            {
                processWnd = new Hashtable();
            }
        }
        [DllImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);
        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);
        [DllImport("user32.dll", EntryPoint = "IsWindow")]
        public static extern bool IsWindow(IntPtr hWnd);
        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(uint dwErrCode);
        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("KERNEL32.DLL ")]
        public static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processid);
        [DllImport("KERNEL32.DLL ")]
        public static extern int CloseHandle(IntPtr handle);
        [DllImport("KERNEL32.DLL ")]
        public static extern int Process32First(IntPtr handle, ref ProcessEntry32 pe);
        [DllImport("KERNEL32.DLL ")]
        public static extern int Process32Next(IntPtr handle, ref ProcessEntry32 pe);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, string lParam);




        public static IntPtr GetHandleByProcessName(string ProcessName)
        {
            List<ProcessEntry32> list = new List<ProcessEntry32>();
            IntPtr handle = CreateToolhelp32Snapshot(0x2, 0);
            IntPtr hh = IntPtr.Zero;
            if ((int)handle > 0)
            {
                ProcessEntry32 pe32 = new ProcessEntry32();
                pe32.dwSize = (uint)Marshal.SizeOf(pe32);
                int bMore = Process32First(handle, ref pe32);
                while (bMore == 1)
                {
                    IntPtr temp = Marshal.AllocHGlobal((int)pe32.dwSize);
                    Marshal.StructureToPtr(pe32, temp, true);
                    ProcessEntry32 pe = (ProcessEntry32)Marshal.PtrToStructure(temp, typeof(ProcessEntry32));
                    Marshal.FreeHGlobal(temp);
                    list.Add(pe);
                    if (pe.szExeFile == ProcessName)
                    {
                        bMore = 2;
                        hh = GetCurrentWindowHandle(pe.th32ProcessID);
                        break;
                    }
                    bMore = Process32Next(handle, ref pe32);
                }
            }
            return hh;
        }
        public static IntPtr GetCurrentWindowHandle(uint proid)
        {
            IntPtr ptrWnd = IntPtr.Zero;
            uint uiPid = proid;
            object objWnd = processWnd[uiPid];
            if (objWnd != null)
            {
                ptrWnd = (IntPtr)objWnd;
                if (ptrWnd != IntPtr.Zero && IsWindow(ptrWnd))  // 从缓存中获取句柄
                {
                    return ptrWnd;
                }
                else
                {
                    ptrWnd = IntPtr.Zero;
                }
            }
            bool bResult = EnumWindows(new WNDENUMPROC(EnumWindowsProc), uiPid);
            // 枚举窗体返回 false 而且没有错误号时表明获取成功
            if (!bResult && Marshal.GetLastWin32Error() == 0)
            {
                objWnd = processWnd[uiPid];
                if (objWnd != null)
                {
                    ptrWnd = (IntPtr)objWnd;
                }
            }
            return ptrWnd;
        }


        private static bool EnumWindowsProc(IntPtr hwnd, uint lParam)
        {
            uint uiPid = 0;
            if (GetParent(hwnd) == IntPtr.Zero)
            {
                GetWindowThreadProcessId(hwnd, ref uiPid);
                if (uiPid == lParam)    // 找到进程相应的主窗体句柄
                {
                    processWnd.Add(uiPid, hwnd);   // 把句柄缓存起来
                    SetLastError(0);    // 设置无错误
                    return false;   // 返回 false 以终止枚举窗体
                }
            }
            return true;
        }

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern int _MemoryReadByteSet(IntPtr hProcess, IntPtr lpBaseAddress, out Byte[] lpBuffer, int nSize, out IntPtr lpNumberOfBytesRead);

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern int _MemoryReadInt32(int hProcess, int lpBaseAddress, ref int lpBuffer, int nSize, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern int _MemoryWriteByteSet(int hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern int _MemoryWriteInt32(int hProcess, int lpBaseAddress, ref int lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcess")]
        public static extern int GetCurrentProcess();

        [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern int OpenProcess(int dwDesiredAccess, int bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        public static extern int CloseHandle(int hObject);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern int _CopyMemory_ByteSet_Float(ref float item, ref byte source, int length);


        const int PROCESS_POWER_MAX = 2035711;




        /// <summary>
        /// 读内存整数型
        /// </summary>
        /// <param name="pID">进程ID</param>
        /// <param name="bAddress">0x地址</param>
        /// <returns>0失败</returns>
        public static int ReadMemoryInt32(int pID, int bAddress)
        {
            int num = 0;
            int handle = GetProcessHandle(pID);
            int num3 = _MemoryReadInt32(handle, bAddress, ref num, 4, 0);
            CloseHandle(handle);
            if (num3 == 0)
            {
                return 0;
            }
            else
            {
                return num;
            }
        }
        public static uint GetProcessIDByProcessName(string ProcessName)
        {
            List<ProcessEntry32> list = new List<ProcessEntry32>();
            IntPtr handle = CreateToolhelp32Snapshot(0x2, 0);
            IntPtr hh = IntPtr.Zero;
            if ((int)handle > 0)
            {
                ProcessEntry32 pe32 = new ProcessEntry32();
                pe32.dwSize = (uint)Marshal.SizeOf(pe32);
                int bMore = Process32First(handle, ref pe32);
                while (bMore == 1)
                {
                    IntPtr temp = Marshal.AllocHGlobal((int)pe32.dwSize);
                    Marshal.StructureToPtr(pe32, temp, true);
                    ProcessEntry32 pe = (ProcessEntry32)Marshal.PtrToStructure(temp, typeof(ProcessEntry32));
                    Marshal.FreeHGlobal(temp);
                    list.Add(pe);
                    if (pe.szExeFile == ProcessName)
                    {
                        bMore = 2;
                        return pe.th32ProcessID;
                    }
                    bMore = Process32Next(handle, ref pe32);
                }
            }
            return 0;
        }
        /// <summary>
        /// 读内存 
        /// </summary>
        /// <param name="pID">进程ID</param>
        /// <param name="bAddress">0x地址</param>
        /// <returns> </returns>
        public static int SearchByHex(int pID,  String hex)
        {
            int handle = GetProcessHandle(pID);

          

            int bAddress = 0;
            int count = 16;
            hex  = hex.ToUpper();
            Byte[] buffer = new Byte[count];
            while (bAddress < 0xFFFFFFF)
            {
                uint prevProtection = 0;
                VirtualProtectEx(new IntPtr(handle), new IntPtr(bAddress), 4, PAGE_READWRITE|PAGE_GUARD, out prevProtection);


                IntPtr size;
                int temp = _MemoryReadByteSet(new IntPtr(handle), new IntPtr(bAddress), out buffer,4, out size);
                Console.WriteLine("ERR:{0}", GetLastError());
                StringBuilder strBuider = new StringBuilder();
                for (int index = 0; index < size.ToInt64(); index++)
                {
                    strBuider.Append(Convert.ToString(buffer[index], 16).PadLeft(2, '0'));
                }
                if (strBuider.ToString().ToUpper().StartsWith(hex))
                {
                    break;
                }
                Console.WriteLine("HEX:{0},{1}", bAddress, strBuider.ToString());
                bAddress += 8;
            }
            CloseHandle(handle);
            return bAddress;
        }
        /// <summary>
        /// 读内存 
        /// </summary>
        /// <param name="pID">进程ID</param>
        /// <param name="bAddress">0x地址</param>
        /// <returns> </returns>
        public static byte[] ReadMemoryByteSet(int pID, long bAddress,int len)
        {
            
            byte[] array = new byte[len]; 
            int handle = GetProcessHandle(pID);
            IntPtr size;
            int temp = _MemoryReadByteSet(new IntPtr(handle), new IntPtr(bAddress), out array, 4, out size);
            CloseHandle(handle);
            return array;
        }
        /// <summary>
        /// 写内存整数型
        /// </summary>
        /// <param name="pID">进程ID</param>
        /// <param name="bAddress">0x地址</param>
        /// <param name="value">写入值</param>
        /// <returns>false失败 true成功</returns>
        public static bool WriteMemoryInt32(int pID, int bAddress, int value)
        {
            int handle = GetProcessHandle(pID);
            int num2 = _MemoryWriteInt32(handle, bAddress, ref value, 4, 0);
            CloseHandle(handle);
            return num2 != 0;
        }

        /// <summary>
        /// 读内存小数型
        /// </summary>
        /// <param name="pID">进程ID</param>
        /// <param name="bAddress">0x地址</param>
        /// <returns>0失败</returns>
        public static float ReadMemoryFloat(int pID, int bAddress)
        {
            int len = 4;
            byte[] array = new byte[len];//不取空字节集也可以正确转换成单精度小数型
            int handle = GetProcessHandle(pID);
            IntPtr size;
            int temp = _MemoryReadByteSet(new IntPtr(handle), new IntPtr(bAddress), out array, 4,out size);
            if (temp == 0)
            {
                return 0f;
            }
            else
            {
                return GetFloatFromByteSet(array, 0);
            }
        }

        /// <summary>
        /// 写内存小数型
        /// </summary>
        /// <param name="pID">进程ID</param>
        /// <param name="bAddress">0x地址</param>
        /// <param name="value">写入数据</param>
        /// <returns>false失败</returns>
        public static bool WriteMemoryFloat(int pID, int bAddress, float value)
        {
            //byte[] byteSet = test.GetByteSet(value);
            byte[] byteSet = BitConverter.GetBytes(value);//https://msdn.microsoft.com/en-us/library/yhwsaf3w
                                                          //byte[] byteSet = Encoding.GetEncoding("gb2312").GetBytes(value.ToString());
            return WriteMemoryByteSet(pID, bAddress, byteSet, 0);
        }

        /// <summary>
        /// 写内存字节集
        /// </summary>
        /// <param name="pID">进程ID</param>
        /// <param name="bAddress">0x地址</param>
        /// <param name="value">字节数据</param>
        /// <param name="length">写入长度 0代表字节数据的长度</param>
        /// <returns>false失败</returns>
        private static bool WriteMemoryByteSet(int pID, int bAddress, byte[] value, int length = 0)
        {
            int handle =GetProcessHandle(pID);
            int nSize = (length == 0) ? value.Length : length;
            int tmp = _MemoryWriteByteSet(handle, bAddress, value, nSize, 0);//byte[]属于引用类型 引用类型不用ref也是以传址方式进行运算
                                                                                  //test.CloseHandle(pID);
            return tmp != 0;
        }

        /// <summary>
        /// 取空白字节集
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static byte[] GetVoidByteSet(int num)
        {
            if (num <= 0)
            {
                num = 1;
            }
            string text = "";
            for (int i = 0; i < num; i++)
            {
                text += "0";
            }
            return Encoding.UTF8.GetBytes(text);
        }

        /// <summary>
        /// 取进程句柄
        /// </summary>
        /// <param name="pID">进程ID</param>
        /// <returns>进程句柄</returns>
        public static int GetProcessHandle(int pID)
        {
            if (pID == -1)
            {
                return GetCurrentProcess();
            }
            else
            {
                return OpenProcess(PROCESS_POWER_MAX, 0, pID);
            }
        }

        /// <summary>
        /// 字节集转小数型
        /// </summary>
        /// <param name="sourceValue">字节集</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public static float GetFloatFromByteSet(byte[] sourceValue, int index)
        {
            float result = 0f;
            _CopyMemory_ByteSet_Float(ref result, ref sourceValue[index], 4);
            return result;
        }

        /// <summary>
        /// 获取字节集
        /// </summary>
        /// <param name="data">需要转换到字节集的数据</param>
        /// <returns></returns>
        public static byte[] GetByteSet(float data)
        {
            return Encoding.UTF8.GetBytes(data.ToString());
        }
    }


    }
