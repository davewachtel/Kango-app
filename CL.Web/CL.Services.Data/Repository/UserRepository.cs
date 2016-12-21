using CL.Services.Contracts.Responses;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using CL.Services.Contracts;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;

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

        public IPutResponse MarkMessageAsReadOrUnRead(String userId, int messageId, bool isRead)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException("userId");
            }
            if (messageId == 0)
            {
                throw new ArgumentNullException("assetId");
            }

            var message = this.Context.Shares
                            .Where(s => s.Id == messageId)
                            .Where(s => s.ToUserId == userId)
                            .FirstOrDefault();

            PutResponse response = new PutResponse();

            if (message != null)
            {
                if (isRead)
                    message.ReadDt = DateTime.UtcNow;
                else
                    message.ReadDt = null;

                response.AffectedRecords = this.Context.SaveChanges();
            }

            return response;
        }

        public Dictionary<String, bool> CheckNumbers(String[] phoneNumbers)
        {
            if (phoneNumbers.Length == 0)
            {
                throw new ArgumentNullException("phoneNumbers");
            }

            Dictionary<String, bool> data = new Dictionary<string, bool>();
            foreach (String phone in phoneNumbers)
            {
                var numbers = this.Context.Users.Where(u => u.PhoneNumber == phone).FirstOrDefault();
                if (numbers != null)
                {
                    data[phone] = true;
                }
                else
                {
                    data[phone] = false;
                }
            }

            return data;
        }

        public Dictionary<String, IList<dynamic>> CheckUsers(String id,String[] phoneNumbers)
        {

          

            if (phoneNumbers.Length == 0)
            {
                throw new ArgumentNullException("phoneNumbers");
            }

            Dictionary<String, IList<dynamic>> maindata = new Dictionary<string, IList<dynamic>>();

            List<dynamic> data = new List<dynamic>();


            
            foreach (String phone in phoneNumbers)
            {
               
                var numbers = this.Context.Users.Where(u => u.PhoneNumber == phone).FirstOrDefault();
                var query = (from frnd in this.Context.Friends
                            join user in this.Context.Users on frnd.Userto equals user.Id
                            where (frnd.Userfrom == id) && user.PhoneNumber == phone
                            select user.PhoneNumber).FirstOrDefault();


                if (query !=null)
                {
                    var fid = from user in this.Context.Users
                                 where user.PhoneNumber == phone
                                 select user.Id;

                    string user_id = fid.First().ToString();

                    data.Add(new {phone = phone, staus = "Friend", User_id = user_id});
                }

                else if (numbers != null)
                {
                    var uid = from user in this.Context.Users
                              where user.PhoneNumber == phone
                              select user.Id;

                    string u_id = uid.First().ToString();

                    data.Add(new { phone = phone, staus = "User", User_id = u_id});
                }
                else
                {
                    data.Add(new { phone = phone, staus = "No User"});
                }
            }

            
            maindata["phone"] = data;
            return maindata;
        }

        public int SendInboxMessages(String fromUserId, IShare shareMessage)
        {
            if (shareMessage.AssetId == 0)
                throw new ArgumentNullException("AssetId");

            if (shareMessage.ToUserId == null || shareMessage.ToUserId.Length == 0)
                throw new ArgumentNullException("ToUserId");

            string path = System.AppDomain.CurrentDomain.BaseDirectory + "App_Data\\CertificatesLive.p12";

            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
              path, "12345", true);


            var query1 = from user in this.Context.Users
                         where user.Id == fromUserId
                         select user.device_id;

            string did = query1.First().ToString();

            var noti1 = from user in this.Context.Users
                        where user.Id == fromUserId
                        select user.notify_me;

            string note1 = noti1.First().ToString();
            if (note1 == "true" || note1 == null)
            {
                PushNotifications.Apple.QueueNotification(new ApnsNotification
                {
                    DeviceToken = did,
                    Payload = JObject.Parse("{\"aps\":{\"alert\":\"You Just share a media\",\"badge\":1}}")
                });

            }
            foreach (String userId in shareMessage.ToUserId)
            {
                var share = new Data.Context.Share()
                {
                    AssetId = shareMessage.AssetId,
                    CreateDt = DateTime.UtcNow,
                    FromUserId = fromUserId,
                    ToUserId = userId
                };

                this.Context.Shares.Add(share);

                var q = from user in this.Context.Users
                         where user.Id == userId
                        select user.device_id;

                string did2 = q.First().ToString();

                var noti2 = from user in this.Context.Users
                            where user.Id == userId
                            select user.notify_me;

                string note2 = noti2.First().ToString();
                if (note2 == "true" || note2 == null)
                {
                    PushNotifications.Apple.QueueNotification(new ApnsNotification
                    {
                        DeviceToken = did2,
                        Payload = JObject.Parse("{\"aps\":{\"alert\":\"Share a media with you by other user\",\"badge\":1}}")
                    });
                }
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

        public bool UpdateProfile(String userId, String PhoneNumber, String notify)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);
            user.PhoneNumber = PhoneNumber;
            user.notify_me = notify;
            return Context.SaveChanges() > 0;
        }

        public bool SetDeviceToken(String userId, String device_id)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);
            user.device_id = device_id;
            return Context.SaveChanges() > 0;
        }

        public IPagedResponse<Contracts.Models.UserView> GetAll(string userid,int pageNumber, int pageSize)
        {
            if (userid == null)
                throw new ArgumentException("Please Provide UserId");

            /*if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.", "pageNumber");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.", "pageSize");*/

            int totalCount = 0;

            List<string> validValues = new List<string>() ;

            var query2 = from frnd in this.Context.Friends
                         where userid == frnd.Userfrom
                         select frnd.Userto;
            if (query2 == null || query2 !=null)
            {
                validValues.Add(userid);
            }
            

            foreach (string item in query2)
            {
                validValues.Add(item);
            }

           var qry = from user in this.Context.Users
                      where !validValues.Contains(user.Id)
                      select user;

            if (pageNumber == 0)
            {
                pageSize = totalCount;
            }

            var pageQuery = this.PagedResult(qry, pageNumber, pageSize, a => a.Email, true, out totalCount);

            List<Contracts.Models.UserView> lstResults = new List<Contracts.Models.UserView>();
            foreach (Context.User users in pageQuery)
            {
                var result = Mappers.CLMapper.MapUser(users);
                lstResults.Add(result);
            }

            return new PagedResponse<Contracts.Models.UserView>()
            {
                TotalCount = totalCount,
                Data = lstResults
            };
        }
    }
}
