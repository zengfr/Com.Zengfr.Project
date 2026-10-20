using System;
using System.Text;
namespace Com.Zengfr.CodeGenerator.Common.Util
{
    public class FormatUtil 
    {

        private static FormatUtil instance;
        public static FormatUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FormatUtil();
                }
                return instance;
            }
        }

        #region  生成模板的辅助类
        /// <summary>
        /// 增加一行格式化字符串
        /// </summary>
        public void AppendFormatLine(StringBuilder code, int tabCount, string format, params object[] args)
        {
            for (int i = 0; i < tabCount; i++)
            {
                format = format.Insert(0, "\t");
            }
            if (args != null && args.Length > 0)
                code.AppendLine(string.Format(format, args));
            else
                code.AppendLine(format);
        }

        /// <summary>
        /// 增加格式化字符串
        /// </summary>
        public void AppendFormat(StringBuilder code, int tabCount, string format, params object[] args)
        {
            for (int i = 0; i < tabCount; i++)
            {
                format = format.Insert(0, "\t");
            }
            if (args != null && args.Length > 0)
                code.Append(string.Format(format, args));
            else
                code.Append(format);
        }



        #endregion

    }
}
