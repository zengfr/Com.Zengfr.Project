using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline.Impl
{
    public class LogPipelineProcessorHander<I, M, O> : IPipelineProcessorHander<I, M, O>
    {
        public bool PostProcess(string stageName, string processorName, double timeSpan, PipelineContext<I, M, O> context)
        {
            Console.WriteLine(string.Format("Exec->Post:{0,-19},{1,-19},TimeSpan:{2:N3}", stageName,   processorName, timeSpan,  ""));
            return true;
        }

        public bool PreProcess(string stageName, string processorName, double timeSpan, PipelineContext<I, M, O> context)
        {
            Console.WriteLine(string.Format("Exec->Pre :{0,-19},{1,-19},TimeSpan:{2:N3}", stageName, processorName, timeSpan,""));
            return true;
        }
    }

}
