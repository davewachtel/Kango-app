using CL.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CL.Services.Web.Models
{
    public class InboxMessageModel : IInboxMessage
    {
        public int MessageId { get; set; }

        public int AssetId { get; set; }

        public String FromUser { get; set; }

        public string Message { get; set; }

        public String TimeAgo { get; set; }

        public bool IsRead { get; set; }

        public InboxMessageModel()
        {

        }

        public static InboxMessageModel Load(IInboxMessage msg)
        {
            InboxMessageModel model = new InboxMessageModel();

            model.MessageId = msg.MessageId;
            model.AssetId = msg.AssetId;
            model.FromUser = msg.FromUser;
            model.Message = msg.Message;
            model.TimeAgo = msg.TimeAgo;
            model.IsRead = msg.IsRead;

            return model;
        }

        public IInboxMessage ToInterface()
        {
            Contracts.InboxMessage msg = new Contracts.InboxMessage();

            msg.MessageId = this.MessageId;
            msg.AssetId = this.AssetId;
            msg.FromUser = this.FromUser;
            msg.Message = this.Message;
            msg.TimeAgo = this.TimeAgo;
            msg.IsRead = this.IsRead;

            return msg;
        }
    }
}