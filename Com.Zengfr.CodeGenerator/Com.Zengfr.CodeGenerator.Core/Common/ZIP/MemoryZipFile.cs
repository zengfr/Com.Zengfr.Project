using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
namespace WebApplication1.ZIP
{


    public class MemoryZipFile
    {
        public MemoryStream Stream { get; private set; }

        public MemoryZipFile()
        {
            Stream = new MemoryStream();
        }

        public void AddEntrie(string fileName, string content)
        {
            AddEntries( new MemoryDataSource[]{ new MemoryDataSource(fileName,content)});
        }
        public void AddEntrie(string fileName, byte[] content)
        {
            AddEntries(new MemoryDataSource[] { new MemoryDataSource(fileName, content) });
        }
        public void AddEntries(IDictionary<string, string> fileDataSources)
        {
            var memoryFileDataSources = new List<MemoryDataSource>();
            foreach (var f in fileDataSources)
            {
                memoryFileDataSources.Add(new MemoryDataSource(f.Key, f.Value));
            }
            AddEntries(string.Empty, memoryFileDataSources.ToArray());
        }
        public void AddEntries(MemoryDataSource[] memoryFileDataSources)
        {
            AddEntries(string.Empty, memoryFileDataSources);
        }

        public void AddEntries(string directory, MemoryDataSource[] memoryFileDataSources)
        {
            if (memoryFileDataSources != null && memoryFileDataSources.Length > 0)
            {
                using (ZipFile f = new ZipFile(this.Stream))
                {
                    f.IsStreamOwner = false;
                    f.BeginUpdate(new MemoryArchiveStorage());
                    foreach (var source in memoryFileDataSources)
                    {
                        f.Add(source, string.Format("{0}{1}", directory, source.Name));
                    }
                    f.CommitUpdate();
                }
            }
        }

        public void SaveToFile(string file)
        {
            File.WriteAllBytes(file, Stream.ToArray());
        }
    }

}