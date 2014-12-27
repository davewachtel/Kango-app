using CL.Services.Contracts.Media;
using CL.Services.Contracts.Responses;
using CL.Services.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Data.Repository
{
    public class MediaRepository : CLRepository
    {
        public MediaRepository() { }

        public IMedia GetMediaByAssetId(int assetId)
        {
            var query = this.Context.Assets
                                .Where(a => a.Id == assetId && a.IsActive);

            var asset = query.FirstOrDefault();
            return Mappers.CLMapper.MapMedia(asset);
        }

        public IPagedResponse<IMedia> GetAll(String userId, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.", "pageNumber");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.", "pageSize");

            int totalCount = 0;

            var myViews = this.Context.Views.Where(v => v.UserId == userId);
            var query = from asset in this.Context.Assets
                        join views in myViews on asset.Id equals views.AssetId into av
                        from subAssets in av.DefaultIfEmpty()
                        where asset.IsActive 
                                && asset.AssetTypeId != (int)CL.Services.Contracts.AssetTypeEnum.YouTube
                                && subAssets.Id == null
                        select asset;
                        
            /*
            var query = this.Context.Assets
                            .Where(a => a.IsActive)
                            .Where(a => a.AssetTypeId != (int)CL.Services.Contracts.AssetTypeEnum.YouTube);
            */



            var pageQuery = this.PagedResult(query, pageNumber, pageSize, a => Guid.NewGuid(), true, out totalCount);

            List<IMedia> lstResults = new List<IMedia>();
            foreach (Context.Asset asset in pageQuery)
            {
                var result = Mappers.CLMapper.MapMedia(asset);
                lstResults.Add(result);
            }

            return new PagedResponse<IMedia>()
            {
                TotalCount = totalCount,
                Data = lstResults
            };
        }
    }
}
