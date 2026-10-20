using Avalonia.Media.Imaging;
using ScrcpyNet;
using ScrcpyNet.Sample.ViewModels;
using Serilog;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RScreen.Ex
{
    class ScrcpyImgManager
    {
        private static readonly ILogger log = Log.ForContext<ScrcpyImgManager>();
        public static ScrcpyImgManager Instance { get; set; } = new ScrcpyImgManager();
        public Dictionary<String, WriteableBitmap?> Imgs { get; set; } = new Dictionary<string, WriteableBitmap?>();
        public WriteableBitmap? GetImg(String serial)
        {
            if (serial == null)
                return null;
            if (Imgs.ContainsKey(serial))
            {
                return Imgs[serial];
            }
            return null;
        }
    }
}
