using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
namespace  Stock.Utils
{
    public class StockTradeConvert
    {
        public static IList<T> Convert<T>(IEnumerable<StockTradeHistory> stockTradeItems, int type, int kind)
            where T : StockTradeHistoryStatistic, new()
        {
            IList<T> items = new List<T>();
            if (stockTradeItems.Any())
            {
                var itemsGrouped = stockTradeItems.GroupBy(t=>GetMinDateTime(t.OpenDateTime,type));
                foreach (var itemGrouped in itemsGrouped)
                {
                    var item = BuidItem<T>(kind, itemGrouped);
                    items.Add(item);
                }
            }
            return items;

        }
        protected static T BuidItem<T>(int kind,IGrouping<DateTime, StockTradeHistory> itemsGrouped) where T : StockTradeHistoryStatistic, new()
        {
            T item = new T();

            item.Symbol = itemsGrouped.First().Symbol;
            item.OpenDate = itemsGrouped.Key.Date;
            item.OpenDateTime = itemsGrouped.Key;
            item.kind = kind;

            var items2 = itemsGrouped.Where(t => t.性质 == kind);
            if (items2.Any())
            {
                item.Open = items2.FirstOrDefault().成交价;
                item.Close = items2.LastOrDefault().成交价;

                item.High = items2.Max(t => t.成交价);
                item.Low = items2.Min(t => t.成交价);

                item.Volume = items2.Sum(t => t.成交量手??0);
                if (item.Volume > 0)
                {
                    item.均价 = items2.Sum(t => t.成交量手 * t.成交价) / item.Volume;
                }
            }
            return item;
        }
        //protected static int GetIndex(DateTime openDateTime, int type)
        //{
        //    var startTime1 = DateTime.Parse(openDateTime.ToString("yyyy-MM-dd 09:30:00"));
        //    var startTime2 = DateTime.Parse(openDateTime.ToString("yyyy-MM-dd 13:00:00"));
        //    var isPM = openDateTime >= startTime2;

        //    var step = Math.Floor((startTime2 - DateTime.Parse(openDateTime.ToString("yyyy-MM-dd 11:30:00"))).TotalMinutes);

        //    var totalMinutes = Math.Floor((openDateTime - startTime1).TotalMinutes);
            
        //    var indexd = (isPM ?(totalMinutes -step): totalMinutes) / type;
        //    var index = (int)indexd;
        //    return (int)indexd;
        //}
        ///// <summary>
        ///// type K线 的最大index
        ///// </summary>
        ///// <param name="type"></param>
        ///// <returns></returns>
        //protected static int GetMaxIndex(int type)
        //{
        //    DateTime openDateTime = DateTime.Today;
        //    var endTime = DateTime.Parse(openDateTime.ToString("yyyy-MM-dd 15:00:00"));
        //    return GetIndex(endTime, type);
        //}
        /// <summary>
        /// type K线 的结束时间
        /// </summary>
        /// <param name="openDate"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected static DateTime GetMaxDateTime(DateTime openDateTime, int type)
        {
            var minDateTime = GetMinDateTime(openDateTime,type);
            var maxDateTime = minDateTime.AddMinutes(type);
            return maxDateTime;
        }
        /// <summary>
        /// type K线 的起始时间
        /// </summary>
        /// <param name="openDate"></param>
        /// <param name="type"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected static DateTime GetMinDateTime(DateTime openDateTime, int type)
        {
            var time = openDateTime;

            var time1 = DateTime.Parse(openDateTime.ToString("yyyy-MM-dd 09:30:00"));
            var time2 = DateTime.Parse(openDateTime.ToString("yyyy-MM-dd 11:30:00"));
            var time3 = DateTime.Parse(openDateTime.ToString("yyyy-MM-dd 12:00:00"));
            var time4 = DateTime.Parse(openDateTime.ToString("yyyy-MM-dd 13:00:00"));
            var time5 = DateTime.Parse(openDateTime.ToString("yyyy-MM-dd 15:00:00"));
            time = time < time1? time1: time;
            time = time < time3&&openDateTime>=time2 ? time2.AddMilliseconds(-1) : time;

            time = time < time4 && time > time3 ? time4 : time;
            time = time >= time5  ? time5.AddMilliseconds(-1) : time;

            var minutesMod = time.Minute%type;
            if (type>30&& time<time3) {
                minutesMod = (time.Minute + 30) % type;
            }
            time = DateTime.Parse(time.ToString("yyyy-MM-dd HH:mm")).AddMinutes(-minutesMod);
           
            return time;
        }
    }
}
