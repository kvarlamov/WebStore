using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Dto.Products
{
    /// <summary>
    /// Information about product
    /// </summary>
    public class ProductDto : INamedEntity, IOrderedEntity
    {
        /// <summary>
        /// number of sort
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// product id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// reference to image
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Brand of product
        /// </summary>
        public BrandDto Brand { get; set; }

        /// <summary>
        /// Price
        /// </summary>
        public decimal Price { get; set; }
    }
}
