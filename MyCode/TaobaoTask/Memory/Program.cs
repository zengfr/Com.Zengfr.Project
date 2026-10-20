using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaobaoTask;

namespace Memory
{
    class Program
    {
        static void Main(string[] args)
        {
          
           
            String name = "hbmameui64.exe";
            Process[] Processes = Process.GetProcessesByName("hbmameui64");
            Process nProcess = Processes[0];
            Console.WriteLine("MainModule:{0},{1}", nProcess.MainModule.ModuleName, nProcess.MainModule.BaseAddress);
            foreach (ProcessModule m in nProcess.Modules)
            {
                Console.WriteLine("Module:{0},{1}", m.ModuleName, m.BaseAddress);
            }



            int pid = (int)APIUtil.GetProcessIDByProcessName(name);
            String hex = "00FF0E660000041A000000C2";
            if (pid > 0)
            {   //00FF0E660000041A000000C2
                Console.WriteLine("IntPtr:{0}", pid);
                int s = APIUtil.SearchByHex((int)pid, hex);
                Console.WriteLine("S:{0}", s);
            }

            Console.ReadLine();
            return;
        }

     

    }
}
