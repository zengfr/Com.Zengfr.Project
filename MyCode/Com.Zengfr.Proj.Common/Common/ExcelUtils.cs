using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;


namespace Com.Zengfr.Proj.Common 
{
   public  class ExcelUtils
    {
        public enum ExcelType:int
        {
            xls2003=0,
            xlsx2007=1,
            stramfile =2
        }
        protected static readonly string Excel2007ContentType= "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        protected static readonly string Excel2003ContentType = "application/vnd.ms-excel";
        protected static readonly string StreamContentType = "application/octet-stream";
        public static string GetContentType(ExcelType type)
        {
            switch (type)
            {
                case ExcelType.xlsx2007:
                    return Excel2007ContentType;
                case ExcelType.stramfile:
                    return StreamContentType;
                default:
                    return Excel2003ContentType;
            }
        }
        public static string GetFileNameExt(ExcelType type)
        {
            switch (type)
            {
                case ExcelType.xlsx2007:
                    return "xlsx";
                case ExcelType.stramfile:
                    return "xlsx";
                default:
                    return "xls";
            }
        }
        public static void AddContentTypeAndHeader(System.Web.HttpResponseBase Response, ExcelType type, string filename, long fileSize)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                filename = string.Format("export-{0}.{1}", DateTime.Now.ToString("yyyyMMdd-HHmmss-fff"),GetFileNameExt(type));
            }
            Response.ContentType = GetContentType(type); 
            Response.AddHeader("Content-Length", fileSize.ToString());
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
        }
        public static void RemoveAndClearResponse(System.Web.HttpResponseBase response)
        {
            response.ClearContent();
            response.ClearHeaders();
            response.Clear();
        }
       
        public static void CreateExcel2Stream(DataSet ds,Stream outputStream, int type)
        {
            byte[] buffer = ExcelUtils.CreateExcel2Byte(ds,type);
            outputStream.Write(buffer, 0, buffer.Length);
            outputStream.Position = 0;
        }
        public static DataSet CreateTestDataSet()
        {
            DataSet ds = new DataSet();
            var dt = new DataTable();
            dt.Columns.Add("col1");
            dt.Columns.Add("col2");
            dt.Columns.Add("col3");
            ds.Tables.Add(dt);
            return ds;
        }
        public static byte[] CreateExcel2Byte(DataSet ds,int type)
        {
            using (var exporter = new NpoiExport(type))
            {
                int index = 0;
                foreach (DataTable dt in ds.Tables)
                {
                    exporter.ExportDataTableToWorkbook(dt, "sheet"+index++);
                }
                byte[] fileBuffer = exporter.GetBytes();
                return fileBuffer;
            }
        }
    }
}
