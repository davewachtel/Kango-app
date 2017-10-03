using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Services.Contracts
{
    public class InboxMessage : IInboxMessage
    {
        public int MessageId { get; set; }
        public int AssetId { get; set; }

        public string FromUser { get; set; }

        public String Message { get; set; }

        public string TimeAgo { get; set; }

        public bool IsRead { get; set; }

        public int read { get; set; }

        public string Phone { get; set; }
    }
}
