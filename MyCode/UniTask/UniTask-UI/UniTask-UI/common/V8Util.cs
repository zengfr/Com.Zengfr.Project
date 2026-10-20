
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniTask_UI.common
{
    internal class V8Util
    {
        public static string ExecuteScript(string sExpression, string sCode)
        {
            using (var engine = new V8ScriptEngine())
            {
                engine.DocumentSettings.AccessFlags = Microsoft.ClearScript.DocumentAccessFlags.EnableFileLoading;
                engine.DefaultAccess = Microsoft.ClearScript.ScriptAccess.Full; // 这两行是为了允许加载js文件

                string scriptContent = string.Empty;
                //加载js文件
                //using (FileStream fs = new FileStream(ScriptFilePath, FileMode.Open, FileAccess.Read))
                //{
                //    using (StreamReader sr = new StreamReader(fs))
                //    {
                //        scriptContent = sr.ReadToEnd().Replace("\r\n", "");
                //    }
                //}
                scriptContent = sCode;
                engine.Execute(scriptContent);  // 取得脚本里的所有内容，Execute一下，然后，调用engine.Script.func(x,y)执行一下。

                var result = engine.Invoke(sExpression);
                return ""+result;
            }
        }
    }
}
