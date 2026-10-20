using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask.ximalaya.entity
{
    
    public class TracksAudioPlayItem
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
        /// 《快乐致富》第十一章 一生中的转折点
        /// </summary>
        public string trackName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string trackUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string trackCoverPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int albumId { get; set; }
        /// <summary>
        /// 快乐致富|吉姆·罗恩（每日更）
        /// </summary>
        public string albumName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string albumUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int anchorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string canPlay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isBaiduMusic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isPaid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int duration { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string src { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hasBuy { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string albumIsSample { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sampleDuration { get; set; }
        /// <summary>
        /// 11月前
        /// </summary>
        public string updateTime { get; set; }
        /// <summary>
        /// 11月前
        /// </summary>
        public string createTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isLike { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string isCopyright { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string firstPlayStatus { get; set; }
    }

    public class playalbumlistData
    {
        /// <summary>
        /// 
        /// </summary>
        public int uid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int albumId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sort { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageNum { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int pageSize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TracksAudioPlayItem> tracksAudioPlay { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string hasMore { get; set; }
    }

    public class playalbumlist
    {
        /// <summary>
        /// 
        /// </summary>
        public int ret { get; set; }
        /// <summary>
        /// 声音播放数据
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public playalbumlistData data { get; set; }
    }
}
