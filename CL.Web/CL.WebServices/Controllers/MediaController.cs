using CL.Services.Contracts.Media;
using CL.Services.Contracts.Responses;
using CL.Services.Web.Models;
using CL.Services.Web.TypeConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CL.Services.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/Media")]
    public class MediaController : CLApiController
    {
        // GET api/media/{id}
        [HttpGet]
        public MediaModel GetMediaByAssetId(int assetId)
        {
            IMedia result = Business.Media.Media.GetMediaByAssetId(assetId);
            MediaModel media = MediaModel.Load(result);

            return media;
        }


        // GET api/media
        [HttpGet]
        public IPagedResponse<MediaModel> Get([FromUri]PagingFilter filter)
        {
            if (filter == null)
                filter = new PagingFilter(1, 25);

            var userId = this.getUserId();

            IPagedResponse<IMedia> results = Business.Media.Media.GetMedia(userId, filter.Page, filter.Size);

            ICollection<MediaModel> models = new List<MediaModel>();
            if (results.Data != null)
            {
                foreach (IMedia a in results.Data)
                {
                    var model = MediaModel.Load(a);
                    models.Add(model);
                }
            }

            return new PagedResponse<MediaModel>()
            {
                TotalCount = results.TotalCount,
                Data = models
            };
        }
    }
}
