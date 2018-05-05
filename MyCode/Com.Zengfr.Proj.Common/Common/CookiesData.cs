using System.Linq;
using Newtonsoft.Json;

namespace Com.Zengfr.Proj.Common
{
    public interface ICookiesData
    {
        string CookiesName { get; set; }
        int ExpiredMinutes { get; set; }
        string ToUserData();
    }
    /// <summary>
    /// 存储登录后的信息
    /// </summary>
    public class CookiesData : ICookiesData
    {
        public CookiesData()
        {
            CookiesName = string.Empty;
            ExpiredMinutes = 60 * 2;
        }
        /// <summary>
        /// 来源 app、web
        /// </summary>
        public virtual string CookiesFrom { get; set; }
        public virtual string CookiesName { get; set; }
        public virtual int ExpiredMinutes { get; set; }

        public virtual bool IsAuthenticated { get; set; }
        public virtual string[] UserRoles { get; set; }

        public virtual string Tel { get; set; }

        public virtual long HandlerID { get; set; }
        public virtual string HandlerName { get; set; }

        public virtual long HandlerCompanyID { get; set; }
        public virtual string HandlerCompanyName { get; set; }

        /// <summary>
        /// 律师
        /// </summary>
        public virtual long LawyerID { get; set; }
        public virtual string LawyerName { get; set; }
        public virtual string LawyerCode { get; set; }
        /// <summary>
        /// 律师的
        /// </summary>
        public virtual long MarketerID { get; set; }

        /// <summary>
        /// 所在 区县
        /// </summary>
        public virtual long DistrictID { get; set; }
        public virtual long CityID { get; set; }
        public virtual string ExceptionMessage { get; set; }
        /// <summary>
        /// VIP 类型（经办人个人/律师）
        /// </summary>
        public virtual byte UserVIPType { get; set; }
        /// <summary>
        /// VIP 类型（经办人公司）
        /// </summary>
        public virtual byte CompanyVIPType { get; set; }
        public virtual string ToUserData()
        {
            var data = string.Empty;
            data = JsonConvert.SerializeObject(this);
            return string.Format("#{0}#", data);
        }
        public virtual bool IsInRoles(string roles)
        {
            var rolesArray = roles.Split(new char[] { '#', ',', ';' });
            if (this.UserRoles != null && rolesArray != null)
            {
                var intersectArray = this.UserRoles.Intersect(rolesArray);
                return intersectArray.Count() > 0;
            }
            return false;
        }
        public static CookiesData GetUserData(string userData)
        {
            var cookiesData = JsonConvert.DeserializeObject<CookiesData>(userData.Trim('#'));
            return cookiesData;
        }


        /// <summary>
        /// 平台管理员
        /// </summary>
        public virtual long PlatformAccountID { get; set; }
        public virtual long ManagerAccountID { get; set; }
    }
}
