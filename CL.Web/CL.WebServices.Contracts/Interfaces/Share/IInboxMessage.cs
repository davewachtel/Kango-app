﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public interface IInboxMessage
    {
        int AssetId { get; }

        String FromUser { get; }
        
        String Message { get; }

        String TimeAgo { get; }

        bool IsRead { get; }
    }
}
