using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
//using MSScriptControl;
//using V8.Net;

namespace TaobaoTask
{
   public class JsUtil
    {
        public static string ExecuteScript(string sExpression, string sCode)
        {
            try
            {
                var square = new Engine()
        .Execute(""+ sCode+" "+ sExpression) // execute a statement
        .GetCompletionValue() // get the latest statement completion value
        .ToObject();
                string str = square.ToString();
                /*V8Engine engine = new V8Engine();
                var result = engine.Execute(sCode);
                string str = result.AsString;*/
                /* MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControlClass();
             scriptControl.UseSafeSubset = true;
                 ((DScriptControlSource_Event)scriptControl).Error +=
 new DScriptControlSource_ErrorEventHandler(ScriptEngine_Error);
                 scriptControl.Language = "javascript"; //"JScript";
             scriptControl.AddCode(sCode);

                 string str = scriptControl.Eval(sExpression).ToString();*/
                return str;
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return null;
        }

        private static void ScriptEngine_Error()
        {  
        }
    }
}
