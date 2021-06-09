using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class ProductMapper
    {
        public static ProductDto ToDto(this Domain.Entities.Product product) => product is null
            ? null
            : new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Order = product.Order,
                Brand = product.Brand.ToDto()
            };
        public static Domain.Entities.Product FromDto(this ProductDto product) => product is null
            ? null
            : new Domain.Entities.Product
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Order = product.Order,
                BrandId = product?.Brand.Id,
                Brand = product.Brand.FromDto()
            };
    }

    public static class BrandMapper
    {
        public static BrandDto ToDto(this Brand brand) => brand is null
            ? null
            : new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name
            };
        public static Brand FromDto(this BrandDto brand) => brand is null
            ? null
            : new Brand
            {
                Id = brand.Id,
                Name = brand.Name
            };
    }

    public static class SectionMapper
    {
        
    }
}