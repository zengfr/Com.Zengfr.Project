using System;
using System.Collections.Generic;
using System.Xml;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Zfrong.Framework.Mvc.Result
{
    public class XmlActionResult : ActionResult
    {
        public XDocument Xml { get; protected set; }
        public string ContentType { get; set; }
        public Encoding Encoding { get; set; }
        public XmlActionResult():base()
        {
            this.ContentType = "text/xml";
            this.Encoding = Encoding.UTF8;
        }
        public XmlActionResult(XDocument xml)
        {
            this.Xml = xml;
            this.ContentType = "text/xml";
            this.Encoding = Encoding.UTF8;
        }
       
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.ContentType = this.ContentType;
            response.HeaderEncoding = this.Encoding;

            var writer = new XmlTextWriter(context.RequestContext.HttpContext.Response.OutputStream, this.Encoding);
            Xml.WriteTo(writer);
            writer.Close();
        }
    }

}