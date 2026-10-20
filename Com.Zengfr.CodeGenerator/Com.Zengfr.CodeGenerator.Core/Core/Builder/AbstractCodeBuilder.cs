using Com.Zengfr.CodeGenerator.Config;
using Com.Zengfr.CodeGenerator.Config.Impl;
using Com.Zengfr.CodeGenerator.Host;
using Com.Zengfr.CodeGenerator.Schema;
using Com.Zengfr.CodeGenerator.Common;
namespace Com.Zengfr.CodeGenerator.Builder
{
    using System.Collections.Generic;
    /// <summary>
    /// 代码生成
    /// </summary>
    public abstract class AbstractCodeBuilder 
    {
        public AbstractCodeBuilder()
        {

        }
       


        public void BuildFiles(AbstractConfig config)
        {
            var host = BuildHost(config);
            var templates = BuildTemplates(config);
            foreach (string template in templates)
            {
                if (!string.IsNullOrEmpty(template))
                {
                    host.TemplateFile = template;
                    Process(host);
                }
            }
        }
        protected abstract IList<string> BuildTemplates(AbstractConfig cfg);
        protected abstract FileBaseHost BuildHost(AbstractConfig cfg);


        private void Process(FileBaseHost fileBaseHost)
        {
            var host = fileBaseHost as AssemblyHost;
            if (host.AssemblySchema != null)
            {
                foreach (var type in host.AssemblySchema.Types)
                {
                    if (type != null)
                    {
                        host.TypeSchema = type;
                        host.FileName = type.Name;
                        BuildFile(host);
                        if (host.OutputSingleFile)
                        {
                            break;
                        }
                    }
                }
            }
        }
        protected void InitAssemblySchema(AssemblyHost host, AbstractConfig cfg)
        {
            var config = cfg as AssemblyConfig;

            if (config != null && config.Assembly != null)
            {
                host.AssemblySchema = SchemaBuilder.BuildAssemblySchema(config.Assembly, true);
            }
            else
            {
                var config2 = cfg as DataAbstractConfig;
                if (config2.DatabaseSchema != null)
                {
                    host.AssemblySchema = SchemaBuilder.BuildAssemblySchema(config2.DatabaseSchema, config2.TablePrefix,config2.NamespacePrefix, config2.Tables, config2.Views);
                }
            }

        }

      

        private void BuildFile(FileBaseHost host)
        {
            string file, content;
            content = BuilderFactory.BuildOutputFileConent(host);
            file = BuilderFactory.BuildOutputFileName(host, false);
            CommonLogger.ShowInfo(string.Format("Write->File:{0}", file));
            BuilderFactory.WriteFile(file, content);
            file = null; content = null;
        }


    }
}
