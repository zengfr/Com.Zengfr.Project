using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    public interface IPipelineStep<I,M,O> : IPipelineProcessor<I, M, O>
    { 
        int Parallelism { get; set; }
        void Put(IEnumerable<IPipelineProcessor<I, M, O>> processors);
        void Put(IPipelineProcessor<I, M, O> processor);
    }
}
