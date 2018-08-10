using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask.ximalaya.entity
{
    public class TracksItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int trackId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isPaid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int tag { get; set; }
        /// <summary>
        /// 使人赞同你的十二种方法 2：如何避免成为敌人（2）
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int playCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showLikeBtn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isLike { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showShareBtn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showCommentBtn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string showForwardBtn { get; set; }
        /// <summary>
        /// 1年前
        /// </summary>
        public string createDateFormat { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
    }

    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public int currentUid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int albumId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int trackTotalCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TracksItem> tracks { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageSize { get; set; }
    }

    public class getTracksListRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public int ret { get; set; }
        /// <summary>
        /// 成功
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Data data { get; set; }
    }
}
