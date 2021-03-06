﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.Extensions.Configuration;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;
        private readonly string _PageSize = "PageSize";

        public CatalogController(IProductData productData, IConfiguration configuration)
        {
            _ProductData = productData;
            _Configuration = configuration;
        }

        public IActionResult Shop(int? sectionId, int? brandId, int page = 1)
        {
            var pageSize = int.Parse(_Configuration[_PageSize]);
            
            var products = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = sectionId,
                BrandId = brandId,
                Page = page,
                PageSize = pageSize
            });

            return View(new CatalogViewModel
            {
                SectionId = sectionId,
                BrandId = brandId,
                Products = products.Products
                .Select(p => p.FromDto())
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Order = p.Order,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Brand = p.Brand?.Name                   
                }).OrderBy(p => p.Order),
                PageViewModel = new PageViewModel
                {
                    PageSize = pageSize,
                    PageNumber = page,
                    TotalItems = products.TotalCount
                }
            });
        }

        public IActionResult Details(int id)
        {
            var product = _ProductData.GetProductById(id);

            if (product is null)
            {
                return NotFound();
            }

            return View(new ProductViewModel 
            { 
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                Brand = product.Brand?.Name
            });
        }

        public IActionResult GetFilteredItems(int? sectionId, int? brandId, int page = 1)
        {
            var products = GetProducts(sectionId, brandId, page);
            return PartialView("Partial/_FeaturesItem", products);
        }

        private IEnumerable<ProductViewModel> GetProducts(int? sectionId, int? brandId, int page)
        {
            var productModel = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = sectionId,
                BrandId = brandId,
                Page = page,
                PageSize = int.Parse(_Configuration[_PageSize])
            });

            return productModel.Products.Select(p => new ProductViewModel
            {
                Id= p.Id,
                Name = p.Name,
                Price = p.Price,
                Order = p.Order,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand?.Name ?? string.Empty
            });
        }
    }
}