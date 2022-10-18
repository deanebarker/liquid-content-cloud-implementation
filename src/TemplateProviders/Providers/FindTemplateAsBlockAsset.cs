using Alloy.Liquid.TemplateProviders;
using EPiServer.ServiceLocation;

namespace Alloy.Liquid;

public partial class Startup
{
    public class FindTemplateAsBlockAsset : ITemplateSourceProvider
    {
        private int templateFolderId;

        public FindTemplateAsBlockAsset(int templateFolderId)
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
                    return currentItem.Property["TemplateCode"].Value.ToString();
                }
            }

            return null;
        }
    }

}
