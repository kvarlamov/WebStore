using System;
using System.Collections.Generic;
using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Dto.Order
{
    public class OrderDto : NamedEntity
    {
        public string Phone { get; set; }
        public string Adress { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<OrderItemDto> OrderItems { get; set; }

    }
}
