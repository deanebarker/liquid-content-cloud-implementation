using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

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
}