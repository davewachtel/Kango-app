using CL.Services.Data.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Data.Repository
{
    public class LoginRepository : IDisposable
    {
        readonly CLIdentityDBContext Context = new CLIdentityDBContext();
        readonly CLUserStore UserManager;

        public LoginRepository()
        {
            this.Context = new CLIdentityDBContext();
            this.UserManager = new CLUserStore(this.Context);
        }

        public IUserStore<Contracts.User> GetUserStore()
        {
            return this.UserManager;
        }

        public void Dispose()
        {
            this.UserManager.Dispose();
            this.Context.Dispose();
        }
    }
}
