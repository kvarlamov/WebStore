using System.Linq;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order order) => order is null ? null : new OrderDto 
        {
            Id = order.Id,
            Name = order.Name,
            Date = order.Date,
            Adress = order.Address,
            Phone = order.Phone,
            OrderItems = order.OrderItems.Select(OrderItemMapper.ToDto)
        };

        public static Order FromDto(this OrderDto order) => order is null
            ? null
            : new Order
            {
                Id = order.Id,
                Name = order.Name,
                Date = order.Date,
                Address = order.Adress,
                Phone = order.Phone,
                OrderItems = order.OrderItems.Select(OrderItemMapper.FromDto).ToArray()
            };
    }

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