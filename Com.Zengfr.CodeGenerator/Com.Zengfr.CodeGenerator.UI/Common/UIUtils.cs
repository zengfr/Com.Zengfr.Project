using System.Reflection;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;
namespace Com.Zengfr.CodeGenerator.UI.Common
{

   
    public interface IMyDockContent
    {
        void Bind(object taget);
        bool Visible { get; set; }

    }
    public class UIUtils
    {
        public static IMyDockContent FindMyDockContent(DockPanel dockPanel, string name)
        {
            return (IMyDockContent)FindDockWindow(dockPanel, name);
        }
        public static DockContent FindDockWindow(DockPanel dockPanel, string name)
        {
            if (dockPanel != null)
            {
                foreach (DockContent d in dockPanel.Contents)
                {
                    if (d.Name == name)
                    {
                        return d;
                    }
                }
            }
            return null;

        }
        private static void ShowMessage(DockPanel dockPanel, string type, string message)
        {
            IMyDockContent d = FindMyDockContent(dockPanel, "OutputDockContent");
            if (d != null)
            {
                if (!d.Visible)
                {
                    d.Visible = true;
                }
                d.Bind(string.Format("{0}:{1,-5} {2}\r\n", System.DateTime.Now.ToString("mm:ss fff"), type, message));
            }
        }
        #region
        public static void ShowInfo(DockPanel dockPanel, string message)
        {
            ShowMessage(dockPanel, "Info", message);
        }
        public static void ShowError(DockPanel dockPanel, System.Exception ex)
        {
            ShowMessage(dockPanel, "Error", BuildError(ex));
        }
        private static string BuildError(System.Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} {1}", ex.Message, ex.StackTrace));
            if (ex.InnerException != null)
            {
                sb.AppendLine(BuildError(ex.InnerException));
            }
            return sb.ToString();
        }


        #endregion
        public static object GetPropertyValue(object obj, string propertyName)
        {
            if (obj != null && !string.IsNullOrEmpty(propertyName))
            {
                System.Reflection.PropertyInfo property = obj.GetType().GetProperty(propertyName);
                if (property != null)
                {
                    return property.GetValue(obj, null);
                }
            }
            return null;
        }
        public static object GetFieldValue(object obj, string fieldName)
        {
            if (obj != null && !string.IsNullOrEmpty(fieldName))
            {
                System.Reflection.FieldInfo fieldInfo = obj.GetType().GetField(fieldName, BindingFlags.Default
                | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null)
                {
                    return fieldInfo.GetValue(obj);
                }
            }
            return null;
        }
    }
}
