using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace UniTask_UI.common
{
    public class ChromeDriverUtil
    {
        static object lockobj = new object();
        static ChromeDriver chrome;
        public static string ExecuteScript(string file, string js)
        {
            var t = "";
            lock (lockobj)
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                if (chrome == null)
                {
                    ChromeDriverService driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;//关闭黑色cmd窗口

                    ChromeOptions options = new ChromeOptions();
                    options.AddExcludedArgument("enable-automation");
                    options.AddArgument("--headless");
                    options.AddAdditionalOption("useAutomationExtension", false);
                    // 禁用图片
                    options.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
                    chrome = new ChromeDriver(driverService, options);
                    chrome.Manage().Window.Size = new System.Drawing.Size(10, 10);
                }
                string url = "file://" + path + file;
                chrome.Navigate().GoToUrl(url);
                chrome.ExecuteScript(js);
                t = chrome.Title;
                //chrome.Close();
                //chrome = null;
            }
            return t;
        }

    }
}
