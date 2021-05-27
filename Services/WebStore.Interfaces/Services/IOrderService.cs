using System.Collections.Generic;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Interfaces.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderDto> GetUserOrders(string UserName);
        OrderDto GetOrderById(int id);
        OrderDto CreateOrder(CreateOrderModel OrderModel, string UserName);
    }
}
