using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public interface IUser : IUser<String>
    {
        string PasswordHash { get; set; }

        string SecurityStamp { get; set; }

        string Email { get; set; }
        bool EmailConfirmed { get; set; }

        IList<IRole> Roles { get; set; }
        string PhoneNumber { get; set; } 
        string notify_me { get; set; }
        string device_id { get; set; }

    }
}
