using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    internal class SyncStage<I, M, O> : StageBase<I, M, O>, IPipelineStage<I, M, O>
    {
        public SyncStage(ICollection<IPipelineStage<I, M, O>> stages) : base(stages)
        {

        }
        public SyncStage(IPipelineStage<I, M, O> stage) : base(stage)
        {
            
        }
       
        public override bool PreProcess(PipelineContext<I, M, O> context)
        {
            var complate = true;
            foreach (var stage in Stages)
            {
                if (complate) {
                    complate = complate && stage.PreProcess(context);
                    Hander.PreProcess(context);
                }
            }
            return complate;
        }

        public override bool Process(PipelineContext<I, M, O> context)
        {
            var complate = true;
            foreach (var stage in Stages)
            {
                if (complate)
                {
                    complate = complate && stage.Process(context);
                    Hander.Process(context);
                }
            }
            return complate;
        }


    }
}
