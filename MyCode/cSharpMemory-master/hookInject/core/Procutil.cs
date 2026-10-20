using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace hookInject
{
   public class ProcUtil
    {
        public static unsafe IntPtr GetProcAddress(string InModule, string InSymbolName)
        {
            return (IntPtr)ShortJmp(EasyHook.LocalHook.GetProcAddress(InModule, InSymbolName));
        }
        static unsafe void* ShortJmp(IntPtr pPtr)
        {
            /*byte* codePtr = (byte*)pPtr;
            if (((byte)0xeb) == *codePtr)
            {
                // Short jump instruction, second byte is the offset, which is signed value
                byte jmpOfs = *(++codePtr);
                codePtr++;
                codePtr += jmpOfs;
            }*/

            return (void*)pPtr;
        }
        public static string getHexStringByPtr(IntPtr pPtr)
        {
            return getHexStringByPtr(pPtr, 0x10);
        }
        public static string getHexStringByPtr(IntPtr pPtr, int size)
        {
            byte[] managedArray = new byte[size];
            Marshal.Copy(pPtr, managedArray, 0, size);
            string hex = ToHexStringFromBytes(managedArray);

            return hex;
        }
        public static IntPtr getjmpptr(IntPtr pPtr, string hex)
        {
            if (hex.StartsWith("E9 "))
            {
                IntPtr p = pPtr + 5 + int.Parse(getjmpoffset(hex), NumberStyles.AllowHexSpecifier | NumberStyles.HexNumber);
                return p;

            }
            return pPtr;
        }
        private static string getjmpoffset(string hex)
        {
            var s = string.Join("", hex.Substring(12, 2), hex.Substring(9, 2), hex.Substring(6, 2), hex.Substring(3, 2));
            return s;
        }
        private static byte[] ToBytesFromHexString(string hexString)
        {
            string[] chars = hexString.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] returnBytes = new byte[chars.Length];
            for (int i = 0; i < chars.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(chars[i], 16);
            }
            return returnBytes;
        }
        private static string ToHexStringFromBytes(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }

            return sb.ToString().ToUpper();
        }
    }
}
