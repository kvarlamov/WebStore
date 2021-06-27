using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Models;

namespace WebStore.Services.Product
{
    public interface ICartStore
    {
        public Cart Cart { get; set; }
    }
}
