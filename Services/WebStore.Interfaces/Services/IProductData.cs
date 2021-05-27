using System.Collections.Generic;
using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();

        IEnumerable<ProductDto> GetProducts(ProductFilter Filter = null);

        ProductDto GetProductById(int id);
    }
}
