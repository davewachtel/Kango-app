using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public interface IShare
    {
        int AssetId { get; }

        String[] ToUserId { get; }

        String Message { get; }

    }
}
