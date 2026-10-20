using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RScreen.Ex;
using Serilog;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;

namespace ScrcpyNet.Sample.ViewModels
{
    public class DeviceViewModel : ViewModelBase
    {
        private static readonly ILogger log = Log.ForContext<DeviceViewModel>();
        [Reactive] public DeviceData? Device { get; private set; }
        [Reactive] public string SerialSuffix { get; private set; } = "N";
        [Reactive] public string Serial { get; private set; } = "";
        [Reactive] public int Idx { get; set; } = 0;
        public bool IsConnected
        {

            get
            {
                return Device != null && Device.State == DeviceState.Online;
            }
        }
        public ReactiveCommand<DeviceData?, Unit> BindDeviceCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> UnBindDeviceCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> OpenDeviceCommand { get; private set;  }
        public ReactiveCommand<Unit, Unit> RebootDeviceCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> ShellDeviceCommand { get; private set; }
        public DeviceViewModel()
        {
            BindDeviceCommand = ReactiveCommand.Create<DeviceData?>(BindDevice);
            UnBindDeviceCommand = ReactiveCommand.Create(UnBindDevice);
            OpenDeviceCommand = ReactiveCommand.Create(OpenDevice);
            RebootDeviceCommand = ReactiveCommand.Create(RebootDevice);
            ShellDeviceCommand = ReactiveCommand.Create(ShellDevice);
        }
        private void ShellDevice()
        {
           var s=Device.Serial;
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var adb = Path.Combine(basePath + "../scrcpy-win64/", "adb.exe");
            ExecProcess(adb,true,new string[] { "-s",s,"shell" });
        }

        private void RebootDevice()
        {
            new AdbClient().Reboot(Device);
        }

        private void OpenDevice()
        {
            // new AdbClient().ExecuteShellCommand(Device, "reboot",null);
            var bitrate = ScreenCfg.Instance.BitrateKb;
            ViewDevice(Device, false, bitrate);
        }

        private void BindDevice(DeviceData? device)
        {
            if (device == null)
            {
                UnBindDevice();
                return;
            }
            Device = device;
            Serial = Device.Serial.Trim();
            SerialSuffix = Serial.Substring(Serial.Length - 8);

        }
        private void UnBindDevice()
        {
            Device = null;
            SerialSuffix = "N";
            Serial = "";

        }
        private static void ExecProcess(String cmd, bool shell, String[] args)
        {
            try
            {

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var arguments = string.Join(" ", args.Where(x => !string.IsNullOrWhiteSpace(x)));
                var processStartInfo = new ProcessStartInfo(cmd, arguments)
                {
                    CreateNoWindow = !shell,
                    WindowStyle = shell? ProcessWindowStyle.Normal: ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = !shell,
                    UseShellExecute = shell,
                    WorkingDirectory = basePath
                };
                log.Information(String.Format("cmd:{0} args:{1}", cmd, arguments));
                var process = Process.Start(processStartInfo);

                if (process != null)
                {
                    process.Exited += (sender, eventArgs) =>
                    {
                        //// only restart once, do not go crazy if device cannot be viewed
                        //if (bitrate == null)
                        //{
                        //    if (process.ExitCode != 0)
                        //    {
                        //        ViewDevice(device, mirror, bitrate: 80000);
                        //    }
                        //}
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, "cmd:" + cmd);
            }
        }
        private static void ViewDevice(DeviceData device, bool mirror, int? bitrate = null)
        {
            try
            {
                var scrCpyPath = AppDomain.CurrentDomain.BaseDirectory;
                var scr = Path.Combine(scrCpyPath + "../scrcpy-win64/", "scrcpy.exe");
                log.Information(String.Format("scr:{0}", scr));
                var args = new[]
                {
                mirror ? "--no-control" : "",
                bitrate == null ? "" : "-b " + bitrate,
                "-s " + device.Serial,
                "--window-title \"" + device.Model + " - " + device.Serial + "\"" };
                ExecProcess(scr, false, args);
            }
            catch (Exception ex)
            {
                log.Error(ex, "ViewDevice:" + device);
            }
        }
    }
}
