using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    public class PipelineContextBase
    {
        public string TraceId { get; set; }
        public DateTime StartTime { get; set; }
        
        public PipelineContextBase() {
            StartTime = DateTime.Now;
        }
    }
    public class PipelineContext<I, O> : PipelineContextBase
    {
        public I Input { get; set; }
        public O Output { get; set; }
    }
    public class PipelineContext<I, M, O> : PipelineContext<I, O>
    {
        public M Context { get; set; }
        public IPipelineHander<I, M, O> Hander { get; set; }

    }
}
