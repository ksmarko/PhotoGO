using DAL.EF;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Identity
{
    public class UnitOfWorkIdentity : IUnitOfWorkIdentity
    {
        private DataContext db;

        private ApplicationUserManager appUserManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;

        public UnitOfWorkIdentity(string connectionString)
        {
            db = new DataContext(connectionString);
            appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
        }
        
        public ApplicationUserManager AppUserManager
        {
            get { return appUserManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return roleManager; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
