using Com.Zengfr.CodeGenerator.Config;
using Com.Zengfr.CodeGenerator.Config.Impl;
using Com.Zengfr.CodeGenerator.Host;
//using Com.Zengfr.CodeGenerator.CstTemplate.Impl;
namespace Com.Zengfr.CodeGenerator.Builder.Impl
{
    /// <summary>
    /// 代码生成
    /// </summary>
    class DatabaseToAssemblyCodeBuilder : CodeBuilderBase
    {

        protected override FileBaseHost BuildHost(BaseConfig cfg)
        {
            var config = cfg;// as DatabaseConfig;
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
        protected override System.Collections.Generic.IList<string> BuildTemplates(BaseConfig cfg)
        {
            return cfg.Templates;
        }
    }
}