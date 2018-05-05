using System;
using System.Linq;
namespace ConsoleApplication3.Stock.Indicators
{
    public interface IIndicator
    {
        void Test(DateTime effectdate);
        string[] Process(DateTime day);
        double[] Process(string code, DateTime day);

    }
    public abstract class AbstractIndicator : IIndicator
    {
        protected abstract string Name { get; }
        public virtual void Test(DateTime effectdate)
        {
            var results = Process(effectdate);
            var data = QueryUtils.QueryByStockCode(effectdate, results.ToArray());
            Console.WriteLine("Test:{0}", this.Name);
            QueryUtils.ShowData(data);
            var dir=Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            var path = string.Format("{0}\\stock\\{2}_{1}.csv",dir,this.Name,DateTime.Today.ToString("yyyy-MM-dd"));
            if (data.Rows.Count > 1)
            {
                QueryUtils.SaveCSV(data, path);
            }
        }
        protected string[] ProcessQuery(string query)
        {
            var objs = QueryUtils.Query<string>(query);
            var results = objs.Distinct()
                .Where(t => !t.Contains("ST"))
                .Where(t => !t.Contains("sh300"))
                .Where(t => !t.Contains("sz300"));
            return results.ToArray();
        }
       public abstract string[] Process(DateTime day);
       public abstract double[] Process(string code, DateTime day);
     
    }
     
}
