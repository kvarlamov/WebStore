using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Database;
using WebStore.Services.Map;

namespace WebStore.Services.Product
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;
        public Section GetSectionById(int id) => GetSections().FirstOrDefault(s => s.Id == id);

        public IEnumerable<Brand> GetBrands() => TestData.Brands;
        public Brand GetBrandById(int id) => GetBrands().FirstOrDefault(b => b.Id == id);

        public IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null)
        {
            var query = TestData.Products;

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            return query.Select(ProductMapper.ToDto);
        }

        public ProductDto GetProductById(int id) => TestData.Products.FirstOrDefault(p => p.Id == id).ToDto();
    }
}
