﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;

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
        
        public IEnumerable<Order> GetUserOrders(string UserName)
        {
            return _db.Orders
                    .Include(order => order.User)
                    .Include(order => order.OrderItems)
                    .Where(order => order.User.UserName == UserName)
                    .ToArray();
        }

        public Order GetOrderById(int id)
        {
            return _db.Orders
                .Include(order => order.OrderItems)
                .FirstOrDefault(order => order.Id == id);
        }

        public Order CreateOrder(OrderViewModel OrderModel, CartViewModel CartModel, string UserName)
        {
            var user = _userManager.FindByNameAsync(UserName).Result;

            using (var transaction = _db.Database.BeginTransaction())
            {
                var order = new Order
                {
                    Name = OrderModel.Name,
                    Address = OrderModel.Address,
                    Phone = OrderModel.Phone,
                    User = user,
                    Date = DateTime.Now
                };

                _db.Orders.Add(order);

                foreach (var item in CartModel.Items)
                {
                    var product_model = item.Key;
                    var quantity = item.Value;

                    var product = _db.Products.FirstOrDefault(p => p.Id == product_model.Id);
                    if (product is null)
                    {
                        throw new InvalidOperationException($"Товар с идентификатором id:{product_model.Id} отсутствует в БД");
                    }

                    var order_item = new OrderItem
                    {
                        Order = order,
                        Price = product.Price,
                        Quantity = quantity,
                        Product = product
                    };

                    _db.OrderItems.Add(order_item);
                }

                _db.SaveChanges();
                transaction.Commit();

                return order;
            }
        }
    }
}
