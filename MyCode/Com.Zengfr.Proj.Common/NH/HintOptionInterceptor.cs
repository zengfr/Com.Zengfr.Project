
namespace Com.Zengfr.Proj.Common.NH
{
    using NHibernate;
    using NHibernate.SqlCommand;
    using log4net;
    [System.Serializable]
    public class HintOptionInterceptor : EmptyInterceptor, IInterceptor
    {
        internal const string Table_COMMENT = " hint-table:";
        internal const string OPTION_COMMENT = " hint-option:";
        internal const string SQL_COMMENT = " hint-sql:";
        static ILog log = LogManager.GetLogger(typeof(HintOptionInterceptor));
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            if (sql.ToString().Contains(" hint-sql:"))
            {
                sql = HintOptionInterceptor.InsertHintSql(sql);
            }
            if (sql.ToString().Contains(" hint-option:"))
            {
                sql = HintOptionInterceptor.InsertHintOption(sql);
            }
            if (sql.ToString().Contains(" hint-table:"))
            {
                sql = HintOptionInterceptor.ApplyHintTable(sql);
            }
            return base.OnPrepareStatement(sql);
        }
        private static SqlString InsertHintSql(SqlString sql)
        {
            return HintOptionInterceptor.InsertHintString(sql, " hint-sql:", " {0}");
        }
        private static SqlString InsertHintOption(SqlString sql)
        {
            return HintOptionInterceptor.InsertHintString(sql, " hint-option:", " option({0})");
        }
        private static SqlString ApplyHintTable(SqlString sql)
        {
            var sqlString = sql.ToString();
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(string.Format("{0}(.+?){{", " hint-table:"));
            System.Text.RegularExpressions.MatchCollection matchCollection = regex.Matches(sqlString);
            if (matchCollection.Count == 0)
            {
                throw new System.InvalidOperationException("Could not find tables");
            }
            foreach (System.Text.RegularExpressions.Match match in matchCollection)
            {
                string value = match.Groups[1].Value;
                sql = HintOptionInterceptor.ApplyHintTable(sql, value);
            }
            return sql;
        }
        private static SqlString InsertHintString(SqlString sql, string commentType, string format)
        {
            var sqlString = sql.ToString();
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(string.Format("{0}{{(.*?)}}", commentType));
            System.Text.RegularExpressions.MatchCollection matchCollection = regex.Matches(sqlString);
            if (matchCollection.Count == 0)
            {
                throw new System.InvalidOperationException("Could not find Hint Comment：" + commentType);
            }
            foreach (System.Text.RegularExpressions.Match match in matchCollection)
            {
                string text = match.Groups[1].Value;
                text = string.Format(format, text);
                sql = sql.Insert(sql.Length, text);
            }
            return sql;
        }
        private static SqlString ApplyHintTable(SqlString sql, string table)
        {
            var sqlString = sql.ToString();
            int startIndex = sqlString.IndexOf(" hint-table:" + table);
            int startIndex2 = sqlString.IndexOf("{", startIndex);
            int num = sqlString.IndexOf("{", startIndex2) + 1;
            int num2 = sqlString.IndexOf("}", num);
            if (num2 < 0)
            {
                throw new System.InvalidOperationException("hint comment should contain content of table");
            }
            string content = " " + sqlString.Substring(num, num2 - num).Trim();
            return HintOptionInterceptor.ApplyHintTable(sql, table, content);
        }
        private static SqlString ApplyHintTable(SqlString sql, string table, string content)
        {
            var sqlString = sql.ToString();
            int startat = sqlString.LastIndexOf(" hint-table:" + table);
            var pattern = table.Replace("[", @"\[").Replace("]", @"\]") + "\\s(\\w+)";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            System.Text.RegularExpressions.MatchCollection matchCollection = regex.Matches(sqlString, startat);
            if (matchCollection.Count == 0)
            {
                throw new System.InvalidOperationException("Could not find aliases for table with name: " + table);
            }
            int num = 0;
            foreach (System.Text.RegularExpressions.Match match in matchCollection)
            {
                int valueLength = match.Groups[1].Value.Length;
                int num2 = match.Groups[1].Index + num + valueLength;
                sql = sql.Insert(num2, content);
                num += content.Length;
            }
            table = null;
            return sql;
        }

        #region
        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            return base.OnSave(entity, id, state, propertyNames, types);
        }
        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            return base.OnFlushDirty(entity, id, currentState, previousState, propertyNames, types);
        }
        public override void OnDelete(object entity, object id, object[] state, string[] propertyNames, NHibernate.Type.IType[] types)
        {
            base.OnDelete(entity, id, state, propertyNames, types);
        }
        #endregion
    }
}
