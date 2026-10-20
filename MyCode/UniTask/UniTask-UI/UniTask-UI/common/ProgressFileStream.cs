using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaobaoTask
{
    public class ProgressFileStream : FileStream
    {
        public ProgressFileStream(string path)
        : base(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read)
    {
        }

        protected long currentTotalSize { get; private set; }

        public event Action<long,int,int> WriteHandler;

        public override void Write(byte[] array, int offset, int count)
        {
            base.Write(array, offset, count);
            currentTotalSize += count;
            var h = WriteHandler;
            if (h != null)
                h(currentTotalSize, offset, count);
        }

    }
}
