using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Assert = Xunit.Assert;
using Moq;
using WebStore.Controllers;
using WebStore.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;
using System.Security.Claims;
using WebStore.Domain.Dto.Order;
using Microsoft.AspNetCore.Http;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CartControllerTests
    {
        [TestMethod]
        public void CheckOut_ModelState_Invalid_Returns_ViewModel()
        {
            var cartServiceMock = new Mock<ICartService>();
            var orderServiceMock = new Mock<IOrderService>();

            var controller = new CartController(cartServiceMock.Object, orderServiceMock.Object);

            controller.ModelState.AddModelError("error", "InvalidModel");

            const string expectedModelName = "Test order";

            var result = controller.CheckOut(new Domain.ViewModels.OrderViewModel { Name = expectedModelName });

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<DetailsViewModel>(viewResult.ViewData.Model);

            Assert.Equal(expectedModelName, model.OrderViewModel.Name);
        }

        [TestMethod]
        public void Checkout_CallsServiceAndReturnsRedirect()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));

            var cartServiceMock = new Mock<ICartService>();
            cartServiceMock
                .Setup(c => c.TransformFromCart())
                .Returns(() => new CartViewModel
                {
                    Items = new Dictionary<ProductViewModel, int>
                    {
                        {new ProductViewModel(), 1 }
                    }
                });

            const int expectedOrderId = 1;

            var orderServiceMock = new Mock<IOrderService>();
            orderServiceMock
                .Setup(c => c.CreateOrder(It.IsAny<CreateOrderModel>(), It.IsAny<string>()))
                .Returns(new OrderDto { Id = 1 });

            var controller = new CartController(cartServiceMock.Object, orderServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = user
                    }
                }
            };

            var result = controller.CheckOut(new OrderViewModel
            {
                Name = "Test",
                Address = "Test address",
                Phone = "Test phone"
            });

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirectResult.ActionName);

            Assert.Equal(expectedOrderId, redirectResult.RouteValues["id"]);
        }
    }
}
