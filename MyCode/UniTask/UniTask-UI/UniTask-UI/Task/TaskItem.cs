using OpenQA.Selenium;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UniTask_UI.Task
{
    [SerializableAttribute]
    [AddINotifyPropertyChangedInterface]
    public partial class TaskItem {
        [DisplayName("ID")]
        public long id { get; set; }
        [DisplayName("IP")]
        public string ip { get;  set; }
        [DisplayName("名称")]
        public string name { get; set; }
        [DisplayName("备注")]
        public string comment { get; set; }
       // [DisplayName("进度")]
        public int[] progress { get; set; }
       // [DisplayName("成功")]
        public int[] success { get; set; }
      //  [DisplayName("失败")]
        public int[] fail { get; set; }
       // [DisplayName("当天收益")]
        public int[] incomeToday { get; set; }
       // [DisplayName("总收益")]
     
        public int[] incomeTotal { get; set; }

        [DisplayName("进度")]
        public string progressStr { get { return string.Join("/", progress); } }
        [DisplayName("成功")]
        public string successStr { get { return string.Join("/", success); } }
        [DisplayName("失败")]
        public string failStr { get { return string.Join("/", fail); } }
        [DisplayName("当天收益")]
        public string incomeTodayStr { get { return string.Join("/", incomeToday); } }
        [DisplayName("总收益")]
        public string incomeTotalStr { get { return string.Join("/", incomeTotal); } }



        [DisplayName("状态")]
        /// <summary>
        /// 当前状态
        /// </summary>
        public int  status { get; set; }
        [DisplayName("当前任务")]
        /// <summary>
        /// 当前任务
        /// </summary>

        public string task { get; set; }
        [DisplayName("日志")]
        /// <summary>
        /// 当前日志
        /// </summary>
        public string log { get; set; }
        [DisplayName("运行时长")]
        /// <summary>
        /// 运行时长
        /// </summary>
        public double TimeSpan { get; set; }
        [DisplayName("开始时间")]
        public DateTime startTime { get; set; }

        public TaskItem() {
            startTime = DateTime.Now;
            success = new int[0];
            fail = new int[0];
            progress = new int[0];
            incomeTotal = new int[0];
            incomeToday = new int[0];
        }
    }
}
