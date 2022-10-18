using Alloy.Liquid.Models;
using Alloy.Liquid.Models.Blocks;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;
using EPiServer.Web.Templating;
using Fluid;
using Optimizely.CMS.Labs.LiquidTemplating.ViewEngine;
using System.ComponentModel.DataAnnotations;

namespace Alloy.Liquid.Liquid.Models.Blocks;

/// <summary>
/// Used to insert a link which is styled as a button
/// </summary>
[SiteContentType(GUID = "426CF12D-1F01-4EA0-922F-0778314DDAF0")]
[SiteImageUrl]
public class TemplateBlock : SiteBlockData
{
    [Display(Order = 1, GroupName = SystemTabNames.Content)]
    [Required]
    [ValidateLiquidParse]
    [ClientEditor(ClientEditingClass = "/js/editor.js")]
    public virtual string TemplateCode { get; set; }
}



[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public class ValidateLiquidParse : ValidationAttribute
{
    public ValidateLiquidParse()
    {

    }

    public override bool IsValid(object value)
    {
        return GetParseException(value.ToString()) == null;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var message = GetParseException(value?.ToString());
        if (message != null)
        {
            return new ValidationResult($"Liquid parse error: {message}");
        }

        return ValidationResult.Success;
    }

    private string GetParseException(string liquidCode)
    {
        var parser = new CmsFluidViewParser(new FluidParserOptions());

        try
        {
            var template = parser.Parse(liquidCode);
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return null;
    }
}


//[EditorDescriptorRegistration(TargetType = typeof(string))]
//public class ConfigureTemplateCodeEditor : EditorDescriptor
//{
//    public override void ModifyMetadata(
//        ExtendedMetadata metadata,
//        IEnumerable<Attribute> attributes)
//    {
//        base.ModifyMetadata(metadata, attributes);
//        if (metadata.PropertyName == "TemplateCode")
//        {
//            metadata.TemplateHint = "AceEditor_handlebars";
//            metadata.ClientEditingClass = "aceeditor/aceEditor";


//        }
//    }
//}