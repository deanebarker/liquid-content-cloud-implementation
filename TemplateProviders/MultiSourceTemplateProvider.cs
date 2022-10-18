using Alloy.Liquid.Models.Blocks;
using Alloy.Liquid.Models.Media;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.Text;

namespace Alloy.Liquid.TemplateProviders
{
    public class MultiSourceTemplateProvider : IFileProvider
    {
        public List<ITemplateSourceProvider> Sources { get; set; } = new();

        public MultiSourceTemplateProvider(params ITemplateSourceProvider[] templateSourceProviders)
        {
            if (templateSourceProviders != null)
            {
                Sources.AddRange(templateSourceProviders);
            }
        }

        public IFileInfo GetFileInfo(string path)
        {
            // Clean up the path
            // The path comes in weird, for some reason
            path = path.Replace("\\", "/").TrimStart("/".ToCharArray());

            // Iterate all the sources
            foreach (var sourceProvider in Sources)
            {
                var sourceCode = sourceProvider.GetSource(path);
                if (sourceCode != null)
                {
                    // Found it, return this...
                    return new Template(sourceCode);
                }
            }

            // No source returned anything
            return NullTemplate.Instance;
        }

        // I don't think Fluid ever calls this (fingers crossed)
        public IDirectoryContents GetDirectoryContents(string subpath) { throw new NotImplementedException(); }
        public IChangeToken Watch(string filter) => TemplateCacheManager.Instance.GetToken(); // below...
    }

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

    public interface ITemplateSourceProvider
    {
        string GetSource(string path);
    }

    // Warning: this use's Microsoft's IChangeToken architecture which is...weird
    // I barely understand this. Sebastian had to help me with
    // Bottom line, when you want to clear the cache:
    //
    //   TemplateCacheManager.Clear();
    public class TemplateCacheManager
    {
        private CancellationTokenSource token;
        public static TemplateCacheManager Instance { get; private set; }

        static TemplateCacheManager()
        {
            Instance = new();

            var contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
            contentEvents.PublishedContent += (s, e) => Instance.CheckForCacheClear(e.Content);
            contentEvents.MovedContent += (s, e) => Instance.CheckForCacheClear(e.Content);
        }

        public TemplateCacheManager() { }

        public IChangeToken Token { get; private set; }

        public IChangeToken GetToken()
        {
            token = new CancellationTokenSource();
            return new CancellationChangeToken(token.Token);
        }

        private void CheckForCacheClear(IContent content)
        {
            if (content is TemplateBlock || content is TemplateFile)
            {
                Clear();
            }
        }

        public void Clear()
        {
            token.Cancel();
        }
    }
}