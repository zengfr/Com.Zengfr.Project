using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyHook;

namespace hookInject.core
{
   public interface IMonitor
    {
           void InstallHook();
           void UninstallHook();
    }
    public abstract class ApiMonitor:IMonitor
    {
        protected MainInjection Injector;

        public ApiMonitor(MainInjection inject)
        {
            this.Injector = inject;
        }

        public abstract void InstallHook();
        public abstract void UninstallHook();
        public static String FormatMessage(String api, String msg)
        {
            //[TIME][PID:TID]:"API":"MSG"
            String str =string.Format( "{0} {1} {2} {3} {4}", DateTime.Now.ToString("HHmmss fff"),
                RemoteHooking.GetCurrentProcessId() ,RemoteHooking.GetCurrentThreadId() ,
                api.PadRight(16,' ') , msg);
            return str;
        }
    }
}
