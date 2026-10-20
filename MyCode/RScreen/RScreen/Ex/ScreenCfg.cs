
using Avalonia.Platform;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RScreen.Ex
{
    public class ScreenCfg
    {
        public static ScreenCfg Instance { get; set; } = new ScreenCfg();
        /// <summary>
        /// 5-10-15-20
        /// </summary>
        [Reactive] public int DisplySize { get; set; } = 4;

        /// <summary>
        /// 1000-8000 /1000
        /// </summary>
        [Reactive] public int BitrateKb { get; set; } = 2000;

        /// <summary>
        /// 10-60 /15
        /// </summary>
        [Reactive] public int MaxFps { get; set; } = 15;

        [Reactive]
        public int MinHeight
        {
            get
            {
                return getMinHeight();
            }
        }
        [Reactive]
        public int MinWidth
        {
            get
            {
                return getMinWidth();
            }
        }
        public int getMinHeight()
        {
            int width = getw();
            int displySize = 1;
            var v = 24 + (width / displySize * 1920 / 1080);
            return v;
        }

        public int getMinWidth()
        {
            int width = getw();
            int displySize = 1;
            var v = (width / displySize);
            return v;
        }
        public int getw()
        {
            var widths = new List<int>() { 78, 100, 150, 200, 300, 375 };
            return widths[6 - DisplySize];
        }
    }
}
