using CL.Services.Contracts.Assets;
using System.Linq;
using System.Data.Entity;
using CL.Services.Data.Context;
using System.Collections.Generic;
using System;
using CL.Services.Contracts.Responses;
using EntityFramework.Extensions;

namespace CL.Services.Data.Repository
{
    public class AssetRepository : CLRepository
    {
        public AssetRepository() { }

        public IAsset GetById(int id)
        {
            var asset =  this.Context.Assets
                            .Where(a => a.Id == id)
                            .Where(a => a.IsActive)
                            .Include(a => a.AssetTags.Select(at => at.Tags))
                            .FirstOrDefault();

            var result = Mappers.CLMapper.MapAsset(asset);
            return result;
        }

        public IPagedResponse<IAsset> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.", "pageNumber");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.", "pageSize");

            int totalCount = 0;

            var query = this.Context.Assets
                            .Where(a => a.IsActive)
                            .Include(a => a.AssetTags.Select(at => at.Tags));
                            

            var pageQuery = this.PagedResult(query, pageNumber, pageSize, a => a.Title, true, out totalCount);

            List <IAsset> lstResults = new List<IAsset>();
            foreach (Asset asset in pageQuery)
            {
                var result = Mappers.CLMapper.MapAsset(asset);
                lstResults.Add(result);
            }

            return new PagedResponse<IAsset>()
            {
                TotalCount = totalCount,
                Data = lstResults
            };
        }

        public IPostResponse Insert(IAsset asset)
        {
            return null;
        }

        public IPutResponse Update()
        {

            return null;
        }

        public int Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Asset Id must exceed 0.");

            var asset = this.Context.Assets
                             .Where(a => a.Id == id)
                             .Where(a => a.IsActive)
                             .FirstOrDefault();

            if (asset == null)
                return 0;


            asset.IsActive = false;
            return this.Context.SaveChanges();

             //return this.Context.Assets.Where(a => a.Id == assetId).Delete();
        }
    }
}
