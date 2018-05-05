using System;
using System.Threading;
using Common.Logging;
using Spring.Http.Client;
using Spring.Http.Client.Interceptor;
using System.Threading;
using System.Net;
using System.Threading.Tasks;
namespace Spring.Rest.Utils
{
    public class ProcessSemaphore
    {
        static ProcessSemaphore()
        {
            SetMaxConcurrent(10);
        }
        public static int Concurrent;
        public static int WaitCount;
        public static int WorkerThreads;
        public static int CompletionPortThreads;
        private static Semaphore semaphore;
        public static void SetMaxConcurrent(int maxConcurrent)
        {
            semaphore = new System.Threading.Semaphore(maxConcurrent, maxConcurrent * 5);

            var max = maxConcurrent *300;
            ServicePointManager.DefaultConnectionLimit = max;
            ServicePointManager.MaxServicePoints=max;
        
            ThreadPool.SetMaxThreads(max, max);

        }
        public static void WaitOne()
        {
            Interlocked.Increment(ref WaitCount);
            semaphore.WaitOne();
            Interlocked.Decrement(ref WaitCount);

            Interlocked.Increment(ref Concurrent);
            ThreadPool.GetAvailableThreads(out WorkerThreads, out CompletionPortThreads);
        }

        public static void Release()
        {
            semaphore.Release();
            Interlocked.Decrement(ref Concurrent);
        }

        
    }

    public class PerfRequestBeforeInterceptor : IClientHttpRequestBeforeInterceptor
    {

        protected static ILog log = LogManager.GetLogger(typeof(PerfRequestBeforeInterceptor));

        public void BeforeExecute(IClientHttpRequestContext request)
        {
            System.Threading.Thread.Sleep(50);
            ProcessSemaphore.WaitOne();
            log.Debug(String.Format("Before:{0},{1};{2},{3};{4}\t{5}", 
                ProcessSemaphore.WaitCount, ProcessSemaphore.Concurrent, 
                ProcessSemaphore.WorkerThreads,ProcessSemaphore.CompletionPortThreads,
                request.Method,request.Uri));
            //log.Info(String.Format("RequestBefore:{0},{1},{2}", request.Method, request.Uri, ""));
        }
    }
    public class PerfRequestSyncInterceptor : IClientHttpRequestSyncInterceptor
    {
        protected static ILog log = LogManager.GetLogger(typeof(PerfRequestSyncInterceptor));
        public IClientHttpResponse Execute(IClientHttpRequestSyncExecution execution)
        {
            IClientHttpResponse response = null;
            try
            {
                response = execution.Execute();
            }
            catch { };
            ProcessSemaphore.Release();
            log.Debug(String.Format(
             "Sync  :{0},{1}\t{2}",
             execution.Method, response != null ? response.StatusCode : 0, execution.Uri));

            return response;
        }
    }

    public class PerfRequestAsyncInterceptor : IClientHttpRequestAsyncInterceptor
    {

        protected static ILog log = LogManager.GetLogger(typeof(PerfRequestAsyncInterceptor));
        public void ExecuteAsync(IClientHttpRequestAsyncExecution execution)
        {
            
            execution.ExecuteAsync(t =>
            {
                ProcessSemaphore.Release();
                log.Debug(String.Format("Async :{0},{1}\t{2}", execution.Method, t.Response != null ? t.Response.StatusCode : 0, execution.Uri));
            });
        }
    }

}
