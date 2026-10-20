using System.IO;
using Com.Zengfr.CodeGenerator.VsAddin.Common;
using EnvDTE;
using EnvDTE90;
using VSLangProj;
using System.Reflection;
namespace Com.Zengfr.CodeGenerator.VsAddin.Common
{
    /// <summary>
    /// 项目生成
    /// </summary>
    public partial class SolutionUtils
    {
        #region Constants

        //private const string DllDirName = "dll";

        #endregion


        public static void Generate(Solution3 sln, string nameSpaces, string[] projNames)
        {

            if (sln == null) return;
            string slnDir = DTEUtils.GetSlnDirectory(sln);

            DTEUtils.ShowInfo(string.Format("根目录:{0}", slnDir));

            //CopyDll(Path.Combine(slnDir, DllDirName));

            // Create solution folder
            //#region

            //Project sfProj = DTEUtils.GetProjectByName(sln, DllDirName);
            //if (sfProj == null)
            //{
            //    sfProj = sln.AddSolutionFolder(DllDirName);
            //}
            //string[] files = Directory.GetFiles(Path.Combine(slnDir, DllDirName));
            //if (files != null)
            //{
            //    foreach (string file in files)
            //    {
            //        sfProj.ProjectItems.AddFromFile(file);
            //    }
            //}
            //sfProj = null;
            //#endregion
            foreach (var projName in projNames)
            {
                Project generatedProj = AddClassLibraryProject(sln, nameSpaces, projName, null, null);
                generatedProj = null;
            }

            // Project modelProj = AddClassLibraryProject(sln, nameSpaces, "Model", null, null);
            // Project dalProj = AddClassLibraryProject(sln, nameSpaces, "DAL", null, modelProj);
            // Project servicesProj = AddClassLibraryProject(sln, nameSpaces, "Services", null, modelProj, dalProj);


            //Project webProj = AddWebApplicationProject(sln, nameSpaces, "Web", null, null);
            //if (webProj != null)
            //{
            //    VSProject vsWebProj = webProj.Object as VSProject;

            //    webProj.ProjectItems.AddFolder("css", Constants.vsProjectItemKindPhysicalFolder);
            //    webProj.ProjectItems.AddFolder("images", Constants.vsProjectItemKindPhysicalFolder);
            //    webProj.ProjectItems.AddFolder("Maps", Constants.vsProjectItemKindPhysicalFolder);
            //    webProj.ProjectItems.AddFolder("UserControls", Constants.vsProjectItemKindPhysicalFolder);

            //    webProj.ProjectItems.AddFromFileCopy(
            //        Path.Combine(GetAddinPath(), @"ibatis-config\dao.config"));

            //    //string templatePath =
            //    //    sln.GetProjectItemTemplate("Interface.zip", "CSharp");
            //    //webProj.ProjectItems.AddFromTemplate(templatePath, "MyInterface.cs");
            //}
            sln.SaveAs(nameSpaces);
            sln = null;
            // modelProj = null; dalProj = null; servicesProj = null; 
        }

        private static Project AddClassLibraryProject(Solution3 sln, string nameSpace, string name, string[] dllReferences, params Project[] projReferences)
        {
            return AddProject(sln, "ClassLibrary.zip", nameSpace, name, dllReferences, projReferences);
        }
        private static Project AddWebApplicationProject(Solution3 sln, string nameSpace, string name, string[] dllReferences, params Project[] projReferences)
        {
            return AddProject(sln, "WebApplicationProject.zip", nameSpace, name, dllReferences, projReferences);
        }
        private static Project AddProject(Solution3 sln, string projectTemplateZip, string nameSpace, string name, string[] dllReferences, params Project[] projReferences)
        {
            string slnPath = Path.GetDirectoryName(sln.FullName);
            string classLibProjTemplatePath =
              sln.GetProjectTemplate(projectTemplateZip, "CSharp");
            string projName = string.Format("{0}.{1}", nameSpace, name);
            Project proj = DTEUtils.GetProjectByName(sln, projName);
            if (proj == null)
            {
                DTEUtils.ShowInfo(string.Format("创建项目:{0}", projName));
                string projFile = Path.Combine(slnPath, projName, projName + ".csproj");
                if (File.Exists(projFile))
                {
                    proj = sln.AddFromFile(Path.Combine(slnPath, projName, projName + ".csproj"), false);
                }
                else
                {
                    proj = sln.AddFromTemplate(classLibProjTemplatePath, Path.Combine(slnPath, projName), projName, false);
                }
                if (proj != null)
                {
                    object fItem = null;
                    try
                    {
                        fItem = proj.ProjectItems.Item("Generated");
                    }
                    catch { }
                    if (fItem == null)
                    {
                        if (!Directory.Exists(Path.Combine(slnPath, projName, "Generated")))
                        {
                            proj.ProjectItems.AddFolder("Generated", Constants.vsProjectItemKindPhysicalFolder);
                        }
                    }
                }
            }
            if (proj != null)
            {
                VSProject vsproj = proj.Object as VSProject;
                if (dllReferences != null)
                {
                    foreach (string dll in dllReferences)
                    {
                        vsproj.References.Add(dll);
                    }
                }
                if (projReferences != null)
                {
                    foreach (Project p in projReferences)
                    {
                        vsproj.References.AddProject(p);
                    }
                }
                vsproj = null;
            }
            return proj;

        }

        private static string GetAddinDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        private static void CopyDll(string targetDir)
        {
            string sourceDir = Path.Combine(GetAddinDirectory(), "dll");

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            if (Directory.Exists(sourceDir))
            {
                string[] files = Directory.GetFiles(sourceDir);
                if (files != null)
                {
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);
                        DTEUtils.ShowInfo(string.Format("复制文件:{0}", file));
                        File.Copy(file, Path.Combine(targetDir, fileName), true);

                    }
                }
                files = null;
            }
            else
            { DTEUtils.ShowError(string.Format("目录不存在:{0}", sourceDir)); }
            sourceDir = null;
        }

    }
}
