using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices;
using hookInject.impl;
using hookInject.core;
using EasyHook;
using System.Text;
using System.Globalization;
using System.Threading;

namespace hookInject
{
    public class MainInjection : EasyHook.IEntryPoint
    {
        private ConcurrentDictionary<string, Queue<string>> _fQueue = new ConcurrentDictionary<string, Queue<string>>();
        private ConcurrentDictionary<string, Dictionary<int, HashSet<int>>> _fHashQueue = new ConcurrentDictionary<string, Dictionary<int, HashSet<int>>>();

        public ServerInterface Ipc { get; set; }
        public Queue<string> Queue { get; set; }

        private List<ApiMonitor> monitors;

        public MainInjection(
          EasyHook.RemoteHooking.IContext context,
          string channelName)
        {
            Queue = new Queue<string>();
            monitors = new List<ApiMonitor>();
            init();
            // Connect to server object using provided channel name
            Ipc = EasyHook.RemoteHooking.IpcConnectClient<ServerInterface>(channelName);

            // If Ping fails then the Run method will be not be called
            Ipc.Ping();
        }
        private void init()
        {
            //add(new FileActivities(this));
            //add(new RegActivities(this));
            //add(new ProcActivities(this));
            //add(new NetActivities(this));

            add(new WFDApiMonitor(this));
        }
        private void add(ApiMonitor m)
        {
            monitors.Add(m);
        }
        private void install()
        {
            foreach (ApiMonitor m in monitors)
            {
                m.InstallHook();
            }
        }
        private void uninstall()
        {
            foreach (ApiMonitor m in monitors)
            {
                m.UninstallHook();
            }
        }
        public void Run(
          EasyHook.RemoteHooking.IContext context,
          string channelName)
        {
            try
            {
                var pid = EasyHook.RemoteHooking.GetCurrentProcessId();
                Ipc.IsInstalled(pid);
                Ipc.ReportMessage(string.Format("{0},{1},{2}", RemoteHooking.IsX64System, RemoteHooking.IsAdministrator, RemoteHooking.IsX64Process(pid)));

                install();
                EasyHook.RemoteHooking.WakeUpProcess();
                loop();
                uninstall();
            }
            catch (Exception ex)
            {
                Ipc.ReportException(ex);
            }
            EasyHook.LocalHook.Release();
        }
        public unsafe EasyHook.LocalHook createHook(string InModule, string InSymbolName, Delegate InNewProc)
        {
            IntPtr pPtr = ProcUtil.GetProcAddress(InModule, InSymbolName);

            string hex = ProcUtil.getHexStringByPtr(pPtr);
            Ipc.ReportMessage(string.Format("\r\nfuncBytes:{0}", hex));

            var hook = EasyHook.LocalHook.Create(pPtr, InNewProc, this);
            hook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });

            hex = ProcUtil.getHexStringByPtr(pPtr);
            Ipc.ReportMessage(string.Format("funcBytes:{0}", hex));
            var pjPtr = ProcUtil.getjmpptr(pPtr, hex);
            if (pjPtr != pPtr)
                Ipc.ReportMessage(string.Format("jmp {0} {1}\r\nfuncBytes:{2}", pPtr.ToString("x"), pjPtr.ToString("x"),
                    ProcUtil.getHexStringByPtr(pjPtr, 0x120)));

            
            Ipc.ReportMessage(string.Format("hooked:{0},{1},{2},{3}",
                InModule.PadRight(16, ' '), InSymbolName.PadRight(16, ' '),
                hook.GetPrivateField<IntPtr>("m_Handle"), GCHandle.ToIntPtr(hook.GetPrivateField<GCHandle>("m_SelfHandle"))));
            return hook;
        }


        private void processQueue()
        {
            string[] queued = null;
            lock (Queue)
            {
                queued = Queue.ToArray();
                Queue.Clear();
            }
            if (queued != null && queued.Length > 0)
            {
                Ipc.ReportMessages(queued);
            }
        }
        private void processfQueue()
        {
            string[] queued = null;
            int len = 0;
            lock (_fQueue)
            {
                foreach (string k in _fQueue.Keys)
                {
                    if (k.Length > 4)
                    {
                        queued = _fQueue[k].ToArray();
                        if (queued != null && queued.Length > 0)
                        {
                            len += queued.Length;
                            _fQueue[k].Clear();
                            string ff = AppDomain.CurrentDomain.BaseDirectory + "\\" + Path.GetFileName(k) + ".dup";
                            if (!File.Exists(ff))
                            {
                                File.Create(ff);
                            }
                            Ipc.ReportMessage(Path.GetFileName(ff) + " " + queued.Length.ToString());
                            File.AppendAllLines(ff, queued);
                        }
                    }
                }
            }
            if (len > 0 && len < 1000)
            {

                System.Threading.Thread.Sleep(5000);
            }
        }
        int lastErr = 0;
        private void loop()
        {
            while (!Ipc.IsStop)
            {
                try
                {
                    System.Threading.Thread.Sleep(50);

                    processQueue();
                    processfQueue();

                    var err = Marshal.GetLastWin32Error();
                    if (err != lastErr)
                    {
                        lastErr = err;
                        Ipc.ReportMessage(string.Format("{0} err:{1} {2}",
                            DateTime.Now.ToString("HHmmss fff"), err, WinErrors.GetSystemMessage(err)));
                    }
                }
                catch (Exception ex)
                {
                    Ipc.ReportException(ex);
                }
            }

        }
        public void initq(string fname)
        {
            if (!this._fQueue.ContainsKey(fname))
            {
                lock (this._fQueue)
                {
                    if (!this._fQueue.ContainsKey(fname))
                    {
                        this._fHashQueue[fname] = new Dictionary<int, HashSet<int>>();
                        this._fQueue[fname] = new Queue<string>();
                    }
                }
            }
            for (int hash = -hashod; hash < hashod; hash++)
            {
                if (!this._fHashQueue[fname].ContainsKey(hash))
                {
                    lock (this._fHashQueue[fname])
                    {
                        if (!this._fHashQueue[fname].ContainsKey(hash))
                        {
                            this._fHashQueue[fname][hash] = new HashSet<int>();
                        }

                    }
                }
            }
        }
        int hashod = 1024;
        public bool enqueue(string fname, string str)
        {
            var hash = str.GetHashCode();
            var hash2 = hash % hashod;
            if (!this._fHashQueue[fname][hash2].Contains(hash))
            {
                bool add = this._fHashQueue[fname][hash2].Add(hash);
                if (add)
                {
                    this._fQueue[fname].Enqueue(str.Trim('\r').Trim('\n').Trim(' '));
                    return true;
                }

            }
            return false;
        }

    }
}
