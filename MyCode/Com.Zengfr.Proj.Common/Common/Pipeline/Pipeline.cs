using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    public class Pipeline<I, M, O>
    {
        internal ConcurrentBag<StageBase<I, M, O>> stages = new ConcurrentBag<StageBase<I, M, O>>();
        protected PipelineContext<I, M, O> context;
        protected IPipelineHander<I, M, O> hander;
        public void SetInput(I input)
        {
            this.context = new PipelineContext<I, M, O>();
            this.context.Input = input;
        }
        public void SetHander(IPipelineHander<I, M, O> hander)
        {
            this.hander = hander;
        }
        public void PutAsyncStage(ICollection<IPipelineStage<I, M, O>> stages)
        {
            PutAsyncStage(stages,2);
        }
        public void PutAsyncStage(ICollection<IPipelineStage<I, M, O>> stages, int maxDegreeOfParallelism)
        {
            stages.Add(new AsyncStage<I, M, O>(stages, maxDegreeOfParallelism));
        }
        public void PutSyncStage(ICollection<IPipelineStage<I, M, O>> stages)
        {
            stages.Add(new SyncStage<I, M, O>(stages));
        }
        public void PutSyncStage(IPipelineStage<I, M, O> stage)
        {
            stages.Add(new SyncStage<I, M, O>(stage));
        }
        public void Process()
        {
            bool completed = false;
            //while (!completed)
            {
                completed = true;
                foreach (var stage in stages)
                {
                    completed = completed && ProcessStage(stage);
                    if (!completed)
                        break;
                }
            }
        }
        internal bool ProcessStage(StageBase<I, M, O> stage)
        {
            bool completed = true;
            if (hander != null&& stage.Hander==null)
            {
                stage.Hander = hander;
            }
            completed = completed && stage.PreProcess(context);
            completed = completed&&stage.Process(context);
            return completed;
        }
    }
    public class Pipeline<I, O> : Pipeline<I, I, O>
    {

    }
}
