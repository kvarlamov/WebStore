﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Infrastructure.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class ProfileController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Orders([FromServices] IOrderService OrderService)
        {
            return View(OrderService
                .GetUserOrders(User.Identity.Name)
                .Select(order => new UserOrderViewModel
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Address,
                    Phone = order.Phone,
                    TotalSum = order.OrderItems.Sum(item => item.Price * item.Quantity)
                }));
        }
    }
}