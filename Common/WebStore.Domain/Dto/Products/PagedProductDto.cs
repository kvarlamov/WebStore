using System.Collections.Generic;

namespace WebStore.Domain.Dto.Products
{
    /// <summary>
    /// Single page of products from catalog
    /// </summary>
    public class PagedProductDto
    {
        /// <summary>
        /// Products on page
        /// </summary>
        public IEnumerable<ProductDto> Products { get; set; }
        
        /// <summary>
        /// Total amount of products
        /// </summary>
        public int TotalCount { get; set; }
    }
}