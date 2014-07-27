using CL.Services.Contracts;
using CL.Services.Contracts.Responses;
using CL.Services.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Business
{
    public class Tag : ITag
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public ITag GetById(int id)
        {
            using (TagRepository repo = new TagRepository())
            {
                return repo.GetById(id);
            }
        }

        public IPagedResponse<ITag> GetAll(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Page number must exceed 0.");

            if (pageSize <= 0)
                throw new ArgumentException("Page size must exceed 0.");

            using (TagRepository repo = new TagRepository())
            {
                return repo.GetAll(pageNumber, pageSize);
            }
        }

        public IPostResponse Insert(ITag asset)
        {
            if (asset.Id != 0)
                throw new ArgumentException("Asset Id must be 0.");

            using (TagRepository repo = new TagRepository())
            {
                return repo.Insert(asset);
            }
        }
        /*
        public static bool Update(IAsset asset)
        {

        }
        */
        public int Delete(int assetId)
        {
            if (assetId <= 0)
                throw new ArgumentException("Asset Id must exceed 0.");

            using (TagRepository repo = new TagRepository())
            {
                return repo.Delete(assetId);
            }
        }
    }
}
