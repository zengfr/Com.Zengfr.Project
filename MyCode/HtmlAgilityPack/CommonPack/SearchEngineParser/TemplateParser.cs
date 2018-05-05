using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;//
using Com.Zfrong.Xml;
using CommonPack.HtmlAgilityPack;
namespace ParserEngine
{
   public class TemplateParser
   {
       #region  ####
       public static List<Table> BuildTables(string input, IList<TableTemplate> tableTemplates)
        {
            List<Table> tables = new List<Table>();
            string value = input;
            Table table; 
            foreach (TableTemplate tableTemplate in  tableTemplates)
            {
               value = TemplateHelper.GetActionValue(input, tableTemplate.AreaActions);//box匹配 
               table = BuildTable(value, tableTemplate);
               tables.Add(table);//
            }
            value = null; 
            return tables;
        }
       public static Table BuildTable(string input, TableTemplate tableTemplate)
       {
           string value = input;
           Table table = new Table(tableTemplate.Name);
           table.Rows = BuildRows(value, tableTemplate.RowTemplate);//Rows
           value = null;
           return table;

       }
       public static List<Row> BuildRows(string input, RowTemplate rowTemplate)
        {
            List<Row> rows = new List<Row>();
            List<string> objs=null;
            objs = TemplateHelper.GetActionValue(input, rowTemplate.AreaAction);//Row匹配
            for (int i = 0; i < objs.Count; i++)
            {
                rows.Add(BuildRow(objs[i], rowTemplate));//Row
            } 
            objs = null;
            return rows;
        }
       private static Row BuildRow(string input, RowTemplate rowTemplate)
        {
            Row row = new Row(rowTemplate.Name);
            row.Cells = BuildCells(input,rowTemplate );
            return row;
        }
       private static List<Cell> BuildCells(string input, RowTemplate rowTemplate)
       {
           List<Cell> cells = new List<Cell>();
           string value = input;
           Cell obj;
           foreach (CellTemplate t in rowTemplate.CellTemplates)
           {
               obj = new Cell(t.Name);
               value =TemplateHelper.GetActionValue(input, t.AreaActions);//cell匹配
               obj.Value = value;//
               cells.Add(obj);//cell
           }
           value = null;
           return cells;
       }
        #endregion
    }
}
