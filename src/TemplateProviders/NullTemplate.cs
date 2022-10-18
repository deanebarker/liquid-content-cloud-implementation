using Microsoft.Extensions.FileProviders;

namespace Alloy.Liquid.TemplateProviders
{
    // This represents a "file" that was not found
    // You still have to return an IFileInfo, you just need to set Exists to false
    public class NullTemplate : IFileInfo
    {
        public static NullTemplate Instance = new();
        public bool Exists => false;
        public long Length => 0;
        public string PhysicalPath => null;
        public string Name => null;
        public DateTimeOffset LastModified => DateTimeOffset.Now;
        public bool IsDirectory => false;
        public NullTemplate(string _ = null) { }
        public Stream CreateReadStream()
        {
            return null;
        }
    }
}