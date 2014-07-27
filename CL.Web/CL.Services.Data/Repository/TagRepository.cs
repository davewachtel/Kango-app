
using CL.Services.Contracts.Assets;
using System.Linq;
using System.Data.Entity;
using CL.Services.Data.Context;
using System.Collections.Generic;
using System;
using CL.Services.Contracts.Responses;
using EntityFramework.Extensions;
using CL.Services.Contracts;


namespace CL.Services.Data.Repository
{
    public class TagRepository : CLRepository
    {
        public TagRepository() { }

        public ITag GetById(int id)
        {
            var tag =  this.Context.Tags
                            .Where(a => a.Id == id)
                            .Where(a => a.IsActive)
                            .FirstOrDefault();

            var result = Mappers.CLMapper.MapTag(tag);
            return result;
        }

        public IPagedResponse<ITag> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.", "pageNumber");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.", "pageSize");

            int totalCount = 0;

            var query = this.Context.Tags
                            .Where(a => a.IsActive);

            var pageQuery = this.PagedResult(query, pageNumber, pageSize, a => a.Title, true, out totalCount);

            List<ITag> lstResults = new List<ITag>();
            foreach (Context.Tag asset in pageQuery)
            {
                var result = Mappers.CLMapper.MapTag(asset);
                lstResults.Add(result);
            }

            return new PagedResponse<ITag>()
            {
                TotalCount = totalCount,
                Data = lstResults
            };
        }

        public IPostResponse Insert(ITag tag)
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

            var asset = this.Context.Tags
                             .Where(a => a.Id == id)
                             .Where(a => a.IsActive)
                             .FirstOrDefault();

            if (asset == null)
                return 0;


            asset.IsActive = false;
            return this.Context.SaveChanges();
        }
    }
}
