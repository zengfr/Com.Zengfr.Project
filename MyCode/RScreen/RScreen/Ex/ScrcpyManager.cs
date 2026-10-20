using FFmpeg.AutoGen;
using ScrcpyNet;
using Serilog;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RScreen.Ex
{
    public class ScrcpyManager
    {
        private static readonly ILogger log = Log.ForContext<ScrcpyManager>();
        public static ScrcpyManager Instance { get; set; } = new ScrcpyManager();
        public Dictionary<String, Scrcpy> Devices { get; set; } = new Dictionary<string, Scrcpy>();
        public Scrcpy? GetScrcpy(DeviceData? device, bool start)
        {
            if (device == null)
                return null;
            String serial = device.Serial;
            Scrcpy scrcpy;

            if (!Devices.ContainsKey(serial))
            {
                lock (Devices)
                {
                    if (!Devices.ContainsKey(serial))
                    {
                        var BitrateKb = ScreenCfg.Instance.BitrateKb;

                        scrcpy = new Scrcpy(device);
                        scrcpy.Port = scrcpy.scrcpyPort + Devices.Count * 2;
                        scrcpy.Bitrate = (long)(BitrateKb * 1000);
                        Devices[serial] = scrcpy;

                    }
                }
            }
            scrcpy = Devices[serial];
            if (start)
            {

                if (!scrcpy.Connected)
                {
                    if (device.State.Equals(DeviceState.Online) || device.State.Equals(DeviceState.Offline))
                    {
                        lock (Devices)
                        {
                            if (!scrcpy.Connected)
                            {
                                try
                                {
                                    scrcpy.Start();
                                    scrcpy.IsRePushServerFile = true;
                                }
                                catch (Exception ex)
                                {
                                    log.Error(ex,"device:" + device.Serial);
                                }
                            }
                        }
                    }
                }
            }
            return scrcpy;
        }

        public void StartScrcpy(String serial)
        {
            Scrcpy scrcpy = Devices[serial];
            if (scrcpy != null)
                scrcpy.Start();
        }
        public void StopScrcpy(String serial)
        {
            Scrcpy scrcpy = Devices[serial];
            if (scrcpy != null && scrcpy.Connected)
                scrcpy.Stop();
        }
        public void StopAllScrcpy()
        {
            foreach (var scrcpy in Devices.Values)
            {
                if (scrcpy != null && scrcpy.Connected)
                    scrcpy.Stop();
            }

        }
    }
}