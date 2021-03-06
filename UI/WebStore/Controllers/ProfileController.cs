﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class ProfileController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Orders([FromServices] IOrderService orderService)
        {
            return View(orderService
                .GetUserOrders(User.Identity.Name)
                .Select(order => new UserOrderViewModel
                {
                    Id = order.Id,
                    Name = order.Name,
                    Address = order.Adress,
                    Phone = order.Phone,
                    TotalSum = order.OrderItems.Sum(item => item.Price * item.Quantity)
                }));
        }
    }
}