using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    public interface IPipelineStage<I,M,O>
    {
        bool PreProcess(PipelineContext<I, M, O> context);
        bool Process(PipelineContext<I, M, O> context);
    }
     
}
