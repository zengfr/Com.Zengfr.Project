using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Com.Zengfr.CodeGenerator.Web.Util
{
    public class FileUtil
    {
        public static string BuildSysBasePath(string type)
        {
            string user = "sys";
            return BuildBasePath(user, type);
        }
        public static string BuildUserBasePath(string type)
        {
            string user = SystemUtil.GetUserId();
            return BuildBasePath(user, type);
        }
        private static string BuildBasePath(string user,string type) {
           
            var baseDir = SystemUtil.BaseDir;
            var basePath = string.Format(@"{0}\{1}\{2}", baseDir, SafePath(user), SafePath(type));
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            return basePath;
        }
        private static string SafePath(string path) {
            return (path ?? string.Empty).Replace(".", string.Empty).Replace(":", string.Empty);
        }
        public static IList<FileNode> GetFileNodes(String basePath, String path,string fileExt, String nodeType)
        {
            var results = new List<FileNode>();
            var dirPath = basePath+ path ?? string.Empty;
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (dir.Exists)
            {
                var dirs = dir.GetDirectories();
                var fies = dir.GetFiles(fileExt);
                results.AddRange(dirs.Select(t => BuildFileNode(basePath, t)).Where(t => { t.Type = nodeType; return t.Name != null; }));
                results.AddRange(fies.Select(t => BuildFileNode(basePath, t)).Where(t => { t.Type = nodeType; return t.Name != null; }));
            }
            return results;
        }

        private static FileNode BuildFileNode(string baseDir, DirectoryInfo d)
        {
            var t = new FileNode();

            t.FullName = d.FullName.Replace(baseDir, "");
            t.Name = d.Name;
            t.IsFile = false;
            t.Extension = d.Extension;
            t.Id = t.FullName.GetHashCode();
            return t;
        }
        private static FileNode BuildFileNode(string baseDir, FileInfo f)
        {
            var t = new FileNode();
            if (!String.IsNullOrWhiteSpace(f.Extension) && Regex.IsMatch(f.Extension, SystemUtil.FilleExtFilter))
            {
                t.FullName = f.FullName.Replace(baseDir, "");
                t.Name = f.Name;
                t.IsFile = true;
                t.Extension = f.Extension;
                t.Id = t.FullName.GetHashCode();
                
            }
            return t;
        }
    }

    public class FileNode {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        public string Extension { get; set; }
        public string Type { get; set; }
        public Boolean IsFile { get; set; }
    }
}