﻿using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Identity;
using DAL.Identity.Entities;
using DAL.Identity.Interfaces;
using DAL.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserManager : IUserManager
    {
        IUnitOfWorkIdentity Database { get; set; }
        IUnitOfWork Data { get; set; }

        public UserManager(IUnitOfWorkIdentity uowi, IUnitOfWork uow)
        {
            Database = uowi;
            Data = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            var user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.Email };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);

                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                User clientProfile = new User { Id = user.Id, Name = userDto.Name };
                Database.ClientManager.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);

            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        public UserDTO GetUserById(string id)
        {
            return GetUsers().Where(x => x.Id == id).Single();
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var appUsers = Database.UserManager.Users;
            var appRoles = Database.RoleManager.Roles;
            var list = new List<UserDTO>();

            foreach (var el in appUsers)
            {
                list.Add(new UserDTO()
                {
                    Id = el.Id,
                    Email = el.Email,
                    UserName = el.UserName,
                    Name = el.User.Name,
                    Role = SelectRoleNameById(el.Roles.Where(x => x.UserId == el.Id).Single().RoleId)
                });
            }

            return list;
        }

        private string SelectRoleNameById(string id)
        {
            var appRoles = Database.RoleManager.Roles;
            return appRoles.Where(r => r.Id == id).Single().Name;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
