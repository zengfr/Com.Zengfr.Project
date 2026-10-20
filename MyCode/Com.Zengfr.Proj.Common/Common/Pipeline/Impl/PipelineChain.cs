using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Com.Zengfr.Proj.Common.Pipeline.Impl;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    public class PipelineChain<I, M, O>: PipelineStep<I, M, O>,IPipelineChain<I, M, O>, IPipelineStep<I, M, O>
    {
        
        
    }
}
