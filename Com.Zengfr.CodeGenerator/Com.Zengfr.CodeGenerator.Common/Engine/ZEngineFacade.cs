using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Com.Zengfr.CodeGenerator.Common.Gen;
using Com.Zengfr.CodeGenerator.Common.Meta;
using Com.Zengfr.CodeGenerator.Common.Util;
using Common.Logging;
using Commons.Collections;
using NVelocity;
using NVelocity.App;
using NVelocity.App.Tools;
using NVelocity.Context;
using NVelocity.Exception;
using NVelocity.Runtime;

namespace Com.Zengfr.CodeGenerator.Common.Engine
{
    public class ZEngineFacade : IZEngineFacade
    {
        private static ILog logger = LogManager.GetLogger(typeof(ZEngineFacade));
        private static String encoding = "utf-8";
        private static ZEngineFacade instance;

        private ZEngine engine = null;
        private bool isInited = false;

        public static ZEngineFacade Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ZEngineFacade();
                }
                return instance;
            }
        }
        public void Init(string templateDir, bool isInWeb)
        {
            ExtendedProperties props = new ExtendedProperties();
            if (isInWeb)
                props = ZPropertiesFactory.Instance.Create4FileInWeb(templateDir);
            else
                props = ZPropertiesFactory.Instance.Create4File(templateDir);
            Init(props);
        }
        public void Init(ExtendedProperties props)
        {
            if (!this.isInited)
            {
                this.isInited = true;
                this.engine = new ZEngine();
                if (!props.ContainsKey(RuntimeConstants.INPUT_ENCODING))
                    props.AddProperty(RuntimeConstants.INPUT_ENCODING, encoding);
                if (!props.ContainsKey(RuntimeConstants.OUTPUT_ENCODING))
                    props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, encoding);
                if (!props.ContainsKey(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE))
                    props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_CACHE, true);
                if (!props.ContainsKey(RuntimeConstants.RESOURCE_MANAGER_DEFAULTCACHE_SIZE))
                    props.AddProperty(RuntimeConstants.RESOURCE_MANAGER_DEFAULTCACHE_SIZE, 99);
                props.AddProperty("file.resource.loader.modificationCheckInterval", 60L);
                this.engine.Init(props);

            }
        }
        public string Process(string templatFileName, IDictionary<String, object> kvs)
        {
            using (StringWriter writer = new StringWriter())
            {
                Process(writer, templatFileName, kvs);
                return writer.ToString();
            }
        }
        public void Process(TextWriter writer, string templatFileName, IDictionary<String, object> kvs)
        {
            if (this.engine == null)
            {
                writer.Write("engine null:" + templatFileName); return;
            }
            if (!this.engine.TemplateExists(templatFileName))
            {
                writer.Write("template not exists:" + templatFileName); return;
            }
            IContext context = new ZContext();
            context.Put("engine", this.engine);
            context.Put("context", context);
            context.Put("this", context);
            context.Put("params", kvs);
            if (kvs != null)
            {
                if (!kvs.ContainsKey("templatFileName"))
                    kvs.Add("templatFileName", templatFileName);
                if (!kvs.ContainsKey("formatter"))
                    kvs.Add("formatter", new VelocityFormatter(context));
                if (!kvs.ContainsKey("logger"))
                    kvs.Add("logger", logger);
                if (!kvs.ContainsKey("stringBuilder"))
                    kvs.Add("stringBuilder", new StringBuilder());
                if (!kvs.ContainsKey("now"))
                    kvs.Add("now", DateTime.Now);

                foreach (var kv in kvs)
                {
                    context.Put(kv.Key, kv.Value);
                }
            }
            try
            {
                Template template = this.engine.GetTemplate(templatFileName);
                template.Merge(context, writer);
            }
            catch (ParseErrorException pe)
            {
                writer.Write(pe.Message);
            }
            catch (MethodInvocationException mi)
            {
                writer.Write(string.Format("{0},{1},{2}", mi.ReferenceName, mi.MethodName, mi.Message));
            }
            catch (Exception ex)
            {
                writer.Write(ex.Message);
            }

        }
        public string GetOutputFilePath(IDictionary<string, object> kvs) {
            ZSysConfig sysConfig = kvs["sysConfig"] as ZSysConfig;
            ZOutputConfig outConfig = kvs["outputConfig"] as ZOutputConfig;

            string outputFile = PathUtil.Instance.GetFilePath(@"\", sysConfig.OutputRootDir, outConfig.OutputDir ?? string.Empty, outConfig.OutputFileName ?? string.Empty);
            outputFile = PathUtil.Instance.GetSafetyPath(outputFile);
            return outputFile;
        }
        public bool GetIsOutputSingleFile(IDictionary<string, object> kvs)
        {
            ZMetaModel model = kvs["model"] as ZMetaModel;
            string outputFile = GetOutputFilePath(kvs);
            string outputFileName = PathUtil.Instance.GetFileName(outputFile);
            var isSingleFile = !outputFileName.StartsWith("" + model.Name, StringComparison.CurrentCultureIgnoreCase);
            return isSingleFile;
        }
        public int SaveToFile(string content, IDictionary<string, object> kvs) {
            string outputFile = GetOutputFilePath(kvs);
            string outputFileName = PathUtil.Instance.GetFileName(outputFile);
            string outputFileExt = PathUtil.Instance.GetExtension(outputFile);
            var isSingleFile = GetIsOutputSingleFile(kvs);

            if (!String.IsNullOrWhiteSpace(outputFileName))
            {
                if (!Directory.Exists(Path.GetDirectoryName(outputFile)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                }
                using (FileStream fs = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                {
                    var enc = Encoding.GetEncoding(encoding);
                    if (encoding=="utf-8")
                        enc = new UTF8Encoding(false);
                    using (StreamWriter writer = new StreamWriter(fs, enc))
                    {
                        StringBuilder sb = new StringBuilder();
                        AppendFileHeader(outputFileExt, sb);
                        writer.Write(sb.ToString());
                        writer.WriteLine(content);
                        sb = null; 
                    }
                }
                return isSingleFile?1:2;
            }
            else
            {
                return -1;
            }
        }
        protected static void AppendFileHeader(string fileExtention, StringBuilder f)
        {
            FormatUtil formatUtil = FormatUtil.Instance;
            switch (fileExtention)
            {
                case ".java":
                    formatUtil.AppendFormatLine(f, 0, @"/*");
                    BuildFileHeader(f);
                    formatUtil.AppendFormatLine(f, 0, "*/");
                    break;
                case ".cs":
                    formatUtil.AppendFormatLine(f, 0, "");
                    BuildFileHeader(f);
                    formatUtil.AppendFormatLine(f, 0, "");
                    break;

                case ".xml":
                case ".config":
                    formatUtil.AppendFormatLine(f, 0, @"<?xml version=""1.0"" encoding=""utf-8"" ?>");
                    formatUtil.AppendFormatLine(f, 0, "<!--");
                    BuildFileHeader(f);
                    formatUtil.AppendFormatLine(f, 0, "-->");
                    break;
                case ".html":
                case ".js":
                case ".aspx":
                case ".svc":
                case ".asmx":
                case ".cmd":
                case ".bat":
                case ".properties":
                    break;

                default:
                    BuildFileHeader(f);
                    break;
            }
        }
        protected static void BuildFileHeader(StringBuilder f)
        {
            FormatUtil formatUtil = FormatUtil.Instance;
            formatUtil.AppendFormatLine(f, 0, "// 自动生成：生成器 zengfr");
            formatUtil.AppendFormatLine(f, 0, "// 创建标识:Copyright (C) 2014-{0} zengfr 版权所有", DateTime.Today.Year);
            formatUtil.AppendFormatLine(f, 0, "// 创建描述:");
            formatUtil.AppendFormatLine(f, 0, "// 创建时间:{0}", DateTime.Now);
            formatUtil.AppendFormatLine(f, 0, "// 功能描述:{0}", "");
            formatUtil.AppendFormatLine(f, 0, "// 修改标识: 无");
            formatUtil.AppendFormatLine(f, 0, "// 修改描述: 无");
            formatUtil.AppendFormatLine(f, 0, "//");
        }
    }
}
