using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Data.Context
{
    internal class CLIdentityDBContext : IdentityDbContext<IdentityUser>
    {
        public CLIdentityDBContext()
            : base("CLEntitiesIdentity")
        {
        }

        public static CLIdentityDBContext Create()
        {
            return new CLIdentityDBContext();
        }
    }
}