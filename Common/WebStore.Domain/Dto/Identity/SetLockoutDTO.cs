using System;

namespace WebStore.Domain.Dto.Identity
{
    public class SetLockoutDTO
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
