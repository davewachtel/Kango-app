using CL.Services.Data.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using CL.Services.Contracts;

namespace CL.Services.Data
{
    internal class CLUserStore : IUserStore<Contracts.User>, IUserPasswordStore<Contracts.User>, IUserSecurityStampStore<Contracts.User>, IUserEmailStore<Contracts.User>, IUserRoleStore<Contracts.User>, IUserClaimStore<Contracts.User>, IUserPhoneNumberStore<Contracts.User>
    {
        readonly CLIdentityDBContext context;
        readonly UserStore<IdentityUser> userStore;

        public CLUserStore(CLIdentityDBContext context)
        {
            this.context = context;
            this.userStore = new UserStore<IdentityUser>(context);
        }
        public Task CreateAsync(Contracts.User user)
        {
            var identity = ToIdentityUser(user);
            context.Users.Add(identity);
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }
        public Task DeleteAsync(Contracts.User user)
        {
            var identity = ToIdentityUser(user);
            context.Users.Remove(identity);
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }
        public async Task<Contracts.User> FindByIdAsync(string userId)
        {
            IdentityUser result = await context.Users.Where(u => u.Id.ToLower() == userId.ToLower()).FirstOrDefaultAsync();
            Contracts.User user = ToUser(result);

            return user;
        }
        public async Task<Contracts.User> FindByNameAsync(string userName)
        {
            IdentityUser result = await context.Users.Where(u => u.UserName.ToLower() == userName.ToLower()).FirstOrDefaultAsync();
            Contracts.User user = ToUser(result);

            return user;
        }

        public Task UpdateAsync(Contracts.User user)
        {
            var identity = ToIdentityUser(user);
            var entry = context.Entry(identity);
            bool isDetached = context.Entry(identity).State == EntityState.Detached;
            if (isDetached)
            {
                context.Users.Attach(identity);
                context.Entry(user).State = EntityState.Modified;
            }
            
            context.Configuration.ValidateOnSaveEnabled = false;
            return context.SaveChangesAsync();
        }

        public Task<string> GetPasswordHashAsync(Contracts.User user)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.GetPasswordHashAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;
        }
        public Task<bool> HasPasswordAsync(Contracts.User user)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.HasPasswordAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;
        }
        public Task SetPasswordHashAsync(Contracts.User user, string passwordHash)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetPasswordHashAsync(identityUser, passwordHash);
            SetApplicationUser(user, identityUser);
            return task;
        }
        public Task<string> GetSecurityStampAsync(Contracts.User user)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.GetSecurityStampAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;
        }
        public Task SetSecurityStampAsync(Contracts.User user, string stamp)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetSecurityStampAsync(identityUser, stamp);
            SetApplicationUser(user, identityUser);
            return task;
        }


        public async Task<Contracts.User> FindByEmailAsync(string email)
        {
            IdentityUser result = await context.Users.Where(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
            Contracts.User user = ToUser(result);

            return user;
        }

        public Task<string> GetEmailAsync(Contracts.User user)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.GetEmailAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;
        }

        public Task<bool> GetEmailConfirmedAsync(Contracts.User user)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.GetEmailConfirmedAsync(identityUser);
            SetApplicationUser(user, identityUser);
            return task;
        }

        public Task SetEmailAsync(Contracts.User user, string email)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetEmailAsync(identityUser, email);
            SetApplicationUser(user, identityUser);
            return task;
        }
   
        public Task SetPhoneNumberAsync(Contracts.User user, string PhoneNumber)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetPhoneNumberAsync(identityUser, PhoneNumber);
            SetApplicationUser(user, identityUser);
            return task;
        }


        public Task SetEmailConfirmedAsync(Contracts.User user, bool confirmed)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetEmailConfirmedAsync(identityUser, confirmed);
            SetApplicationUser(user, identityUser);
            return task;
        }


        public Task AddToRoleAsync(Contracts.User user, string roleName)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.AddToRoleAsync(identityUser, roleName);
            SetApplicationUser(user, identityUser);
            return task;
        }

        public Task<IList<string>> GetRolesAsync(Contracts.User user)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.GetRolesAsync(identityUser);
            return task;
        }

        public Task<bool> IsInRoleAsync(Contracts.User user, string roleName)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.IsInRoleAsync(identityUser, roleName);
            return task;
        }

        public Task RemoveFromRoleAsync(Contracts.User user, string roleName)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.AddToRoleAsync(identityUser, roleName);
            SetApplicationUser(user, identityUser);
            return task;
        }

        public Task AddClaimAsync(Contracts.User user, System.Security.Claims.Claim claim)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.AddClaimAsync(identityUser, claim);
            return task;
        }

        public Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(Contracts.User user)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.GetClaimsAsync(identityUser);
            return task;
        }

        public Task RemoveClaimAsync(Contracts.User user, System.Security.Claims.Claim claim)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.RemoveClaimAsync(identityUser, claim);
            SetApplicationUser(user, identityUser);
            return task;
        }

        private Contracts.User ToUser(IdentityUser identity)
        {
            if (identity == null)
                return null;

            Contracts.User user = new Contracts.User();
            SetApplicationUser(user, identity);

            return user;
        }

        private static void SetApplicationUser(Contracts.User user, IdentityUser identityUser)
        {
            if (identityUser != null)
            {
                user.Id = identityUser.Id;
                user.PasswordHash = identityUser.PasswordHash;
                user.UserName = identityUser.UserName;
                user.SecurityStamp = identityUser.SecurityStamp;
                user.Email = identityUser.Email;
                user.EmailConfirmed = identityUser.EmailConfirmed;
                user.PhoneNumber = identityUser.PhoneNumber;

            }
            else
                user = null;
        }

        private IdentityUser ToIdentityUser(Contracts.IUser user)
        {
            return new IdentityUser
            {
                Id = user.Id,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber
            };
        }

        public void Dispose()
        {
            this.userStore.Dispose();
        }

        public Task<string> GetPhoneNumberAsync(Contracts.User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(Contracts.User user)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(Contracts.User user, bool confirmed)
        {
            //throw new NotImplementedException();
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetPhoneNumberConfirmedAsync(identityUser, confirmed);
            SetApplicationUser(user, identityUser);
            return task;
        }
    }
}
