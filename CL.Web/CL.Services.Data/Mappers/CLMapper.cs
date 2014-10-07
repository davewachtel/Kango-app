using CL.Services.Contracts.Assets;
using CL.Services.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Contracts = CL.Services.Contracts;

namespace CL.Services.Data.Mappers
{
    public class CLMapper
    {
        public static Asset MapAsset(IAsset asset)
        {
            if (asset == null)
                return null;

            Context.Asset result = new Context.Asset();
            result.Id = asset.Id;
            result.AssetTypeId = (int)asset.AssetType;
            result.Title = asset.Title;
            result.Description = asset.Description;
            result.Url = asset.Url;

            /*
            if (asset.Tags != null)
            {
                foreach (var assetTag in asset.Tags)
                {
                    if (assetTag.Tags != null)
                    {
                        var tag = MapTag(assetTag.Tags);
                        result.Tags.Add(tag);
                    }
                }
            }
            */
            return result;
        }

        public static IAsset MapAsset(Asset asset)
        {
            if (asset == null)
                return null;

            Contracts.Asset result = new Contracts.Asset();
            result.Id = asset.Id;
            result.AssetType = (Contracts.AssetTypeEnum)asset.AssetTypeId;
            result.Title = asset.Title;
            result.Description = asset.Description;
            result.Url = asset.Url;

            if (asset.AssetTags != null)
            {
                foreach (var assetTag in asset.AssetTags)
                {
                    if (assetTag.Tags != null)
                    {
                        var tag = MapTag(assetTag.Tags);
                        result.Tags.Add(tag);
                    }
                }
            }

            return result;
        }

        public static Contracts.Media.IMedia MapMedia(Asset asset)
        {
            if (asset == null)
                return null;

            Contracts.Media.Media result = new Contracts.Media.Media();
            result.Id = asset.Id;
            result.MediaType = (Contracts.MediaTypeEnum)asset.AssetTypeId;
            result.Title = asset.Title;
            result.Url = asset.Url;

            return result;
        }

        public static Contracts.ITag MapTag(Tag tag)
        {
            if (tag == null)
                return null;

            Contracts.Tag result = new Contracts.Tag();
            result.Id = tag.Id;
            result.Name = tag.Title;

            return result;
        }
    }
}
