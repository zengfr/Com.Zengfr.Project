
using System.Web;
namespace Com.Zengfr.Proj.Common
{
    public class WebUtils
    {
        public static string WebsiteBaseUrl
        {
            get
            {
                var scheme = HttpContext.Current.Request.Url.Scheme;
                var authority = HttpContext.Current.Request.Url.Authority;
                return string.Format("{0}://{1}", scheme, authority);
            }
        }
    }
}
