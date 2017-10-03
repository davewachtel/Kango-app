using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CL.Services.Data.Context;
using CL.Services.Web.Models;
using CL.Services.Contracts.Responses;
using CL.Services.Contracts.Interfaces;
using CL.Services.Business;
using CL.Services.Contracts;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;

namespace CL.Services.Web.Controllers
{
    [AllowAnonymous]
    [Authorize]
    [RoutePrefix("api/Friends")]
    public class FriendsController : CLApiController
    {
        [HttpPost]
        [Route("getFriend")]
        public IPagedResponse<Contracts.Models.UserView> GetFriends([FromBody] GetFriendsByUId f)
        {
            //String UserId = this.getUserId();
            return new Business.Friends().GetAll(f.UserId, f.pagenumber, f.pagesize);
        }

        [HttpPost]
        [Route("AllUsers")]
        public IPagedResponse<Contracts.Models.UserView> GetAll([FromBody] Pagination p)
        {
            return new Business.Friends().AllUsers(p.UserId, p.pagenumber, p.pagesize);
        }

        [HttpPost]
        [Route("addFriend")]
        public IHttpActionResult Insert([FromBody] FriendModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                App_Start.PrettyHttpError error = new App_Start.PrettyHttpError(ModelState);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, error));
            }

            return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, new Business.Friends().Insert(model.ToInterface())));


        }


        
        [HttpPost]
        [Route("deleteFriend")]
        public IDeleteResponse DeleteFriend([FromBody] DeleteFrnd frnd)
        {
            int count = 0;
            if (frnd.User_from != null && frnd.User_to != null)
                count = Friends.Delete(frnd.User_from, frnd.User_to);

            return new DeleteResponse()
            {
                AffectedRecords = count
            };
        }

    }
}