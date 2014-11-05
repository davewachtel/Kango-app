using CL.Services.Contracts;
using CL.Services.Data.Context;
using CL.Services.Data.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Data.Repository
{
    public class UserRepository : IDisposable
    {
        readonly CLIdentityDBContext context = new CLIdentityDBContext();
        public UserRepository()
        {

        }

        public IUserStore<CL.Services.Contracts.IUser> GetUserStore()
        {
            IUserStore<Model.UserModel> store = new UserStore<Model.UserModel>(this.context);

            return (IUserStore<CL.Services.Contracts.IUser>)store;
        }

        public void Dispose()
        {
            if(this.context != null)
            {
                this.context.Dispose();
            }
        }
    }
}
