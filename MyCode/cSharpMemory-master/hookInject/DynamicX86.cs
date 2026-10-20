using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace hookInject
{
   public class DynamicX86
    {
        const uint PAGE_EXECUTE_READWRITE = 0x40;
        const uint MEM_COMMIT = 0x1000;
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetHandleCount(byte[] value);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool VirtualProtect(IntPtr lpAddress, int dwSize, int flNewProtect, out int lpflOldProtect);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        static void Main(string[] args)
        {
            List<byte> bodyBuilder = new List<byte>();
            bodyBuilder.Add(0xb8);
            bodyBuilder.AddRange(BitConverter.GetBytes(42));
            bodyBuilder.Add(0xc3);
            Func<int> ptr = Inject<int>(bodyBuilder);
            Console.WriteLine(ptr());
        }
        public static Func<T> Inject<T>(List<byte> asm)
        {
          return  Inject<T>(asm.ToArray());
        }
        public static Func<T> Inject<T>(byte[] asm)
        {
            IntPtr buf = VirtualAlloc(IntPtr.Zero, (uint)asm.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
            Marshal.Copy(asm, 0, buf, asm.Length);

            Func<T> ptr = (Func<T>)Marshal.GetDelegateForFunctionPointer(buf, typeof(Func<T>))  ;
            return ptr;
        }
        public static unsafe int? InjectAndRunX86ASM( Func<int> del, byte[] asm)
        {
            if (del != null)
                fixed (byte* ptr = &asm[0])
                {
                    FieldInfo _methodPtr = typeof(Delegate).GetField("_methodPtr", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo _methodPtrAux = typeof(Delegate).GetField("_methodPtrAux", BindingFlags.NonPublic | BindingFlags.Instance);

                    //_methodPtr.SetValue(del, ptr);
                    //_methodPtrAux.SetValue(del, ptr);

                    return del();
                }
            else
                return null;
        }
    }
}
