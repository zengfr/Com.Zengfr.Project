using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    internal abstract class StageBase<I, M, O>:IPipelineStage<I, M, O>
    {
        public String Name { get; set; }
        public bool Enabled { get; set; }
        public int PreProcessOrder { get; set; }
        public int PostProcessOrder { get; set; }
        public IPipelineProcessorHander<I, M, O> ProcessorHander { get; set; }
        protected ICollection<IPipelineProcessor<I, M, O>> Processors { get; set; }
        public StageBase(ICollection<IPipelineProcessor<I, M, O>> processors)
        {
            this.Processors = processors;
            this.Enabled = true;
        }
        public StageBase(IPipelineProcessor<I, M, O> processor) {
            Processors = new List<IPipelineProcessor<I, M, O>>();
            Processors.Add(processor);
            this.Enabled = true;
        }

        public abstract bool PreProcess(PipelineContext<I, M, O> context);
        public abstract bool PostProcess(PipelineContext<I, M, O> context);
    }
}
