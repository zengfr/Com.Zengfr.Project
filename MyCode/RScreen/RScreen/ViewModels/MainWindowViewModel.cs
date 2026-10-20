using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Serilog;
using SharpAdbClient;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using RScreen.Ex;
using System.Net;
using Avalonia.Threading;
using System.Linq;

namespace ScrcpyNet.Sample.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> RefreshDevicesCommand { get; }

        public ReactiveCommand<Unit, Unit> RebootDevicesCommand { get; }
        public ReactiveCommand<Unit, Unit> WakeUpDevicesCommand { get; }
        public ReactiveCommand<Unit, Unit> MenuDevicesCommand { get; }
        public ReactiveCommand<Unit, Unit> AppDevicesCommand { get; }
        public ReactiveCommand<Unit, Unit> HomeDevicesCommand { get; }
        public ReactiveCommand<Unit, Unit> BackDevicesCommand { get; }

        public ObservableCollectionExtended<DeviceViewModel> DeviceViews { get; } = new ObservableCollectionExtended<DeviceViewModel>();
        public ObservableCollectionExtended<DeviceData> AvailableDevices { get; } = new ObservableCollectionExtended<DeviceData>();


        private static readonly ILogger log = Log.ForContext<MainWindowViewModel>();


        [Reactive] public ScreenCfg ScreenCfg { get; private set; }
        [Reactive] public int ItemMinHeight { get; private set; }
        [Reactive] public int ItemMinWidth { get; private  set; }
        static bool init = false;
        int DeviceMaxCount { get; set; } = 40;
        public MainWindowViewModel()
        {
            ScreenCfg = ScreenCfg.Instance;
            InitViews();
           
            RebootDevicesCommand = ReactiveCommand.Create(RebootAllDevice);

            WakeUpDevicesCommand= ReactiveCommand.Create(WakeUpAllDevice);
            HomeDevicesCommand = ReactiveCommand.Create(HomeAllDevice);
            MenuDevicesCommand = ReactiveCommand.Create(MenuAllDevice);
            AppDevicesCommand = ReactiveCommand.Create(AppAllDevice);
            BackDevicesCommand = ReactiveCommand.Create(BackAllDevice);

            RefreshDevicesCommand = ReactiveCommand.Create(LoadAvailableDevices);

            Task.Run(async () =>
            {
                // Start ADB server if needed
                var srv = new AdbServer();
                if (!srv.GetStatus().IsRunning)
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    {
                        srv.StartServer("ScrcpyNet/adb.exe", false);
                    }
                    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    {
                        srv.StartServer("/usr/bin/adb", false);
                    }
                    else
                    {
                        log.Warning("Can't automatically start the ADB server on this platform.");
                    }
                }
                var monitor = new DeviceMonitor(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)));
                monitor.DeviceConnected += OnDeviceConnected;
                monitor.DeviceDisconnected += OnDeviceDisconnected;
                monitor.Start();

                //await LoadAvailableDevicesCommand.Execute();

                 RefreshDevicesCommand.Execute().Subscribe();
            });

        }

        private void AppAllDevice()
        {
            keyevent(187);
        }

        private void BackAllDevice()
        {
            keyevent(4);
        }

        private void MenuAllDevice()
        {
            keyevent(82);
        }
        private void HomeAllDevice()
        {
            keyevent(3);
        }

        private void WakeUpAllDevice()
        {
            keyevent(224);
        }
        private void keyevent(int keyeventCode)
        {
            var client = new AdbClient();
            var ss = client.GetDevices().OrderBy(t => t.Serial);
            var ct  = CancellationToken.None;
            foreach (var s in ss)
            {
                try

                {
                   // AKEYCODE_APP_SWITCH 187
                        //back 4
                     //app 284
                     //POWER 26
                     // KEYCODE_MENU = 82
                     // KEYCODE_HOME = 3
                     // KEYCODE_WAKEUP = 224
                    client.ExecuteRemoteCommandAsync("input keyevent " + keyeventCode, s, null, ct);
                }
                catch (Exception ex)
                {
                    log.Error(ex, "keyevent:" + s);
                }
            }
        }
            private void RebootAllDevice()
        {
            var client = new AdbClient();
            var ss = client.GetDevices().OrderBy(t => t.Serial);
            foreach (var s in ss)
            {
                try
                {
                    client.Reboot(s); 
                }
                catch (Exception ex)
                {
                    log.Error(ex,"RebootDevice:" + s);
                }
            }
        }

        void OnDeviceConnected(object? sender, DeviceDataEventArgs e)
        {
            if (init)
            {
                log.Information(String.Format("Connected:{0},{1}", e.Device.State, e.Device.Serial));
                ScrcpyViewManager.Instance.GetView(e.Device, true);
                ShowAvailableDevices();
                ShowDevices();
            }

        }
        void OnDeviceDisconnected(object? sender, DeviceDataEventArgs e)
        {
            if (init)
            {
                ScrcpyViewManager.Instance.RemoveView(e.Device, true);
                ScrcpyManager.Instance.StopScrcpy(e.Device.Serial);
                ShowAvailableDevices();
                ShowDevices();
            }
        }

        private void ShowDevices()
        {
            log.Information("ShowDevices:start");
            var dvs = ScrcpyViewManager.Instance.Views.Values.OrderBy(t => t.Serial).ThenBy(t => t.Idx);
            var dvs2 = new List<DeviceViewModel>();
            dvs2.AddRange(dvs);

            int j = dvs2.Count;
            DeviceMaxCount = ScreenCfg.DisplySize * 20;
            for (int i = 0; i < DeviceMaxCount; i++)
            {
                if (i >= j)
                {
                    var dv = new DeviceViewModel();
                    dvs2.Add(dv);
                }
                dvs2[i].Idx = i + 1;
            }
            Dispatcher.UIThread.Post(() =>
            {
                ItemMinHeight = ScreenCfg.MinHeight;
                ItemMinWidth = ScreenCfg.MinWidth;
                DeviceViews.Load(dvs2);
                log.Information("ShowDevices:post end");
            });

        }
        private void ShowAvailableDevices()
        {
            var ads = new AdbClient().GetDevices().OrderBy(t => t.Serial);
            AvailableDevices.Load(ads);
        }
        private void LoadAvailableDevices()
        {
            ShowAvailableDevices();
            InitDeviceViews();
        }
        public void InitViews()
        {
            ItemMinHeight = ScreenCfg.MinHeight;
            ItemMinWidth = ScreenCfg.MinWidth;
            var dvs2 = new List<DeviceViewModel>();
            for (int j = 0; j < DeviceMaxCount; j++)
            {
                var dv = new DeviceViewModel();
                dv.Idx = j + 1;
                dvs2.Add(dv);
            }
            //Dispatcher.UIThread.Post(() =>
           // {
                DeviceViews.Load(dvs2);
           // });
        }
        private void InitDeviceViews()
        {
            Task.Run(async () =>
            {
                try
                {
                    log.Information("InitDeviceViews:start");
                    init = false;
                    foreach (var ad in AvailableDevices)
                    {
                        ScrcpyViewManager.Instance.GetView(ad, true);
                    }
                    log.Information("InitDeviceViews:end");
                    ShowDevices();
                    init = true;
                }
                catch (Exception ex)
                {
                    log.Error(ex,"InitDevices");
                }
            });
        }

    }
}
