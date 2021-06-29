using WebStore.Domain.Dto.Products;

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
                Brand = product.Brand.ToDto(),
                Section = product.Section.ToDTO()
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
                BrandId = product.Brand?.Id,
                Brand = product.Brand.FromDto(),
                SectionId = product.Section.Id,
                Section = product.Section?.FromDTO()
            };
    }
}