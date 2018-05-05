using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.ActiveRecord;

using NHibernate;
using ConsoleApplication3.Stock.Indicators;
using System.IO;
namespace ConsoleApplication3.Stock
{
    public class QueryUtils
    {
        public static DataTable QueryByStockCode(DateTime date, string[] stockCodes)
        {
            var sql = string.Format(@"SELECT  d.effectDate,d.[stockName],s.[StockCode],
z.主力罗盘,z.涨跌幅,lx.天数 as 连续净流天数,lx.主力罗盘 as 连续净流主力罗盘,

b.[trade收盘价],b.[changeratio涨跌幅],b.[turnover换手率],
d.[zhScore] ,d.[basicScore],d.[fundsScore],d.[tradeScore],d.[messageScore],
y.[交易类看多指标数变化量] as 看多,y.[交易类看空指标数变化量] as 看空,y.[今日发出提示信号指标数] as 今日信号,y.[未来10个交易日上涨百分5的概率]
  
FROM  [stock] s(nolock)
  join [dbo].[doctor10jqkascore] d (nolock) on d.[stockCode]=s.[StockCode]

  join [dbo].[Stock和讯指标云] y (nolock) on y.[stockcode]=s.[stockcode]

  join [Stock历史成交分布] b (nolock) on b.代码=s.[stockCode]

 join Stock资金流入趋势 z (nolock) on z.[代码]=s.stockcode

 join Stock主力连续净流 lx (nolock)  on  lx.[代码]=s.stockcode

  where s.StockCode in ('{0}') 
and d.effectDate='{1}' 
and y.[effectDate]='{1}' 
and b.[opendate日期] ='{1}' and z.日期='{1}'",
string.Join("','", stockCodes),
 date.ToString("yyyy-MM-dd"));
            return QueryDataTable(sql);
        }

        public static List<T> Query<T>(string sql)
        {
            List<T> objs =
                ActiveRecordMediator.Execute(typeof(ActiveRecordBase),
                delegate(ISession session, object instance)
                {
                    return session.CreateSQLQuery(sql)
                        .List<T>();
                }, null) as List<T>;

            return objs;

        }
        public static DataTable QueryDataTable(string sql)
        {
            DataTable dataTable = new DataTable();
            Castle.ActiveRecord.Framework.ISessionFactoryHolder sessionHolder =
                Castle.ActiveRecord.ActiveRecordMediator.GetSessionFactoryHolder();
            NHibernate.ISession session = sessionHolder.CreateSession(typeof(ActiveRecordBase));

            try
            {
                IDbCommand command = session.Connection.CreateCommand();
                command.CommandText = sql;
                IDataReader rdr = command.ExecuteReader();
                dataTable.Load(rdr, LoadOption.Upsert);
            }
            finally
            {
                sessionHolder.ReleaseSession(session);
            }
            return dataTable;
        }
        public static void ShowData(DataTable data)
        {
            int index = 0;
            foreach (DataRow item in data.Rows)
            {
                if (index == 0)
                {
                    Console.Write(index);
                    foreach (DataColumn c in data.Columns)
                    {
                        Console.Write("," + c.ColumnName);
                    }
                    Console.WriteLine();
                }
                Console.Write(index);
                foreach (DataColumn c in data.Columns)
                {
                    Console.Write("," + item[c.ColumnName]);
                }
                Console.WriteLine();
                index++;
            }
        }
        public static void SaveCSV(DataTable dt, string fileName)
        {
            var dir =Path.GetDirectoryName(fileName);
            if(!Directory.Exists(dir)){
                Directory.CreateDirectory(dir);
            }
            //var fileName = filePath + string.Format("{0}.csv", DateTime.Today.ToString("yyyy-MM-dd"));
            using (var fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                var encoding = System.Text.Encoding.GetEncoding("gb2312");
                using (var sw = new StreamWriter(fs, encoding))
                {
                    //sw.Write(0xFEFF);  //乱码
                    //sw.Write(0xEFBBBF);
                    StringBuilder data = new StringBuilder();

                    //写出列名称
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        data.Append(dt.Columns[i].ColumnName + ",");
                    }
                    sw.WriteLine(data);

                    //写出各行数据
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        data.Clear();
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            data.Append(dt.Rows[i][j] + ",");
                        }
                        sw.WriteLine(data);
                    }

                }
            }
        }
    }
}
