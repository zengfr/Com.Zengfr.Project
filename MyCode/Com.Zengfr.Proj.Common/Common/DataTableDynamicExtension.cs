using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common
{
    public static class DataTableDynamicExtension
    {
        public static void AddColumn<T>(this DataTable dt, string colName )
        {
            AddColumn<T>(dt,  colName,  default(T));
        }
        public static void AddColumn<T>(this DataTable dt, string colName, T defaultT)
        {
            if (!dt.Columns.Contains(colName))
                dt.Columns.Add(colName, typeof(T));
        }
        public static void AddColumns<T>(this DataTable dt, params string[] colNames)
        {
            foreach (string colName in colNames)
            {
                if (!dt.Columns.Contains(colName))
                    dt.Columns.Add(colName, typeof(T));
            }
        }
        public static void RemoveColumns(this DataTable dt, params string[] colNames)
        {
            foreach (string colName in colNames)
            {
                if (dt.Columns.Contains(colName))
                    dt.Columns.Remove(colName);
            }
        }
        public static void AddColumnsByDays<T>(this DataTable dt, string colPrefix, int days)
        {
            AddColumnsByDays<T>(dt, colPrefix, 0, days);
        }
        public static void AddColumnsByDays<T>(this DataTable dt, string colPrefix, int startDay, int endDay)
        {
            var colNames = BuildColumnNamesByDays(dt, colPrefix, startDay, endDay);
            AddColumns<T>(dt, colNames.ToArray());
        }
        public static void AddColumnsByIndexs<T>(this DataTable dt, string colPrefix, int startIndex, int endIndex)
        {
            var colNames = BuildColumnNamesByIndexs(dt, colPrefix, startIndex, endIndex);
            AddColumns<T>(dt, colNames.ToArray());
        }
        public static void RemoveColumnsByDays<T>(this DataTable dt, string colPrefix, int startDay, int endDay)
        {
            var colNames = BuildColumnNamesByDays(dt, colPrefix, startDay, endDay);
            RemoveColumns(dt, colNames.ToArray());
        }
        public static void RemoveColumnsByIndexs(this DataTable dt, string colPrefix, int startIndex, int endIndex)
        {
            var colNames = BuildColumnNamesByIndexs(dt, colPrefix, startIndex, endIndex);
            RemoveColumns(dt, colNames.ToArray());
        }
        public static List<string> BuildColumnNamesByDays(this DataTable dt, string colPrefix, int startDay, int endDay)
        {
            var colNames = new List<string>();
            for (int i = startDay; i < endDay; i++)
            {
                colNames.Add(BuildColumnNameByDay(dt, colPrefix, i));
            }
            return colNames;
        }
        public static string BuildColumnNameByDay(this DataTable dt, string colPrefix, int day)
        {
            return string.Format("{0}{1}", colPrefix, DateTime.Today.AddDays(day).ToString("MMdd"));
        }
        public static List<string> BuildColumnNamesByIndexs(this DataTable dt, string colPrefix, int startIndex, int endIndex)
        {
            var colNames = new List<string>();
            for (int i = startIndex; i < endIndex; i++)
            {
                colNames.Add(BuildColumnNameByIndex(dt, colPrefix, i));
            }
            return colNames;
        }
        public static string BuildColumnNameByIndex(this DataTable dt, string colPrefix, int index)
        {
            return string.Format("{0}{1}", colPrefix, index.ToString().PadLeft(2, '0'));
        }

        public static void SetPrimaryKeyColumns<T>(this DataTable dt, params string[] keyColNames)
        {
            AddColumns<T>(dt, keyColNames);
            var cols = new DataColumn[keyColNames.Length];
            int index = 0;
            foreach (string keyColName in keyColNames)
            {
                cols[index++] = dt.Columns[keyColName];
            }
            dt.PrimaryKey = cols;
        }

        public static DataRow AddRow<K>(this DataTable dt, params K[] ks)
        {
            if (dt.PrimaryKey != null && dt.PrimaryKey.Length == ks.Length)
            {
                var dr = dt.NewRow();
                int index = 0;
                foreach (K k in ks)
                {
                    var keyColumnName = dt.PrimaryKey[index++].ColumnName;
                    dr[keyColumnName] = k;
                }
                dt.Rows.Add(dr);
                return dr;
            }
            return null;
        }
        public static bool PrimaryKeyIsEqual(this DataTable sourceDataTable, DataTable targetDataTable)
        {
            if (sourceDataTable.PrimaryKey != null && targetDataTable.PrimaryKey != null &&
               string.Join("", sourceDataTable.PrimaryKey.Select(t => t.ColumnName)) ==
               string.Join("", targetDataTable.PrimaryKey.Select(t => t.ColumnName))
               )
            {
                return true;
            }
            return false;
        }
        public static void MergeTableFrom<K>(this DataTable targetDataTable, DataTable sourceDataTable, bool allowUpdateToNull)
        {
            var sourceColumns = sourceDataTable.GetColumnNames();
            MergeTable<K>(sourceDataTable, targetDataTable, allowUpdateToNull);
        }
        public static void MergeTableFrom<K>(this DataTable targetDataTable, DataTable sourceDataTable, bool allowUpdateToNull, params string[] sourceColumns)
        {
            MergeTable<K>(sourceDataTable, targetDataTable, allowUpdateToNull, sourceColumns);
        }
        public static void MergeTable<K>(this DataTable sourceDataTable, DataTable targetDataTable, bool allowUpdateToNull)
        {
            var sourceColumns = sourceDataTable.GetColumnNames();
            MergeTable<K>(sourceDataTable, targetDataTable, allowUpdateToNull, sourceColumns);
        }
        public static void MergeTable<K>(this DataTable sourceDataTable, DataTable targetDataTable,bool allowUpdateToNull, params string[] sourceColumns)
        {
            var primaryKeyEqual = PrimaryKeyIsEqual(sourceDataTable, targetDataTable);
            if (primaryKeyEqual)
            {
                foreach (string sourceColumn in sourceColumns)
                {
                    if (!targetDataTable.Columns.Contains(sourceColumn))
                    {
                        targetDataTable.Columns.Add(sourceColumn);
                    }
                }
                var keyColumnNames = GetPrimaryKeyColumnNames(sourceDataTable);
                foreach (DataRow dr in sourceDataTable.Rows)
                {
                    var keys = DataRowDynamicExtension.GetPrimaryKeys<K>(dr);
                    foreach (string sourceColumn in sourceColumns)
                    {
                        if (allowUpdateToNull || (dr[sourceColumn] != null && dr[sourceColumn] != DBNull.Value &&!string.IsNullOrWhiteSpace(dr[sourceColumn].ToString())))
                        {
                            targetDataTable.SetValue(keys, sourceColumn, dr[sourceColumn]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 复制列值 整列
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="targetColumn"></param>
        public static void CopyColumn(this DataTable dataTable, string sourceColumn, string targetColumn)
        {
            foreach (DataRow dr in dataTable.Rows)
            {
                dr[targetColumn] = dr[sourceColumn];
            }
        }
        /// <summary>
        /// 复制列值 单行
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="targetColumn"></param>
        /// <param name="ks"></param>
        public static void CopyColumnByKey<K>(this DataTable dataTable, string sourceColumn, string targetColumn, K k)
        {
            CopyColumnByKey<K>(dataTable, sourceColumn, targetColumn, new K[] { k });
        }
        /// <summary>
        /// 复制列值 单行
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="sourceColumn"></param>
        /// <param name="targetColumn"></param>
        /// <param name="ks"></param>
        public static void CopyColumnByKey<K>(this DataTable dataTable, string sourceColumn, string targetColumn, K[] ks)
        {
            var dr = FindDataRow<K>(dataTable, ks);
            if (dr != null)
            {
                dr[targetColumn] = dr[sourceColumn];
            }

        }
        public static string[] GetColumnNames(this DataTable dt)
        {
            var columnNames = new List<string>();
            foreach (DataColumn dc in dt.Columns)
                columnNames.Add(dc.ColumnName);
            return columnNames.ToArray();
        }
        public static string[] GetPrimaryKeyColumnNames(this DataTable dt)
        {
            var columnNames = new List<string>();
            foreach (var dc in dt.PrimaryKey)
                columnNames.Add(dc.ColumnName);
            return columnNames.ToArray();
        }

        public static void SetValue<K, V>(this DataTable dt, K k, string col, V v)
        {
            SetValue<K, V>(dt, new K[] { k }, col, v);
        }
        public static void SetValue<K, V>(this DataTable dt, K[] ks, string col, V v)
        {
            AddColumns<V>(dt, col);
            var dr = dt.FindDataRow<K>(ks);
            if (dr == null)
            {
                dr = dt.AddRow<K>(ks);
            }
            dr[col] = v;
        }
        public static DataRow FindDataRow<K>(this DataTable dt, K[] ks)
        {
            if (ks.Length == 1)
                return dt.Rows.Find(ks[0]);
            return dt.Rows.Find(ks);
        }
        public static String GetStringValue<K>(this DataTable dt, K k, string col)
        {
            return GetValue<K, String>(dt, k, col);
        }
        public static Int32 GetIntValue<K>(this DataTable dt, K k, string col)
        {
            return GetValue<K, Int32>(dt, k, col);
        }
        public static Int64 GetLongValue<K>(this DataTable dt, K k, string col)
        {
            return GetValue<K, Int64>(dt, k, col);
        }
        public static Decimal GetDecimalValue<K>(this DataTable dt, K k, string col)
        {
            return GetValue<K, Decimal>(dt, k, col);
        }
        public static DateTime GetDateTimeValue<K>(this DataTable dt, K k, string col)
        {
            return GetValue<K, DateTime>(dt, k, col);
        }
        public static V GetValue<K, V>(this DataTable dt, K k, string col)
        {
            return GetValue<K, V>(dt, new K[] { k }, col);
        }
        public static V GetValue<K, V>(this DataTable dt, K[] ks, string col)
        {
            return GetValue<K, V>(dt, ks, col, default(V));
        }
        public static V GetValue<K, V>(this DataTable dt, K[] ks, string col, V defaultV)
        {
            AddColumns<V>(dt, col);
            var dr = dt.FindDataRow<K>(ks);
            if (dr[col] != null && dr[col] != DBNull.Value)
                return (V)dr[col];
            return defaultV;
        }
        public static IList<V> GetValues<K, V>(this DataTable dt, K k, params string[] cols)
        {
            return GetValues<K, V>(dt, new K[] { k }, cols);
        }
        public static IList<V> GetValues<K, V>(this DataTable dt, K[] ks, params string[] cols)
        {
            AddColumns<V>(dt, cols);
            var dr = dt.FindDataRow<K>(ks);
            return cols.Where(t => dr[t] != null && dr[t] != DBNull.Value).Select(t => (V)dr[t]).ToList();
        }
        public static void MergeValueFrom<K>(this DataTable dt, K k, string col, char separator, params string[] cols)
        {
            MergeValueFrom<K>(dt, new K[] { k }, col, separator, cols);
        }
        public static void MergeValueFrom<K>(this DataTable dt, K[] ks, string col, char separator, params string[] cols)
        {
            var values = GetValues<K, string>(dt, ks, cols);
            if (values.Any())
            {
                SetValue<K, string>(dt, ks, col, string.Join("" + separator, values));
            }
        }
        public static void MergeLongValueSumFrom<K>(this DataTable dt, K k, string col, params string[] cols)
        {
            MergeValueSumFrom<K, Int64>(dt, new K[] { k }, col, cols);
        }
        public static void MergeIntValueSumFrom<K>(this DataTable dt, K k, string col, params string[] cols)
        {
            MergeValueSumFrom<K, Int32>(dt, new K[] { k }, col, cols);
        }
        public static void MergeDecimalValueSumFrom<K>(this DataTable dt, K k, string col, params string[] cols)
        {
            MergeValueSumFrom<K, Decimal>(dt, new K[] { k }, col, cols);
        }
        public static void MergeValueSumFrom<K, V>(this DataTable dt, K k, string col, params string[] cols)
        {
            MergeValueSumFrom<K, V>(dt, new K[] { k }, col, cols);
        }
        public static void MergeValueSumFrom<K, V>(this DataTable dt, K[] ks, string col, params string[] cols)
        {
            var values = GetValues<K, V>(dt, ks, cols);
            var type = typeof(V);
            if (values.Any())
            {
                switch (type.Name)
                {
                    case "Decimal":
                        SetValue<K, decimal>(dt, ks, col, values.Sum(t => decimal.Parse("" + t)));
                        break;
                    case "Int64":
                        SetValue<K, long>(dt, ks, col, values.Sum(t => long.Parse("" + t)));
                        break;
                    case "Int32":
                        SetValue<K, int>(dt, ks, col, values.Sum(t => int.Parse("" + t)));
                        break;
                    case "Double":
                        SetValue<K, double>(dt, ks, col, values.Sum(t => double.Parse("" + t)));
                        break;
                    case "Single":
                        SetValue<K, float>(dt, ks, col, values.Sum(t => float.Parse("" + t)));
                        break;
                }
            }
        }
        public static void MergeValue<K, V>(this DataTable dt, K k, string col, char separator, V v)
        {
            MergeValue<K, V>(dt, new K[] { k }, col, separator, v, 0);
        }
        public static void MergeValue<K, V>(this DataTable dt, K[] ks, string col, char separator, V v)
        {
            MergeValue<K, V>(dt, ks, col, separator, v, 0);
        }
        public static void MergeValue<K, V>(this DataTable dt, K k, string col, char separator, V v, int limit)
        {
            MergeValue<K, V>(dt, new K[] { k }, col, separator, v, limit);
        }
        public static void MergeValue<K, V>(this DataTable dt, K[] ks, string col, char separator, V v, int limit)
        {
            var value = GetValue<K, V>(dt, ks, col);
            var newValue = string.Format("{0}{1}{2}", value, separator, v);
            if (limit > 0)
            {
                var newValues = newValue.Split(separator);
                if (newValues.Length > limit)
                {
                    newValue = string.Join("" + separator, newValues.Where((t, i) => i >= newValues.Length - limit));
                }
            }
            SetValue<K, string>(dt, ks, col, newValue.Trim(separator));
        }
    }
}
