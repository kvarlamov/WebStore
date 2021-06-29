using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class OrderItemMapper
    {
        public static OrderItemDto ToDto(this OrderItem item) => item is null
            ? null
            : new OrderItemDto
            {
                Id = item.Id,
                Price = item.Price,
                Quantity = item.Quantity
            };
        public static OrderItem FromDto(this OrderItemDto item) => item is null
            ? null
            : new OrderItem
            {
                Id = item.Id,
                Price = item.Price,
                Quantity = item.Quantity
            };
    }
}