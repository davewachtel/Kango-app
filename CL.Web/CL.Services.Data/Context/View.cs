//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CL.Services.Data.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class View
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int AssetId { get; set; }
        public Nullable<int> ShareId { get; set; }
        public int Duration { get; set; }
        public System.DateTime CreateDt { get; set; }
        public bool IsLiked { get; set; }
    
        public virtual Asset Asset { get; set; }
        public virtual Share Share { get; set; }
        public virtual User User { get; set; }
    }
}
