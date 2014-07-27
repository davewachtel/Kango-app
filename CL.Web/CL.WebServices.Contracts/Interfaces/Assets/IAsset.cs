using System;
using System.Collections.Generic;

namespace CL.Services.Contracts.Assets
{
    public interface IAsset
    {
        int Id { get; }

        AssetTypeEnum AssetType { get; }

        String Title { get; }

        String Description { get; }

        String Url { get; }

        IList<ITag> Tags { get; }
    }
}
