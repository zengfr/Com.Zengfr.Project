using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask.youzan.gys
{
    public class List
    {
        public String alias;

        public String category;

        public int deposit;

        public Boolean is_quit_supplier;

        public int kdt_id;

        public String logo;

        public int seller_num;

        public int goods_num;

        public String team_name;

        public List<Top_goods_list> top_goods_list;
    }
}
