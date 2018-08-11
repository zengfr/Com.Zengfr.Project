using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Com.Zengfr.Proj.Common.Pipeline.Impl;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    public class PipelineChain<I, M, O>
    {
        internal ConcurrentBag<StageBase<I, M, O>> stages = new ConcurrentBag<StageBase<I, M, O>>();
        protected PipelineContext<I, M, O> context;
        protected IPipelineProcessorHander<I, M, O> processorHander;
        public void SetInput(I input)
        {
            this.context = new PipelineContext<I, M, O>();
            this.context.Input = input;
        }
        public O GetOutput()
        {
            return this.context.Output;
        }
        public void SetProcessorHander(IPipelineProcessorHander<I, M, O> processorHander)
        {
            this.processorHander = processorHander;
        }
        public void PutStage(Func<String,PipelineContext<I, M, O>,bool> func, string name, int parallelism)
        {
            var processor = BuildProcessor(func);
            processor.Name = name;
            PutStage(processor, parallelism);
        }
        public void PutStage(ICollection<Func<String, PipelineContext<I, M, O>, bool>> funcs, string name, int parallelism)
        {
            var processors = BuildProcessors(funcs);
            PutStage(processors,name, parallelism);
        }
        public void PutStage(IPipelineProcessor<I, M, O> processor, int parallelism)
        {
            if (parallelism <= 1)
            {
                PutSyncStage(processor);
            }
            else
            {
                PutAsyncStage(processor);
            }
        }
        public void PutStage(ICollection<IPipelineProcessor<I, M, O>> processors, string name, int parallelism)
        {
            if (parallelism <= 1)
            {
                PutSyncStage(processors, name);
            }
            else
            {
                PutAsyncStage(processors, name,parallelism);
            }
        }
        protected void PutAsyncStage(ICollection<IPipelineProcessor<I, M, O>> processors, string name, int parallelism)
        {
            stages.Add(new AsyncStage<I, M, O>(processors, parallelism) { Name = BuildStageName(name) });
        }
        protected void PutAsyncStage(IPipelineProcessor<I, M, O> processor)
        {
            stages.Add(new AsyncStage<I, M, O>(processor) { Name=BuildStageName(processor.Name) });
        }
        protected void PutSyncStage(ICollection<IPipelineProcessor<I, M, O>> processors,string name)
        {
            stages.Add(new SyncStage<I, M, O>(processors) { Name = BuildStageName(name) });
        }
        protected void PutSyncStage(IPipelineProcessor<I, M, O> processor)
        {
            stages.Add(new SyncStage<I, M, O>(processor) { Name = BuildStageName(processor.Name) });
        }
        private string BuildStageName(string name) {
            return string.Format("S-{0}", name);
        }
        public ICollection<IPipelineProcessor<I, M, O>> BuildProcessors<I, M, O>(ICollection<Func<String, PipelineContext<I, M, O>, bool>> funcs)
        {
            var processors = new List<IPipelineProcessor<I, M, O>>();
            int index = 0;
            foreach(var func in funcs)
            {
                var pl = BuildProcessor<I, M, O>(func);
                pl.Name = string.Format("F-{0}", index++);
                processors.Add(pl);
            }
            return processors;
        }
        static Random rnd = new Random();
        public IPipelineProcessor<I, M, O> BuildProcessor<I, M, O>(Func<String, PipelineContext<I, M, O>, bool> func)
        {
            var pl = new FuncPipelineProcessor<I, M, O>(func);
            return pl;
        }
        public void Process()
        {
            bool completed = true;
            var stagesOrdered2 = stages.OrderBy(t => t.PreProcessOrder);
            foreach (var stage in stagesOrdered2)
            {
                completed = completed && PreProcessStage(stage);
                if (!completed)
                    break;
            }
            if (completed)
            {
                var stagesOrdered3 = stages.OrderBy(t => t.PostProcessOrder);
                foreach (var stage in stagesOrdered3)
                {
                    completed = completed && PostProcessStage(stage);
                    if (!completed)
                        break;
                }
            }

        }
        internal bool PreProcessStage(StageBase<I, M, O> stage)
        {
            bool completed = true;
            if (stage.Enabled)
            {
                if (processorHander != null && stage.ProcessorHander == null)
                {
                    stage.ProcessorHander = processorHander;
                }
                completed = completed && stage.PreProcess(context);
            }
            return completed;
        }
        internal bool PostProcessStage(StageBase<I, M, O> stage)
        {
            bool completed = true;
            if (stage.Enabled)
            {
                if (processorHander != null && stage.ProcessorHander == null)
                {
                    stage.ProcessorHander = processorHander;
                }
                completed = completed && stage.PostProcess(context);
            }
            return completed;
        }
    }
    public class PipelineChain<I, O> : PipelineChain<I, I, O>
    {

    }
}
