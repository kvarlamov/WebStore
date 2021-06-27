﻿using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain.Dto.Order;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;
        private readonly IOrderService _OrderService;

        public CartController(ICartService CartService, IOrderService OrderService)
        {
            _CartService = CartService;
            _OrderService = OrderService;
        }

        public IActionResult Details() => View(new DetailsViewModel
        {
            CartViewModel = _CartService.TransformFromCart(),
            OrderViewModel = new OrderViewModel()
        });

        public IActionResult AddToCart(int id)
        {
            _CartService.AddToCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult DecrementFromCart(int id)
        {
            _CartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveFromCart(int id)
        {
            _CartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveAll()
        {
            _CartService.RemoveAll();
            return RedirectToAction("Details");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Details), new DetailsViewModel
                {
                    CartViewModel = _CartService.TransformFromCart(),
                    OrderViewModel = model
                });
            }

            var createOrderModel = new CreateOrderModel
            {
                OrderViewModel = model,
                OrderItems = _CartService.TransformFromCart().Items
                .Select(i => new OrderItemDto
                {
                    Id = i.Key.Id,
                    Price = i.Key.Price,
                    Quantity = i.Value
                })
                .ToList()
            };

            var order = _OrderService.CreateOrder(createOrderModel, User.Identity.Name);

            _CartService.RemoveAll();

            return RedirectToAction("OrderConfirmed", new { id = order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}