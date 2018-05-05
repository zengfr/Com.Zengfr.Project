using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Com.Zfrong.Common.Command
{
   public abstract class CommandBase
    {
        /// <summary>
        /// 执行Command对象所包含的命令
        /// </summary>
        /// <param name="sender">Command调用者</param>
        /// <param name="paras">提供给Command的参数列表</param>
        public abstract void CommandBody(object sender,params object[] paras);

        public void ExecuteAsync(object sender, params object[] paras)
        {
            ThreadStart start = new ThreadStart(delegate()
            {
                this.CommandBody(sender, paras);
            });
            start.BeginInvoke(null, null);
        }
        public void Execute(object sender, params object[] paras)
        {
            this.CommandBody(sender, paras);
        }
       public void Execute(object sender, params string[] paras)
       {
           this.CommandBody(sender, paras);
       }
      
        public abstract CommandFamily Family
        { get;}
    }
}
