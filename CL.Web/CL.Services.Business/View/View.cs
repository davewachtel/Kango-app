using CL.Services.Contracts;
using CL.Services.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Business.View
{
    public class View
    {
        public bool Insert(String userId, int assetId, int duration, bool isLiked)
        {
            if(duration <= 0)
                throw new ArgumentOutOfRangeException("duration");
            

            using(UserRepository rep = new UserRepository())
            {
                return rep.InsertView(userId, assetId, duration, DateTime.UtcNow, isLiked);
            }
        }
    }
}
