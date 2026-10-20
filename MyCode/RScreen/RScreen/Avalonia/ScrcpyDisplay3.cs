using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting.Unicode;
using Avalonia.Rendering;
using Avalonia.Threading;
using DynamicData.Binding;
using Newtonsoft.Json.Linq;
using RScreen.Ex;
using ScrcpyNet.Sample.ViewModels;
using Serilog;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Timers;
using static Avalonia.AvaloniaLocator;
using AvaloniaPlatform = Avalonia.Platform;

using AVPoint = Avalonia.Point;

namespace ScrcpyNet.Avalonia
{
    public class ScrcpyDisplay3 : TemplatedControl
    {
        private static readonly ILogger log = Log.ForContext<ScrcpyDisplay3>();

        public static readonly DirectProperty<ScrcpyDisplay3, IList<DeviceViewModel>> DeviceViewsProperty =
            AvaloniaProperty.RegisterDirect<ScrcpyDisplay3, IList<DeviceViewModel>>(
                nameof(DeviceViews),
                o => o.DeviceViews,
        (o, v) => o.DeviceViews = v);

        public IList<DeviceViewModel> DeviceViews { get; set; }


        static ScrcpyDisplay3()
        {
            FocusableProperty.OverrideDefaultValue(typeof(ScrcpyDisplay3), true);
        }

        public ScrcpyDisplay3()
        {


        }

        protected override void OnUnloaded(RoutedEventArgs e)
        {

            base.OnUnloaded(e);

        }
        public void stack_DoubleClick(object sender, TappedEventArgs e)
        {

        }
    }
}
