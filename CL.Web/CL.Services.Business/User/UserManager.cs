using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace CL.Services.Business.User
{
    public class UserManager : UserManager<Contracts.User>
    {
        public UserManager(IUserStore<Contracts.User> store)
            : base(store)
        {
        }

        public static UserManager Create()
        {
            IUserStore<Contracts.User> store = Business.User.User.GetStore();
            var manager = new UserManager(store);
            
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<Contracts.User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            return manager;
        }
    }
}
