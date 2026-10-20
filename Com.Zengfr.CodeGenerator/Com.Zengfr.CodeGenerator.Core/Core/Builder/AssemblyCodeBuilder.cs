using Com.Zengfr.CodeGenerator.Config;
using Com.Zengfr.CodeGenerator.Host;
//using Com.Zengfr.CodeGenerator.CstTemplate.Impl;
//using EnvDTE90;
namespace Com.Zengfr.CodeGenerator.Builder.Impl
{
   public class AssemblyCodeBuilder : AbstractCodeBuilder
    {
        protected override FileBaseHost BuildHost(AbstractConfig cfg)
        {
            var config = cfg;

            var host = new AssemblyHost();
            host.OutputRootDir = config.OutputRootDir;
            host.OutputFolder = config.OutputFolder;

            host.NamespacePrefix = config.NamespacePrefix;
            host.NamespaceSuffix = config.NamespaceSuffix;
            host.ProjectName = config.ProjectName;

            host.FileNamePrefix = config.FileNamePrefix;
            host.FileNameSuffix = config.FileNameSuffix;
            host.FileExtention = config.FileExtention;
            InitAssemblySchema(host, config);
            return host;
        }



        protected override System.Collections.Generic.IList<string> BuildTemplates(AbstractConfig cfg)
        {
            return cfg.Templates;
        }
    }
}
