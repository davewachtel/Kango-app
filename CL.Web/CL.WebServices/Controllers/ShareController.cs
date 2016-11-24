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

            if (count >= 1)
            {
                object response = new { message = "You have shared the media Successfully", fromUserId = fromUserId, ToUserId = share.ToUserId, AssetId = share.AssetId };

                return Request.CreateResponse(HttpStatusCode.OK, response, Configuration.Formatters.JsonFormatter);
                
            }

            else
            {
                object response2 = new { message = "Something went wrong please try again"};
                return Request.CreateResponse(HttpStatusCode.BadRequest, response2, Configuration.Formatters.JsonFormatter);

            }
        }
    }
}