using CL.Services.Business.User;
using CL.Services.Web.Models.User;
using CL.Services.Web.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CL.Services.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        private AuthenticationRepository _repo = null;
        public LoginController()
        {
            var userManager = UserManager.Create();
            _repo = new AuthenticationRepository(userManager);
        }

        // POST api/Login/Register
        [Route("Register")]
        public async Task<IHttpActionResult> Register([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var user = new Contracts.User()
            {
                UserName = userModel.UserName,
                Email = userModel.UserName
            };

            IdentityResult result = await _repo.RegisterUser(user, userModel.Password);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}