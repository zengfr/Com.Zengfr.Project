using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using DecorateLib.business.domain;
using NHibernate;
//using DecorateLib.bases.dao.hibernate;

namespace Com.Zfrong.Common.Data.NH.Page
{


    /// <summary>
    ///  使用Hql查询的的分页查询类
    ///  支持执行Count查询和取出全部数据统计size三种方式取得总记录条数
    ///  see http://www.javaeye.com/display/opensourceframework/HibernateUtils
    ///  @author ajax
    ///  @see Page
    /// </summary>
    public class HqlPage
    {

        public static Page getPageInstanceByCount(IQuery query, int pageNo, int pageSize, int totalCount)
        {
            return getPageResult(query, pageNo, pageSize, totalCount);
        }

        /* Asp.net2.0不支持游标
        protected static Page getPageInstanceByScroll(IQuery query, int pageNo, int pageSize)
        {
            ScrollableResults scrollableResults = query.scroll(ScrollMode.SCROLL_SENSITIVE);
            scrollableResults.last();
            int totalCount = scrollableResults.getRowNumber();
            return getPageResult(query, pageNo, pageSize, totalCount + 1);
        }
        */
        public static Page getPageInstanceByList(IQuery query, int pageNo, int pageSize)
        {
            //query.scroll(ScrollMode.FORWARD_ONLY);//快速前进游标模式
            int totalCount =query.List().Count;
            return getPageResult(query, pageNo, pageSize, totalCount);
        }

        private static Page getPageResult(IQuery q, int pageNo, int pageSize, int totalCount)
        {
            if (totalCount < 1) return new Page();
            int startIndex = Page.getStartOfPage(pageNo, pageSize);
            IList list = q.SetFirstResult(startIndex).SetMaxResults(pageSize).List();
            return new Page(startIndex, totalCount, pageSize, list);
        }

        public static Page getPageInstanceByCount(ISQLQuery query, int pageNo, int pageSize, int totalCount)
        {
            return getPageResult(query, pageNo, pageSize, totalCount);
        }
        public static Page getPageInstanceByList(ISQLQuery query, int pageNo, int pageSize)
        {
            int totalCount = query.List().Count;
            return getPageResult(query, pageNo, pageSize, totalCount);
        }

        private static Page getPageResult(ISQLQuery q, int pageNo, int pageSize, int totalCount)
        {
            if (totalCount < 1) return new Page();
            int startIndex = Page.getStartOfPage(pageNo, pageSize);
            IList list = q.SetFirstResult(startIndex).SetMaxResults(pageSize).List();
            return new Page(startIndex, totalCount, pageSize, list);
        }
    }
}
