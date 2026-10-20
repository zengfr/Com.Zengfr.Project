using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NVelocity;

namespace Com.Zengfr.CodeGenerator.Common.Engine
{
    public class ZContext: VelocityContext
    {
        public void Put<T>(string key, T value)
        {
             Put(key, value);
        }
        public T Get<T>(string key)
        {
           return (T) Get(key);
        }
    }
}
