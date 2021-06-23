using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using WebStore.DAL.Context;

namespace WebStore.ServiceHosting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleStore<IdentityRole> _RoleStore;
        public RolesController(WebStoreContext db)
        {
            _RoleStore = new RoleStore<IdentityRole>(db) { AutoSaveChanges = true };
        }
    }
}
