using CL.Services.Web.Controllers;
using CL.Services.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CL.Services.Web.Controller
{
    [Authorize]
    [RoutePrefix("api/Share")]
    public class ShareController : CLApiController
    {
        
        [HttpPost]
        public HttpResponseMessage ShareAsset([FromBody] ShareModel share)
        {
            String fromUserId = this.getUserId();
            int count = Business.User.User.SendInboxMessages(fromUserId, share);

            return new HttpResponseMessage(HttpStatusCode.NoContent); //204
        }
        
    }
}