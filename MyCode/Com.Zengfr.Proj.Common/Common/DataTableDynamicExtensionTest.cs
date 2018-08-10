using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Threading.Tasks;
namespace Com.Zengfr.Proj.Common
{
    [TestClass]
    public class DataTableDynamicallyUtilTest
    {
        [TestMethod]
        public void UtilTest()
        {
            DataTable dt = new DataTable();
            dt.AddColumns<string>("A", "B", "C");
            dt.AddColumn("X", 123456);
            dt.AddColumns<int>("E", "F", "G");
            dt.SetPrimaryKeyColumns<string>("K");
            dt.AddColumnsByDays<string>("M", 3);
            dt.AddColumnsByIndexs<string>("N", 0, 7);
            
            long key = 11L;
            dt.AddRow(key);
            dt.SetValue(key, "B", "BV");
            dt.SetValue(key, "E", 123);

            var v = dt.GetValue<long, int>(key, "E");
            var vv = dt.GetIntValue(key, "E");

            dt.MergeValue(key, "A", ',', "123");
            dt.MergeValueFrom(key, "C", ',', "A", "B");
            
            dt.MergeValueSumFrom<long,int>(key, "G", "E", "F");
            dt.MergeIntValueSumFrom(key, "G", "E", "F");
        }
    }
}
