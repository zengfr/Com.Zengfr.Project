using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    
    public interface IPipelineHander<I,M,O>  
    {
        bool PreProcess(String parentName, String name,double timeSpan, PipelineContext<I, M, O> context);
        bool PostProcess(String parentName, String name, double timeSpan, PipelineContext<I, M, O> context);
    }
}
