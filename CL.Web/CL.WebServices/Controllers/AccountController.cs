using CL.Services.Business.User;
using CL.Services.Web.Models.User;
using CL.Services.Web.Repository;
using CL.Services.Data.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CL.Services.Contracts.Responses;
using CL.Services.Web.TypeConverter;
using CL.Services.Business;

namespace CL.Services.Web.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Account")]
    public class AccountController : CLApiController
    {
        private AuthenticationRepository _repo = null;
        UserRepository repo = new UserRepository();


        public AccountController()
        {
            var userManager = UserManager.Create();
            _repo = new AuthenticationRepository(userManager);
            
        }

     

        [HttpPost]
        [Route("checkemail")]
        public async Task<IHttpActionResult> Checkemail([FromBody] sysEmail userModel)
        {
            if (!ModelState.IsValid || userModel == null)
            {
                
                App_Start.PrettyHttpError error = new App_Start.PrettyHttpError(ModelState);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, error));
            }

            var userManager = UserManager.Create();
            Contracts.User user = userManager.FindByEmail(userModel.Email);
            if (user == null)
            {
                object response1 = new { message = "This email does not exist in Db" };

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response1, Configuration.Formatters.JsonFormatter));
                
            }
            
                object response2 = new { message = "This email already taken" };

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, response2, Configuration.Formatters.JsonFormatter));
            
            
        }

        [HttpPost]
        [Route("profile")]
        public async Task<IHttpActionResult> Profile([FromBody] profie userModel)
        {
            if (!ModelState.IsValid || userModel == null)
            {
     
                App_Start.PrettyHttpError error = new App_Start.PrettyHttpError(ModelState);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, error));
            }

            var userManager = UserManager.Create();
            Contracts.User user = userManager.FindById(userModel.Id);

            if (user == null)
            {
                object response1 = new { message = "This Userid does not exist in Db" };

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, response1, Configuration.Formatters.JsonFormatter));
                //return NotFound();
            }
            else
            {
                object rUser = new { Email = user.Email, Id = user.Id, PhoneNumber = user.PhoneNumber, Notification =  user.notify_me};
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, rUser, Configuration.Formatters.JsonFormatter));
            }
        }

        [HttpPost]
        [Route("updateprofile")]
        public async Task<IHttpActionResult> Update([FromBody] updateProfile userModel)
        {
            if (!ModelState.IsValid || userModel == null)
            {
               
                App_Start.PrettyHttpError error = new App_Start.PrettyHttpError(ModelState);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, error));
            }
            var userManager = UserManager.Create();
            Contracts.User user = userManager.FindById(userModel.Id);
            if (user == null)
            {
                object response1 = new { message = "This Userid does not exist in Db" };

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, response1, Configuration.Formatters.JsonFormatter));

            }
            else
            {

                bool result = repo.UpdateProfile(userModel.Id, userModel.PhoneNumber, userModel.noti);

                if (result == false)
                {
                    object response1 = new { message = "Something Went wrong" };

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.NotFound, response1, Configuration.Formatters.JsonFormatter));
                }
                else
                {

                    object response2 = new { message = "You Profile is Updated Now" };

                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response2, Configuration.Formatters.JsonFormatter));
                }
            }
        }

        // POST api/Account
        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register([FromBody] UserModel userModel)
        {
            if (!ModelState.IsValid || userModel == null)
            {
                //return BadRequest(ModelState);
                App_Start.PrettyHttpError error = new App_Start.PrettyHttpError(ModelState);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, error));
            }

            
            var user = new Contracts.User()
            {
                UserName = userModel.UserName,
                Email = userModel.UserName,
                PhoneNumber = userModel.PhoneNumber
            };

            IdentityResult result = await _repo.RegisterUser(user, userModel.Password, userModel.PhoneNumber);
            
            IHttpActionResult errorResult = GetErrorResult(result);


            if (errorResult != null)
            {
                return errorResult;
            }
            else
            {
                object response = new { message = "You have registered Successfully", Email = userModel.UserName, Password = userModel.Password };

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter));
            }

        }

        [HttpPost]
        [Route("checkNumbers")]
        public Dictionary<String, bool> CheckNumbers([FromBody] CheckNumbers user)
        {
            var response = Business.User.User.CheckNumbers(user.PhoneNumber);
            return response;
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

        private class HttpResponseMessage
        {
            private object responseCode;

            public HttpResponseMessage(object responseCode)
            {
                this.responseCode = responseCode;
            }

            public object Content { get; set; }
        }

    }
}