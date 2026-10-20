using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline.Impl
{
    public class LogPipelineHander<I, M, O> : IPipelineHander<I, M, O>
    {
        public bool PostProcess(string parentName, string name, double timeSpan, PipelineContext<I, M, O> context)
        {
            Console.WriteLine(string.Format("Exec->Post:{0,-32},{1,-32},TimeSpan:{2:N3}", parentName, name, timeSpan,  ""));
            return true;
        }

        public bool PreProcess(string parentName, string name, double timeSpan, PipelineContext<I, M, O> context)
        {
            Console.WriteLine(string.Format("Exec->Pre :{0,-32},{1,-32},TimeSpan:{2:N3}", parentName, name, timeSpan,""));
            return true;
        }
    }

}
