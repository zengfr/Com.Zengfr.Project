using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.Pipeline
{
    internal class SyncStage<I, M, O> : StageBase<I, M, O>, IPipelineSyncStage<I, M, O>
    {
        public SyncStage(ICollection<IPipelineProcessor<I, M, O>> processors) : base(processors)
        {

        }
        public SyncStage(IPipelineProcessor<I, M, O> processor) : base(processor)
        {
            
        }
       
        public override bool PreProcess(PipelineContext<I, M, O> context)
        {
            var complate = true;
            foreach (var processor in Processors)
            {
                if (complate & processor.Enabled)
                {
                    var sTime = DateTime.Now;
                    complate = complate && processor.PreProcess(context);
                    this.ProcessorHander.PreProcess(this.Name, processor.Name, (DateTime.Now - sTime).TotalMilliseconds, context);
                }
            }
            return complate;
        }

        public override bool PostProcess(PipelineContext<I, M, O> context)
        {
            var complate = true;
            foreach (var processor in Processors)
            {
                if (complate & processor.Enabled)
                {
                    var sTime = DateTime.Now;
                    complate = complate && processor.PostProcess(context);
                    this.ProcessorHander.PostProcess(this.Name, processor.Name, (DateTime.Now - sTime).TotalMilliseconds, context);
                }
            }
            return complate;
        }


    }
}
