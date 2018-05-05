using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Com.Zfrong.Common.Data.AR.Base
{
    /// <summary>
    /// DisplayNameAttribute 的摘要说明
    /// </summary>
    public class DisplayAttribute : Attribute
    {
        public string Name;
        public DisplayAttribute()
            : base()
        {
            this.Name = "";
        }
        public DisplayAttribute(string name)
        {
            this.Name = name;
        }
        public DisplayAttribute(Type type)
        {
            this.Name = type.Name;
        }
    }
}
