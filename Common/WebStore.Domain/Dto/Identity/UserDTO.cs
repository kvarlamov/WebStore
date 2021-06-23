using System.Text;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.Dto.Identity
{
    public abstract class UserDTO
    {
        public User User { get; set; }
    }
}
