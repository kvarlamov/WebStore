using System.ComponentModel.DataAnnotations;

namespace WebStore.Domain.ViewModels
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
