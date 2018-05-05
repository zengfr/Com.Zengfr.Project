using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask
{
   public class BeepUtil
    {
        public enum MessageBeepType
        {
            Default = -1,
            Ok = 0x00000000,
            Error = 0x00000010,
            Question = 0x00000020,
            Warning = 0x00000030,
            Information = 0x00000040,
        }
        public const int MB_ICONEXCLAMATION = 48;
        /// <summary>
        /// //Ok = 0x00000000,  
        //Error = 0x00000010,  
        //Question = 0x00000020,  
        //Warning = 0x00000030,  
        //Information = 0x00000040
        /// </summary>
        /// <param name="uType"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool MessageBeep(uint uType);
        [DllImport("kernel32.dll")]
        public static extern bool Beep(int freq, int duration);

        /// <summary>
        /// PlaySound("提示时奏幻想空间.WAV",0,SND_ASYNC|SND_FILENAME);
        /// </summary>
        /// <param name="pszSound"></param>
        /// <param name="hmod"></param>
        /// <param name="fdwSound"></param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        public static extern bool PlaySound(string pszSound, int hmod, int fdwSound);
        public const int SND_FILENAME = 0x00020000;
        public const int SND_SYNC = 0x0000;
        public const int SND_ASYNC = 0x0001;
        public const int SND_LOOP = 0x0008; 

        public static bool PlaySound(string pszSound) {
            var path = string.Format("{0}sound\\{1}", AppDomain.CurrentDomain.BaseDirectory, pszSound);
            return PlaySound(path, 0, SND_FILENAME| SND_ASYNC);
        }
        public static bool PlaySoundSync(string pszSound)
        {
            var path = string.Format("{0}sound\\{1}", AppDomain.CurrentDomain.BaseDirectory, pszSound);
            return PlaySound(path, 0, SND_FILENAME | SND_SYNC);
        }
        public static bool PlaySoundLoop(string pszSound)
        {
            var path = string.Format("{0}sound\\{1}", AppDomain.CurrentDomain.BaseDirectory, pszSound);
            return PlaySound(path, 0, SND_FILENAME | SND_SYNC| SND_LOOP);
        }
    }
}
