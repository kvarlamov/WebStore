using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebStore.Clients.Base;
using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration configuration) : base(configuration, "api/products")
        {

        }

        public IEnumerable<Brand> GetBrands() => Get<List<Brand>>($"{_ServiceAddress}/brands");

        public ProductDto GetProductById(int id) => Get<ProductDto>($"{_ServiceAddress}/{id}");

        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null) => Post(_ServiceAddress, Filter)
            .Content
            .ReadAsAsync<List<ProductDto>>()
            .Result;

        public IEnumerable<Section> GetSections() => Get<List<Section>>($"{_ServiceAddress}/sections");
    }
}
