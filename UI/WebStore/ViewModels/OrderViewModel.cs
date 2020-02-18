using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Имя является обязательным")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указан номер телефона")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Не указан адрес")]
        public string Address { get; set; }
    }
}
