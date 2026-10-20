using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyHook;
using hookInject;

namespace MemConsole
{
    public class MyHook
    {
        static string dllName = "hookInject.dll";
       
        public static void Main2(string[] args)
        {
            var processesName = "mame64";
             processesName = "hbmameui64";
            Injected(processesName);
        }
        public static void Injected(string processesName)
        {
            try
            {
                Config.HelperLibraryLocation = AppDomain.CurrentDomain.BaseDirectory;

                Console.WriteLine("run");
                RegGACAssemblys(dllName, true);

                int targetPID = 0;
                // Will contain the name of the IPC server channel
                string channelName = null;
                Process[] localByName = Process.GetProcessesByName(processesName);
                var Ipc = RemoteHooking.IpcCreateServer<ServerInterface>(ref channelName, WellKnownObjectMode.SingleCall);
                string injectionLibrary = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), dllName);

                if (localByName.Any())
                {
                    targetPID = localByName[0].Id;
                    Thread.Sleep(3000);
                    RemoteHooking.Inject(targetPID, injectionLibrary,injectionLibrary, channelName);
                    Console.WriteLine("Injected to process {0}", targetPID);
                }
                Console.WriteLine("<Press any key to exit>");
                Console.ReadKey(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex);
            }
        }
        private static bool RegGACAssemblys(string dllName, bool reg)
        {

            RegGACAssembly("EasyHook.dll", reg);
            RegGACAssembly("EasyLoad64.dll", reg);
            RegGACAssembly(dllName, reg);
            return true;
        }
        private static bool RegGACAssembly(string dllName, bool reg)
        {
            var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dllName);
            new System.EnterpriseServices.Internal.Publish().GacRemove(dllPath);
            if (reg)
                if (!RuntimeEnvironment.FromGlobalAccessCache(Assembly.LoadFrom(dllPath)))
                {
                    new System.EnterpriseServices.Internal.Publish().GacInstall(dllPath);
                }
            return true;
        }

    }
}
