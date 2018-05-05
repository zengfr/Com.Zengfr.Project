using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;

public class NpoiExport : IDisposable
{
    const int MaximumNumberOfRowsPerSheet = 65500;
    const int MaximumSheetNameLength = 25;
    protected IWorkbook Workbook { get; set; }

    public NpoiExport(int type)
    {
        switch (type) {
            case 1:
                this.Workbook = new XSSFWorkbook();break;
            case 2:
                this.Workbook = new XSSFWorkbook(); break;
            default:
                this.Workbook = new HSSFWorkbook(); break;

        }
        this.Workbook.MissingCellPolicy = MissingCellPolicy.RETURN_BLANK_AS_NULL;
    }
    
    protected string EscapeSheetName(string sheetName)
    {
        var escapedSheetName = sheetName
                                    .Replace("/", "-")
                                    .Replace("\\", " ")
                                    .Replace("?", string.Empty)
                                    .Replace("*", string.Empty)
                                    .Replace("[", string.Empty)
                                    .Replace("]", string.Empty)
                                    .Replace(":", string.Empty);

        if (escapedSheetName.Length > MaximumSheetNameLength)
            escapedSheetName = escapedSheetName.Substring(0, MaximumSheetNameLength);

        return escapedSheetName;
    }

    protected ISheet CreateExportDataTableSheetAndHeaderRow(DataTable exportData, string sheetName, ICellStyle headerRowStyle)
    {
        var sheet = this.Workbook.CreateSheet(EscapeSheetName(sheetName));

        if (exportData.Columns.Count >0|| exportData.Rows.Count > 0)
        {
            var row = sheet.CreateRow(0);

            for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
            {
                var cell = row.CreateCell(colIndex);
                cell.SetCellValue(exportData.Columns[colIndex].ColumnName);

                if (headerRowStyle != null)
                    cell.CellStyle = headerRowStyle;
            }
        }

        return sheet;
    }

    public void ExportDataTableToWorkbook(DataTable exportData, string sheetName)
    {
        // Create the header row cell style
        var headerLabelCellStyle = this.Workbook.CreateCellStyle();
        headerLabelCellStyle.BorderBottom = BorderStyle.Thin;//  CellBorderType.THIN;
        headerLabelCellStyle.BorderTop = BorderStyle.Thin;
        headerLabelCellStyle.BorderLeft = BorderStyle.Thin;
        headerLabelCellStyle.BorderRight = BorderStyle.Thin;
        headerLabelCellStyle.WrapText = false;

        var headerLabelFont = this.Workbook.CreateFont();
        headerLabelFont.FontHeightInPoints = 12;
        headerLabelFont.Boldweight = (short)FontBoldWeight.Bold;
        headerLabelCellStyle.SetFont(headerLabelFont);

        var sheet = CreateExportDataTableSheetAndHeaderRow(exportData, sheetName, headerLabelCellStyle);

        if (exportData.Rows.Count > 0)
        {
            var currentNPOIRowIndex = 1;
            var sheetCount = 1;

            var dataCellStyle = this.Workbook.CreateCellStyle();
            dataCellStyle.BorderBottom = BorderStyle.Thin;
            dataCellStyle.BorderTop = BorderStyle.Thin;
            dataCellStyle.BorderLeft = BorderStyle.Thin;
            dataCellStyle.BorderRight = BorderStyle.Thin;
            dataCellStyle.WrapText = false;
            for (var rowIndex = 0; rowIndex < exportData.Rows.Count; rowIndex++)
            {
                if (currentNPOIRowIndex >= MaximumNumberOfRowsPerSheet)
                {
                    sheetCount++;
                    currentNPOIRowIndex = 1;

                    sheet = CreateExportDataTableSheetAndHeaderRow(exportData,
                                                                   sheetName + "-" + sheetCount,
                                                                   headerLabelCellStyle);
                }

                var row = sheet.CreateRow(currentNPOIRowIndex++);

                for (var colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
                {
                    var cell = row.CreateCell(colIndex); cell.CellStyle = dataCellStyle;
                    cell.SetCellValue(exportData.Rows[rowIndex][colIndex].ToString());
                }
            }
        }
        //add 20161016
        for (int colIndex = 0; colIndex < exportData.Columns.Count; colIndex++)
        {
            sheet.AutoSizeColumn(colIndex, false);
        }
    }

    public byte[] GetBytes()
    {
        using (var buffer = new NPOIMemoryStream(false))
        {
            this.Workbook.Write(buffer);
            return buffer.ToArray();
        }
    }
    public void Dispose()
    {
        if (this.Workbook != null)
            this.Workbook.Close();// Dispose();
    }
    protected class NPOIMemoryStream : MemoryStream
    {
        public bool AllowClose { get; set; }
        public NPOIMemoryStream():this(true)
        {

        }
        public NPOIMemoryStream(bool allowClose)
        {
            AllowClose = allowClose;
        }
        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}