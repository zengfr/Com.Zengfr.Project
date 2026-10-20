using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemConsole.UI
{
    class TabbedWebView : TabPage
    {
        ChromiumWebBrowser browser;

        public event Action<TabbedWebView> NeedCloseTab;
        public event Action<ChromiumWebBrowser, CfxOnLoadingStateChangeEventArgs> OnLoadingStateChange;
        public event Action<string> OnBeforePopup;

        public TabbedWebView(string address)
        : base()
        {
            browser = new ChromiumWebBrowser(address) { Dock = DockStyle.Fill };
            Controls.Add(browser);
            browser.DisplayHandler.OnConsoleMessage += DisplayHandler_OnConsoleMessage;
            browser.LoadHandler.OnLoadingStateChange += LoadHandler_OnLoadingStateChange;
            browser.ContextMenuHandler.OnBeforeContextMenu += ContextMenuHandler_OnBeforeContextMenu;
            browser.ContextMenuHandler.OnContextMenuCommand += ContextMenuHandler_OnContextMenuCommand;
            browser.LifeSpanHandler.OnBeforePopup += LifeSpanHandler_OnBeforePopup;
            browser.RequestHandler.OnOpenUrlfromTab += RequestHandler_OnOpenUrlfromTab;
            browser.JsDialogHandler.OnJsDialog += JsDialogHandler_OnJsDialog;
        }

        private void JsDialogHandler_OnJsDialog(object sender, CfxOnJsDialogEventArgs e)
        {
            e.SetReturnValue(true);
        }

        private void RequestHandler_OnOpenUrlfromTab(object sender, CfxOnOpenUrlfromTabEventArgs e)
        {
            e.SetReturnValue(true);
            OnBeforePopup?.Invoke(e.TargetUrl);
        }

        private void LifeSpanHandler_OnBeforePopup(object sender, CfxOnBeforePopupEventArgs e)
        {
            e.SetReturnValue(true);
            OnBeforePopup?.Invoke(e.TargetUrl);
        }

        private void ContextMenuHandler_OnContextMenuCommand(object sender, CfxOnContextMenuCommandEventArgs e)
        {
            if (26501 == e.CommandId)
            {
                CfxWindowInfo windowInfo = new CfxWindowInfo();

                windowInfo.Style = WindowStyle.WS_OVERLAPPEDWINDOW | WindowStyle.WS_CLIPCHILDREN | WindowStyle.WS_CLIPSIBLINGS | WindowStyle.WS_VISIBLE;
                windowInfo.ParentWindow = IntPtr.Zero;
                windowInfo.WindowName = "Dev Tools";
                windowInfo.X = 200;
                windowInfo.Y = 200;
                windowInfo.Width = 800;
                windowInfo.Height = 600;

                browser.BrowserHost.ShowDevTools(windowInfo, new CfxClient(), new CfxBrowserSettings(), null);
            }
        }

        private void ContextMenuHandler_OnBeforeContextMenu(object sender, CfxOnBeforeContextMenuEventArgs e)
        {
            e.Model.Clear();
            e.Model.AddItem(26501, "Show DevTools");
        }

        private void LoadHandler_OnLoadingStateChange(object sender, CfxOnLoadingStateChangeEventArgs e)
        {
            OnLoadingStateChange?.Invoke(browser, e);
        }

        private void DisplayHandler_OnConsoleMessage(object sender, CfxOnConsoleMessageEventArgs e)
        {
            if (e.Message == "action:windowclose")
            {
                NeedCloseTab?.Invoke(this);
            }
        }
    }

    public partial class UcWebBrowser : UserControl
    {
        public UcWebBrowser()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
        }

        public UcWebBrowser(string address)
            : this()
        {
            InitBrowser(address);
        }

        private void InitBrowser(string address)
        {
            AddTab(address);
        }

        public void AddTab(string address)
        {
            TabbedWebView tab = new TabbedWebView(address);
            tab.NeedCloseTab += UcWebBrowser_NeedCloseTab;
            tab.OnLoadingStateChange += Tab_OnLoadingStateChange;
            tab.OnBeforePopup += Tab_OnBeforePopup;
            tbMain.TabPages.Add(tab);
            tbMain.SelectedTab = tab;
        }

        private void Tab_OnBeforePopup(string url)
        {
            this.InvokeOnUiThreadIfRequired(() =>
            {
                AddTab(url);
            });
        }

        private void Tab_OnLoadingStateChange(ChromiumWebBrowser sender, CfxOnLoadingStateChangeEventArgs e)
        {
            lbStatus.SetText(e.IsLoading ? "Loading..." : sender.Url.ToString());
            if (!e.IsLoading)
            {

            }
        }

        private void UcWebBrowser_NeedCloseTab(TabbedWebView tab)
        {
            RemoveTab(tab);
        }

        public void RemoveTab(TabPage tab)
        {
            tab.Controls[0].Dispose();
            tab.Dispose();
            tbMain.TabPages.Remove(tab);
        }
    }
    public static class ControlExtensions
    {
        /// <summary>
        /// Executes the Action asynchronously on the UI thread, does not block execution on the calling thread.
        /// </summary>
        /// <param name="control">the control for which the update is required</param>
        /// <param name="action">action to be performed on the control</param>
        public static void InvokeOnUiThreadIfRequired(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

    }
}
