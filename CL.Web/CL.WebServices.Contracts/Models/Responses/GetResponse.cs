using System.Collections.Generic;

namespace CL.Services.Contracts.Responses
{
    public class GetResponse<T>
    {
        public ICollection<T> Data { get; set; }
    }
}
