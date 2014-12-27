using CL.Services.Contracts.Responses;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using CL.Services.Contracts;

namespace CL.Services.Data.Repository
{
    public class UserRepository : CLRepository
    {
        //TODO: This is deleting views 1 at a time.  Needs to run a bulk delete.
        public void RemoveViewsByUserId(String userId)
        {
            //context.ExecuteStoreCommand("DELETE FROM Views WHERE UserId = {0}", customerId);
            
            this.Context.Views.RemoveRange(this.Context.Views.Where(v => v.UserId == userId));
            this.Context.SaveChanges();
        }

        
        public IPagedResponse<IInboxMessage> GetInboxByUserId(String userId, int pageNumber, int pageSize)
        {
            var query = this.Context.Shares
                        .Where(s => s.ToUserId == userId && s.Asset.IsActive)
                        .Include(s => s.FromUser);

            int totalCount;
            var pageQuery = this.PagedResult(query, pageNumber, pageSize, s => s.CreateDt, false, out totalCount);

            List<IInboxMessage> lstResults = new List<IInboxMessage>();
            foreach (Context.Share share in pageQuery)
            {
                var result = Mappers.CLMapper.MapInboxMessage(share);
                lstResults.Add(result);
            }

            return new PagedResponse<IInboxMessage>()
            {
                TotalCount = totalCount,
                Data = lstResults
            };
        }
        
        public int SendInboxMessages(String fromUserId, IShare shareMessage)
        {
            if (shareMessage.AssetId == 0)
                throw new ArgumentNullException("AssetId");

            if (shareMessage.ToUserId == null || shareMessage.ToUserId.Length == 0)
                throw new ArgumentNullException("ToUserId");

            foreach(String userId in shareMessage.ToUserId)
            {
                var share = new Data.Context.Share()
                {
                   AssetId = shareMessage.AssetId,
                   CreateDt = DateTime.UtcNow,
                   FromUserId = fromUserId,
                   ToUserId = userId
                };

                this.Context.Shares.Add(share);
            }

            return this.Context.SaveChanges();
        }

        public bool InsertView(String userId, int assetId, int duration, DateTime createDt, bool isLiked)
        {
            var view = new Data.Context.View()
             {
                 UserId = userId,
                 AssetId = assetId,
                 Duration = duration,
                 CreateDt = createDt,
                 IsLiked = isLiked
             };

            Context.Views.Add(view);
            return Context.SaveChanges() > 0;
        }
    }
}
