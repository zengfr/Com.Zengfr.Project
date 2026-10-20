using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Com.Zengfr.CodeGenerator.Common.Util
{
   public class PathUtil
    {
        private static PathUtil instance;
        public static PathUtil Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PathUtil();
                }
                return instance;
            }
        }
        public string GetSafetyPath(string path)
        {
            path = (path ?? string.Empty).Replace("..", "").Replace("//", "/");
            path = Regex.Replace(path, @"(\.){2,}", "_");
            path = Regex.Replace(path, @"(^:)\\", @"$1");
            path = Regex.Replace(path, @"[^A-Za-z0-9\u4e00-\u9fa5\.\\_:/-]", "_");
            return path;
        }
        public string GetFileName(string filePath)
        {
            return Path.GetFileName(GetSafetyPath(filePath));
        }
        public string GetExtension(string filePath)
        {
            return Path.GetExtension(GetSafetyPath(filePath));
        }
        public string GetFileNameWithoutExt(string filePath)
        {
            return Path.GetFileNameWithoutExtension(GetSafetyPath(filePath));
        }
        public string[] GetFiles(string dir, string searchPattern)
        {
            string[] files = Directory.GetFiles(GetSafetyPath(dir), searchPattern);
            return files;
        }
        public string[] GetFilesName(string dir, string searchPattern)
        {
            var files = GetFiles(dir, searchPattern).Select(t => t.Replace(dir, string.Empty));
            return files.ToArray();
        }
        public string[] GetDirs(string root, string searchPattern)
        {
            var files = Directory.EnumerateDirectories(root, searchPattern, SearchOption.AllDirectories);
            return files.ToArray();
        }
        public string[] GetDirsName(string root, string searchPattern)
        {
            var files = GetDirs(root,  searchPattern).Select(t=>t.Replace(root,string.Empty));
            return files.ToArray();
        }
        public string GetFilePath(string separator, params string[] filePath)
        {
            return GetSafetyPath(string.Join(""+ separator, filePath));
        }
    }
}
