using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Zengfr.Proj.Common
{
    public class EnumerablePagedAdapter<T>  
    {
        public IEnumerable<T> PagedItems{ get; protected set;}
        public IEnumerable<T> AllItems { get; protected set; }
        public int PageIndex { get; protected set; }
        public int PageSize { get; protected set; }
        public int TotalCount { get; protected set; }
        public EnumerablePagedAdapter(IEnumerable<T>  results,int resultsTotalCount,
            int pageIndex,int pageSize, int pagedType)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Init(results,  resultsTotalCount,pagedType);
        }
        protected void Init(IEnumerable<T> results, int resultsTotalCount, int pagedType)
        {
            Tuple<IEnumerable<T>, int> rest = new Tuple<IEnumerable<T>, int>(results, resultsTotalCount);
            switch (pagedType) {
                case 1://分页获取
                    AllItems = rest.Item1;
                    TotalCount = rest.Item2;

                    PagedItems = rest.Item1;
                    break;
                case 2://获取后内存分页
                    AllItems = rest.Item1;
                    TotalCount = rest.Item2;

                    PagedItems = AllItems.Skip(this.PageIndex * this.PageSize).Take(this.PageSize);
                    break;
                default:
                    break;
            } 
        }
    }
}
