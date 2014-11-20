using CL.Services.Business;
using CL.Services.Contracts;
using CL.Services.Contracts.Responses;
using CL.Services.Web.Models;
using CL.Services.Web.TypeConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CL.Services.Web.Controllers
{
    [Authorize(Roles="Admin")]
    [RoutePrefix("Tag")]
    public class TagController : ApiController
    {
        //GET api/tag/1
        [HttpGet]
        public TagModel GetAssetById(int id)
        {
            ITag result = new Business.Tag().GetById(id);
            var asset = TagModel.Load(result);

            return asset;
        }

        //GET api/tag
        [HttpGet]
        public IPagedResponse<TagModel> GetAll([FromUri]PagingFilter filter)
        {
            if (filter == null)
                filter = new PagingFilter(1, 25);

            IPagedResponse<ITag> results = new Business.Tag().GetAll(filter.Page, filter.Size);

            ICollection<TagModel> assets = new List<TagModel>();
            if (results.Data != null)
            {
                foreach (ITag a in results.Data)
                {
                    var asset = TagModel.Load(a);
                    assets.Add(asset);
                }
            }

            return new PagedResponse<TagModel>()
            {
                TotalCount = results.TotalCount,
                Data = assets
            };
        }

        //POST api/tag
        [HttpPost]
        public IPostResponse Insert([FromBody] TagModel model)
        {
            if (model == null)
                throw new ArgumentNullException("Please provide a asset to insert.");

            if (model.Id > 0)
                throw new System.ArgumentException("The Asset cannot have an Id.  If you'd like to update an Asset, call update method.");

            return new Business.Tag().Insert(model.ToInterface());
        }

        //PUT api/tag/1
        [HttpPut]
        public IPutResponse Update(int id, [FromBody]AssetModel model)
        {
            return new PutResponse()
            {
                AffectedRecords = 0
            };
        }

        //DELETE api/tag/1
        [HttpDelete]
        public IDeleteResponse Delete(int id)
        {
            int count = 0;
            if (id > 0)
                count = new Business.Tag().Delete(id);

            return new DeleteResponse()
            {
                AffectedRecords = count
            };
        }
    }
}
