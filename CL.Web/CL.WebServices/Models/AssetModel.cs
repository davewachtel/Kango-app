using CL.Services.Contracts;
using CL.Services.Contracts.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CL.Services.Web.Models
{
    public class AssetModel
    {
        public static AssetModel Load(IAsset a)
        {
            if (a == null)
                throw new ArgumentNullException("Asset cannot be null.");

            AssetModel model = new AssetModel();
            model.Id = a.Id;
            model.AssetType = a.AssetType;
            model.Title = a.Title;
            model.Description = a.Description;
            model.Url = a.Url;

            if (a.Tags != null)
            {
                List<TagModel> lstTags = new List<TagModel>();
                foreach (ITag tag in a.Tags)
                {
                    lstTags.Add(TagModel.Load(tag));
                }

                model.Tags = lstTags;
            }

            return model;
        }

        public AssetModel()
        {
            this.Tags = new List<TagModel>();
        }

        public int Id { get; set; }

        public Contracts.AssetTypeEnum AssetType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public IList<TagModel> Tags { get; set; }

        public IAsset ToInterface()
        {
            Asset asset = new Contracts.Asset();
            asset.Id = this.Id;
            asset.Title = this.Title;
            asset.Description = this.Description;
            asset.AssetType = this.AssetType;
            asset.Url = this.Url;

            if (this.Tags != null)
            {
                foreach (TagModel tag in this.Tags)
                {
                    asset.Tags.Add(tag.ToInterface());
                }
            }

            return asset;
        }
    }
}