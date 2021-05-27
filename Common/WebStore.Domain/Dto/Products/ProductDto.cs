using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Dto.Products
{
    public class ProductDto : INamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public BrandDto Brand { get; set; }
        public decimal Price { get; set; }
    }
}
