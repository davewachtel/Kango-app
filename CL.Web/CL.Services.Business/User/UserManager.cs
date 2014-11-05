using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CL.Services.Business.User
{
    public class UserManager : UserManager<Contracts.IUser>
    {
        public UserManager(IUserStore<Contracts.IUser> store)
            : base(store)
        {
        }

        public static UserManager Create()
        {
            IUserStore<CL.Services.Contracts.IUser> store = Business.User.User.GetStore();
            var manager = new UserManager(store);
            
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Contracts.IUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            return manager;
        }
    }
}
