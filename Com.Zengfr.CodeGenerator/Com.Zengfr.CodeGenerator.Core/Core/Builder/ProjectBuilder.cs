
namespace Com.Zengfr.CodeGenerator.Template.Builder
{
    using Com.Zengfr.CodeGenerator.Common;
    public class ProjectBuilder
    {

        public virtual string BaseDirectory { get; set; }
        public virtual string Namespace { get; set; }

        public virtual string ProjectName { get; set; }
        public virtual string[] References { get; set; }
        public virtual string[] ProjectReferences { get; set; }
     
        public ProjectBuilder()
        {
           
        }
        public virtual string BuildFileName()
        {
            string path;
            path = string.Format("{0}\\{1}.{2}\\{1}.{2}.{3}",
               this.BaseDirectory,
                this.Namespace,
                this.ProjectName, "csproj");
            return path;
        }
        public virtual string BuildFileContent()
        {
            return BuildProjectFileContent();
        }
        protected virtual string BuildProjectFileContent()
        {
            using (CodeStringBuilder f = new CodeStringBuilder())
            {
                f.AppendFormatLine(0, @"<?xml version=""1.0"" encoding=""utf-8""?>");
                f.AppendFormatLine(0, @"<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">");
                f.AppendFormatLine(0, @"<PropertyGroup>");
                f.AppendFormatLine(0, @" <Configuration Condition="" '$(Configuration)' == '' "">Debug</Configuration>");
                f.AppendFormatLine(0, @"<Platform Condition="" '$(Platform)' == '' "">x86</Platform>");
                if (this.ProjectName.IndexOf("Web") != -1)
                {
                    f.AppendFormatLine(0, @"<ProjectGuid>{76B57F2E-0D8B-423E-B7D0-5576AB764716}</ProjectGuid>");
                    f.AppendFormatLine(0, @"<ProjectTypeGuids>{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}</ProjectTypeGuids>");
                }
                else
                {
                    f.AppendFormatLine(0, @"<ProjectGuid>{BEE07B6E-2173-4F90-9D2B-629B1B6C7A3B}</ProjectGuid>");
                }

                f.AppendFormatLine(0, @"<OutputType>Library</OutputType>");
                f.AppendFormatLine(0, string.Format(@"<RootNamespace>{0}.{1}</RootNamespace>", this.Namespace, this.ProjectName));
                f.AppendFormatLine(0, string.Format(@"<AssemblyName>{0}.{1}</AssemblyName>", this.Namespace, this.ProjectName));
                f.AppendFormatLine(0, @"<TargetFrameworkVersion>v4.0</TargetFrameworkVersion>");
                f.AppendFormatLine(0, @"<SchemaVersion>2.0</SchemaVersion>");
                f.AppendFormatLine(0, @"</PropertyGroup>");
                f.AppendFormatLine(0, @"<PropertyGroup Condition="" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' "">");
                f.AppendFormatLine(0, @"<DebugSymbols>false</DebugSymbols>");
                f.AppendFormatLine(0, @"<DebugType>full</DebugType>");
                f.AppendFormatLine(0, @"<Optimize>false</Optimize>");
                f.AppendFormatLine(0, @"<OutputPath>..\bin\Debug\</OutputPath>");
                f.AppendFormatLine(0, @"<DefineConstants>DEBUG;TRACE</DefineConstants>");
                f.AppendFormatLine(0, @"<ErrorReport>prompt</ErrorReport>");
                f.AppendFormatLine(0, @"<WarningLevel>4</WarningLevel>");
                f.AppendFormatLine(0, @"</PropertyGroup>");

                f.AppendFormatLine(0, @"<ItemGroup>");
                f.AppendFormatLine(0, @"<Reference Include=""System"" />");
                f.AppendFormatLine(0, @"<Reference Include=""System.Core"" />");
                f.AppendFormatLine(0, @"<Reference Include=""System.Data"" />");
                f.AppendFormatLine(0, @"<Reference Include=""System.Design"" />");
                f.AppendFormatLine(0, @"<Reference Include=""System.Xml"" />");
                if (this.References != null && this.References.Length > 0)
                {
                    foreach (var reference in this.References)
                    {
                        f.AppendFormatLine(0, @"<Reference Include=""{0}"">", reference);
                        if (reference.IndexOf(".dll") != -1)
                            f.AppendFormatLine(0, @"<HintPath>..\bin\{0}</HintPath>", reference);
                        f.AppendFormatLine(0, @"</Reference>");
                    }
                }
                f.AppendFormatLine(0, "</ItemGroup>");

                f.AppendFormatLine(0, "<ItemGroup>");
                if (this.ProjectReferences != null && this.ProjectReferences.Length > 0)
                {
                    foreach (var projectReference in this.ProjectReferences)
                    {
                        f.AppendFormatLine(0, @"<ProjectReference Include=""..\{0}.{1}\{0}.{1}.csproj"">", this.Namespace, projectReference);
                        f.AppendFormatLine(0, @"<Name>{0}.{1}</Name>", this.Namespace, projectReference);
                        f.AppendFormatLine(0, @"</ProjectReference>");
                    }
                }
                f.AppendFormatLine(0, "</ItemGroup>");

                f.AppendFormatLine(0, @"<Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />");
                f.AppendFormatLine(0, "</Project>");
                return f.Content;
            }

        }
    }
}
