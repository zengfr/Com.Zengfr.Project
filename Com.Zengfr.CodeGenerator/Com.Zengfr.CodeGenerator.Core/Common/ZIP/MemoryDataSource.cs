using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
namespace WebApplication1.ZIP
{
    public class MemoryDataSource : IStaticDataSource
    {
        public string Name { get; set; }
        public Stream Stream { get; private set; }
        public MemoryDataSource(string name, string content)
        {
            Build(name, content);
        }
        public MemoryDataSource(string name, string content, Encoding encoding)
        {
            Build(name, content, encoding);
        }
        public MemoryDataSource(string name, byte[] content)
        {
            Build(name, content);
        }
        public MemoryDataSource(string name, Stream stream)
        {
            Build(name, stream);
        }

        public void Build(string name, string content)
        {
            Build(name, content, Encoding.UTF8);
        }
        public void Build(string name, string content, Encoding encoding)
        {
            Build(name, encoding.GetBytes(content));
        }
        public void Build(string name, byte[] content)
        {
            Build(name, new MemoryStream(content));
        }
        public void Build(string name, Stream stream)
        {
            this.Name = name;
            this.Stream = stream;
        }
        public Stream GetSource()
        {
            if (Stream.CanSeek)
            {
                Stream.Seek(0, SeekOrigin.Begin);
            }
            return Stream;
        }
    }
}