using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class SitemapController : Controller
    {
        // GET
        public IActionResult Index([FromServices] IProductData productData)
        {
            var nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index", "Home")),
                new SitemapNode(Url.Action("ContactUs", "Home")),
                new SitemapNode(Url.Action("Blog", "Home")),
                new SitemapNode(Url.Action("BlogSingle", "Home")),
                new SitemapNode(Url.Action("Shop", "Catalog")),
                new SitemapNode(Url.Action("Index", "WebApiTest"))
            };
            
            foreach (var section in productData.GetSections())
            {
                nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new {SectionId = section.Id})));
            }

            foreach (var brand in productData.GetBrands())
            {
                nodes.Add(new SitemapNode(Url.Action("Shop", "Catalog", new {BrandId = brand.Id})));
            }
            
            foreach (var product in productData.GetProducts(new ProductFilter()).Products)
            {
                nodes.Add(new SitemapNode(Url.Action("Details", "Catalog", new {product.Id})));
            }
            
            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}