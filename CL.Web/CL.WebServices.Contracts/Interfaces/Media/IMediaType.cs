using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts.Media
{
    public interface IMediaType
    {
        int Id { get; set; }
        String Name { get; set; }
    }
}
