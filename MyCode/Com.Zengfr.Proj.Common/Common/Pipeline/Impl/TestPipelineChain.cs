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
            PipelineChain<long, long, long> pc = new PipelineChain<long, long, long>();
            pc.SetInput(1);
            pc.SetProcessorHander(new LogPipelineProcessorHander<long, long, long>());
            PutStage(pc, 0, 0, 20);
            PutStage(pc, 2, 20, 20);

            var funcs2 = BuildFuncs<long, long, long>(40, 20);
            pc.PutStage(funcs2, "A", 2);

            var funcs3 = BuildFuncs<long, long, long>(60, 20);
            pc.PutStage(funcs3, "B", 1);

            pc.Process();

            pc.GetOutput();
            Console.ReadLine();

        }
        private static void PutStage<I, M, O>(PipelineChain<I, M, O> pc, int parallelism, int index, int count)
        {
            Func<String, PipelineContext<I, M, O>, bool> func = BuildFunc<I, M, O>();
            for (int i = index; i < index + count; i++)
            {
                pc.PutStage(func, "" + i, parallelism);
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
