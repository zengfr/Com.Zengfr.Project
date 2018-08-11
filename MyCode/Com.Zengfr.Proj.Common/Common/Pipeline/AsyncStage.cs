using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    internal class AsyncStage<I, M, O> : StageBase<I, M, O>, IPipelineAsyncStage<I, M, O>
    {
        public int Parallelism { get; set; }
        
        ParallelOptions ParallelOptions { get; set; }
        public AsyncStage(ICollection<IPipelineProcessor<I, M, O>> stages, int parallelism) :
            base(stages)
        {
            this.Parallelism = parallelism;
            ParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = this.Parallelism };
        }
        public AsyncStage(IPipelineProcessor<I, M, O> processor) : base(processor)
        {
            this.Parallelism = 2;
            ParallelOptions = new ParallelOptions { MaxDegreeOfParallelism = this.Parallelism };
        }
        public override bool PreProcess(PipelineContext<I, M, O> context)
        {
            var complate = true;
            Parallel.ForEach(Processors, ParallelOptions, (processor, loopState) =>
            {
                if (complate & processor.Enabled&& !loopState.IsStopped)
                {
                    var sTime = DateTime.Now;
                    complate = complate&& processor.PreProcess(context);
                    this.ProcessorHander.PreProcess(this.Name, processor.Name,(DateTime.Now- sTime).TotalMilliseconds, context);
                }
            });
            return complate;
        }

        public override bool PostProcess(PipelineContext<I, M, O> context)
        {
            var complate = true;
            Parallel.ForEach(Processors, ParallelOptions, (processor, loopState) =>
            {
                if (complate&& processor.Enabled && !loopState.IsStopped)
                {
                    var sTime = DateTime.Now;
                    complate = complate && processor.PostProcess(context);
                    this.ProcessorHander.PostProcess(this.Name, processor.Name, (DateTime.Now - sTime).TotalMilliseconds, context);
                     
                }
            });
            return complate;
        }
    }
}