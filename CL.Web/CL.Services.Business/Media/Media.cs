using CL.Services.Contracts.Media;
using CL.Services.Contracts.Responses;
using CL.Services.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Business.Media
{
    public abstract class Media
    {
        public static IPagedResponse<IMedia> GetMedia(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.");

            using (MediaRepository repo = new MediaRepository())
            {
                return repo.GetAll(pageNumber, pageSize);
            }
        }
    }
}
