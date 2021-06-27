using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using WebStore.Domain.Models;

namespace WebStore.Services.Product
{
    public class CookiesCartStore : ICartStore
    {
        private string _cartName;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private void ReplaceCookie(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_cartName);
            cookies.Append(_cartName, cookie, new CookieOptions { Expires = DateTime.Now.AddDays(15) });
        }

        public CookiesCartStore(IHttpContextAccessor httpContextAccessor)
        {            
            _httpContextAccessor = httpContextAccessor;

            var user = httpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            _cartName = $"cart[{user_name}]";
        }

        public Cart Cart
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
    }
}
