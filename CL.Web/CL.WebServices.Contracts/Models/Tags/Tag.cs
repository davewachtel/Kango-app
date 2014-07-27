using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public class Tag : ITag
    {
        public int Id { get; set; }
        public String Name { get; set; }
    }
}
