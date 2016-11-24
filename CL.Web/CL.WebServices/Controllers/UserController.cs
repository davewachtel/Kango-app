using CL.Services.Business.User;
using CL.Services.Contracts.Responses;
using CL.Services.Web.Models;
using CL.Services.Web.Repository;
using CL.Services.Web.TypeConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CL.Services.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/User")]
    public class UserController : CLApiController
    {

        [HttpGet]
        public object GetUserById(String userId)
        {
            return null;
        }

        [Route("{userId}/Inbox")]
        [HttpGet]
        public IPagedResponse<Contracts.IInboxMessage> GetInboxByUserId([FromUri] String userId, [FromUri] PagingFilter filter)
        {
            if(filter == null)
                filter = new PagingFilter(1, 25);
            

            var response = Business.User.User.GetInboxByUserId(userId, filter.Page, filter.Size);
            return response;
        }

        [Route("{userId}/Inbox")]
        [HttpPut]
        public IPutResponse MarkMessageAsReadOrUnRead([FromUri] String userId, [FromBody] InboxMessageModel message)
        {
            var response = Business.User.User.MarkMessageAsReadOrUnRead(userId, message);
            return response;
        }

        [Route("{userId}/Views")]
        [HttpDelete]
        public HttpResponseMessage RemoveViewsByUserId([FromUri] String userId)
        {
            Business.User.User.RemoveViewsByUserId(userId);

            return new HttpResponseMessage(HttpStatusCode.NoContent); //204
        }


    }
}
