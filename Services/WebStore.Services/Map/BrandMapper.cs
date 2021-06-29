using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
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
}