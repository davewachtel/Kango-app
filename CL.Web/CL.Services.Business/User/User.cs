using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Services.Data.Repository;
using System.Security.Claims;

namespace CL.Services.Business.User
{
    public class User : Contracts.IUser
    {
        private Contracts.IUser _User { get; set; }
        public User(Contracts.IUser user)
        {
            this._User = user;
        }

        #region IUser Implementation
        public string Id
        {
            get { return this._User.Id; }
        }

        public string UserName
        {
            get { return this._User.UserName; }
            set { this._User.UserName = value; }
        }
        #endregion

        public static IUserStore<CL.Services.Contracts.IUser> GetStore()
        {
            UserRepository repo = new UserRepository();
            return repo.GetUserStore();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
