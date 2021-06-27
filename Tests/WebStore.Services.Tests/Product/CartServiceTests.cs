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
using WebStore.Domain.Entities;
using WebStore.Domain.Dto.Products;

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

        [TestMethod]
        public void CartService_AddToCart_WorkCorrect()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>()
            };

            var productDataMock = new Mock<IProductData>();

            var cartStoreMock = new Mock<ICartStore>();
            cartStoreMock
                .Setup(c => c.Cart)
                .Returns(cart);

            var cartService = new CartService(productDataMock.Object, cartStoreMock.Object);

            const int expectedId = 5;

            cartService.AddToCart(expectedId);

            Assert.Equal(1, cart.ItemsCount);
            Assert.Single(cart.Items);
            Assert.Equal(expectedId, cart.Items[0].ProductId);
        }

        public void CartService_RemoveFromCart_RemoveCorrectItem()
        {
            const int item_id = 1;
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = item_id, Quantity = 1 },
                    new CartItem { ProductId = 2, Quantity = 3 }
                }
            };

            var product_data_mock = new Mock<IProductData>();

            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock
               .Setup(c => c.Cart)
               .Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.RemoveFromCart(item_id);

            Assert.Single(cart.Items);
            Assert.Equal(2, cart.Items[0].ProductId);
        }

        [TestMethod]
        public void CartService_RemoveAll_ClearCart()
        {
            const int item_id = 1;
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = item_id, Quantity = 1 },
                    new CartItem { ProductId = 2, Quantity = 3 }
                }
            };

            var product_data_mock = new Mock<IProductData>();

            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock
               .Setup(c => c.Cart)
               .Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.RemoveAll();

            Assert.Empty(cart.Items);
        }

        [TestMethod]
        public void CartService_Decrement_Correct()
        {
            const int item_id = 1;
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = item_id, Quantity = 3 },
                    new CartItem { ProductId = 2, Quantity = 5 }
                }
            };

            var product_data_mock = new Mock<IProductData>();

            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock
               .Setup(c => c.Cart)
               .Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.DecrementFromCart(item_id);

            Assert.Equal(7, cart.ItemsCount);
            Assert.Equal(2, cart.Items.Count);
            Assert.Equal(item_id, cart.Items[0].ProductId);
            Assert.Equal(2, cart.Items[0].Quantity);
        }

        [TestMethod]
        public void CartService_Remove_Item_When_Decrement_to_0()
        {
            const int item_id = 1;
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = item_id, Quantity = 1 },
                    new CartItem { ProductId = 2, Quantity = 5 }
                }
            };

            var product_data_mock = new Mock<IProductData>();

            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock
               .Setup(c => c.Cart)
               .Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            cart_service.DecrementFromCart(item_id);

            Assert.Equal(5, cart.ItemsCount);

            Assert.Single(cart.Items);
        }

        [TestMethod]
        public void CartService_TransformFromCart_WorkCorrect()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 1 },
                    new CartItem { ProductId = 2, Quantity = 5 }
                }
            };

            var products = new List<ProductDto>
            {
                new ProductDto
                {
                    Id = 1,
                    Name = "Product 1",
                    Price = 1.1m,
                    Order = 0,
                    ImageUrl = "Product1.png",
                    Brand = new BrandDto {Id = 1, Name = "Brand 1"}
                },
                new ProductDto
                {
                    Id = 2,
                    Name = "Product 2",
                    Price = 2.1m,
                    Order = 0,
                    ImageUrl = "Product1.png",
                    Brand = new BrandDto {Id = 1, Name = "Brand 1"}
                },
            };

            var product_data_mock = new Mock<IProductData>();
            product_data_mock
               .Setup(c => c.GetProducts(It.IsAny<ProductFilter>()))
               .Returns(products);

            var cart_store_mock = new Mock<ICartStore>();
            cart_store_mock
               .Setup(c => c.Cart)
               .Returns(cart);

            var cart_service = new CartService(product_data_mock.Object, cart_store_mock.Object);

            var result = cart_service.TransformFromCart();

            Assert.Equal(6, result.ItemsCount);
            Assert.Equal(1.1m, result.Items.First().Key.Price);
        }
    }
}
