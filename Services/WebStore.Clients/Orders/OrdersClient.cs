using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using WebStore.Clients.Base;
using WebStore.Domain.Dto.Order;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Orders
{
    public class OrdersClient : BaseClient, IOrderService
    {
        public OrdersClient(IConfiguration config) : base(config, "api/orders")
        {

        }

        public OrderDto CreateOrder(CreateOrderModel OrderModel, string UserName) => Post($"{_ServiceAddress}/{UserName}", OrderModel)
            .Content
            .ReadAsAsync<OrderDto>()
            .Result;

        public OrderDto GetOrderById(int id) => Get<OrderDto>($"{_ServiceAddress}/{id}");

        public IEnumerable<OrderDto> GetUserOrders(string UserName) => Get<List<OrderDto>>($"{_ServiceAddress}/user/{UserName}");
    }
}
