using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase, IProductData
    {
        private readonly IProductData _productData;

        public ProductsController(IProductData productData)
        {
            _productData = productData;
        }

        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands() => _productData.GetBrands();

        [HttpGet("{id}"), ActionName("Get")]
        public ProductDto GetProductById(int id) => _productData.GetProductById(id);

        [HttpPost, ActionName("Post")]
        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null) => _productData.GetProducts(Filter);

        [HttpGet("sections")]
        public IEnumerable<Section> GetSections() => _productData.GetSections();
    }
}
