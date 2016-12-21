using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Services.Data.Context;
using CL.Services.Contracts.Responses;
using CL.Services.Data.Repository;
using CL.Services.Contracts.Interfaces;
using CL.Services.Contracts;

namespace CL.Services.Business
{
    public class Friends
    {
        public IPagedResponse<Contracts.Models.UserView> GetAll(String UserId, int pageNumber, int pageSize)
        {
            /*if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.");*/

            using (FriendRepository repo = new FriendRepository())
            {
                return repo.GetAll(UserId, pageNumber, pageSize);
            }
        }
        public IPagedResponse<Contracts.Models.UserView> AllUsers(string userid,int pageNumber, int pageSize)
        {
            if (userid == null)
                throw new ArgumentException("Please Provide UserId");

            /*if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.");*/

            using (UserRepository repo = new UserRepository())
            {
                return repo.GetAll(userid, pageNumber, pageSize);
            }
        }

        public IPostResponse Insert(IFriend friend)
        {
            using (FriendRepository repo = new FriendRepository())
            {
                return repo.Insert(friend);
            }
        }

        public static int Delete(string UserId1, string UserId2)
        {
            if (UserId1 == null && UserId2 == null)
                throw new ArgumentException("Please Provide UserId1 and UserId2");

            using (FriendRepository repo = new FriendRepository())
            {
                return repo.Delete(UserId1, UserId2);
            }
        }

    }
}
