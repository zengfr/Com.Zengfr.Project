using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.Zengfr.CodeGenerator.Web.Util
{
    public class FileNodeUtil
    {
        public static IList<FileNode> GetFileNodes(string path, string fileExt, string nodeType) {
            var results = new List<FileNode>();

            var sysBasePath = FileUtil.BuildSysBasePath(nodeType);
            var nodes1 = FileUtil.GetFileNodes(sysBasePath, path, fileExt, nodeType);

            var userBasePath = FileUtil.BuildUserBasePath(nodeType);
            var nodes2 = FileUtil.GetFileNodes(userBasePath, path, fileExt, nodeType);

            results.AddRange(nodes1);
            results.AddRange(nodes2);
            return results;

        }
    }
}