using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace ThreadWorker
{
    public class UrlReWritor
    {
        #region 字段
        private const string IndexFile = "index.html";
        private static string[] Pattern ={ @"(/?|&)([^=]*)=([^&#]+)", @"([%\||:\*&?<:>""!])" };
        //private static string[] Replace = new string[] { @"/$2-$3/", @"" };
        private static string[] Replace = new string[] {@"$2-$3_", @"" };
        #endregion
        #region
        public static string ReWriteUrl(Uri url)
        {
            return ReWriteUrl(url.ToString());//
        }
        public static string ReWriteUrl(string url)
        {
            ReWriteUrl(ref url);
            return url;//
        }
        public static void ReWriteUrl(ref string url)
        {
            if (url == null || url == "") return;

            string query, queryRep, ext;
            url = Uri.EscapeUriString(url.ToLower());

            int p = url.IndexOf("?");
            if (p != -1)
            {
                query = url.Substring(p);//
                ext = System.IO.Path.GetExtension(url.Substring(0,p));//
                if (ext.Length != 0 && @"/\".IndexOf(ext) == -1)
                {
                    url = url.Replace(ext + "?", ext + "_zfr?");// ".shtml");
                    query = query.Replace(ext + "?", ext + "_zfr?");
                }

                queryRep =RemoveForbidChar(query);//
                //for (int i = 0; i < Pattern.Length; i++)
                //{
                //    if (Regex.IsMatch(queryRep, Pattern[i], RegexOptions.IgnoreCase | RegexOptions.Compiled))
                //    {
                //        queryRep = Regex.Replace(queryRep, Pattern[i], Replace[i], RegexOptions.Compiled | RegexOptions.IgnoreCase);
                //    }
                //}
                queryRep = "/"+queryRep.Replace("amp;", "");
                if (queryRep.Length > 48)
                    queryRep = "/long_" +queryRep.Length +"_"+queryRep.GetHashCode()+"_";

                url = url.Replace(query, queryRep)+IndexFile;//|\/:*?<>"";//
            }
        }
        public static string RemoveForbidCharToPath(ref string str)
        { 
            RemoveForbidChar(ref str);
            str = str.Insert(1, ":");
            return str;
        }
        public static string RemoveForbidChar(string str)
        {
            RemoveForbidChar(ref str);
            return str;
        }
        public static string RemoveForbidChar(ref string str)
        {
            for (int i = 0; i < Pattern.Length; i++)
            {
                if (Regex.IsMatch(str, Pattern[i], RegexOptions.IgnoreCase | RegexOptions.Compiled))
                {
                    str = Regex.Replace(str, Pattern[i], Replace[i], RegexOptions.Compiled | RegexOptions.IgnoreCase);
                }
            }
            return str;//
        }
        #endregion
        #region ConvertToFilePath
        public static string ConvertToFilePath(string basePath, string uri)
        {
           return ConvertToFilePath(basePath,new Uri(uri));
        }
        public static string ConvertToFilePath(string basePath, Uri uri)
        {
            string result = basePath;
            int index1;
            int index2;

            // add ending slash if needed
            if (result[result.Length - 1] != '\\')
                result = result + "\\";

            // strip the query if needed

            String path = uri.PathAndQuery;
            int queryIndex = path.IndexOf("?");
            if (queryIndex != -1)
                path = path.Substring(0, queryIndex);

            // see if an ending / is missing from a directory only

            int lastSlash = path.LastIndexOf('/');
            int lastDot = path.LastIndexOf('.');

            if (path[path.Length - 1] != '/')
            {
                if (lastSlash > lastDot)
                    path += "/" + IndexFile;
            }

            // determine actual filename		
            lastSlash = path.LastIndexOf('/');

            string filename = "";
            if (lastSlash != -1)
            {
                filename = path.Substring(1 + lastSlash);
                path = path.Substring(0, 1 + lastSlash);
                if (filename.Equals(""))
                    filename = IndexFile;
            }

            path = "/" + uri.Host + "/" + path;//zfr
            // create the directory structure, if needed

            index1 = 1;
            do
            {
                index2 = path.IndexOf('/', index1);
                if (index2 != -1)
                {
                    string dirpart = "\\" + path.Substring(index1, index2 - index1);
                    result += dirpart;
                    result += "\\";

                    //try
                    //{
                    //    Directory.CreateDirectory(result);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Log.Write(ex,"Ŀ¼:"+result);
                    //}
                    index1 = index2 + 1;
                }
            } while (index2 != -1);

            // attach name			
            result += filename;
            filename = null;//

            // DoReplace(ref result);//
            // if (!Directory.Exists(Path.GetDirectoryName(result)))
            // Directory.CreateDirectory(Path.GetDirectoryName(result));//zfr

            return result;
        }
        #endregion
    }
}
