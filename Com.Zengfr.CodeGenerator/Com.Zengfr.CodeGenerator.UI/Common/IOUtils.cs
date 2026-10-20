using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
namespace Com.Zengfr.CodeGenerator.UI.Common
{

    internal class IOUtils
    {
        public static string GetAddinDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public static string GetTemplatesDirectory()
        {
            return Path.Combine(GetAddinDirectory(), "Templates");
        }
        public static IList<string> GetFilesName(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            return di.GetFiles("*", SearchOption.AllDirectories).Select(t => t.Name).ToList();
        }
        public static List<string> GetFilesFullName(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            if (di.Exists)
            {
                return di.GetFiles("*", SearchOption.AllDirectories).Select(t => t.FullName).ToList();
            }
            return null;
        }
        public static List<string> GetTemplatesFullName(string path)
        {
            var fs = IOUtils.GetFilesFullName(VarUtils.Config.TemplatesDir);
            if (fs != null)
                fs.RemoveAll(t => ".tt.".IndexOf(System.IO.Path.GetExtension(t) + ".") == -1);
            return fs;
        }


    }
}
