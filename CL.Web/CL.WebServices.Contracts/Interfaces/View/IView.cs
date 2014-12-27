using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public interface IView
    {
        String UserId { get; }
        int AssetId { get; }
        int? ShareId { get; }
        int Duration { get; }
        bool IsLiked { get; }
    }
}
