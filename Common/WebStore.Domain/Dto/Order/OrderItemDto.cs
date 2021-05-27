using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Dto.Order
{
    public class OrderItemDto : BaseEntity
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
