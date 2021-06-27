using System;
using System.Collections.Generic;
using System.Text;
using Assert = Xunit.Assert;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebStore.Domain.Models;
using System.Linq;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Product;

namespace WebStore.Services.Tests.Product
{
    [TestClass]
    public class CartServiceTests
    {
        [TestMethod]
        public void Cart_Class_ItemsCount_ReturnsCorrectQuality()
        {
            const int expectedCount = 4;

            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem{ProductId = 1, Quantity = 1},
                    new CartItem{ProductId = 2, Quantity = 3}
                }
            };

            var result = cart.ItemsCount;

            Assert.Equal(expectedCount, result);
        }

        public void CartService_AddToCart_WorkCorrect()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>()
            };

            var productDataMock = new Mock<IProductData>();

            var cartService = new CookieCartService(productDataMock.Object);
        }
        
    }
}
