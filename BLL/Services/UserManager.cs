using BLL.DTO;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserManager : IUserManager
    {
        IUnitOfWork Database { get; set; }

        public UserManager(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void AddUser(UserDTO item)
        {
            if (item == null)
                throw new ArgumentNullException();

            var user = new UserProfile()
            {
                Name = item.Name
            };

            Database.Users.Create(user);
            Database.Save();
        }
    }
}
