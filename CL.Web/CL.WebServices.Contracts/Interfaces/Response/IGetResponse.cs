﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts.Responses
{
    public interface IGetResponse<T>
    {
        ICollection<T> Data { get; }
    }
}
