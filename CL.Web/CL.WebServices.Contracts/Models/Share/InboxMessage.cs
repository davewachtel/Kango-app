using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public class InboxMessage : IInboxMessage
    {
        public int AssetId { get; set; }

        public string FromUser { get; set; }

        public String Message { get; set; }

        public string TimeAgo { get; set; }

        public bool IsRead { get; set; }
    }
}
