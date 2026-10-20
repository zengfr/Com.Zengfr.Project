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
    public class ScrcpyViewManager
    {
        private static readonly ILogger log = Log.ForContext<ScrcpyViewManager>();
        public static ScrcpyViewManager Instance { get; set; } = new ScrcpyViewManager();
        public Dictionary<String, DeviceViewModel> Views { get; set; } = new Dictionary<string, DeviceViewModel>();

        public DeviceViewModel? GetView(DeviceData? device, bool connect)
        {
            if (device == null) return null;
            DeviceViewModel? view;

            var sel = device.Serial;
            if (!Views.ContainsKey(sel))
            {
                lock (Views)
                {
                    if (!Views.ContainsKey(sel))
                    {
                        var dv = new DeviceViewModel();
                        Views[sel] = dv;
                    }
                }
            }
            view = Views[sel];
            lock (device)
            {
                try
                {
                    if (connect && view != null)
                    {
                        view.BindDeviceCommand.Execute(device).Subscribe();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex,"device:" + device.Serial);
                }
            }
            return view;
        }
        public void RemoveView(DeviceData device, bool disconnect)
        {
            var sel = device.Serial;
            var view = Views[sel];
            lock (device)
            {
                try
                {
                    Views.Remove(sel);
                    if (disconnect && view != null)
                    {
                        view.UnBindDeviceCommand.Execute().Subscribe();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex,"device:" + device.Serial);
                }
            }
        }
    }
}