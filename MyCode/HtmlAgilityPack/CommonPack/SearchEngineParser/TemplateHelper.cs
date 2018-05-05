using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ParserEngine
{
    class TemplateHelper
    {
        /// <summary>
        /// 循环过滤 OK
        /// </summary>
        /// <param name="value"></param>
        /// <param name="baseCell"></param>
        /// <returns></returns>
        public static string DoFilters(string input, IList<Replace> filters)
        {
            string value =input;
            foreach (Replace filter in filters)
            {
               value=DoFilters(value,filter);
            }
            return value;
        }
        private static string DoFilters(string input, Replace filter)
        {
            string value = string.Empty;
            value = Regex.Replace(input, filter.OldText, filter.NewText, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
            return value;
        }
        public static List<string> GetActionValue(string input, ListAction action)
        {
            List<string> objs = RegexHelper.GetList(input, action.ActionInput.Input, action.ActionInput.GroupIndex);
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i] = DoFilters(objs[i], action.Filters);
            }
            return objs;
        }
        public static string GetActionValue(string input, List<OneAction> actions)
        {
            string value = input;
            foreach (OneAction action in actions)
            {
                value = GetActionValue(input,action);
            }
            return value;
        }
        public static string GetActionValue(string input, OneAction action)
        {
            string  rtn=RegexHelper.GetOne(input, action.ActionInput.Input, action.ActionInput.GroupIndex,action.ListIndex);
            if(action.StripHTML)
               rtn = RegexHelper.StripHTML(rtn);
            rtn=DoFilters(rtn,action.Filters);
            return rtn;
        }
       
    }
}
