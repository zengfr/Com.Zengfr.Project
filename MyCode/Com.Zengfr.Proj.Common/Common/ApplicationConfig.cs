using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Zengfr.Proj.Common
{
    public class ApplicationConfig
    {
        /// <summary>
        /// 订单过期失效日期
        /// </summary>
        public static int ItemOrderExpiredFailureDay { get; set; }
        /// <summary>
        /// 平台佣金比例
        /// </summary>
        public static decimal? CommissionRate { get; set; }
        /// <summary>
        /// 直接推广费比例
        /// </summary>
        public static decimal? PromotionBonusRateA { get; set; }
        /// <summary>
        /// 间接推广费比例
        /// </summary>
        public static decimal? PromotionBonusRateB { get; set; }

        static ApplicationConfig()
        {
            ItemOrderExpiredFailureDay = AppSettingsUtils.Get("ItemOrderExpiredFailureDay", 7);
            CommissionRate = 0.2M;
            PromotionBonusRateA = 0.5M;
            PromotionBonusRateB = 0.1M;
        }
    }
}
