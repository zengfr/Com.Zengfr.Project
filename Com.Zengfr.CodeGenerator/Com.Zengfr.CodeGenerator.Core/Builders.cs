
namespace Com.Zengfr.CodeGenerator.Template
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Com.Zengfr.CodeGenerator.Schema;
    using Com.Zengfr.CodeGenerator.Template.Builder;
    using WebApplication1.ZIP;
    using System.Diagnostics;
    public class Builders
    {
        public static void Test()
        {


            string baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            string nameSpace = "Com.Zengfr.abc";
            string[] names = new string[] { "order", "product" };

            Console.WriteLine("Start");
            // BuildByCodeTemplate(baseDirectory, nameSpace, assemblySchema, true);
            Console.WriteLine("End");

            Console.ReadLine();
        }
        public static void
            BuildByTemplates(string baseDirectory, string nameSpace, AssemblySchema assemblySchema, bool genProjFile, bool genImplementEmptyFile, bool genZipFile)
        {

            IDictionary<string, IDictionary<string, string>> projectNames = TemplatesBuilder.BuildProjectNamesAndFiles(baseDirectory, nameSpace, assemblySchema, genImplementEmptyFile);

            IDictionary<string, string> solutionAndprojectFiles = new Dictionary<string, string>();

            //项目
            if (genProjFile)
            {
                var references = TemplatesBuilder.GetReferences(nameSpace);
                var projectReferences = TemplatesBuilder.GetProjectReferences(nameSpace);

                ProjectBuilder projectBuilder = new ProjectBuilder();
                projectBuilder.BaseDirectory = baseDirectory;
                projectBuilder.Namespace = nameSpace;

                foreach (var projectName in projectNames)
                {
                    projectBuilder.ProjectName = projectName.Key;
                    if (references.ContainsKey(projectName.Key))
                    {
                        projectBuilder.References = references[projectName.Key];
                    }
                    if (projectReferences.ContainsKey(projectName.Key))
                    {
                        projectBuilder.ProjectReferences = projectReferences[projectName.Key];
                    }
                    var projFile = projectBuilder.BuildFileName();
                    solutionAndprojectFiles.Add(projFile, projectBuilder.BuildFileContent());
                    if (projectName.Key.IndexOf("DataAccess") != -1)
                    {
                        ExecMSBuild(projFile);
                    }
                }
                //解决方案
                SolutionBuilder solutionBuilder = new SolutionBuilder();
                solutionBuilder.BaseDirectory = baseDirectory;
                solutionBuilder.Namespace = nameSpace;
                solutionBuilder.ProjectNames = projectNames.Keys.ToArray();
                solutionAndprojectFiles.Add(solutionBuilder.BuildFileName(), solutionBuilder.BuildFileContent());

                references = null; projectReferences = null; projectBuilder = null; solutionBuilder = null;
            }


            MemoryZipFile zip = new MemoryZipFile();
            AddEntriesToZipFile(zip, baseDirectory, solutionAndprojectFiles);
            AddEntriesToZipFile(zip, baseDirectory, projectNames.Values);
            zip.SaveToFile(string.Format("{0}\\gen_{1}.zip", baseDirectory, DateTime.Now.ToString("yyyyMMddHHmm")));

            solutionAndprojectFiles = null; projectNames = null; zip = null;

        }

        private static void AddEntriesToZipFile(MemoryZipFile zip, string baseDirectory, ICollection<IDictionary<string, string>> files)
        {
            if (files != null)
            {
                foreach (var fs in files)
                {
                    AddEntriesToZipFile(zip,baseDirectory, fs);
                }
            }
        }

        private static void AddEntriesToZipFile(MemoryZipFile zip, string baseDirectory, IDictionary<string, string> files)
        {
            if (files != null)
            {
                foreach (var fs in files)
                {
                    zip.AddEntrie(fs.Key.Replace(baseDirectory,string.Empty), fs.Value);
                }
            }
        }
        private static void ExecMSBuild(string csprojFile)
        {
            var ms = string.Format("{0}\\Microsoft.NET\\Framework\\V{1}\\", Environment.GetFolderPath(Environment.SpecialFolder.Windows),Environment.Version.ToString(3));
            string cmd = string.Format("{0}\\MSBuild.exe {1} /t:build /p:Configuration=Release",ms, csprojFile);
            Exec(cmd);
        }
        private static void Exec(string cmd)
        {
            using (Process myPro = new Process())
            {
                myPro.StartInfo.FileName = "cmd.exe";
                myPro.StartInfo.UseShellExecute = false;
                myPro.StartInfo.RedirectStandardInput = true;
                myPro.StartInfo.RedirectStandardOutput = true;
                myPro.StartInfo.RedirectStandardError = true;
                myPro.StartInfo.CreateNoWindow = true;
                myPro.Start();
                //如果调用程序路径中有空格时，cmd命令执行失败，可以用双引号括起来 ，在这里两个引号表示一个引号（转义）
                string str = string.Format(@"""{0}"" {1}", cmd, "&exit");

                myPro.StandardInput.WriteLine(str);
                myPro.StandardInput.AutoFlush = true;
                myPro.WaitForExit();

                
            }
        }
    }
}
