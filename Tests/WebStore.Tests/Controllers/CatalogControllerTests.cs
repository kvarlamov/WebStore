using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Controllers;
using WebStore.Domain.Dto.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void Details_Returns_With_CorrectItem()
        {
            #region Arrange

            const int expectedId = 1;
            const decimal expectedPrice = 10m;
            var expectedName = $"Item id {expectedId}";
            var expectedBrandName = $"Brand of item {expectedId}";

            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns<int>(id => new Domain.Dto.Products.ProductDto
                {
                    Id = id,
                    Name = $"Item id {id}",
                    ImageUrl = $"Image_id_{id}.png",
                    Order = 0,
                    Price = expectedPrice,
                    Brand = new Domain.Dto.Products.BrandDto
                    {
                        Id = 1,
                        Name = $"Brand of item {id}"
                    }
                });

            var controller = new CatalogController(productDataMock.Object);

            #endregion

            #region Act

            var result = controller.Details(expectedId);

            #endregion

            #region Assert

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(viewResult.Model);

            Assert.Equal(expectedId, model.Id);
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedPrice, model.Price);
            Assert.Equal(expectedBrandName, model.Brand);

            #endregion
        }

        [TestMethod]
        public void Details_Returns_NotFoundIfProductNotExist()
        {
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProductById(It.IsAny<int>()))
                .Returns(default(ProductDto));

            var controller = new CatalogController(productDataMock.Object);

            var result = controller.Details(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void Shop_Return_CorrectView()
        {
            var productDataMock = new Mock<IProductData>();
            productDataMock
                .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
                .Returns<ProductFilter>(filter => new []
                {
                    new ProductDto
                    {
                        Id = 1,
                        Name = "Product 1",
                        Order = 0,
                        Price = 10m,
                        ImageUrl = "Product1.png",
                        Brand = new BrandDto
                        {
                            Id = 1,
                            Name = "Brand of product 1"
                        }
                    },
                    new ProductDto
                    {
                        Id = 2,
                        Name = "Product 2",
                        Order = 0,
                        Price = 20m,
                        ImageUrl = "Product2.png",
                        Brand = new BrandDto
                        {
                            Id = 1,
                            Name = "Brand of product 2"
                        }
                    }
                });

            var controller = new CatalogController(productDataMock.Object);

            const int expectedSectionId = 1;
            const int expectedBrandId = 5;

            var result = controller.Shop(expectedSectionId, expectedBrandId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CatalogViewModel>(viewResult.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
            Assert.Equal(expectedBrandId, model.BrandId);
            Assert.Equal(expectedSectionId, model.SectionId);

            Assert.Equal("Brand of product 1", model.Products.First().Brand);
        }
    }
}
