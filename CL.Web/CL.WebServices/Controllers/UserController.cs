using CL.Services.Business.User;
using CL.Services.Contracts.Responses;
using CL.Services.Web.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CL.Services.Web.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {

        [Route("History")]
        [HttpDelete]
        public HttpResponseMessage History_Delete()
        {
            if (User.Identity != null)
            {
                var userManager = UserManager.Create();
                using (var _repo = new AuthenticationRepository(userManager))
                {
                    //Business.User.User user = new User();

                }
            }

            return new HttpResponseMessage(HttpStatusCode.NoContent); //204
        }
    }
}
