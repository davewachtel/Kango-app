using CL.Services.Contracts;
using CL.Services.Contracts.Assets;
using CL.Services.Contracts.Models;
using CL.Services.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Services.Data.Repository
{
    public class AssetTypeRepository : CLRepository
    {
        public IPagedResponse<IAssetType> GetAll(int page, int size)
        {
            IList<IAssetType> result = new List<IAssetType>();
            foreach (AssetTypeEnum e in Enum.GetValues(typeof(AssetTypeEnum)))
            {
                result.Add(new AssetType((int)e, e.ToString()));
            }

            return new PagedResponse<IAssetType>()
            {
                TotalCount = result.Count,
                Data = result.Skip((page - 1) * size).Take(size).ToList()
            };
        }
    }
}
