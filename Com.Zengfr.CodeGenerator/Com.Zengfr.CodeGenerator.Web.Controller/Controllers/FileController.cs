using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using Com.Zengfr.CodeGenerator.Web.Controllers;
using Com.Zengfr.CodeGenerator.Web.Util;

namespace Com.Zengfr.CodeGenerator.WebUI.Controllers
{
    public class FileController : ApiController
    {
        [HttpGet]
        public IList<FileNode> readFile(String path)
        {
            var results = new List<FileNode>();
            var nodeType = "gen";
            var fileExt = "*.*";
            var nodes = FileNodeUtil.GetFileNodes(path, fileExt, nodeType);
            results.AddRange(nodes);
            return results;
        }
    }
}