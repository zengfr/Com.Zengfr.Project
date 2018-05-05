using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Com.Zengfr.Proj.Common.NH
{
    public class SchemaUpdateUtils
    {
        public static void ExportSql(Configuration config)
        {
            var se = new NHibernate.Tool.hbm2ddl.SchemaExport(config);
            var sb = new StringBuilder();
            TextWriter output = new StringWriter(sb);
            se.SetDelimiter(";");
            se.Execute(true, false, false, null, output);

            var sql = sb.ToString();
            WriteFile("SchemaExport.sql", sql);
            WriteFile("SchemaExport_noConstraint.sql", RemoveConstraint(sql));

            sb = null; output.Close(); se = null;
        }
        protected static string RemoveConstraint(string sql)
        {
            var regexOptions = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline;
            sql = Regex.Replace(sql, "add constraint(.*?);", "--c--", regexOptions);
            sql = Regex.Replace(sql, "add constraint(.*?)references(.*?)\n", "--c--\n", regexOptions);
            sql = Regex.Replace(sql, "alter table(.*?)--c--", "--", RegexOptions.RightToLeft | regexOptions);
            sql = Regex.Replace(sql, "(--|\r|\n|\t)", string.Empty);
            sql = Regex.Replace(sql, @"\s{2,}", string.Empty);
            sql = Regex.Replace(sql, "IDENTITY NOT NULL", "IDENTITY(12345,1) NOT NULL");
            return sql;
        }
        public static void ExportUpdateSql(Configuration config)
        {
            var f = string.Format("{0}\\SchemaUpdate{1}.sql", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));

            StringBuilder stringBuilder = new System.Text.StringBuilder();
            Action<string> updateExport = x => stringBuilder.Append(x);

            var su = new SchemaUpdate(config);
            su.Execute(updateExport, false);

            stringBuilder.AppendLine();
            var updatesSql = RemoveConstraint(stringBuilder.ToString());
            stringBuilder.Clear(); stringBuilder = null;
            if (!string.IsNullOrWhiteSpace(updatesSql)&& updatesSql.Length>2)
            {
                using (var file = new FileStream(f, FileMode.Append, FileAccess.Write))
                using (var sw = new StreamWriter(file, Encoding.UTF8))
                {
                    sw.Write(updatesSql);
                }
            }
            
        }
        public static string GetBaseDirectoryPath(string subDir)
        {
            var dir = string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, subDir);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }
        protected static void WriteFile(string file, string content)
        {
            File.WriteAllText(string.Format("{0}\\{1}", AppDomain.CurrentDomain.BaseDirectory, file), content, Encoding.UTF8);
        }
    }
}
