using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Dto.Products;
using WebStore.Domain.ViewModels.BreadCrumbs;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BreadCrumbsViewComponent(IProductData productData)
        {
            _ProductData = productData;
        }
        
        public IViewComponentResult Invoke(BreadCrumbType Type, int id, BreadCrumbType FromType)
        {
            switch (Type)
            {
                default: return View(Array.Empty<BreadCrumbViewModel>());
                
                case BreadCrumbType.Section:
                    return View(
                        new[]
                        {
                            new BreadCrumbViewModel
                            {
                                BreadCrumbType = Type,
                                Id = id.ToString(),
                                Name = _ProductData.GetSectionById(id).Name
                            },
                        });
                    break;
                case BreadCrumbType.Brand:
                    return View(
                        new[]
                        {
                            new BreadCrumbViewModel
                            {
                                BreadCrumbType = Type,
                                Id = id.ToString(),
                                Name = _ProductData.GetBrandById(id).Name
                            },
                        });
                    break;
                case BreadCrumbType.Product:
                    return View(GetProductBreadCrumbs(_ProductData.GetProductById(id), FromType));
                    break;
            }
            
            return View();
        }

        private static IEnumerable<BreadCrumbViewModel> GetProductBreadCrumbs(ProductDto product, BreadCrumbType fromType)
        {
            return new[]
            {
                new BreadCrumbViewModel
                {
                    BreadCrumbType = fromType,
                    Id = fromType == BreadCrumbType.Section ? product.Section.Id.ToString() : product.Brand.Id.ToString(),
                    Name = fromType == BreadCrumbType.Section ? product.Section.Name : product.Brand.Name
                },
                new BreadCrumbViewModel
                {
                    BreadCrumbType = BreadCrumbType.Product,
                    Id = product.Id.ToString(),
                    Name = product.Name
                }
            };
        }
    }
}