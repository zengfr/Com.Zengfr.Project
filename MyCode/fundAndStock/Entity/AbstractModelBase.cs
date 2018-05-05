using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.ActiveRecord;

namespace Entity
{

    public abstract class AbstractModelBase
    {
        [Property]
        public virtual DateTime DataChangeLastTime { get; set; }
        public AbstractModelBase()
        {
            DataChangeLastTime = DateTime.Now;
        }
    }
}
