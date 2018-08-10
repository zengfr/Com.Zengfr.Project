using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask.youzan.marketdetail
{
    public class Goods_detail
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string alias { get; set; }
        /// <summary>
        /// 【清新口气 /  漱口5秒=刷牙一次】SPLAT 牙齿口腔清洁护理泡沫 美白牙齿 + 防止蛀牙+ 缓解牙龈发炎 /活性炭牙膏 恋爱牙膏 儿童牙膏（冰淇淋味）
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string price { get; set; }
    /// <summary>
    /// 
    /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string class1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string class2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string kdt_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string buy_way { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pre_sale { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string kdt_goods_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_display { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sold_status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fx_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string min_retail_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string max_retail_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fx_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fx_delivery_template_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string created_at { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fx_description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string recommend_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fx_auth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string level_discount_auth { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fx_description_template_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fx_description_type { get; set; }
        /// <summary>
        /// 7天无理由退换货，必须要求包装完好不影响二次销售，来回运费需分销商承担，造成严重损坏的需赔付产品损失。发货时造成的质量问题由商家承担.
        /// </summary>
        public string after_sales_policy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string distribution_policy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public Stock stock { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string supplier_goods_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int stock_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sold_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int is_discount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string origin_fx_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
       /* public Fx_price_range fx_price_range { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Origin_fx_price_range origin_fx_price_range { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Min_retail_price_range min_retail_price_range { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Max_retail_price_range max_retail_price_range { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Profit profit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Profit_rate profit_rate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_activity_goods { get; set; }*/
        /// <summary>
        /// 
        /// </summary>
        public List<string> activity_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string activity_fx_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string activity_retail_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int activity_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string default_retail_price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> attachment_urls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> thumb_urls { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_best { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_brand { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string brand_name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public Delivery_template delivery_template { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int total_sold_num { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_added { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string allow_add { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string seller_goods_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_supplier_goods { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string supplier_goods_is_display { get; set; }
    }
}
