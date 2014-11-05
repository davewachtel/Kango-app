using CL.Services.Data.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Data.Context
{
    internal class CLIdentityDBContext : IdentityDbContext<UserModel>
    {
        public CLIdentityDBContext()
            : base("CLEntities", throwIfV1Schema: false)
        {
        }

        public static CLIdentityDBContext Create()
        {
            return new CLIdentityDBContext();
        }
    }
}