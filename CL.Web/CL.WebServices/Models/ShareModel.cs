using CL.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CL.Services.Web.Models
{
    public class ShareModel : IShare
    {
        public int AssetId { get; set; }

        public String[] ToUserId { get; set; }

        public String Message { get; set; }


        public static ShareModel Load(IShare share)
        {
            ShareModel model = new ShareModel();

            model.AssetId = share.AssetId;
            model.Message = share.Message;
            model.ToUserId = share.ToUserId;

            return model;
        }

        public IShare ToInterface()
        {
            Contracts.Share share = new Contracts.Share();
            share.AssetId = this.AssetId;
            share.Message = this.Message;
            share.ToUserId = this.ToUserId;

            return share;
        }
    }
}