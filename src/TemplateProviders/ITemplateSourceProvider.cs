namespace Alloy.Liquid.TemplateProviders
{
    public interface ITemplateSourceProvider
    {
        string GetSource(string path);
    }
}