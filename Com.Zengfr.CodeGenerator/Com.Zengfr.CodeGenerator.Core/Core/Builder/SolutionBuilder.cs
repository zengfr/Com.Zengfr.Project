
namespace Com.Zengfr.CodeGenerator.Template.Builder
{
    using Com.Zengfr.CodeGenerator.Common;
   
    public class SolutionBuilder
    {
        public virtual string BaseDirectory { get; set; }
        public virtual string Namespace { get; set; }

        public virtual string[] ProjectNames { get; set; }
       

        public SolutionBuilder()
        {
            
        }
        public virtual string BuildFileName()
        {
            string path;
            path = string.Format("{0}\\{1}.{2}",
                this.BaseDirectory,
                this.Namespace,
               "sln");
            return path;
        }
        public virtual string BuildFileContent()
        {
            return BuildProjectFileContent(this.Namespace,this.ProjectNames);
        }
        protected static string BuildProjectFileContent(string namespaces, string[] projectNames)
        {
            using (CodeStringBuilder f = new CodeStringBuilder())
            {
                f.AppendFormatLine(0, @"");
                f.AppendFormatLine(0, @"Microsoft Visual Studio Solution File, Format Version 11.00");
                f.AppendFormatLine(0, @"# Visual Studio 2010");
                foreach (var projectName in projectNames)
                {
                    f.AppendFormatLine(0, @"Project(""{0}"") = ""{1}.{2}"", ""{1}.{2}\\{1}.{2}.csproj"", ""{3}""",
                        "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}",
                        namespaces, projectName, "{BEE07B6E-2173-4F90-9D2B-629B1B6C7A3B}");
                    f.AppendFormatLine(0, @"EndProject");
                }
                string[] folders = new string[] { "DataAccess", "Business", "Services", "Contracts", "Web" };
                foreach (var folder in folders)
                {
                    f.AppendFormatLine(0, @"Project(""{0}"") = ""{1}"", ""{2}"", ""{3}""",
                           "{2150E333-8FDC-42A3-9474-1A3956D46DE8}",
                           folder, folder, "{BEE07B6E-2173-4F90-9D2B-629B1B6C7A3C}");
                    f.AppendFormatLine(0, @"EndProject");

                }
                f.AppendFormatLine(0, @"Global");
                f.AppendFormatLine(0, @"GlobalSection(SolutionConfigurationPlatforms) = preSolution");
                f.AppendFormatLine(0, @"Debug|Any CPU = Debug|Any CPU");
                f.AppendFormatLine(0, @"EndGlobalSection");
                f.AppendFormatLine(0, @"GlobalSection(ProjectConfigurationPlatforms) = postSolution");
                f.AppendFormatLine(0, @"{BEE07B6E-2173-4F90-9D2B-629B1B6C7A3B}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                f.AppendFormatLine(0, @"{BEE07B6E-2173-4F90-9D2B-629B1B6C7A3B}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                f.AppendFormatLine(0, @"EndGlobalSection");
                f.AppendFormatLine(0, @"GlobalSection(SolutionProperties) = preSolution");
                f.AppendFormatLine(0, @"HideSolutionNode = FALSE");
                f.AppendFormatLine(0, @"EndGlobalSection");
                f.AppendFormatLine(0, @"EndGlobal");
                return f.Content;
            }

        }
    }
}
