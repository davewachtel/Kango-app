using CL.Services.Contracts.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CL.Services.Web.Models
{
    public class MediaModel
    {
        public static MediaModel Load(IMedia m)
        {
            if (m == null)
                throw new ArgumentNullException("Media cannot be null.");

            MediaModel model = new MediaModel();
            model.Id = m.Id;
            model.MediaType = m.MediaType;
            model.Title = m.Title;
            model.Url = m.Url;

            return model;
        }

        public MediaModel()
        {

        }

        public int Id { get; set; }

        public Contracts.MediaTypeEnum MediaType { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public IMedia ToInterface()
        {
            Media media = new Media();
            media.Id = this.Id;
            media.Title = this.Title;
            media.MediaType = this.MediaType;
            media.Url = this.Url;

            return media;
        }
    }
}