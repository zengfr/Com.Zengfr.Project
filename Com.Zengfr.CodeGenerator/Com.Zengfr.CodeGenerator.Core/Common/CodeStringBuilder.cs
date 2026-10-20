using System;
using System.Text;
namespace Com.Zengfr.CodeGenerator.Common
{
    public class CodeStringBuilder : IDisposable
    {

        protected StringBuilder StringBuilder = new StringBuilder();

        public virtual string Content
        {
            get { return StringBuilder.ToString(); }
        }
        public virtual void AppendFormatLine(int tabCount, string format, params object[] args)
        {
            AppendFormatLine(StringBuilder, tabCount, format, args);
        }
        public virtual void AppendFormat(int tabCount, string format, params object[] args)
        {
            AppendFormat(StringBuilder, tabCount, format, args);
        }


        #region IDisposable 成员

        public void Dispose()
        {
            if (StringBuilder != null)
            {
                StringBuilder.Clear();
            }
        }

        #endregion

        #region IDisposable 成员

        void IDisposable.Dispose()
        {
            if (StringBuilder != null)
            {
                StringBuilder.Clear();
            }
        }

        #endregion


        #region  生成模板的辅助类
        /// <summary>
        /// 增加一行格式化字符串
        /// </summary>
        public static void AppendFormatLine(StringBuilder code, int tabCount, string format, params object[] args)
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
        public static void AppendFormat(StringBuilder code, int tabCount, string format, params object[] args)
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
