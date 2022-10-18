using Alloy.Liquid.Liquid.Models.Blocks;
using Alloy.Liquid.Liquid.Models.Media;
using EPiServer.ServiceLocation;
using Microsoft.Extensions.Primitives;

namespace Alloy.Liquid.TemplateProviders
{
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