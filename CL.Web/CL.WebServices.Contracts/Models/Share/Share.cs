using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public class Share : IShare
    {
        public int AssetId { get; set; }

        public String[] ToUserId { get; set; }

        public String Message { get; set; }
    }
}
