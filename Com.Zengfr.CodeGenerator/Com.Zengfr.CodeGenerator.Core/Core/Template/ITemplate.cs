namespace Com.Zengfr.CodeGenerator.Template
{
    using System;
    using Com.Zengfr.CodeGenerator.Schema;
    public partial interface ITemplate
    {
        string OutputRootDir { set; get; }
        string NamespacePrefix { get; set; }

        string ProjectName { set; get; }

        string TemplateFile { get; set; }
        bool GenImplementEmptyFile { set; get; }
        void Process(AssemblySchema assemblySchema);
        event Action<string, string> WriteFile;
    }

}
