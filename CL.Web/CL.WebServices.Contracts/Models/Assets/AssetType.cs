using CL.Services.Contracts.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts.Models
{
    public class AssetType : IAssetType
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public AssetType(int id, String name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
