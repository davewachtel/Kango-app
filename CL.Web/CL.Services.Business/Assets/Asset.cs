using CL.Services.Contracts.Assets;
using CL.Services.Contracts.Responses;
using CL.Services.Data.Repository;
using System;
using System.Collections.Generic;

namespace CL.Services.Business.Assets
{
    public abstract class Asset
    {
        public static IAsset GetAssetById(int id)
        {
            using (AssetRepository repo = new AssetRepository())
            {
                return repo.GetById(id);
            }
        }

        public static IPagedResponse<IAsset> GetAssets(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.");

            using (AssetRepository repo = new AssetRepository())
            {
                return repo.GetAll(pageNumber, pageSize);
            }
        }

        public static IPostResponse Insert(IAsset asset)
        {
            if (asset.Id != 0)
                throw new ArgumentException("Asset Id must be 0.");

            using (AssetRepository repo = new AssetRepository())
            {
                return repo.Insert(asset);
            }
        }
        /*
        public static bool Update(IAsset asset)
        {

        }
        */
        public static int Delete(int assetId)
        {
             if (assetId <= 0)
                throw new ArgumentException("Asset Id must exceed 0.");

            using (AssetRepository repo = new AssetRepository())
            {
                return repo.Delete(assetId);
            }
        }
    }
}
