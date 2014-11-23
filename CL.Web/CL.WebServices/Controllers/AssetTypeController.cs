using CL.Services.Business.Assets;
using CL.Services.Contracts.Assets;
using CL.Services.Contracts.Responses;
using System.Collections.Generic;
using System.Web.Http;

namespace CL.Services.Web.Controllers
{
    [RoutePrefix("api/Asset/Type")]
    public class AssetTypeController : ApiController
    {
        //
        // GET: /AssetType/
        public IPagedResponse<IAssetType> GetAll()
        {
            int page = 1;
            int size = 10;

            AssetType at = new AssetType();
            return at.GetAll(page, size);
        }
    }
}
