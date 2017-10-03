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

        public IPutResponse MarkMessageAsReadOrUnRead(String userId, int messageId, int Read)
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
                if (Read == 1)
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
               
                //var numbers = this.Context.Users.Where(u => u.PhoneNumber == phone).FirstOrDefault();
                var  n = this.Context.Users.Where(u => u.PhoneNumber == phone).Select(u => new { u.Id, u.PhoneNumber}).ToList();
                var query = (from frnd in this.Context.Friends
                            join user in this.Context.Users on frnd.Userto equals user.Id
                            where (frnd.Userfrom == id) && user.PhoneNumber == phone
                            select user.Id).FirstOrDefault();
 
                if (query != null && n.Count >= 1)
                {
                    if (id == query)
                    {
                        //do nothing
                    }
                    else
                    {
                        data.Add(new { phone = phone, staus = "Friend", User_id = query });
                    }
                }
                else if (n.Count >= 1)
                {
                    //foreach (var item in n)
                    //{
                    if (id == n[0].Id)
                    {
                        //do nothing
                    }
                    else
                    {
                        data.Add(new { phone = n[0].PhoneNumber, staus = "User", User_id = n[0].Id });
                    }
                    //}
                }
                else
                {
                    data.Add(new { phone = phone, staus = "No User" });
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

            //PushNotifications.Apple = new ApnsServiceBroker(config);

            var note = this.Context.Users.Where(u => u.Id == fromUserId).Select(u => new { u.device_id, u.notify_me }).ToList();

            string note1 = note[0].notify_me.ToString();
            /*if ((note1 == "True" || note1 == null) && (!String.IsNullOrEmpty(note[0].device_id)))
            {
                PushNotifications.Apple.QueueNotification(new ApnsNotification
                {
                    DeviceToken = note[0].device_id,
                    Payload = JObject.Parse("{\"aps\":{\"alert\":\"You Just shared a media\",\"sound\":\"default\"},\"asset_id\":"+ shareMessage.AssetId+"}")
                });

            }*/
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
                this.Context.SaveChanges();

                int msgid = share.Id;

                var note2 = this.Context.Users.Where(u => u.Id == userId).Select(u => new { u.device_id, u.notify_me }).ToList();

                string noti = note2[0].notify_me.ToString();

                if ((noti == "True" || noti == null) && (!String.IsNullOrEmpty(note2[0].device_id)))
                {
                    PushNotifications.Apple.QueueNotification(new ApnsNotification
                    {
                        DeviceToken = note2[0].device_id,
                        Payload = JObject.Parse("{\"aps\":{\"alert\":\"A friend sent you something awesome on Kango!\",\"sound\":\"default\"},\"asset_id\":" + shareMessage.AssetId + ",\"msgId\":" + msgid + "}")
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

        public bool UpdateProfile(String userId, String PhoneNumber, Boolean notify)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);

           
                user.PhoneNumber = PhoneNumber;
        
                
            if(user.notify_me != notify)
            {
                user.notify_me = notify;
            }
            return Context.SaveChanges() > 0;
        }

        public bool SetDeviceToken(String userId, String device_id)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);
            user.device_id = device_id;
            return Context.SaveChanges() > 0;
        }

        public bool PhoneNumberExist(String PhoneNumber)
        {
            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                var number = this.Context.Users.Where(u => u.PhoneNumber == PhoneNumber).FirstOrDefault();

                if (number != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
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
