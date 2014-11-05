using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public class User : IUser
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}
