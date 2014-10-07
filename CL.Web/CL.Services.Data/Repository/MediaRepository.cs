using CL.Services.Contracts.Media;
using CL.Services.Contracts.Responses;
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

        public IPagedResponse<IMedia> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.", "pageNumber");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.", "pageSize");

            int totalCount = 0;

            var query = this.Context.Assets
                            .Where(a => a.IsActive)
                            .Where(a => a.AssetTypeId != (int)CL.Services.Contracts.AssetTypeEnum.YouTube);

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
