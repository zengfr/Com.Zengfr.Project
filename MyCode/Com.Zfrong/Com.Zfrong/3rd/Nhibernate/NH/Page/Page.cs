using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
//using DecorateLib.business.domain;

namespace Com.Zfrong.Common.Data.NH.Page
{
    /// <summary>
    /// ��ҳ����.
    /// �������ݼ���ҳ��Ϣ.
    /// @author Jacky Chen
    /// </summary>
    [Serializable]
    public class Page
    {
        public const int DEFAULT_PAGE_SIZE = 10;

        /// <summary>
        /// ��ǰҳ��һ�����ݵ�λ��,��0��ʼ
        /// </summary>
        private int start;

        /// <summary>
        /// ÿҳ�ļ�¼��
        /// </summary>
        private int pageSize = DEFAULT_PAGE_SIZE;

        /// <summary>
        /// ��ǰҳ�д�ŵļ�¼
        /// </summary>
        private IList data;

        /// <summary>
        /// �ܼ�¼��
        /// </summary>
        private int totalCount;

        /// <summary>
        /// ���췽����ֻ�����ҳ
        /// </summary>
        public Page()
        {
            this.start = 0;
            this.totalCount = 0;
            this.pageSize = DEFAULT_PAGE_SIZE;
            this.data = new ArrayList();
        }

        /// <summary>
        /// Ĭ�Ϲ��췽��
        /// </summary>
        /// <param name="start">��ҳ���������ݿ��е���ʼλ��</param>
        /// <param name="totalSize">���ݿ����ܼ�¼����</param>
        /// <param name="pageSize">��ҳ����</param>
        /// <param name="data">��ҳ����������</param>
        public Page(int start, int totalSize, int pageSize, IList data)
        {
            this.start = start;
            this.totalCount = totalSize;
            this.pageSize = pageSize;
            this.data = data;
        }

        /// <summary>
        /// ȡ���ݿ��а������ܼ�¼��
        /// </summary>
        public int TotalCount
        {
            get { return this.totalCount; }
        }

        /// <summary>
        /// ȡ��ҳ��
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
        /// ȡÿҳ��������
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
        /// ��ǰҳ�еļ�¼
        /// </summary>
        public IList Data
        {
            get { return data; }
        }

        /// <summary>
        /// ȥ��ǰҳ��,��0��ʼ
        /// </summary>
        public int PageIndex
        {
            get { return (start / pageSize); }
        }

        /// <summary>
        /// �Ƿ�����ҳ
        /// </summary>
        /// <returns></returns>
        public bool hasFirstPage()
        {
            return (this.PageIndex != 0);
        }

        /// <summary>
        /// �Ƿ�����һҳ
        /// </summary>
        /// <returns></returns>
        public bool hasNextPage()
        {
            return (this.PageIndex < (this.PageCount - 1));
        }

        /// <summary>
        /// �Ƿ�����һҳ
        /// </summary>
        /// <returns></returns>

        public bool hasPreviousPage()
        {
            return (this.PageIndex > 0);
        }

        /// <summary>
        /// �Ƿ���ĩҳ
        /// </summary>
        /// <returns></returns>
        public bool hasLaxtPage()
        {
            return (this.PageIndex != (this.PageCount - 1));
        }

        /// <summary>
        /// ��ȡ��һҳ��һ�����ݵ�λ�ã�ÿҳ����ʹ��Ĭ��ֵ
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        public static int getStartOfPage(int pageNo)
        {
            return getStartOfPage(pageNo, DEFAULT_PAGE_SIZE);
        }

        /// <summary>
        /// ��ȡ��һҳ��һ�����ݵ�λ��,startIndex��0��ʼ
        /// </summary>
        /// <param name="pageNo">��ǰҳ��</param>
        /// <param name="pageSize">��ǰҳ��ʾ����</param>
        /// <returns></returns>
        public static int getStartOfPage(int pageNo, int pageSize)
        {
            int startIndex = pageNo * pageSize;
            return startIndex < 0 ? 0 : startIndex;
        }
    }
}