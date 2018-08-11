using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    
    public interface IPipelineProcessorHander<I,M,O>  
    {
        bool PreProcess(String stageName, String processorName,double timeSpan, PipelineContext<I, M, O> context);
        bool PostProcess(String stageName, String processorName, double timeSpan, PipelineContext<I, M, O> context);
    }
}
