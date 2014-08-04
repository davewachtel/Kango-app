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
            var asset = this.Context.Assets
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

            List<IAsset> lstResults = new List<IAsset>();
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
            Asset entity = new Asset();
            entity.AssetTypeId = (int)asset.AssetType;
            entity.Title = asset.Title;
            entity.Description = asset.Description;
            entity.Url = asset.Url;
            entity.IsActive = true;

            this.ProcessTags(entity, asset);

            this.Context.Assets.Add(entity);
            int count = this.Context.SaveChanges();

            return new PostResponse()
            {
                Id = entity.Id
            };
        }

        private Context.Asset ProcessTags(Context.Asset asset, IAsset value)
        {
            if (value.Tags == null || value.Tags.Count == 0)
                return asset;

            Dictionary<int, Context.Tag> dicExistingTags = this.GetTags(value);

            //Remove Tags from Asset that don't match List.
            //This means that the user has deleted some tags and we want to remove the association.
            this.MarkAssetTagsForDelete(asset, dicExistingTags);

            //Add Tags.
            this.AddTags(asset, value, dicExistingTags);

            return asset;
        }

        private void AddTags(Context.Asset asset, IAsset value, Dictionary<int, Context.Tag> dicExistingTags)
        {
            foreach (Contracts.Tag vtag in value.Tags)
            {
                //If not an existing tag, create a new tag.
                Tag tag = dicExistingTags.Values.FirstOrDefault(t => t.Title.ToLower() == vtag.Name.ToLower());
                if (tag == null)
                {
                    asset.AssetTags.Add(new AssetTag()
                    {
                        Asset = asset,
                        Tags = new Tag()
                                {
                                    Title = vtag.Name,
                                    IsActive = true
                                }
                    });
                }
                else if (!asset.AssetTags.Any(t => t.Tags.Id == tag.Id))
                {
                    asset.AssetTags.Add(new AssetTag()
                    {
                        Asset = asset,
                        Tags = tag
                    });
                }
            }
        }

        private void MarkAssetTagsForDelete(Context.Asset asset, Dictionary<int, Context.Tag> dicExistingTags)
        {
            if (asset.AssetTags != null)
            {
                for (int i = asset.AssetTags.Count - 1; i >= 0; i--)
                {
                    Context.AssetTag at = asset.AssetTags.ElementAt(i);
                    if (!dicExistingTags.ContainsKey(at.TagId))
                        this.Context.Entry(at).State = EntityState.Deleted;
                }
            }
        }

        private Dictionary<int, Context.Tag> GetTags(IAsset asset)
        {
            Dictionary<int, Context.Tag> tags = new Dictionary<int, Context.Tag>();
            if (asset == null || asset.Tags == null || asset.Tags.Count == 0)
                return tags;

            IEnumerable<String> lstTags = asset.Tags.Select<Contracts.ITag, String>(t => t.Name);

            tags = this.Context.Tags
                    .Where(t => lstTags.Any(at => at.ToLower() == t.Title.ToLower() && t.IsActive))
                    .ToDictionary(t => t.Id);

            return tags;
        }

        public int Update(int id, IAsset asset)
        {
            if (id <= 0)
                throw new ArgumentException("Asset Id must exceed 0.");

            var entity = this.Context.Assets
                             .Where(a => a.Id == id)
                             .Where(a => a.IsActive)
                             .Include(a => a.AssetTags.Select(at => at.Tags))
                             .FirstOrDefault();

            if (entity == null)
                return 0;

            entity.AssetTypeId = (int)asset.AssetType;
            entity.Title = asset.Title;
            entity.Description = asset.Description;
            entity.Url = asset.Url;

            this.ProcessTags(entity, asset);


            return this.Context.SaveChanges();
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
