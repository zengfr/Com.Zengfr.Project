
namespace Com.Zengfr.CodeGenerator.Common
{
    public class CommonLogger
    {
        public static event System.Action<string> Logging;

        public static void ShowInfo(string message)
        {
            if (Logging != null)
            {
                Logging(message);
                // Logging(string.Format("{0}:{1,-5} {2}\r\n", System.DateTime.Now.ToString("mm:ss fff"), "Info", message));
            }
        }
    }
}
