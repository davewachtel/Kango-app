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

    class CLUserStore : IUserStore<Contracts.User>, IUserPasswordStore<Contracts.User>, IUserSecurityStampStore<Contracts.User>, IUserEmailStore<Contracts.User>
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
            context.Users.Attach(identity);
            context.Entry(user).State = EntityState.Modified;
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

        public Task SetEmailConfirmedAsync(Contracts.User user, bool confirmed)
        {
            var identityUser = ToIdentityUser(user);
            var task = userStore.SetEmailConfirmedAsync(identityUser, confirmed);
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
                EmailConfirmed = user.EmailConfirmed
            };
        }

        public void Dispose()
        {
            this.userStore.Dispose();
        }
    }
}
