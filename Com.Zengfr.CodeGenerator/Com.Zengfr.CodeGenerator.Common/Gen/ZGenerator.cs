using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Com.Zengfr.CodeGenerator.Common.Engine;
using Com.Zengfr.CodeGenerator.Common.Meta;
using Com.Zengfr.CodeGenerator.Common.Util;
using Common.Logging;

namespace Com.Zengfr.CodeGenerator.Common.Gen
{
    public class ZGenerator
    {
        private static ILog logger = LogManager.GetLogger(typeof(ZGenerator));
        public static ZSysConfig BuildSysConfig(bool isInWebEnv, String templateRootDir, String outputRootDir)
        {
            ZSysConfig sysConfig = new ZSysConfig();
            sysConfig.IsInWebEnv = isInWebEnv;
            if (!String.IsNullOrWhiteSpace(templateRootDir))
                sysConfig.TemplateRootDir = templateRootDir;
            if (!String.IsNullOrWhiteSpace(outputRootDir))
                sysConfig.OutputRootDir = outputRootDir;
            return sysConfig;
        }
        public static ZInputConfig BuildInputConfig(ZSysConfig sysConfig, ZMeta meta,string namespacePrefix)
        {
            ZInputConfig inConfig = new ZInputConfig();
            if (!String.IsNullOrWhiteSpace(namespacePrefix))
                inConfig.NamespacePrefix = namespacePrefix;
            inConfig.MetaModelNames = meta.Models.Select(t => t.Name).ToList();
            inConfig.TemplateDirs = PathUtil.Instance.GetDirsName(sysConfig.TemplateRootDir, "*").ToList();
            return inConfig;
        }
        public static void Process(ZSysConfig sysConfig, ZMeta meta, ZInputConfig config)
        {
            var startTime = DateTime.Now;
            logger.Info(string.Format("Process Init:{0}", startTime.ToString()));
            ZEngineFacade.Instance.Init(sysConfig.TemplateRootDir, sysConfig.IsInWebEnv);

            var modelNames = config.MetaModelNames.ToDictionary(t => t);
            var models = meta.Models.Where(t => modelNames.ContainsKey(t.Name)).ToList();
            var templateDirs = config.TemplateDirs;
            logger.Info(string.Format("Process Init:{0},{1}", models.Count, String.Join("|", modelNames.Keys)));
            logger.Info(string.Format("Process Init:{0},{1}", templateDirs.Count, String.Join("|", templateDirs)));
            if (models.Count > 0 && templateDirs.Count > 0)
            {
                foreach (var templateDir in templateDirs)
                {
                    string[] templatefiles = PathUtil.Instance.GetFiles(PathUtil.Instance.GetFilePath(@"\", sysConfig.TemplateRootDir, templateDir), "*." + sysConfig.TemplateFileExt);
                    foreach (var templateFile in templatefiles)
                    {
                        var templateFileName = PathUtil.Instance.GetFileNameWithoutExt(templateFile);

                        foreach (var model in meta.Models)
                        {
                            IDictionary<string, object> kvs = new Dictionary<string, object>();
                            kvs.Add("meta", meta);
                            kvs.Add("models", models);
                            kvs.Add("model", model);
                            kvs.Add("attrsMap", model.Attributes.ToDictionary(t=> t.Name,t=>t.TypeName));
                            kvs.Add("config", config);

                            var outConfig = new ZOutputConfig();
                            kvs.Add("outputConfig", outConfig);
                            kvs.Add("sysConfig", sysConfig);

                            var metaUtil = ZMetaUtil.Instance;
                            var stringUtil = StringUtil.Instance;
                            var reflectionUtil = ReflectionUtil.Instance;
                            var formatUtil = FormatUtil.Instance;

                            kvs.Add("logger", logger);
                            kvs.Add("metaUtil", metaUtil);
                            kvs.Add("stringUtil", stringUtil);
                            kvs.Add("reflectionUtil", reflectionUtil);
                            kvs.Add("formatUtil", formatUtil);
                            kvs.Add("jpaUtil", JpaUtil.Instance);
                            kvs.Add("javaUtil", JavaUtil.Instance);
                            kvs.Add("cSharpUtil", CSharpUtil.Instance);

                            logger.Info(string.Format("-".PadRight(69, '-')));
                            logger.Info(string.Format("Process Start:{0},{1},{2}", templateFileName, model.Name, string.Empty));
                            string content = ZEngineFacade.Instance.Process(templateFile.Replace(sysConfig.TemplateRootDir, string.Empty), kvs);
                            var retCode = ZEngineFacade.Instance.SaveToFile(content, kvs);
                            var isSingleFile = retCode == 1;
                            if (retCode<0)
                            {
                                logger.Error(string.Format("OutputFileName IsNullOrWhiteSpace->{0},{1},{2}", templateFileName, model.Name, content));
                            }
                            string outputFile = ZEngineFacade.Instance.GetOutputFilePath(kvs);
                            string outputFileName = PathUtil.Instance.GetFileName(outputFile);
                            logger.Info(string.Format("Process End  :{0},{1},retCode:{2},{3},{4}", templateFileName, model.Name, retCode, content.Length, outputFileName));
                            content = null; kvs = null; outConfig = null;
                            if (isSingleFile)
                            {
                                break;
                            }

                        }
                    }
                }
                var timeSpan = (DateTime.Now - startTime).TotalSeconds;
                logger.Info(string.Format("Process TimeSpan:{0},{1}", timeSpan, timeSpan / (modelNames.Count * templateDirs.Count)));
            }
            logger.Info(string.Format("Process Finish:{0},{1}", modelNames.Count, templateDirs.Count));
            models = null; templateDirs = null; modelNames = null;
        }

        
    }
}
