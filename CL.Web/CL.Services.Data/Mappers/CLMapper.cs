using CL.Services.Contracts.Assets;
using CL.Services.Contracts.Interfaces;
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

        public static Contracts.IInboxMessage MapInboxMessage(Share share)
        {
            if (share == null)
                return null;

            Contracts.InboxMessage result = new Contracts.InboxMessage();
            result.MessageId = share.Id;
            result.AssetId = share.AssetId;
            result.FromUser = share.FromUser.UserName;
            //result.Message = share.Message;
            result.TimeAgo = ToFriendlyTimeAgo(share.CreateDt);
            result.IsRead = share.ReadDt.HasValue;

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

        public static IFriend MapFriend(Friend frnd)
        {
            if (frnd == null)
                return null;

            Contracts.Friend result = new Contracts.Friend();
            result.Friend_Id = frnd.Friend_Id;
            result.Userto = frnd.Userto;
            result.Userfrom = frnd.Userfrom;

            return result;
        }

        public static Contracts.Models.UserView MapUser(User user)
        {
            if (user == null)
                return null;

            Contracts.Models.UserView result = new Contracts.Models.UserView();
            result.Id = user.Id;
            result.PhoneNumber = user.PhoneNumber;
            result.Email = user.Email;
            //result.UserName = user.UserName;
           

            return result;
        }

        private static String ToFriendlyTimeAgo(DateTime date)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 0)
            {
                return "not yet";
            }
            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 2 * MINUTE)
            {
                return "a minute ago";
            }
            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 90 * MINUTE)
            {
                return "an hour ago";
            }
            if (delta < 24 * HOUR)
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 48 * HOUR)
            {
                return "yesterday";
            }
            if (delta < 30 * DAY)
            {
                return ts.Days + " days ago";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
             
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
            
        }
    }
}
