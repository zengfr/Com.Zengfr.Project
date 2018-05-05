using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using DecorateLib.business.domain;

namespace Com.Zfrong.Common.Data.NH.Page
{
    /// <summary>
    /// 分页对象.
    /// 包含数据及分页信息.
    /// @author Jacky Chen
    /// </summary>
    [Serializable]
    public class Page
    {
        public const int DEFAULT_PAGE_SIZE = 10;

        /// <summary>
        /// 当前页第一条数据的位置,从0开始
        /// </summary>
        private int start;

        /// <summary>
        /// 每页的记录数
        /// </summary>
        private int pageSize = DEFAULT_PAGE_SIZE;

        /// <summary>
        /// 当前页中存放的记录
        /// </summary>
        private IList data;

        /// <summary>
        /// 总记录数
        /// </summary>
        private int totalCount;

        /// <summary>
        /// 构造方法，只构造空页
        /// </summary>
        public Page()
        {
            this.start = 0;
            this.totalCount = 0;
            this.pageSize = DEFAULT_PAGE_SIZE;
            this.data = new ArrayList();
        }

        /// <summary>
        /// 默认构造方法
        /// </summary>
        /// <param name="start">本页数据在数据库中的起始位置</param>
        /// <param name="totalSize">数据库中总记录条数</param>
        /// <param name="pageSize">本页容量</param>
        /// <param name="data">本页包含的数据</param>
        public Page(int start, int totalSize, int pageSize, IList data)
        {
            this.start = start;
            this.totalCount = totalSize;
            this.pageSize = pageSize;
            this.data = data;
        }

        /// <summary>
        /// 取数据库中包含的总记录数
        /// </summary>
        public int TotalCount
        {
            get { return this.totalCount; }
        }

        /// <summary>
        /// 取总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (totalCount % pageSize == 0)
                {
                    return totalCount / pageSize;
                }
                else
                {
                    return totalCount / pageSize + 1;
                }
            }
        }

        /// <summary>
        /// 取每页数据容量
        /// </summary>
        public int PageSize
        {
            get
            {
                if (pageSize <= 0)
                {
                    pageSize = DEFAULT_PAGE_SIZE;
                }
                return pageSize;
            }
        }

        /// <summary>
        /// 当前页中的记录
        /// </summary>
        public IList Data
        {
            get { return data; }
        }

        /// <summary>
        /// 去当前页码,从0开始
        /// </summary>
        public int PageIndex
        {
            get { return (start / pageSize); }
        }

        /// <summary>
        /// 是否有首页
        /// </summary>
        /// <returns></returns>
        public bool hasFirstPage()
        {
            return (this.PageIndex != 0);
        }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        /// <returns></returns>
        public bool hasNextPage()
        {
            return (this.PageIndex < (this.PageCount - 1));
        }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        /// <returns></returns>

        public bool hasPreviousPage()
        {
            return (this.PageIndex > 0);
        }

        /// <summary>
        /// 是否有末页
        /// </summary>
        /// <returns></returns>
        public bool hasLaxtPage()
        {
            return (this.PageIndex != (this.PageCount - 1));
        }

        /// <summary>
        /// 获取任一页第一条数据的位置，每页条数使用默认值
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public static int getStartOfPage(int pageNo)
        {
            return getStartOfPage(pageNo, DEFAULT_PAGE_SIZE);
        }

        /// <summary>
        /// 获取任一页第一条数据的位置,startIndex从0开始
        /// </summary>
        /// <param name="pageNo">当前页码</param>
        /// <param name="pageSize">当前页显示条数</param>
        /// <returns></returns>
        public static int getStartOfPage(int pageNo, int pageSize)
        {
            int startIndex = pageNo * pageSize;
            return startIndex < 0 ? 0 : startIndex;
        }
    }
}