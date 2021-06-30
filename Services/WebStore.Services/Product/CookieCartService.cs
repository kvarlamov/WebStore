using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;

namespace WebStore.Services.Product
{
    public class CookieCartService : ICartService
    {
        private string _cartName;
        private readonly IProductData _productData;
        private readonly IHttpContextAccessor _httpContextAccessor;


        private void ReplaceCookie(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cookie, new CookieOptions { Expires = DateTime.Now.AddDays(15) });
        }

        public CookieCartService(IProductData productData, IHttpContextAccessor httpContextAccessor)
        {
            _productData = productData;
            _httpContextAccessor = httpContextAccessor;

            var user = httpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            _cartName = $"cart[{user_name}]";
        }

        private Cart Cart
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cart_cookie = context.Request.Cookies[_cartName];
                if (cart_cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(_cartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookie(cookies, cart_cookie);
                return JsonConvert.DeserializeObject<Cart>(cart_cookie);
            }
            set
            {
                ReplaceCookie(_httpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
            }
        }


        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
            {
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            }
            else
            {
                item.Quantity++;
            }

            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
            {
                return;
            }
            if(item.Quantity > 0)
            {
                item.Quantity--;
            }
            if (item.Quantity == 0)
            {
                cart.Items.Remove(item);
            }

            Cart = cart;
        }
        
        public void RemoveFromCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
            {
                return;
            }

            cart.Items.Remove(item);
            Cart = cart;
        }

        public void RemoveAll()
        {
            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public CartViewModel TransformFromCart()
        {
            var products = _productData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToList()
            });

            var product_view_models = products.Products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Order = p.Order,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand?.Name
            });

            return new CartViewModel
            {
                Items = Cart.Items.ToDictionary(
                    x => product_view_models.First(p => p.Id == x.ProductId),
                    x => x.Quantity)
            };
        }
    }
}
