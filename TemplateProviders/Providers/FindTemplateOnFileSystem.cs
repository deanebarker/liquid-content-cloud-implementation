using Alloy.Liquid.TemplateProviders;

namespace Alloy.Liquid;

public partial class Startup
{
    public class FindTemplateOnFileSystem : ITemplateSourceProvider
    {
        private string templatePath;

        public FindTemplateOnFileSystem(string templatePath)
        {
            this.templatePath = templatePath;
        }

        public string GetSource(string path)
        {
            var fullPath = Path.Combine(templatePath, path);

            if (File.Exists(fullPath))
            {
                var content = File.ReadAllText(fullPath);
                return content;
            }

            return null;
        }
    }

}
