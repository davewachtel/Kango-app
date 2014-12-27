using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CL.Services.Web.Controllers
{
    public class CLApiController : ApiController
    {
        private IEnumerable<Claim> getClaims()
        {
            if (User.Identity != null)
            {
                var claim = (User.Identity as System.Security.Claims.ClaimsIdentity);
                if (claim != null)
                {
                    return claim.Claims;
                }
            }

            throw new ApplicationException("The system cannot determine the logged in user.");
        }

        protected String getUserId()
        {
            var claims = this.getClaims();
            return claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }

        protected String getUsername()
        {
            var claims = this.getClaims();
            return claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
        }

    }
}