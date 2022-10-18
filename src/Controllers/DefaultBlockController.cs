using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using Fluid;
using Fluid.ViewEngine;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Optimizely.CMS.Labs.LiquidTemplating.ViewEngine;
using System;
using System.Threading.Tasks;

namespace Alloy.Liquid.Liquid.Controllers
{
    [TemplateDescriptor(Inherited = true)]
    public class DefaultBlockController : AsyncBlockComponent<BlockData>
    {
        protected override async Task<IViewComponentResult> InvokeComponentAsync(BlockData currentBlock)
        {
            var _typeRepo = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var blockTypeName = _typeRepo.Load(((IContent)currentBlock).ContentTypeID).Name;

            //if (currentBlock.Property["TemplateCode"] != null && currentBlock.Property["TemplateCode"].Value != null)
            //{
            //    var parser = new FluidParser(new Fluid.FluidParserOptions() { AllowFunctions = true });
            //    var template = parser.Parse(currentBlock.Property["TemplateCode"].Value.ToString());
            //    var context = new TemplateContext(currentBlock);
            //    var html = template.Render(context);
            //    return await Task.FromResult(new HtmlContentViewComponentResult(new HtmlString(html)));
            //}


            return await Task.FromResult(View(string.Format("~/Shared/Blocks/{0}.liquid", blockTypeName), currentBlock));
        }

    }
}