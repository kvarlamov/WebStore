using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Dto.Order;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase, IOrderService
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("{UserName?}")]
        public OrderDto CreateOrder(CreateOrderModel OrderModel, string UserName) => _orderService.CreateOrder(OrderModel, UserName);

        [HttpGet("{id}"), ActionName("Get")]
        public OrderDto GetOrderById(int id) => _orderService.GetOrderById(id);

        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDto> GetUserOrders(string UserName) => _orderService.GetUserOrders(UserName);
    }
}
