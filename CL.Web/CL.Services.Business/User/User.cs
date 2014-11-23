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
        Contracts.IUser _User { get; set; }

        public User(Contracts.IUser user)
        {
            this._User = user;
        }

        public static IUserStore<Contracts.User> GetStore()
        {
            LoginRepository repo = new LoginRepository();
            return repo.GetUserStore();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager manager, string authenticationType)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var user = ToUser(this._User);
            var userIdentity = await manager.CreateIdentityAsync(user, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        private Contracts.User ToUser(Contracts.IUser user)
        {
            return new Contracts.User()
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp
            };
        }

        public string PasswordHash
        {
            get { return this._User.PasswordHash; }
            set { this._User.PasswordHash = value; }
        }

        public string SecurityStamp
        {
            get { return this._User.SecurityStamp; }
            set { this._User.SecurityStamp = value; }
        }

        public string Email
        {
            get { return this._User.Email; }
            set { this._User.Email = value; }
        }

        public bool EmailConfirmed
        {
            get { return this._User.EmailConfirmed; }
            set { this._User.EmailConfirmed = value; }
        }

        public IList<Contracts.IRole> Roles
        {
            get { return this._User.Roles; }
            set { this._User.Roles = value; }
        }

        public string Id
        {
            get { return this._User.Id; }
        }

        public string UserName
        {
            get { return this._User.UserName; }
            set { this._User.UserName = value; }
        }
    }
}
