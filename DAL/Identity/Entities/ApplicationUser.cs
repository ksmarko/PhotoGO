using DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DAL.Identity.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual User User { get; set; }
    }
}
