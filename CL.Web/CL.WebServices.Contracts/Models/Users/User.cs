using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public class User : Contracts.IUser
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        public User(string userName)
            : this()
        {
            this.UserName = userName;
            this.Roles = new List<IRole>();
        }

        public virtual String Id { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string UserName { get; set; }

        public virtual IList<IRole> Roles { get; set; }

        public virtual string Email { get; set; }

        public virtual bool EmailConfirmed { get; set; }

        public static User ToUser(Contracts.IUser user)
        {
            var newUser =  new User()
            {
                Id = user.Id,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed
            };

            if(user.Roles != null)
            {
                foreach(IRole role in user.Roles)
                {
                    newUser.Roles.Add(role);
                }
            }

            return newUser;
        }
    }
}
