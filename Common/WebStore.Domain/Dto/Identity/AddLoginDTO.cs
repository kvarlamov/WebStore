using Microsoft.AspNetCore.Identity;

namespace WebStore.Domain.Dto.Identity
{
    public class AddLoginDTO : UserDTO
    {
        public UserLoginInfo UserLoginInfo { get; set; }
    }
}
