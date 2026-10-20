using System.IO;
using System.Reflection;
using EnvDTE;
using EnvDTE80;
using EnvDTE90;
using Microsoft.VisualStudio.CommandBars;
namespace Com.Zengfr.CodeGenerator.VsAddin.Common
{
    internal class DTEUtils
    {
        #region
        public static CommandBarButton CreateCodeWindowMenuItem(DTE2 _applicationObject, string name)
        {
            Microsoft.VisualStudio.CommandBars.CommandBar menuBarCommandBar =
                ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["Code Window"];

            Microsoft.VisualStudio.CommandBars.CommandBarPopup oPopup = null;
            try
            {
                oPopup = (CommandBarPopup)menuBarCommandBar.Controls["Addin"];
            }
            catch { }
            CommandBarPopup oPopup2;
            if (oPopup == null)
            {
                oPopup2 = (CommandBarPopup)
                menuBarCommandBar.Controls.Add(MsoControlType.msoControlPopup,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value, 1, true);
                oPopup2.Caption = "Addin";
                oPopup2.Enabled = true;
            }
            else
            {
                oPopup2 = oPopup;
            }
            CommandBarButton oControl = null;
            try
            {
                oControl = (CommandBarButton)oPopup2.Controls[name];
            }
            catch { }
            CommandBarButton oControl2;
            if (oControl == null)
            {
                oControl2 = (CommandBarButton)
                oPopup2.Controls.Add(MsoControlType.msoControlButton,
                System.Reflection.Missing.Value,
                System.Reflection.Missing.Value, 1, true);
                oControl2.Caption = name;
                oControl2.Enabled = true;
            }
            else
            {
                oControl2 = oControl;
            }

            return oControl2;

        }
        #endregion
        #region
        public static Window FindWindow(DTE2 _applicationObject, string guidpos)
        {
            Window newWinobj = null;
            try
            {
                var wins2obj = (Windows2)_applicationObject.Windows;
                newWinobj = wins2obj.Item(guidpos);
            }
            catch (System.Exception ex)
            {
                ShowError(ex);
            }
            return newWinobj;
        }
        public static Window CreateToolWindow<T>(AddIn _addInInstance, DTE2 _applicationObject, string caption, string guidpos)
        {
            var t = typeof(T);

            return CreateToolWindow(_addInInstance, _applicationObject, t.Assembly,
                  t.FullName, caption, guidpos);

        }
        public static Window CreateToolWindow(AddIn _addInInstance, DTE2 _applicationObject, Assembly assembly, string className, string caption, string guidpos)
        {
            EnvDTE80.Windows2 wins2obj;
            object ctlobj = null;

            // A toolwindow must be connected to an add-in, so this line 
            // references one.
            EnvDTE.AddIn addinobj = _addInInstance;
            wins2obj = (Windows2)_applicationObject.Windows;


            string assemblypath = assembly.Location;

            // Create the new tool window and insert the user control in it.
            Window newWinobj = null;
            try
            {
                newWinobj = wins2obj.Item(guidpos);
            }
            catch
            {
                try
                {
                    // the window does not exist, so create it
                    newWinobj = wins2obj.CreateToolWindow2(addinobj, assemblypath, className, caption, guidpos, ref ctlobj);
                    if (newWinobj != null)
                    {
                        newWinobj.AutoHides = false;
                        newWinobj.IsFloating = false;
                        newWinobj.Linkable = true;
                    }
                }
                catch (System.Exception ex)
                {

                }

            }

            finally
            {
                if (newWinobj != null)
                {
                    newWinobj.Visible = true;
                }
            }
            return newWinobj;
        }
        public static OutputWindowPane CreatePane(DTE2 _applicationObject, string paneName)
        {
            bool gotPane = false;
            OutputWindow outputWindow = null;
            outputWindow = (OutputWindow)_applicationObject.Windows.Item(EnvDTE.Constants.vsWindowKindOutput).Object;
            // Now get the panes
            OutputWindowPane outputPane = null;
            OutputWindowPane retPane = null;
            OutputWindowPanes panes = outputWindow.OutputWindowPanes;
            // Reuse the existing pane (if it exists)
            for (int i = 1; i <= panes.Count; i++)
            {
                outputPane = panes.Item(i);
                if (outputPane.Name == paneName)
                {
                    retPane = outputPane;
                    gotPane = true;
                }
            }
            // return from only one place
            if (!gotPane)
                retPane = outputWindow.OutputWindowPanes.Add(paneName);
            // Make the pane active
            retPane.Activate();
            return retPane;
        }
        #endregion

        #region
        public static string GetSlnDirectory(Solution3 sln)
        {
            if (sln != null && !string.IsNullOrEmpty(sln.FullName))
            {
                return Path.GetDirectoryName(sln.FullName);
            }
            return null;
        }
        public static string GetProjectDirectory(Solution3 sln, string projName)
        {
            Project p = GetProjectByName(sln, projName);
            string projDir = string.Empty;
            if (p != null)
            {
                projDir = p.FullName; p = null;
                return Path.GetDirectoryName(projDir);
            }
            return null;
        }
        public static Project GetProjectByName(Solution3 sln, string projName)
        {
            foreach (Project p in sln.Projects)
            {
                if (p.Name == projName)
                {
                    ShowInfo(string.Format("找到项目:{0}", p.Name));
                    return p;
                }
            }
            ShowError(string.Format("找不到项目:{0}", projName));
            return null;
        }
        #endregion

        #region
        public static void ShowInfo(string message)
        {
            ShowMessage("Info", message);
        }
        public static void ShowError(System.Exception ex)
        {
            ShowMessage("Error", string.Format("{0} {1}", ex.Message, ex.StackTrace));
            if (ex.InnerException != null)
            {
                ShowError(ex.InnerException);
            }
        }
        public static void ShowError(string message)
        {
            ShowMessage("Error", message);
        }
        private static void ShowMessage(string type, string message)
        {
            //if (VarUtils.OutputPane != null)
            //{
            //    VarUtils.OutputPane.OutputString(string.Format("{0}:{1,-5} {2}\r\n", System.DateTime.Now.ToString("mm:ss fff"), type, message));
            //}
        }
        #endregion

    }
}
