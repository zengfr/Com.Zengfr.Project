using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline
{
  
    public interface IPipelineProcessor<I, M, O>
    {
        bool PreProcess(PipelineContext<I, M, O> context);
        bool PostProcess(PipelineContext<I, M, O> context);
        bool Disabled { get; set; }
        int Retry { get; set; }
        String Name { get; set; }
    }
   
}
