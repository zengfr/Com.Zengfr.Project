using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common
{
    public static class DataRowDynamicExtension
    {
        public static K[] GetPrimaryKeys<K>(this DataRow dr)
        {
            var keyColumnNames = DataTableDynamicExtension.GetPrimaryKeyColumnNames(dr.Table);
            return GetPrimaryKeys<K>(dr, keyColumnNames);
        }
        public static K[] GetPrimaryKeys<K>(this DataRow dr, string[] keyColumnNames)
        {
            var keys = new List<K>();
            foreach (var columnName in keyColumnNames)
            {
                keys.Add((K)dr[columnName]);
            }
            return keys.ToArray();
        }
        public static void CopyRow(this DataRow sourceRow, DataRow targetRow, bool allowUpdateToNull, params string[] copyColumns)
        {
            foreach (var copyColumn in copyColumns) {
                if (sourceRow.Table.Columns.Contains(copyColumn))
                {
                    if (!targetRow.Table.Columns.Contains(copyColumn))
                    {
                        targetRow.Table.AddColumn(copyColumn, sourceRow[copyColumn]);
                    }
                    if (allowUpdateToNull || (sourceRow[copyColumn] != null && sourceRow[copyColumn] != DBNull.Value && !string.IsNullOrWhiteSpace(sourceRow[copyColumn].ToString())))
                    {
                        targetRow[copyColumn] = sourceRow[copyColumn];
                    }
                }
            }
        }
        public static void CopyRowFrom(this DataRow targetRow, DataRow sourceRow, bool allowUpdateToNull, params string[] copyColumns)
        {
            CopyRow(sourceRow, targetRow,  allowUpdateToNull, copyColumns);
        }
    }
}
