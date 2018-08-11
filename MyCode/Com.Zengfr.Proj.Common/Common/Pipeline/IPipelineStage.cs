using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    public interface IPipelineStage<I,M,O> : IPipelineProcessor<I, M, O>
    { 
        int PreProcessOrder { get; set; }
        int PostProcessOrder { get; set; }
    }
    public interface IPipelineAsyncStage<I, M, O> : IPipelineStage<I, M, O>
    {
        int Parallelism { get; set; }
    }
    public interface IPipelineSyncStage<I, M, O> : IPipelineStage<I, M, O>
    { 

    }
}
