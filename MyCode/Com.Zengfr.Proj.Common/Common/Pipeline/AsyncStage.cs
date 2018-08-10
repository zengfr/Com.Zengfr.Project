using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    internal class AsyncStage<I, M, O> : StageBase<I, M, O>, IPipelineStage<I, M, O>
    {
        ParallelOptions ParallelOptions { get; set; }
        public AsyncStage(ICollection<IPipelineStage<I, M, O>> stages, int maxDegreeOfParallelism) :
            base(stages)
        {
            ParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
        }
        public AsyncStage(ICollection<IPipelineStage<I, M, O>> stages) :
            base(stages)
        {
            ParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 2 };
        }
        public override bool PreProcess(PipelineContext<I, M, O> context)
        {
            var complate = true;
            Parallel.ForEach(Stages,ParallelOptions ,stage =>
            {
                if (complate)
                {
                    complate= complate&&stage.PreProcess(context);
                    Hander.PreProcess(context);
                }
            });
            return complate;
        }

        public override bool Process(PipelineContext<I, M, O> context)
        {
            var complate = true;
            Parallel.ForEach(Stages, stage =>
            {
                if (complate)
                {
                    complate = complate && stage.Process(context);
                    Hander.Process(context);
                }
            });
            return complate;
        }
    }
}