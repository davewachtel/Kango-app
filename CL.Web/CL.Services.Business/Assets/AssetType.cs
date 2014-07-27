using CL.Services.Contracts;
using CL.Services.Contracts.Assets;
using CL.Services.Contracts.Responses;
using CL.Services.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data = CL.Services.Data;

namespace CL.Services.Business.Assets
{
    public class AssetType
    {
        public IPagedResponse<IAssetType> GetAll(int page, int size)
        {
            using (AssetTypeRepository repo = new AssetTypeRepository())
            {
                return repo.GetAll(page, size);
            }
        }
    }
}
