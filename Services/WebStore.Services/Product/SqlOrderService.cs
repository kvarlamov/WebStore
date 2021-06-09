using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;
using WebStore.Domain.Dto.Order;
using WebStore.Services.Map;

namespace WebStore.Services.Product
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext _db;
        private readonly UserManager<User> _userManager;

        public SqlOrderService(WebStoreContext db, UserManager<User> UserManager)
        {
            _db = db;
            _userManager = UserManager;
        }
        
        public IEnumerable<OrderDto> GetUserOrders(string UserName)
        {
            return _db.Orders
                    .Include(order => order.User)
                    .Include(order => order.OrderItems)
                    .Where(order => order.User.UserName == UserName)
                    .Select(o => o.ToDto());
        }

        public OrderDto GetOrderById(int id)
        {
            var o = _db.Orders
                .Include(order => order.OrderItems)
                .FirstOrDefault(order => order.Id == id);
            return o.ToDto();
        }

        public OrderDto CreateOrder(CreateOrderModel OrderModel, string UserName)
        {
            var user = _userManager.FindByNameAsync(UserName).Result;

            using (var transaction = _db.Database.BeginTransaction())
            {
                var order = new Order
                {
                    Name = OrderModel.OrderViewModel.Name,
                    Address = OrderModel.OrderViewModel.Address,
                    Phone = OrderModel.OrderViewModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };

                _db.Orders.Add(order);

                foreach (var item in OrderModel.OrderItems)
                {
                    var product = _db.Products.FirstOrDefault(p => p.Id == item.Id);
                    if (product is null)
                    {
                        throw new InvalidOperationException($"Товар с идентификатором id:{item.Id} отсутствует в БД");
                    }

                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = item.Quantity,
                        Product = product
                    };

                    _db.OrderItems.Add(order_item);
                }

                _db.SaveChanges();
                transaction.Commit();

                return order.ToDto();
            }
        }
    }
}
