using System.ComponentModel;
namespace Com.Zengfr.CodeGenerator.UI.Common
{
    using System;
    using System.Threading;
    using System.Windows.Forms;
    public interface IAsyncControl
    {
        IAsyncResult BeginInvoke(Delegate method);

        IAsyncResult BeginInvoke(Delegate method, params object[] args);
    }
    public partial class AsyncInvoker
    {
        public static void BeginInvoke(Control control, Action func, Action action)
        {
            func.BeginInvoke((result) =>
            {
                func.EndInvoke(result);
                if (action != null)
                {
                    control.BeginInvoke(action);
                }
            }, null);
        }
        public static void BeginInvoke<R>(Control control, Func<R> func, Action<R> action)
        {
            func.BeginInvoke((result) =>
            {
                R ret = func.EndInvoke(result);
                if (action != null)
                {
                    control.BeginInvoke(action, ret);
                }
            }, null);
        }
        public static void BeginInvoke<I, R>(Control control, I arg, Func<I, R> func, Action<R> action)
        {
            func.BeginInvoke(arg, (result) =>
            {
                R ret = func.EndInvoke(result);
                if (action != null)
                {
                    control.BeginInvoke(action, ret);
                }
            }, null);
        }
    }
    public partial class AsyncInvoker
    {
        public static void BeginInvoke(IAsyncControl control, Action func, Action action)
        {
            func.BeginInvoke((result) =>
            {
                func.EndInvoke(result);
                if (action != null)
                {
                    control.BeginInvoke(action);
                }
            }, null);
        }
        public static void BeginInvoke<R>(IAsyncControl control, Func<R> func, Action<R> action)
        {
            func.BeginInvoke((result) =>
            {
                R ret = func.EndInvoke(result);
                if (action != null)
                {
                    control.BeginInvoke(action, ret);
                }
            }, null);
        }
        public static void BeginInvoke<I, R>(IAsyncControl control, I arg, Func<I, R> func, Action<R> action)
        {
            func.BeginInvoke(arg, (result) =>
            {
                R ret = func.EndInvoke(result);
                if (action != null)
                {
                    control.BeginInvoke(action, ret);
                }
            }, null);
        }

        public static void ReportProgress<T>(int percentProgress, T userState, Action<int, T> action)
        {
            if (action != null)
            {
                var operation = AsyncOperationManager.CreateOperation(null);
                var progressReporter = new SendOrPostCallback(delegate(object args)
                {
                    var p = args as ProgressChangedEventArgs;
                    action.BeginInvoke(p.ProgressPercentage, (T)p.UserState, null, null);
                }
               );
                operation.Post(progressReporter, new ProgressChangedEventArgs(percentProgress, userState));
            }
        }
    }


}
