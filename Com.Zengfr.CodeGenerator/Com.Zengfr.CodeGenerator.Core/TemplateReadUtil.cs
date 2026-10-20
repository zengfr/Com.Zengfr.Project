using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common
{
    public class TemplateReadUtil
    {
        static System.Resources.ResourceManager resourceManager;
        static TemplateReadUtil()
        {
            resourceManager = new ResourceManager("Com.Zengfr.CodeGenerator.template", typeof(TemplateReadUtil).Assembly);
        }
        public static string Read(string templateFile)
        {
            var templateContent = string.Empty;
            if (File.Exists(templateFile))
            {
                templateContent = File.ReadAllText(templateFile);
            }
            else
            {
                templateContent = resourceManager.GetString(Path.GetFileNameWithoutExtension(templateFile), System.Globalization.CultureInfo.CurrentCulture);
            }
            return templateContent;
        }
    }
}
