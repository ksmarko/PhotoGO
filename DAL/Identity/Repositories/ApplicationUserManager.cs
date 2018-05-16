using DAL.Identity.Entities;
using Microsoft.AspNet.Identity;

namespace DAL.Identity.Repositories
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }
    }
}
