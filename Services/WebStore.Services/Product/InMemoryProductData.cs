using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Database;

namespace WebStore.Services.Product
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null)
        {
            var query = TestData.Products;

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            return query.Select(p => new ProductDto
            {
                Id = p.Id,
                Brand = p.Brand is null ? null : new BrandDto 
                { 
                    Id = p.Brand.Id,
                    Name = p.Brand.Name
                },
                Name = p.Name,
                Order= p.Order,
                Price = p.Price,
                ImageUrl  = p.ImageUrl
            });
        }

        public ProductDto GetProductById(int id)
        {
            var product = TestData.Products.FirstOrDefault(p => p.Id == id);
            return new ProductDto
            {
                Id = product.Id,
                Brand = product.Brand is null ? null : new BrandDto
                {
                    Id = product.Brand.Id,
                    Name = product.Brand.Name
                },
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            };
        }
    }
}
