using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Zengfr.Proj.Common.Pipeline.Impl
{
    public class PipelineFactory
    {
        static PipelineFactory instance;
        public static PipelineFactory Instance {
            get {
                if (instance == null)
                    instance = new PipelineFactory();
                return instance;
            }
        }
       public  IPipelineChain<I, M, O> CreateChain<I, M, O>() {
            return new PipelineChain<I, M, O>();
        }
        public IPipelineStep<I, M, O> CreateStep<I, M, O>()
        {
            return new PipelineStep<I, M, O>();
        }
        public IPipelineStep<I, M, O> BuildStep<I, M, O>(IEnumerable<IPipelineProcessor<I, M, O>> processors, int parallelism)
        {
            var step = CreateStep<I, M, O>();
            step.Parallelism = parallelism;
            step.Put(processors);
            return step;
        }

        public IPipelineStep<I, M, O> BuildStep<I, M, O>(IPipelineProcessor<I, M, O> processor)
        {
            var step = CreateStep<I, M, O>();
            step.Put(processor);
            return step;
        }
        public IPipelineStep<I, M, O> BuildStep<I, M, O>(Func<String, PipelineContext<I, M, O>, bool> func, string name)
        {
            var processor = BuildProcessor(func, name);
            var step = BuildStep(processor);
            step.Name = name;
            return step;
        }
        public IPipelineStep<I, M, O> BuildStep<I, M, O>(IEnumerable<Func<String, PipelineContext<I, M, O>, bool>> funcs,string name, int parallelism)
        {
            var processors = BuildProcessors(funcs,name);
            var step = BuildStep(processors, parallelism);
            step.Name = name;
            return step;
        }

        public ICollection<IPipelineProcessor<I, M, O>> BuildProcessors<I, M, O>(IEnumerable<Func<String, PipelineContext<I, M, O>, bool>> funcs,string name)
        {
            var processors = new List<IPipelineProcessor<I, M, O>>();
            long index = 0;
            foreach (var func in funcs)
            {
                var pl = BuildProcessor(func,string.Format("{0}-{1}",name,index++));
                processors.Add(pl);
            }
            return processors;
        }
        static Random rnd = new Random();
        public IPipelineProcessor<I, M, O> BuildProcessor<I, M, O>(Func<String, PipelineContext<I, M, O>, bool> func,string name)
        {
            var pl = new FuncPipelineProcessor<I, M, O>(func);
            pl.Name = name;
            return pl;
        }
    }
}
