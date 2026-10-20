

namespace Com.Zengfr.CodeGenerator.Template.Builder
{
    using System.Collections.Generic;
    using Com.Zengfr.CodeGenerator.Builder;
    using Com.Zengfr.CodeGenerator.Host;
    using Com.Zengfr.CodeGenerator.Schema;
    using Com.Zengfr.CodeGenerator.Common;
    public class TemplatesBuilder
    {

        /// <summary>
        /// 返回项目名称
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <param name="nameSpace"></param>
        /// <param name="names"></param>
        /// <param name="genImpl"></param>
        /// <returns></returns>
        public static IDictionary<string, IDictionary<string, string>> BuildProjectNamesAndFiles(string outputRootDir, string namespacePrefix, AssemblySchema assemblySchema, bool genImplementEmptyFile)
        {
            IDictionary<string, IDictionary<string, string>> projectNames = new Dictionary<string, IDictionary<string, string>>();

            var templates = GetTemplates();
            foreach (var kv in templates)
            {
                foreach (var file in kv.Value)
                {
                    var host = new AssemblyHost();
                    host.OutputRootDir = outputRootDir;
                    host.NamespacePrefix = namespacePrefix;
                    host.GenImplementEmptyFile = genImplementEmptyFile;

                    host.ProjectName = kv.Key;
                    host.TemplateFile = file;

                    var fs = Process(host, assemblySchema);
                    if (!projectNames.ContainsKey(host.ProjectName))
                    {
                        projectNames.Add(host.ProjectName, fs);
                    }
                    else
                    {
                        foreach (var f in fs)
                        {
                            if (!projectNames[host.ProjectName].ContainsKey(f.Key))
                            {
                                projectNames[host.ProjectName].Add(f);
                            }
                        }
                    }
                    fs = null; host = null;
                }
            }
            return projectNames;
        }
        #region ITemplateBase Members


        public static IDictionary<string, string> Process(AssemblyHost host, AssemblySchema assemblySchema)
        {
            IDictionary<string, string> files = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(host.TemplateFile))
            {
                CommonLogger.ShowInfo(string.Format("Process->Template:{0}", host.TemplateFile));
                host.AssemblySchema = assemblySchema;
                foreach (var sourceType in assemblySchema.Types)
                {
                    host.TypeSchema = sourceType;
                    host.FileName = sourceType.Name;
                    var fs = BuildFileAndContent(host);
                    foreach (var f in fs)
                    {
                        files.Add(f);
                    }
                    if (host.OutputSingleFile)
                    {
                        break;
                    }
                }
                //输出
                host.ProjectName = host.ProjectName;
            }
            return files;
        }
        /// <summary>
        /// 生成文件以及内容 key=file value=content
        /// </summary>
        /// <param name="host"></param>
        private static IDictionary<string, string> BuildFileAndContent(FileBaseHost host)
        {
            IDictionary<string, string> files = new Dictionary<string, string>();

            string file, content;
            content = BuilderFactory.BuildOutputFileConent(host);
            file = BuilderFactory.BuildOutputFileName(host, false);

            files.Add(file, content);
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
                files.Add(file, content);
            }
            file = null; content = null;
            return files;
        }
        private static void CopyProperties(object source, object target)
        {
            Common.ConvertUtils.CopyProperties(source, target);
        }
        #endregion

        public static IDictionary<string, string[]> GetTemplates()
        {
            IDictionary<string, string[]> templates = new Dictionary<string, string[]>();
            templates.Add("Domain", new string[] { "BaseModel.txt", "Model.txt", "Model.config.txt", "ModelSchema.txt" });
            templates.Add("Domain.Mapping", new string[] { "ModelMapping.txt" });

            templates.Add("DataAccess", new string[] { "BaseDao.txt", "DeepTransformer.cs.txt", "LocalSessionFactoryObject.txt", "Dao.txt", "Dao.config.txt" });
            templates.Add("IDataAccess", new string[] { "IBaseDao.txt", "IDao.txt" });

            templates.Add("Business", new string[] { "BaseBo.txt", "Bo.txt", "Bo.config.txt", "Validator.txt", "BaseMapper.txt", "Mapper.txt", });
            templates.Add("IBusiness", new string[] { "IBo.txt" });

            templates.Add("Contracts", new string[] { "DataContract.txt", "ServiceContract.txt", "ContractServiceContract.txt" });
            templates.Add("Services", new string[] { "Service.txt", "ContractService.txt", "Services.config.txt", "ContractServices.config.txt", "Services.WebServices.config.txt" });

            templates.Add("ServiceStack", new string[] { "ServiceStackContract-Response.txt", "ServiceStackContract-Command.txt", "ServiceStackContract-Query.txt", "ServiceStackCurdService.txt", "ServiceStackQueryService.txt", "ServiceStackServiceHost.txt" });

            templates.Add("WebServices", new string[] { 
                "Services.svc.txt",   "ContractServices.svc.txt",  
                "Services.Web.config.txt",

            });
            templates.Add("Console", new string[] { 
                "Client.config.txt","App.config.txt","Test.txt"});

            templates.Add("Web", new string[] { 
                "Client.config.txt",
                "WCF-WebController.txt","WCF-WebControllerBase.txt",
                "Knockout-Controller.cs.txt","Knockout-ControllerBase.cs.txt",
                "ServiceStack-Controller.cs.txt","ServiceStack-ControllerBase.cs.txt",
                "Controllers.config.txt",
                 "extjs-WebExtController.txt",

                "JQuery.js.txt","List.Aspx.txt", "Edit.Aspx.txt",

               "bootstrap-form.txt","bootstrap-table.txt","bootstrap-template.txt",

              
               "extjs-DataModel.txt","extjs-DataStore.txt",
               "extjs-FormPanel.txt","extjs-GridPanel.txt",
               "extjs-PropertyGrid.txt","extjs-SearchPanel.txt",
                "extjs-template.txt", "extjs-Data.txt",
                 "extjs-List.txt", "extjs-InfoPanel.txt",

                 
               "Knockout-Edit.aspx.txt","Knockout-List.aspx.txt",

               
             });
            return templates;
        }
        public static IDictionary<string, string[]> GetProjectReferences(string namespacePrefix)
        {
            IDictionary<string, string[]> objs = new Dictionary<string, string[]>();
            objs.Add("Domain", new string[] { });
            objs.Add("Domain.Mapping", new string[] { "Domain" });

            objs.Add("DataAccess", new string[] { "Domain", "IDataAccess" });
            objs.Add("IDataAccess", new string[] { "Domain" });

            objs.Add("Business", new string[] { "IBusiness", "Domain", "Contracts", "IDataAccess" });
            objs.Add("IBusiness", new string[] { "Domain", "Contracts", });

            objs.Add("Contracts", new string[] { "Domain" });
            objs.Add("Services", new string[] { "Business", "IBusiness", "Domain", "Contracts", "IDataAccess" });

            objs.Add("ServiceStack", new string[] { "Business", "IBusiness", "Domain", "Contracts", "IDataAccess" });


            objs.Add("WebServices", new string[] { "Contracts", "Domain", "IDataAccess", "IBusiness" });
            objs.Add("Console", new string[] { "Contracts", "Domain" });

            objs.Add("Web", new string[] { "Contracts", "Domain", "IDataAccess", "IBusiness" });
            return objs;
        }
        public static IDictionary<string, string[]> GetReferences(string namespacePrefix)
        {
            IDictionary<string, string[]> objs = new Dictionary<string, string[]>();
            objs.Add("Domain", new string[] { "System.ComponentModel.DataAnnotations", "NHibernate.dll", "NHibernate.Mapping.Attributes.dll" });
            objs.Add("Domain.Mapping", new string[] { "NHibernate.dll", "NHibernate.Mapping.Attributes.dll" });

            objs.Add("DataAccess", new string[] { "NHibernate.dll", "NHibernate.Mapping.Attributes.dll", "Common.Logging.dll", "Spring.Aop.dll", "Spring.Core.dll", "Spring.Data.dll", "Spring.Data.NHibernate32.dll" });
            objs.Add("IDataAccess", new string[] { "NHibernate.dll" });

            objs.Add("Business", new string[] { "Common.Logging.dll", "EmitMapper.dll", "Spring.Aop.dll", "Spring.Core.dll", "Spring.Data.dll", "NHibernate.dll" });
            objs.Add("IBusiness", new string[] { });

            objs.Add("Contracts", new string[] { "System.ServiceModel", "System.Runtime.Serialization" });
            objs.Add("Services", new string[] { "Common.Logging.dll", "EmitMapper.dll", "Spring.Aop.dll", "Spring.Core.dll", "Spring.Data.dll", "NHibernate.dll" });
            objs.Add("ServiceStack", new string[] { "ServiceStack.dll", "ServiceStack.Common.dll", "ServiceStack.Interfaces.dll", "erviceStack.ServiceInterface.dll", "NHibernate.dll" });


            objs.Add("WebServices", new string[] { "System.ServiceModel", "Common.Logging.dll", "NHibernate.dll", "Spring.Aop.dll", "Spring.Core.dll", "Spring.Data.dll", "Spring.Data.NHibernate32.dll", "Spring.Services.dll" });
            objs.Add("Console", new string[] { "System.ServiceModel", "Common.Logging.dll", "Spring.Aop.dll", "Spring.Core.dll", "Spring.Data.dll" });

            objs.Add("Web", new string[] { "System.ServiceModel" });
            return objs;
        }
    }
}
