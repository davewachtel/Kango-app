using CL.Services.Contracts.Responses;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using CL.Services.Contracts;
using CL.Services.Contracts.Interfaces;
using CL.Services.Data;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;

namespace CL.Services.Data.Repository
{
    public class FriendRepository : CLRepository
    {
        public void RemoveFriendById(int Id)
        {
            this.Context.Friends.RemoveRange(this.Context.Friends.Where(v => v.Friend_Id == Id));
            this.Context.SaveChanges();

        }

        public IPostResponse Insert(IFriend friend)
        {
            var q1 = this.Context.Friends
                             .Where(a => a.Userto == friend.Userto && a.Userfrom == friend.Userfrom)
                             .FirstOrDefault();

            var q2 = this.Context.Friends
                             .Where(a => a.Userfrom == friend.Userto && a.Userto == friend.Userfrom)
                             .FirstOrDefault();

            string path = System.AppDomain.CurrentDomain.BaseDirectory + "App_Data\\CertificatesLive.p12";

            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
              path, "12345", true);


            if (q1 != null || q2 != null)
            {
                return new PostResponse()
                {
                    Id = 0
                };
            }
           
                var frnd = new Data.Context.Friend()
                {
                    Userto = friend.Userto,
                    Userfrom = friend.Userfrom
                };



                Context.Friends.Add(frnd);
                bool result = Context.SaveChanges() > 0;

                if (result == true)
                {
                    var frnd2 = new Data.Context.Friend()
                    {
                        Userto = friend.Userfrom,
                        Userfrom = friend.Userto
                    };

                    Context.Friends.Add(frnd2);
                    Context.SaveChanges();

                var query1 = from user in this.Context.Users
                             where user.Id == friend.Userfrom
                             select user.device_id;

                string did = query1.First().ToString();


                PushNotifications.Apple.QueueNotification(new ApnsNotification
                {
                    DeviceToken = did,
                    Payload = JObject.Parse("{\"aps\":{\"alert\":\"A Friend request sent by you to an other Kangoo user\",\"badge\":1}}")
                });


                var query2 = from user in this.Context.Users
                             where user.Id == friend.Userto
                             select user.device_id;

                string did2 = query1.First().ToString();

                PushNotifications.Apple.QueueNotification(new ApnsNotification
                {
                    DeviceToken = did2,
                    Payload = JObject.Parse("{\"aps\":{\"alert\":\"A Friend request sent to you from an other Kangoo user\",\"badge\":1}}")
                });



            }

                return new PostResponse()
                {
                    Id = frnd.Friend_Id
                };
            
        }

        public IPagedResponse<Contracts.Models.UserView> GetAll(String Userfrom,int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.", "pageNumber");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.", "pageSize");

            int totalCount = 0;

            var query = from frnd in this.Context.Friends
                        join user in this.Context.Users on frnd.Userto equals user.Id
                        where frnd.Userfrom == Userfrom
                        select user;

            var pageQuery = this.PagedResult(query, pageNumber, pageSize, a => a.Id, true, out totalCount);

            List<Contracts.Models.UserView> lstResults = new List<Contracts.Models.UserView>();
            foreach (Context.User asset in pageQuery)
            {
                var result = Mappers.CLMapper.MapUser(asset);
                lstResults.Add(result);
            }

            return new PagedResponse<Contracts.Models.UserView>()
            {
                TotalCount = totalCount,
                Data = lstResults
            };
        }

        public int Delete(string id1, string id2)
        {
            if (id1 == null && id2 == null)
                throw new ArgumentException("Please Provide User_from and User_to");

            var frnd = this.Context.Friends
                             .Where(a => a.Userfrom == id1)
                             .Where(a => a.Userto == id2)
                             .FirstOrDefault();

            if (frnd == null)
            {
                return 0;
            }
            else
            {
                var frnd2 = this.Context.Friends
                             .Where(a => a.Userfrom == id2)
                             .Where(a => a.Userto == id1)
                             .FirstOrDefault();

                this.Context.Friends.Remove(frnd2);
            }

            this.Context.Friends.Remove(frnd);

            return this.Context.SaveChanges();



        }
    }
}
