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
    /// <summary>
    /// controller of products
    /// </summary>
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase, IProductData
    {
        private readonly IProductData _productData;

        public ProductsController(IProductData productData)
        {
            _productData = productData;
        }

        /// <summary>
        /// get all brands
        /// </summary>
        /// <returns>return list of brands</returns>
        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands() => _productData.GetBrands();

        /// <summary>
        /// get product info by id
        /// </summary>
        /// <param name="id">id of product</param>
        /// <returns>product by id</returns>
        [HttpGet("{id}"), ActionName("Get")]
        public ProductDto GetProductById(int id) => _productData.GetProductById(id);

        /// <summary>
        /// selection products by filter
        /// </summary>
        /// <param name="Filter">criteria of products find</param>
        /// <returns>List of products with filtration</returns>
        [HttpPost, ActionName("Post")]
        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null) => _productData.GetProducts(Filter);


        /// <summary>
        /// get all sections
        /// </summary>
        /// <returns>return list of sections</returns>
        [HttpGet("sections")]
        public IEnumerable<Section> GetSections() => _productData.GetSections();
    }
}
