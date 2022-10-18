using Microsoft.Extensions.FileProviders;
using System.Text;

namespace Alloy.Liquid.TemplateProviders
{
    public class Template : IFileInfo
    {
        private string fileContents;
        public bool Exists => true;
        public long Length => fileContents.Length;
        public string PhysicalPath => null;
        public string Name => null;
        public DateTimeOffset LastModified => DateTimeOffset.Now;
        public bool IsDirectory => false;
        public Template(string _fileContents)
        {
            fileContents = _fileContents;
        }
        public Stream CreateReadStream()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(fileContents));
        }
    }
}