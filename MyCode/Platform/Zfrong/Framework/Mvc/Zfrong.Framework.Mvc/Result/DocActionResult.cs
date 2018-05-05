using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Web.UI;
namespace Zfrong.Framework.Mvc.Result
{
    public class WordActionResult<T> : DocActionResult<T>
    {
        public WordActionResult(IEnumerable<T> collection): base(collection, "application/vnd.ms-word","doc")
        {

        }
    }
    public class ExcelActionResult<T> : DocActionResult<T>
    {
        public ExcelActionResult(IEnumerable<T> collection)
            : base(collection, "application/vnd.ms-excel", "xls")
        {

        }
    }
    public class DocActionResult<T>:System.Web.Mvc.FileResult
    {
        private IEnumerable<T> data;
        public DocActionResult(IEnumerable<T> collection,string contentType,string ext)
            : base(contentType)
        {
            this.data = collection;
            this.FileDownloadName = DateTime.Now.ToString("yyyyMMdd,HHmmss,fff")+ext;
        }
        protected override void WriteFile(HttpResponseBase response)
        {
            StringWriter sw = new StringWriter();

            HtmlTextWriter hw = new HtmlTextWriter(sw);

            System.Web.UI.WebControls.GridView gv = new System.Web.UI.WebControls.GridView();
            gv.AllowPaging = false;
            gv.DataSource = data;
            gv.DataBind();

            gv.RenderControl(hw);

            response.Output.Write(sw.ToString());

            response.Flush();

            response.End(); 
        }
    }
}