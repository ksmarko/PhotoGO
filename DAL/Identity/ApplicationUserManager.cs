using DAL.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
                : base(store)
        {
            //Данный класс будет управлять пользователями: добавлять их в базу данных и аутентифицировать.
        }
    }
}
