using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline.Impl
{
    public class FuncPipelineProcessor<I, M, O> : IPipelineProcessor<I, M, O>
    {
        public int Retry
        {
            get;

            set;
        }

        public bool Disabled
        {
            get;

            set;
        }

        public string Name
        {
            get;
            set;
        }

        public FuncPipelineProcessor(Func<String,PipelineContext<I, M, O>, bool> func) {
            this.func = func;
        }
        Func<String, PipelineContext<I, M, O>, bool> func;
        
        public bool PostProcess(PipelineContext<I, M, O> context)
        {
            return func.Invoke("PostProcess", context);
        }

        public bool PreProcess(PipelineContext<I, M, O> context)
        {
            return func.Invoke("PreProcess", context);
        }
    }
}
