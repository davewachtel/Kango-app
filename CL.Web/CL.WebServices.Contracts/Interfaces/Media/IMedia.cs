using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts.Media
{
    public interface IMedia
    {
        int Id { get; set; }
        String Title { get; set; }
        String Url { get; set; }
        MediaTypeEnum MediaType { get; set; }
    }
}
