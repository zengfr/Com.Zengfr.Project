using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FFmpeg.AutoGen;
using RScreen.Ex;
using ScrcpyNet.Sample.ViewModels;
using System;

namespace ScrcpyNet.Sample.Avalonia
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (DataContext is MainWindowViewModel vm)
            {
                foreach (var d in vm.DeviceViews)
                {
                    d.UnBindDeviceCommand.Execute().Subscribe();
                }
                ScrcpyManager.Instance.StopAllScrcpy();
            }
        }
    }
}
