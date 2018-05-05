using System;
using System.Collections.Generic;
using System.Text;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataMapper;
using System.Data;
namespace Com.Zfrong.Common.Data.IBatisNet
{
    public interface IBaseDao : IDao
    {
        DataSet ExecutePaginatedQueryForDataSet(string statementName, object paramObject, int pageIndex, int pageSize, string sort);
        DataTable ExecutePaginatedQueryForDataTable(string statementName, object paramObject, int pageIndex, int pageSize, string sort);
    }
}
