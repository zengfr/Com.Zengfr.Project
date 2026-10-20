using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Com.Zengfr.Proj.Common.Pipeline;

namespace Com.Zengfr.Proj.Common.Pipeline.Impl
{
    public class TestPipelineChain
    {
        public static void Main()
        {
            Console.BufferHeight = 3000;
            PipelineContext<long, long, long> context = new PipelineContext<long, long, long>();
            context.Hander = new LogPipelineHander<long, long, long>();

            var pf= PipelineFactory.Instance;
            IPipelineChain <long, long, long> pc = pf.CreateChain<long, long, long>();
          
            PutStep<long, long, long>(pf,pc, "A", 0, 15);
 
            var funcs2 = BuildFuncs<long, long, long>(30, 15);
            pc.Put(pf.BuildStep(funcs2,"B", 2));

            var funcs3 = BuildFuncs<long, long, long>(45, 15);
            pc.Put(pf.BuildStep(funcs3, "C", 1));

            pc.PreProcess(context);
            pc.PostProcess(context);
            Console.ReadLine();

        }
        private static void PutStep<I, M, O>(PipelineFactory pf, IPipelineChain<I, M, O> pc,string name, int index, int count)
        {
            Func<String, PipelineContext<I, M, O>, bool> func = BuildFunc<I, M, O>();
            for (int i = index; i < index + count; i++)
            {
                pc.Put(pf.BuildStep(func,string.Format("{0}-{1}", name, i)));
            }

        }
        private static ICollection<Func<String, PipelineContext<I, M, O>, bool>> BuildFuncs<I, M, O>(int index, int count)
        {
            var funcs = new List<Func<String, PipelineContext<I, M, O>, bool>>();
            for (int i = index; i < index + count; i++)
            {
                var func = BuildFunc<I, M, O>();
                funcs.Add(func);
            }
            return funcs;
        }
         static Random rnd = new Random();
        private static Func<String, PipelineContext<I, M, O>, bool> BuildFunc<I, M, O>()
        {
            Func<String, PipelineContext<I, M, O>, bool> func = (m,context) =>
            {
                Thread.Sleep(rnd.Next(11, 55));
                return true;
            };
            
            return func;
        }
    }
}
