using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace hookInject
{
    public class ServerInterface : MarshalByRefObject
    {
        public void IsInstalled(Int32 InClientPID)
        {
            ReportMessage(string.Format("injected:{0}", InClientPID));
        }

        public void ReportMessages(String[] InFileNames)
        {
            for (int i = 0; i < InFileNames.Length; i++)
            {
                ReportMessage(InFileNames[i]);
            }
        }
        public void ReportMessage(string fmt,params string[] args)
        {
			ReportMessage(string.Format(fmt,args));
		}
        public void ReportMessage(string message)
        {
            Console.WriteLine(  message);
        }
        public void ReportError(int err)
        {
            ReportMessage("{0} error:{1}", err.ToString( ), WinErrors.GetSystemMessage( err));
        }
        public void ReportException(Exception InInfo)
        {
            ReportMessage(string.Format("{0} error:{1} {2}",DateTime.Now.ToString("HHmmss fff"), InInfo.ToString(),InInfo.StackTrace));
        }
        int count = 0;

        public bool IsStop { get; internal set; }

        public void Ping()
        {
            var oldTop = Console.CursorTop;
            if (oldTop > 0)
            {
                var oldLeft = Console.CursorLeft;
                
                var chars = "\\|/-";

                Console.CursorVisible = false;
                Console.SetCursorPosition(Console.WindowWidth - 1, oldTop - 1);
                Console.Write(chars[count++ % chars.Length]);

                Console.SetCursorPosition(oldLeft, oldTop);
                Console.CursorVisible = true;
            }
        }
    }
}
