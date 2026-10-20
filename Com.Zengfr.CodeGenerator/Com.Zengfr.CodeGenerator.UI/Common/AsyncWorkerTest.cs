namespace Com.Zengfr.CodeGenerator.UI.Common
{
    using System.Windows.Forms;
    public partial class AsyncWorkerTest
    {


        public static void Test()
        {
            var control = new Button();
            // AsyncInvoker.BeginInvoke(control,DoWork, DoWorkC);

        }
        public static void DoWork()
        {
            AsyncInvoker.ReportProgress<object>(33, null, ProgressChanged);
        }
        public static void DoWorkC()
        {

        }
        public static void ProgressChanged(int vlaue,object arg)
        {
            //
        }


    }

}