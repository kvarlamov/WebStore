using System.Collections.Generic;
using System.Security.Claims;

namespace WebStore.Domain.Dto.Identity
{
    public abstract class ClaimDTO : UserDTO
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}
