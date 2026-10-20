using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniTask_UI.Properties;
//using DeviceManager.Properties;

namespace DeviceManager
{
    static class Program2
    {
        static string scrCpyPath= "E:\\tools\\scrcpy-win64\\";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
       public static void Main2()
        {
            RestoreConfig();
            SetScrPath();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var menu = new ContextMenu(); 
            menu.Popup += (sender, args) =>
            {
                var client = new AdbManager( scrCpyPath);

                menu.MenuItems.Clear();
                menu.MenuItems.Add(new MenuItem("Set scrcpy path", (o, eventArgs) => { ChooseScrPath(); }));

                try
                {
                    var devices = client.GetDevices();

                    if (CanViewDevices())
                    {
                        var viewMenu = devices.Select(device =>
                                new MenuItem(device.Model + " - " + device.Serial, (o, _) => { ViewDevice(device, false); }))
                            .ToArray();

                        menu.MenuItems.Add(new MenuItem("View", viewMenu));
                        menu.MenuItems.Add(new MenuItem("View all", (o, _) =>
                        {
                            int c = 4;
                            int w = Screen.PrimaryScreen.WorkingArea.Width / 10;
                            int h= Screen.PrimaryScreen.WorkingArea.Height / 4;
                            int port = 27183;
                            foreach (var deviceData in devices)
                            {
                                Thread.Sleep(1500);
                                client.WakeUp(deviceData);
                                for (int i = 0; i < c; i++) {
                                    Thread.Sleep(1500);
                                    int x = w * ( (int)(i % 10));
                                    int y = h *((int)(i / 10));
                                    ViewDevice(deviceData, false,port,6*100*1000, w, h,x,y);
                                    port += 2;
                                }
                            }
                        }));
                        menu.MenuItems.Add(new MenuItem("View all in mirror mode", (o, _) =>
                        {
                            foreach (var deviceData in devices)
                            {
                                Thread.Sleep(1500);
                                ViewDevice(deviceData, true);
                            }
                        }));
                    }

                    var restartMenu = devices.Select(device => new MenuItem(device.Model + " - " + device.Serial,
                        (o, eventArgs) => { RestartAction(client, device); })).ToArray();

                    menu.MenuItems.Add(new MenuItem("Restart", restartMenu));
                }
                catch
                {
                    TryRestartAdb();

                    menu.MenuItems.Add(new MenuItem("Cannot connect to adb server", (o, eventArgs) => { }));
                }
                finally
                {
                    menu.MenuItems.Add(new MenuItem("Exit", (o, eventArgs) => { Application.Exit(); }));
                }

            };

            // Show the system tray icon.

            using (var pi = new ProcessIcon(menu))
            {
                pi.Display();
                // Make sure the application runs!
                Application.Run();
            }
            
        }

        private static void TryRestartAdb()
        {
            try
            {
                var adb = Path.Combine(scrCpyPath, "adb.exe");
                if (File.Exists(adb))
                {
                    var process = Process.Start(new ProcessStartInfo(adb, "reconnect")
                    {
                        UseShellExecute = true,
                        RedirectStandardOutput = true
                    });

                    process.OutputDataReceived += (sender, args) => { Debug.WriteLine(args.Data); };
                    process.WaitForExit();
                }
            }
            catch
            {
                /* om om om */
            }
        }

        private static void SetScrPath()
        {
            if (File.Exists("scrcpy-noconsole.exe") && string.IsNullOrEmpty(scrCpyPath))
            {
                //Settings.Default.ScrCpyPath = Directory.GetCurrentDirectory();
                //Settings.Default.Save();
            }
        }

        private static void RestartAction(AdbManager client, DeviceData device)
        {
            client.Reboot(device);

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    var targetDevice = client.GetDevices().SingleOrDefault(d => d.Serial == device.Serial);

                    if (targetDevice != null && targetDevice.State == DeviceState.Online)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5));
                        ViewDevice(targetDevice, false);
                        return;
                    }
                }
            });
        }

        private static void ChooseScrPath()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (!string.IsNullOrEmpty(scrCpyPath))
                {
                    fbd.SelectedPath = scrCpyPath;
                }

                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    if (File.Exists(Path.Combine(fbd.SelectedPath, "scrcpy.exe")))
                    {
                       // Settings.Default.ScrCpyPath = fbd.SelectedPath;
                      //  Settings.Default.Save();
                    }
                }
            }
        }

        private static bool CanViewDevices()
        {
            var scr = Path.Combine(scrCpyPath, "scrcpy.exe");
            return File.Exists(scr);
        }

        private static void Log(string message)
        {
            File.AppendAllLines("log.log", new [] { message});
        }

        private static void ViewDevice(DeviceData device, bool mirror,int port=27183, int? bitrate = null, int w = 200, int h = 100, int wx=0,int wy=0)
        {

            var scr = Path.Combine(scrCpyPath, "scrcpy.exe");

            var args = new[]
            {
                mirror ? "--no-control" : "","-p "+port,
                "--disable-screensaver","--stay-awake","--max-fps 54","-m 580",
                "-s " + device.Serial,
                "--window-title \"" + device.Model + " - " + device.Serial+"-"+wx+"-"+wy + "\"",
                bitrate == null ? "" : "-b " + bitrate,
                "--window-x "+wx,"--window-y "+wy,
                "--window-width "+w,
                "--window-height "+h,
            };

            var arguments = string.Join(" ", args.Where(x => !string.IsNullOrWhiteSpace(x)));

            var processStartInfo = new ProcessStartInfo(scr, arguments)
            {  CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden,
               UseShellExecute = false,//true //zfr
                WorkingDirectory =scrCpyPath
            };

            Log($"ViewDevice: {device.Model} - {device.Serial}. Args: {arguments}");

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

        private static void RestoreConfig()
        {
            var config = Application.ExecutablePath + ".config";
            if (File.Exists(config)) return;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DeviceManager.App.config";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null) return;

                using (var file = File.Create(config))
                {
                    stream.CopyTo(file);
                }
            }
        }
    }
    public class ProcessIcon : IDisposable
    {
        public NotifyIcon Icon { get; set; }

        public ProcessIcon(ContextMenu menuStrip)
        {
            Icon = new NotifyIcon
            {
                Icon = Resources.TrayIcon,
                ContextMenu = menuStrip
            };
        }

        public void Display()
        {
            Icon.Text = @"Android device manager";
            Icon.Visible = true;
        }

        public void Dispose()
        {
            Icon.Dispose();
        }
    }

}
