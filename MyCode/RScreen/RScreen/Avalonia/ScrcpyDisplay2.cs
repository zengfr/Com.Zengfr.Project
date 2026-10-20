using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting.Unicode;
using Avalonia.Platform;
using Avalonia.Rendering;
using Avalonia.Threading;
using Newtonsoft.Json.Linq;
using RScreen.Ex;
using Serilog;
using SharpAdbClient;
using System;
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using System.Timers;
using static Avalonia.AvaloniaLocator;
using AvaloniaPlatform = Avalonia.Platform;

using AVPoint = Avalonia.Point;

namespace ScrcpyNet.Avalonia
{
    public class ScrcpyDisplay2 : TemplatedControl
    {
        private static readonly ILogger log = Log.ForContext<ScrcpyDisplay2>();

        public static readonly DirectProperty<ScrcpyDisplay2, DeviceData?> DeviceProperty =
            AvaloniaProperty.RegisterDirect<ScrcpyDisplay2, DeviceData?>(
                nameof(DeviceData),
                o => o.Device,
                (o, v) => o.Device = v);

        public int RenderedFrames { get; private set; }

        private bool isTouching;
        private bool hasNewFrame;
        private DeviceData? device;
        private Scrcpy? scrcpy;
        private Image? renderTarget;
        private WriteableBitmap? bmp;
        DispatcherTimer _timer;
        static ScrcpyDisplay2()
        {
            FocusableProperty.OverrideDefaultValue(typeof(ScrcpyDisplay2), true);
        }

        public ScrcpyDisplay2()
        {

            // IAvaloniaDependencyResolver c = AvaloniaLocator.Current;
            // var  timer= AvaloniaLocator.RegistrationHelper<IRenderTimer>.ToSingleton<IRenderTimer>()
            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000 / 50), IsEnabled = false };
            _timer.Tick += (o, e) => RenderTick(TimeSpan.Zero);
            _timer.Start();
            // new UiThreadRenderTimer(60).Tick += RenderTick;
        }

        protected override void OnUnloaded(RoutedEventArgs e)
        {
            if (scrcpy != null)
                scrcpy.VideoStreamDecoder.OnFrame -= OnFrame;
            if (_timer != null)
            {
                _timer.IsEnabled = false;
                _timer.Stop();
            }
            device = null;
            bmp = null;
            base.OnUnloaded(e);

        }
        public DeviceData? Device
        {
            get
            {
                return device;
            }
            set
            {
                if (device == null && value == null)
                {
                    return;
                }
                var isChange = (device == null && value != null || value == null && device != null || device.Serial != value.Serial);

                log.Debug(String.Format("set device:{0},{1},{2} ", isChange, device, value));
                if (isChange)
                {
                    if (scrcpy != null)
                    {
                        if (device != null)
                            ScrcpyImgManager.Instance.Imgs[device.Serial] = bmp;
                        scrcpy.VideoStreamDecoder.OnFrame -= OnFrame;
                        _timer.IsEnabled = false;
                    }
                    SetAndRaise(DeviceProperty, ref device, value);

                    scrcpy = ScrcpyManager.Instance.GetScrcpy(device, true);
                   
                    if (scrcpy != null)
                    {
                        if (device != null)
                            bmp = ScrcpyImgManager.Instance.GetImg(device.Serial);
                        scrcpy.VideoStreamDecoder.OnFrame += OnFrame;
                        hasNewFrame = true;
                        _timer.IsEnabled = true;
                    }
                    else
                    {
                        bmp = null;
                        _timer.IsEnabled = false;
                    }
                    initRender();
                }
            }
        }
        private void initRender()
        {
           
            if (renderTarget != null)
            {
                Dispatcher.UIThread.Post(() => { renderTarget.Source = bmp; });
            }
        }
        private unsafe void OnFrame(object? sender, FrameData frameData)
        {
            if (scrcpy != null)
            {
                if (renderTarget != null && frameData != null)
                {
                    if (bmp == null || bmp.Size.Width != frameData.Width || bmp.Size.Height != frameData.Height)
                    {
                        bmp = new WriteableBitmap(new PixelSize(frameData.Width, frameData.Height),
                            new Vector(96, 96), AvaloniaPlatform.PixelFormat.Bgra8888, AvaloniaPlatform.AlphaFormat.Opaque);

                        Dispatcher.UIThread.Post(() => renderTarget.Source = bmp);
                    }

                    using (var l = bmp.Lock())
                    {
                        var dest = new Span<byte>(l.Address.ToPointer(), frameData.Data.Length);
                        frameData.Data.CopyTo(dest);


                      //  using (var g = System.Drawing.Graphics.FromImage(Image.)
                      //{
                      //    g.DrawString("F:"+ RenderedFrames, new Font("Tahoma", 14), System.Drawing.Brushes.Red, 0, 0);
                      // }
                    }

                    hasNewFrame = true;
                }
            }
        }
        //private static IPlatformRenderInterface GetFactory()
        //{
        //    return AvaloniaLocator.Current.GetRequiredService<IPlatformRenderInterface>();
        //}
        //public DrawingContext CreateDrawingContext()
        //{
        //   var  PlatformImpl = RefCountable.Create(GetFactory().LoadBitmap(stream));
        //    var platform = PlatformImpl.Item.CreateDrawingContext();
        //    platform.Clear(Colors.Transparent);
        //    return new PlatformDrawingContext(platform);
        //}
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            renderTarget = e.NameScope.Find<Image>("PART_RenderTargetImage");
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (scrcpy != null)
            {
                e.Handled = true;

                var msg = new KeycodeControlMessage();
                msg.KeyCode = KeycodeHelper.ConvertKey(e.Key);
                msg.Metastate = KeycodeHelper.ConvertModifiers(e.KeyModifiers);
                scrcpy.SendControlCommand(msg);
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (scrcpy != null)
            {
                e.Handled = true;

                var msg = new KeycodeControlMessage();
                msg.Action = AndroidKeyEventAction.AKEY_EVENT_ACTION_UP;
                msg.KeyCode = KeycodeHelper.ConvertKey(e.Key);
                msg.Metastate = KeycodeHelper.ConvertModifiers(e.KeyModifiers);
                scrcpy.SendControlCommand(msg);
            }

            base.OnKeyDown(e);
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (scrcpy != null && renderTarget != null)
            {
                var point = e.GetCurrentPoint(renderTarget);

                if (point.Properties.IsRightButtonPressed)
                {
                    e.Handled = true;
                    scrcpy.SendControlCommand(new BackOrScreenOnControlMessage() { Action = AndroidKeyEventAction.AKEY_EVENT_ACTION_DOWN });
                    scrcpy.SendControlCommand(new BackOrScreenOnControlMessage() { Action = AndroidKeyEventAction.AKEY_EVENT_ACTION_UP });
                }
                else if (point.Properties.IsLeftButtonPressed)
                {
                    e.Handled = true;
                    isTouching = true;
                    SendTouchCommand(AndroidMotionEventAction.AMOTION_EVENT_ACTION_DOWN, point.Position);
                }
            }

            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (scrcpy != null && renderTarget != null)
            {
                var point = e.GetCurrentPoint(renderTarget);

                if (isTouching)
                {
                    e.Handled = true;
                    isTouching = false;
                    SendTouchCommand(AndroidMotionEventAction.AMOTION_EVENT_ACTION_UP, point.Position);
                }
            }

            base.OnPointerReleased(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (scrcpy != null && renderTarget != null)
            {
                var point = e.GetCurrentPoint(renderTarget);

                if (isTouching && point.Position.X >= 0 && point.Position.Y >= 0)
                {
                    SendTouchCommand(AndroidMotionEventAction.AMOTION_EVENT_ACTION_MOVE, point.Position);
                }
            }

            base.OnPointerMoved(e);
        }

        protected void SendTouchCommand(AndroidMotionEventAction action, AVPoint position)
        {
            if (scrcpy != null && renderTarget != null)
            {
                var msg = new TouchEventControlMessage();
                msg.Action = action;
                msg.Position.Point.X = (int)position.X;
                msg.Position.Point.Y = (int)position.Y;
                msg.Position.ScreenSize.Width = (ushort)renderTarget.Bounds.Width;
                msg.Position.ScreenSize.Height = (ushort)renderTarget.Bounds.Height;
                TouchHelper.ScaleToScreenSize(msg.Position, scrcpy.Width, scrcpy.Height);
                scrcpy.SendControlCommand(msg);

                //log.Debug("Sending {Action} for position {PositionX}, {PositionY}", action, msg.Position.Point.X, msg.Position.Point.Y);
            }
        }

        private void RenderTick(TimeSpan obj)
        {
            if (hasNewFrame && renderTarget != null)
            {
                try
                {
                    // This sometimes crashes. Not sure if it's an Avalonia issue?
                    renderTarget.InvalidateVisual();
                    hasNewFrame = false;
                    RenderedFrames++;
                }
                catch
                {
                    Log.Error("Crash!");
                }
            }
        }

    }
}
