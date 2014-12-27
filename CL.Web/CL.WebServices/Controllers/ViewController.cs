using CL.Services.Contracts.Responses;
using CL.Services.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CL.Services.Web.Controllers
{
    [Authorize]
    [Route("api/View")]
    public class ViewController : CLApiController
    {
        [HttpPost]
        public IPostResponse Insert([FromBody] ViewModel data)
        {
            String userId = this.getUserId();

            var view = new Business.View.View();
            bool result = view.Insert(this.getUserId(), data.assetId, data.duration, data.isLiked);

            return new PostResponse()
                {
                    Id = 0
                };
        }
    }
}
