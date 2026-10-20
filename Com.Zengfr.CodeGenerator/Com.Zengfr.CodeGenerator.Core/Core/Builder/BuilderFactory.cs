
namespace Com.Zengfr.CodeGenerator.Builder
{
    using System.IO;
    using Com.Zengfr.CodeGenerator.Common;
    using Com.Zengfr.CodeGenerator.Host;
    public class BuilderFactory
    {


        //public static void WriteFile(string fileName, string content)
        //{
        //    WriteLocalFile(fileName, content);

        //}

        //public static void WriteLocalFile(string fileName, string content)
        //{
        //    var dir = Path.GetDirectoryName(fileName);
        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }
        //    CommonLogger.ShowInfo(string.Format("Write->Length:{0} File:{1}", content.Length, fileName));
        //    File.WriteAllText(fileName, content, Encoding.UTF8);
        //    fileName = null; content = null;
        //}
        public static string BuildOutputFileConent(FileBaseHost host)
        {
            //CommonLogger.ShowInfo(string.Format("Template:{0}", host.TemplateFile));
            return T4Engine.ProcessTemplate(host);
        }
        public static string BuildOutputFileName(FileBaseHost host, bool implement)
        {
            if (string.IsNullOrEmpty(host.OutputFolder))
                host.OutputFolder = string.Empty;
            if (string.IsNullOrEmpty(host.FileNameSuffix))
                host.FileNameSuffix = string.Empty;

            var dir = Path.Combine(string.Format("{0}.{1}", host.NamespacePrefix, host.ProjectName),
               host.OutputFolder, implement ? "" : "generated");

            var file = string.Format("{0}{1}{2}{3}",
                host.FileNamePrefix,
                host.FileName,
                implement ? host.FileNameSuffix.Replace("_gen", "") : host.FileNameSuffix,
                host.FileExtention);
            var f = Path.Combine(dir, file);
            CommonLogger.ShowInfo(string.Format("  Output:{0}", f));
            return Path.Combine(host.OutputRootDir, f);
        }
    }
}
