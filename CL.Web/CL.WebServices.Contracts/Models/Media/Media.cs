using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts.Media
{
    public class Media : IMedia
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public MediaTypeEnum MediaType { get; set; }
    }
}
