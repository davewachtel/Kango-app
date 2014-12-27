using CL.Services.Business.Assets;
using CL.Services.Contracts.Assets;
using CL.Services.Contracts.Responses;
using CL.Services.Web.Models;
using CL.Services.Web.TypeConverter;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CL.Services.Web.Controllers
{
    [AllowAnonymous]  //Temporary until Login page is built.
    [RoutePrefix("api/Asset")]
    public class AssetController : CLApiController
    {
        //GET api/asset/1
        [HttpGet]
        public AssetModel GetAssetById(int id)
        {
            IAsset result = Asset.GetAssetById(id);
            var asset = AssetModel.Load(result);

            return asset;
        }

        //GET api/asset
        [HttpGet]
        public IPagedResponse<AssetModel> GetAssets([FromUri]PagingFilter filter)
        {
            if (filter == null)
                filter = new PagingFilter(1, 25);

            IPagedResponse<IAsset> results = Asset.GetAssets(filter.Page, filter.Size);

            ICollection<AssetModel> assets = new List<AssetModel>();
            if (results.Data != null)
            {
                foreach (IAsset a in results.Data)
                {
                    var asset = AssetModel.Load(a);
                    assets.Add(asset);
                }
            }

            return new PagedResponse<AssetModel>()
            {
                TotalCount = results.TotalCount,
                Data = assets
            };
        }

        //POST api/asset
        [HttpPost]
        public IPostResponse InsertAsset([FromBody] AssetModel model)
        {
            if (model == null)
                throw new ArgumentNullException("Please provide a asset to insert.");

            if (model.Id > 0)
                throw new System.ArgumentException("The Asset cannot have an Id.  If you'd like to update an Asset, call update method.");

            return Asset.Insert(model.ToInterface());
        }

        //PUT api/asset/1
        [HttpPut]
        public IPutResponse UpdateAsset([FromUri]int id, [FromBody]AssetModel model)
        {
            int count = 0;
            if (id > 0)
                count = Asset.Update(id, model.ToInterface());

            return new PutResponse()
            {
                AffectedRecords = count
            };
        }

        //DELETE api/asset/1
        [HttpDelete]
        public IDeleteResponse DeleteAsset(int id)
        {
            int count = 0;
            if (id > 0)
                count = Asset.Delete(id);

            return new DeleteResponse()
            {
                AffectedRecords = count
            };
        }
    }
}