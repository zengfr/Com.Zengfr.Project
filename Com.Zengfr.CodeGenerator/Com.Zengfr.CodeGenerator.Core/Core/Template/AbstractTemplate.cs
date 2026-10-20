
namespace Com.Zengfr.CodeGenerator.Template
{
    using Com.Zengfr.CodeGenerator.Builder;
    using Com.Zengfr.CodeGenerator.Host;
    using Com.Zengfr.CodeGenerator.Schema;

    public abstract class AbstractTemplate : ITemplate
    {
        public virtual string OutputRootDir { set; get; }
        public virtual string NamespacePrefix { get; set; }

        public virtual string ProjectName { set; get; }
        public abstract string TemplateFile { set; get; }
        public virtual bool GenImplementEmptyFile { set; get; }
        #region
        public event System.Action<string, string> WriteFile;
        #endregion





        #region ITemplateBase Members

        // private AssemblyHost Host { get; set; }
        public void Process(AssemblySchema assemblySchema)
        {
            //if (this.Host == null)
            //{
            //    this.Host = new AssemblyHost();
            //}
            var host = new AssemblyHost();
            if (!string.IsNullOrEmpty(this.TemplateFile))
            {
                CopyProperties(this, host);
                host.AssemblySchema = assemblySchema;
                foreach (var sourceType in assemblySchema.Types)
                {
                    host.TypeSchema = sourceType;
                    host.FileName = sourceType.Name;
                    BuildFile(host);
                    if (host.OutputSingleFile)
                    {
                        break;
                    }
                }
                //输出
                this.ProjectName = host.ProjectName;
            }
        }
        /// <summary>
        /// 生成文件以及内容
        /// </summary>
        /// <param name="host"></param>
        private void BuildFile(FileBaseHost host)
        {
            string file, content;
            content = BuilderFactory.BuildOutputFileConent(host);
            //生成文件
            file = BuilderFactory.BuildOutputFileName(host, false);
            BuilderFactory.WriteFile(file, content);
            //
            if (host.GenImplementEmptyFile)
            {
                file = BuilderFactory.BuildOutputFileName(host, true);
                var regexOptions = System.Text.RegularExpressions.RegexOptions.IgnoreCase |
                    System.Text.RegularExpressions.RegexOptions.Singleline |
                    System.Text.RegularExpressions.RegexOptions.Compiled;

                if (System.Text.RegularExpressions.Regex.IsMatch(content, "{body}(.*?){/body}", regexOptions))
                {
                    content = System.Text.RegularExpressions.Regex.Replace(content, "{body}(.*?){/body}", "", regexOptions);
                }
                else
                {
                    content = string.Empty;
                }
                BuilderFactory.WriteFile(file, content);
            }
            file = null; content = null;
        }
        private void CopyProperties(object source, object target)
        {
            Common.ConvertUtils.CopyProperties(source, target);
        }
        #endregion







    }
}
