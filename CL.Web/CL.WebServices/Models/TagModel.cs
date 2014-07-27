using CL.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CL.Services.Web.Models
{
    public class TagModel : ITag
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public TagModel()
        {

        }

        public static TagModel Load(ITag tag)
        {
            TagModel model = new TagModel();

            model.Id = tag.Id;
            model.Name = tag.Name;

            return model;
        }

        public ITag ToInterface()
        {
            Contracts.Tag tag = new Contracts.Tag();
            tag.Id = this.Id;
            tag.Name = this.Name;

            return tag;
        }
    }
}