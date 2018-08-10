using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask.youzan.marketdetail
{
    /// <summary>
    /// 商品 详情页 https://www.youzan.com/v2/fenxiao/market/detail.json?alias=2omn3z0lqs4tb&csrf_token=59851700379347860318930666842273372104167266342180527683054279358688253553305
    /// </summary>
    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
    }
}
