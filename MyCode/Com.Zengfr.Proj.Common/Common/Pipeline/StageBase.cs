using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    internal abstract class StageBase<I, M, O>:IPipelineStage<I, M, O>
    {
        public IPipelineHander<I, M, O> Hander { get; set; }
        protected ICollection<IPipelineStage<I, M, O>> Stages { get; set; }
        public StageBase(ICollection<IPipelineStage<I, M, O>> stages)
        {
            Stages = stages;
        }
        public StageBase(IPipelineStage<I, M, O> stage) {
            Stages = new List<IPipelineStage<I, M, O>>();
            Stages.Add(stage);
        }

        public abstract bool PreProcess(PipelineContext<I, M, O> context);
        public abstract bool Process(PipelineContext<I, M, O> context);
    }
}
