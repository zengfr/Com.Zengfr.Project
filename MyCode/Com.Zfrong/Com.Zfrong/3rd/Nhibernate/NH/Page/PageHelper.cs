using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
//using DecorateLib.bases.dao;
using NHibernate;
using NHibernate.Criterion;
using System.Text.RegularExpressions;

namespace Com.Zfrong.Common.Data.NH.Page
{

    public enum PageMode
    {
        COUNT_MODE = 1,
        SCROLL_MODE = 2,//Asp.net2.0不支持游标模式
        LIST_MODE = 3,
    }
    /// <summary>
    /// Hibernate Entity Dao基类.
    /// 带SpringSide扩展的分页函擿具体的分页代码在Page类中.
    /// @author Jacky Chen
    /// @see AbstractHibernateDao
    /// @see HqlPage
    /// @see CriteriaPage
    /// </summary>
    public class PageHelper //: AbstractDAOHibernate
    {
        public ISession Session;//

        public Page GetPagedQuery<T>(ICriterion expression, int pageNo)
        {
            return GetPagedQuery<T>(expression,pageNo,10);//
        }
        /// <summary>
        /// 进行分页查询
        /// </summary>
        /// <param name="pageNo">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <returns></returns>
        public Page GetPagedQuery<T>(ICriterion expression, int pageNo, int pageSize)
        {
            ICriteria criteria = this.Session.CreateCriteria(typeof(T));
            criteria.Add(expression);//
            criteria.SetResultTransformer(NHibernate.Transform.Transformers.RootEntity);
            return GetPagedQuery(criteria, pageNo, pageSize);
        }
        #region String  hql
        /*--------------------------- hql --------------------------*/
        /// <summary>
        /// 默认使用 COUNT_MODE 模式
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page GetPagedQuery(String hql, int pageNo, int pageSize)
        {
            return GetPagedQuery(hql, pageNo, pageSize,PageMode.COUNT_MODE);
        }

        /// <summary>
        /// 默认使用 COUNT_MODE 模式 pagesize=10
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="mode">COUNT_MODE/LIST_MODE</param>
        /// <returns></returns>
        public Page GetPagedQuery(String hql, int pageNo)
        {
            return GetPagedQuery(hql, pageNo, 10);
        }
        /// <summary>
        /// 可以指定Jdbc driver是否支持scroll方式
        /// </summary>
        /// <param name="hql"></param>
        /// <param name="args"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Page GetPagedQuery(String hql, int pageNo, int pageSize,PageMode mode)
        {
            IQuery query =CreateQuery(hql);
            
            switch (mode)
            {
                case PageMode.LIST_MODE:
                    String countQuery= " select count (*) " + removeSelect(removeOrders(hql));
                    IList<int> countList = this.Session.CreateQuery(countQuery).List<int>();
                    int totalCount = countList[0];
                    return HqlPage.getPageInstanceByCount(query, pageNo, pageSize, totalCount); break;//
                case PageMode.COUNT_MODE:
                    return HqlPage.getPageInstanceByList(query, pageNo, pageSize); break;//
            }
            return null;//
        }
        #endregion
        #region Criteria
        /// <summary>
        /// Criteria分页查询，默认支持 COUNT_MODE 模式
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page GetPagedQuery(ICriteria criteria, int pageNo, int pageSize)
        {
            return GetPagedQuery(criteria, pageNo, pageSize, PageMode.COUNT_MODE);
        }
        public Page GetPagedQuery(ICriteria criteria, int pageNo)
        {
            return GetPagedQuery(criteria, pageNo, 10);
        }
        /// <summary>
        /// Criteria分页查询，可以指定是否支持scroll
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Page GetPagedQuery(ICriteria criteria, int pageIndex, int pageSize, PageMode mode)
        {
            //return null;// CriteriaPage.getPageInstance(criteria, pageNo, pageSize, mode);
            //获取记录总数
            int totalCount = Convert.ToInt32(criteria.SetProjection(Projections.Count("id")).UniqueResult());
            criteria.SetProjection( null );
            //设置分页          
            criteria.SetFirstResult((pageIndex-1) * pageSize).SetMaxResults(pageSize);
            int startIndex = Page.getStartOfPage(pageIndex, pageSize);
            return new Page(startIndex, totalCount, pageSize, criteria.List());
        }
        #endregion
        #region  Query
        /*--------------------------- Query --------------------------*/
        /// <summary>
        /// 默认支持 COUNT_MODE 模式  pageSize=10
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Page GetPagedQuery(IQuery query, int pageNo)
        {
            return GetPagedQuery(query, pageNo, 10);
        }
        /// <summary>
        /// 默认支持 COUNT_MODE 模式
        /// </summary>
        /// <param name="query"></param>
        /// <param name="args"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public Page GetPagedQuery(IQuery query, int pageNo, int pageSize)
        {
            return GetPagedQuery(query, pageNo, pageSize, PageMode.COUNT_MODE);
        }

        /// <summary>
        /// 可以指定Jdbc driver是否支持scroll方式
        /// </summary>
        /// <param name="query"></param>
        /// <param name="args"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public Page GetPagedQuery(IQuery query, int pageNo, int pageSize, PageMode mode)
        {
            switch (mode)
            {
                case PageMode.COUNT_MODE:
                    int total = 10;//
                    return HqlPage.getPageInstanceByCount(query, pageNo, pageSize, total); break;//
                case PageMode.LIST_MODE:
                    return HqlPage.getPageInstanceByList(query, pageNo, pageSize); break;//
            }
            return null;//   
        }
        #endregion
        #region  SQLQuery
        public Page GetPagedQuery(ISQLQuery query, int pageNo)
        {
            return GetPagedQuery(query, pageNo, 10);
        }
        public Page GetPagedQuery(ISQLQuery query, int pageNo, int pageSize)
        {
            return GetPagedQuery(query, pageNo, pageSize, PageMode.COUNT_MODE);
        }
        public Page GetPagedQuery(ISQLQuery query, int pageNo, int pageSize, PageMode mode)
        {
            switch (mode)
            {
                case PageMode.COUNT_MODE:
                    int total = 10;//
                    return HqlPage.getPageInstanceByCount(query, pageNo, pageSize, total); break;//
                case PageMode.LIST_MODE:
                    return HqlPage.getPageInstanceByList(query, pageNo, pageSize); break;//
            }
            return null;//   
        }
        public IQuery CreateQuery(string hql)
        {
            return Session.CreateQuery(hql);//
        }
        public ISQLQuery CreateSQLQuery(string sql)
        {
            return Session.CreateSQLQuery(sql);//
        }
#endregion
        #region 私有方法
        /*---------------- 私有方法 ---------------*/

        /// <summary>
        /// 去除select 子句，未考虑union的情况
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        private static String removeSelect(String hql)
        {
            //Assert.hasText(hql);
            //Spring.Util.StringUtils.HasText(hql);
            int beginPos = hql.ToLower().IndexOf("from");
            //Assert.isTrue(beginPos != -1, " hql : " + hql + " must has a keyword 'from'");
            if( (beginPos == -1) )
            {
                throw new Exception("hql:"+hql+" must has a keyword 'from' ");
            }
            return hql.Substring(beginPos);
        }

        /// <summary>
        /// 去除orderby 子句
        /// </summary>
        /// <param name="hql"></param>
        /// <returns></returns>
        private static String removeOrders(String hql)
        {
            Regex r = new Regex("order\\s*by[\\w|\\W|\\s|\\S]*"); // 定义一个Regex对象实例
            Match m = r.Match(hql); // 在字符串中匹配

            while (m.Success)
            {
                hql = hql.Replace(m.Value, "");
                m = m.NextMatch();
            }

            return hql;
        }
        #endregion
    }
}