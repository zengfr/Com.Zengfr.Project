using System;
using System.Text;
//using cSharpMemoryLib;
//using cSharpMemoryLib.Helper;
using Memory;
using System.Linq;
namespace MemConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //FillUtil.findAndFill(@"C:\Users\zengfr\Desktop\EASy68K\srecwtr\p.bin", false);
            // MemoryScanner.Main2();
            var processesName = "mame64";
            //processesName = "hbmameui64";
            MyHook.Injected(processesName);
           
            // Test4MemLib(args);
            Console.ReadLine();
        }
            static async void Test4MemLib(string[] args)
        {
                Mem MemLib = new Mem();
              MemLib.OpenProcess("mame64.exe");
            StringBuilder sb = new StringBuilder();
            foreach (var item in MemLib.modules)
            {
                sb.AppendFormat("{0} - {1}{2}", item.Key, item.Value, Environment.NewLine);
            }
            string result = sb.ToString().TrimEnd();
            Console.WriteLine("Modules:{0}", result);
            ;//"00 FF 0E 66 00 00 04 1A 00 00 00 C2"
            long myAoBScan = (await MemLib.AoBScan("FF 00 66 0E 00 00 1A 04 00 00 C2 00")).FirstOrDefault();
            // baseAddress=18cbc040
            long myAoBScan2 = (await MemLib.AoBScan("FF 00 66 0E")).FirstOrDefault();
            byte[] bb = MemLib.readBytes("base+0xffb2e0",16);
            byte[] bb2 = MemLib.readBytes("0x18cbc040", 16);
            Console.WriteLine("myAoBScan:{0}", myAoBScan);
            Console.ReadLine();
        }
        //static void Test4cSharpMemory(string[] args)
        //{
        //    var cSharpMem = new cSharpMemory(ProcessFinder.GetProcessByName("hbmameui64"));

        //    IntPtr bAddress = new IntPtr(0x10000);
        //    int count = 16;
        //    String hex = "00FF0E660000041A000000C2";
        //    hex = "4e89 0000 0a0a 207c";
        //    hex = hex.Replace(" ","");
        //    while (bAddress.ToInt64()<0xffff0000)
        //    { 
        //        Byte[] temp = cSharpMem.ReadByteArray(bAddress, count);
        //        StringBuilder strBuider = new StringBuilder();
        //        for (int index = 0; index < temp.Length; index++)
        //        {
        //            strBuider.Append(Convert.ToString(temp[index], 16).PadLeft(2, '0'));
        //        }
        //        if (strBuider.ToString().ToUpper().Contains(hex))
        //        {
        //            break;
        //        }
        //        Console.WriteLine("HEX:{0},{1},{2}", bAddress, Convert.ToString(bAddress.ToInt64(), 16), strBuider.ToString());
        //        bAddress += count;
        //    }

           
        //    Console.ReadLine();

        //    //IntPtr address = new IntPtr(0x17FC1B8CBE0);

        //    //cSharpMem.WriteASCII(address, "Hello");
        //    Console.ReadLine();



        //}
    }
}
