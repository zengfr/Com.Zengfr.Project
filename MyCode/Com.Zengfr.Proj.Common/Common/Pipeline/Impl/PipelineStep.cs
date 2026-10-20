using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    public class PipelineStep<I, M, O> : IPipelineStep<I, M, O>
    {
        protected IList<IPipelineProcessor<I, M, O>> processors = new List<IPipelineProcessor<I, M, O>>();
        public int Retry { get; set; }

        public int Parallelism { get; set; }
        public bool Disabled { get; set; }
        public String Name { get; set; }
        
        public PipelineStep()
        {
            
        }

        public bool PreProcess(PipelineContext<I, M, O> context)
        {
            var result = true;
            if (Parallelism < 2)
            {
                foreach (var processor in processors)
                {
                    if (result & !processor.Disabled)
                    {
                        result = result && PreProcess(processor, context);
                    }
                }
            }
            else
            {
                var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = this.Parallelism };
                Parallel.ForEach(processors, parallelOptions, (processor, loopState) =>
                {
                    if (result & !processor.Disabled && !loopState.IsStopped)
                    {
                        result = result &&PreProcess(processor,context);
                    }
                });
            }
            return result;
        }
        public bool PostProcess(PipelineContext<I, M, O> context)
        {
            var result = true;
            var processors2 = processors.Reverse();
            if (Parallelism < 2)
            {
                foreach (var processor in processors2)
                {
                    if (result & !processor.Disabled)
                    {
                        result = result && PostProcess(processor, context);
                    }
                }
            }
            else
            {
                var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = this.Parallelism };
                Parallel.ForEach(processors2, parallelOptions, (processor, loopState) =>
                {
                    if (result & !processor.Disabled && !loopState.IsStopped)
                    {
                        result = result &&   PostProcess(processor,context);
                    }
                });
            }
            return result;
        }
        protected bool PreProcess(IPipelineProcessor<I, M, O> processor,PipelineContext<I, M, O> context) {

            var startTime = DateTime.Now;
            var rs=processor.PreProcess(context);
            if (context.Hander != null)
            {
                context.Hander.PreProcess(GetProcessorName(this), GetProcessorName(processor), (DateTime.Now-startTime).TotalMilliseconds, context);
            }
            return rs;
        }
        protected bool PostProcess(IPipelineProcessor<I, M, O> processor, PipelineContext<I, M, O> context)
        {

            var startTime = DateTime.Now;
            var rs = processor.PostProcess(context);
            if (context.Hander != null)
            {
                context.Hander.PostProcess(GetProcessorName(this), GetProcessorName(processor), (DateTime.Now - startTime).TotalMilliseconds, context);
            }
            return rs;
        }
        protected String GetProcessorName(IPipelineProcessor<I, M, O> processor) {
            return processor.Name ?? GetTypeName(processor.GetType());
        }
        protected String GetTypeName(Type type)
        {
            if (type.IsGenericType)
            {
                return string.Format("{0}", type.GetGenericTypeDefinition().Name, string.Join("-", type.GenericTypeArguments.Select(t => t.Name)));
            }
            return type.Name;
        }
        public void Put(IEnumerable<IPipelineProcessor<I, M, O>> processors)
        {
            foreach (var processor in processors)
                Put(processor);
        }

        public void Put(IPipelineProcessor<I, M, O> processor)
        {
            processors.Add(processor);
        }
    }
}
