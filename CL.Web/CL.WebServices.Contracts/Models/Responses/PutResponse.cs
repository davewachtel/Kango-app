using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts.Responses
{
    public class PutResponse : IPutResponse
    {
        public int AffectedRecords { get; set; }
    }
}
