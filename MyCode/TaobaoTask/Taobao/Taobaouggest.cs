using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaobaoTask.Taobao;
using Com.Zengfr.Proj.Common;
namespace TaobaoTask
{
   public  class TaobaoKeyword
    {
        public void Run()
        {
            var baseDirectory = string.Format("{0}\\", AppDomain.CurrentDomain.BaseDirectory);

            Queue1= IOSerialization.ReadFromBinaryFile<Queue<string>>(baseDirectory + "q.bin");
            QueueAll=IOSerialization.ReadFromBinaryFile<Dictionary<string, byte>>(baseDirectory + "qAll.bin");
            if (QueueAll == null)
                QueueAll = new Dictionary<string, byte>();
            if (Queue1 == null)
                Queue1 = new Queue<string>();
            if (Queue1.Count + QueueAll.Count <= 0)
            {
                var words = TaobaoUtils.GetmarketlistWords();
                foreach (var word in words)
                {
                    AddToQueue(word);
                }
            }

            var results = new List<result>();
            var saveTime = DateTime.Now;
            do
            {
                var word = Queue1.Dequeue();
                if ((!QueueAll.ContainsKey(word)) || QueueAll[word] != 1)
                {
                    results.AddRange(RunsuggestByWord(word));
                    QueueAll[word] = 1;
                    if (CSVUtil.SaveToFile(ref saveTime, 60 * 6, results))
                    {
                        IOSerialization.WriteToBinaryFile<Queue<string>>(baseDirectory + "q.bin", Queue1, false);
                        IOSerialization.WriteToBinaryFile<Dictionary<string, byte>>(baseDirectory + "qAll.bin", QueueAll, false);

                    }
                }
            } while (Queue1.Count>0);
        }
        Queue<string> Queue1 = new Queue<string>();
        Dictionary<string,byte> QueueAll = new Dictionary<string, byte>();
        public string AddToQueue(string word)
        {
            if (!string.IsNullOrWhiteSpace(word))
            {
                word = word.Replace("<b>", string.Empty).Replace("</b>", string.Empty);
            }
            if (!QueueAll.ContainsKey(word))
            {
                Queue1.Enqueue(word);
                QueueAll.Add(word, 0);
                ConsoleWriteLine(string.Format("AddToQueue:{0},{1},{2}", Queue1.Count, QueueAll.Count, word));
            }
            return word;
        }
       
        private static void ConsoleWriteLine(string message)
        {
            Console.WriteLine(string.Format("{0}\t{1}", DateTime.Now.ToString("dd HHmmss fff"), message));
        }
        public List<result> RunsuggestByWord(string word)
        {
            var results = new List<result>();
            
            var suggestResult = TaobaoUtils.suggest(word);
            if (suggestResult != null)
            {
                var wordResult = new List<Tuple<string,long,string>>();
                if ( suggestResult.result != null
               && suggestResult.result.Length > 0)
                {
                    long count = 0;
                    foreach (var rr  in suggestResult.result)
                    {
                        if (rr.Length > 0)
                        {
                            count = 0;
                            if (rr.Length > 1)
                            {
                                long.TryParse(rr[1], out count);
                            }
                            rr[0]=AddToQueue(rr[0]);
                            var result = RunsearchByWord(rr[0], count);
                            if (result.nid > 0)
                            {
                                results.Add(result);
                            }
                            wordResult.Add(new Tuple<string, long, string>(rr[0],count,null));
                        }
                    }
                }

                var index = 0;
                if (suggestResult != null
              && suggestResult.magic != null
              && suggestResult.magic.Length > 0)
                {
                    foreach (var magic in suggestResult.magic)
                    {
                        index = magic.index;
                        if (wordResult.Count >= index&&index>0) {
                            foreach (var data in magic.data)
                            {
                                foreach (var item in data)
                                {
                                    AddToQueue(string.Format("{0} {1}", wordResult[index-1].Item1, item.title));
                                }
                            }
                        }
                    }
                }
            }
            return results;
        }
        public result RunsearchByWord(string word,long count)
        {
            var result = new result();
            result.count = count;
            result.word = word;

            var searchResult = TaobaoUtils.search(word);
            if (searchResult != null
                && searchResult.mods != null
                && searchResult.mods.itemlist != null
                && searchResult.mods.itemlist.data != null
                && searchResult.mods.itemlist.data.auctions != null
                && searchResult.mods.itemlist.data.auctions.Length>0)
            {
                //foreach (var  in searchResult.mods.itemlist.data.auctions)
                {

                    var auction = searchResult.mods.itemlist.data.auctions.Last();
                   
                    result.category = auction.category??0;
                    result.comment_count = auction.comment_count ?? 0;
                    
                    result.item_loc = auction.item_loc;
                    result.nid = auction.nid ?? 0;
                    result.pid = auction.category ?? 0;
                    result.raw_title = auction.raw_title;
                    result.reserve_price = auction.reserve_price ?? 0;
                    result.user_id = auction.user_id ?? 0;
                    result.view_fee = auction.view_fee ?? 0;
                    result.view_price = auction.view_price ?? 0;

                    result.view_sales = auction.view_sales;
                    ConsoleWriteLine(string.Format("{0},{1},{2},{3}", word,count,  result.view_sales, result.view_price));
                }
            }
            return result;
        }
        public class result
        {
            public string word { get; set; }
            public long count { get; set; }
            public long nid { get; set; }
            public long category { get; set; }
            public long pid { get; set; }
            public string raw_title { get; set; }

            public decimal view_price { get; set; }
            public decimal view_fee { get; set; }
            public string item_loc { get; set; }
            public decimal reserve_price { get; set; }
            public long user_id { get; set; }
            public string view_sales { get; set; }
            public long comment_count { get; set; }
        }

        
    }
}
