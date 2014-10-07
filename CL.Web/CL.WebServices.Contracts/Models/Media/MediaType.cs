using CL.Services.Contracts.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public class MediaType : IMediaType
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public MediaType(int id, String name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
