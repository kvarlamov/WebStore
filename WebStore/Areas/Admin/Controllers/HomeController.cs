﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles =Role.Administrator)]
    public class HomeController : Controller
    {
        private readonly IProductData _ProductData;

        public HomeController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductList() => View(_ProductData.GetProducts());

        public IActionResult Edit(int? id) => View();

        public IActionResult Delete(int? id) => View();

        [HttpPost, ActionName(nameof(Delete))]
        public IActionResult DeleteConfirm(int? id) => RedirectToAction("ProductList");
    }
}