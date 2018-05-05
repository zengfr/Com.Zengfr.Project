using System;

namespace Com.Zengfr.Proj.Common.Web
{
    [Serializable]
    public class AjaxPagedResult<T> : AjaxResult
    {
        public AjaxPagedResult()
            : base()
        {

        }
        public virtual int total { get; set; }
        public virtual int page { get; set; }
        public virtual T[] items { get; set; }
        /// <summary>
        /// 统计项 求和汇总等
        /// </summary>
        public virtual decimal[] statistItems { get; set; }
    }
    [Serializable]
    public class AjaxCombboxResult : AjaxResult
    {
        public AjaxCombboxResult()
            : base()
        {

        }
        public virtual string id { get; set; }
        public virtual string label { get; set; }
        public virtual string value { get; set; }

    }
    [Serializable]
    public class AjaxItemResult<T> : AjaxResult
    {
        public AjaxItemResult()
            : base()
        {

        }
        public virtual T item { get; set; }
    }
    [Serializable]
    public class AjaxResult
    {
        public AjaxResult() { Message = string.Empty; }
        public virtual bool HasErrors { get; set; }
        public virtual int StatusCode { get; set; }
        public virtual int Type { get; set; }
        public virtual string Message { get; set; }
    }
}