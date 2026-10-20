using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using Com.Zengfr.CodeGenerator.Common;
namespace Com.Zengfr.CodeGenerator.Host
{
    public class T4Engine
    {
        public static object sycObject = new object();
        static Microsoft.VisualStudio.TextTemplating.Engine engine;
        static System.Resources.ResourceManager resourceManager;
        /// <summary>
        /// 支持文件和资源文件
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static string ProcessTemplate(FileBaseHost host)
        {
            lock (sycObject)
            {
                if (engine == null)
                {
                    engine = new Microsoft.VisualStudio.TextTemplating.Engine();
                }
                if (resourceManager == null)
                {
                    resourceManager = new System.Resources.
                    ResourceManager("Com.Zengfr.CodeGenerator.template", typeof(T4Engine).Assembly);
                }
                string codeContent = string.Empty;
                var templateContent = string.Empty;
                if (File.Exists(host.TemplateFile))
                {
                    templateContent = File.ReadAllText(host.TemplateFile);
                }
                else
                {
                    templateContent = resourceManager.GetString(Path.GetFileNameWithoutExtension(host.TemplateFile),
                         System.Globalization.CultureInfo.CurrentCulture
                        );
                }
                if (!string.IsNullOrEmpty(templateContent))
                {
                    try
                    {
                        codeContent = engine.ProcessTemplate(templateContent, host);
                    }
                    catch (Exception ex)
                    {
                        ;//
                    }
                    if (host.ErrorCollection.HasErrors)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (CompilerError compilerError in host.ErrorCollection)
                        {
                            stringBuilder.AppendLine(compilerError.ToString());
                        }
                        codeContent += string.Format("{0}{1}", Environment.NewLine, stringBuilder.ToString());
                        CommonLogger.ShowInfo(string.Format("Template:{0}->Errors:{1}", host.TemplateFile, stringBuilder.ToString()));
                        stringBuilder.Clear();
                        stringBuilder = null;
                    }
                }
                using (CodeStringBuilder f = new CodeStringBuilder())
                {
                    AppendFileHeader(host.FileExtention, f);
                    f.AppendFormatLine(0, "{0}", codeContent);
                    return f.Content;
                }
            }
        }
        protected static void AppendFileHeader(string fileExtention, CodeStringBuilder f)
        {
            switch (fileExtention)
            {
                case ".cs":
                    f.AppendFormatLine(0, "");
                    BuildFileHeader(f);
                    f.AppendFormatLine(0, "");
                    break;

                case ".xml":
                case ".config":
                    f.AppendFormatLine(0, @"<?xml version=""1.0"" encoding=""utf-8"" ?>");
                    f.AppendFormatLine(0, "<!--");
                    BuildFileHeader(f);
                    f.AppendFormatLine(0, "-->");
                    break;
                case ".html":
                case ".js":
                case ".aspx":
                case ".svc":
                case ".asmx":
                    break;

                default:
                    BuildFileHeader(f);
                    break;
            }
        }
        protected static void BuildFileHeader(CodeStringBuilder f)
        {
            f.AppendFormatLine(0, "//");
            f.AppendFormatLine(0, "// 创建标识:Copyright (C) 2014-{0} zengfr 版权所有", DateTime.Today.Year);
            f.AppendFormatLine(0, "// 创建描述:");
            f.AppendFormatLine(0, "// 创建时间:{0}", DateTime.Now);
            f.AppendFormatLine(0, "// 功能描述:{0}", "");
            f.AppendFormatLine(0, "// 修改标识: 无");
            f.AppendFormatLine(0, "// 修改描述: 无");
            f.AppendFormatLine(0, "//");
        }
    }
}
