using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace TaskApplication.Task
{
   public class SpiderQueue
   {
       public static SpiderQueue Instance = new SpiderQueue();
        private StreamReader QueueReader = null;
        private StreamWriter QueueWriter = null;
        string QueueFile = "C:\\QueueFile.txt";
        int bufferSize =512;
        Queue<string> Queue = new Queue<string>();
        SpiderQueue()
        {
            if (this.QueueWriter == null)
            {
                FileStream enqueueStream = new FileStream(QueueFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);
                this.QueueWriter = new StreamWriter(enqueueStream, Encoding.UTF8, bufferSize);
            }
            if (this.QueueReader == null)
            {
                FileStream dequeueStream = new FileStream(QueueFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                this.QueueReader = new StreamReader(dequeueStream, Encoding.UTF8,false, bufferSize);
            }
        }
        public void Enqueue(string url)
        {
            if (url == null || !url.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            this.Queue.Enqueue(url);

           // this.QueueWriter.WriteLine(url);
        }
        public void Flush()
        {
           // this.QueueWriter.Flush();
        }
        public string Dequeue()
        {  if(this.Queue.Count!=0)
              return this.Queue.Dequeue();
           // if (this.QueueReader.EndOfStream)
            { 
                return null;
            }
           // return this.QueueReader.ReadLine();
        }
    }
}
