using Alloy.Liquid.TemplateProviders;
using EPiServer.Framework.Blobs;
using EPiServer.ServiceLocation;
using System.Text;

namespace Alloy.Liquid;

public partial class Startup
{
    public class FindTemplateAsMediaAsset : ITemplateSourceProvider
    {
        private int templateFolderId;

        public FindTemplateAsMediaAsset(int templateFolderId)
        {
            this.templateFolderId = templateFolderId;
        }

        public string GetSource(string path)
        {
            // Try to find an asset for this
            var repo = ServiceLocator.Current.GetInstance<IContentRepository>();
            var currentItem = repo.Get<IContent>(new ContentReference(templateFolderId));

            foreach (var segment in path.Split("/"))
            {
                currentItem = repo.GetChildren<IContent>(currentItem.ContentLink).FirstOrDefault(i => i.Name.ToLower() == segment.ToLower());
                if (currentItem == null)
                {
                    break;
                }

                if (segment.ToLower().EndsWith(".liquid"))
                {
                    return Encoding.UTF8.GetString(((MediaData)currentItem).BinaryData.ReadAllBytes());
                }
            }

            return null;
        }
    }

}
