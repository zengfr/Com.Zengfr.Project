using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    public interface IPipelinePreProcessor<I, M, O>
    {
        bool PreProcess(PipelineContext<I, M, O> context);
    }
    public interface IPipelinePostProcessor<I, M, O>
    {
        bool PostProcess(PipelineContext<I, M, O> context);
    }
    public interface IPipelineProcessor<I, M, O> : IPipelinePreProcessor<I, M, O>, IPipelinePostProcessor<I, M, O>
    {
        String Name { get; set; }
        bool Enabled { get; set; }
    }
   
}
