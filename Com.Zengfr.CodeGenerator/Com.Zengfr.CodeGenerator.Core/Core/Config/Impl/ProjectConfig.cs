
namespace Com.Zengfr.CodeGenerator.Core.Config.Impl
{
    public class ProjectConfig
    {
        public virtual string BaseDirectory { get; set; }
        public virtual string Namespace { get; set; }

        public virtual string ProjectName { get; set; }
        public virtual string[] References { get; set; }
        public virtual string[] ProjectReferences { get; set; }


    }
}
