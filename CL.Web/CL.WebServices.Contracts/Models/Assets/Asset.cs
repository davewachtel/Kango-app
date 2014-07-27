using CL.Services.Contracts.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public class Asset : IAsset
    {
        public int Id { get; set; }

        public AssetTypeEnum AssetType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public IList<ITag> Tags { get; set; }

        public Asset()
        {
            this.Tags = new List<ITag>();
        }
    }
}
