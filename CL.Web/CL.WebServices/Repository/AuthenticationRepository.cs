using CL.Services.Web.Models.User;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CL.Services.Web.Repository
{
    public class AuthenticationRepository : IDisposable
    {
        private UserManager<Contracts.User> UserManager { get; set; }

        public AuthenticationRepository(UserManager<Contracts.User> manager)
        {
            this.UserManager = manager;
        }

        public async Task<IdentityResult> RegisterUser(Contracts.User userModel, String password, String PhoneNumber)
        {
            var result = await this.UserManager.CreateAsync(userModel, password);

            return result;
        }
        public async Task<IdentityResult> UpdatePhone(String id, String PhoneNumber)
        {
            var result = await this.UserManager.SetPhoneNumberAsync(id, PhoneNumber);

            return result;
        }

        public async Task<Contracts.IUser> FindUser(string userName, string password)
        {
            Contracts.IUser user = await this.UserManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            this.UserManager.Dispose();
        }
    }
}