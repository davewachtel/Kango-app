using CL.Services.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;



namespace CL.Services.Web.Models
{
   
    public class FriendModel
    {
        public int Friend_Id { get; set; }

        [Required]
        [Display(Name = "Userto")]
        public string Userto { get; set; }

        [Required]
        [Display(Name = "Userfrom")]
        public string Userfrom { get; set; }

        public IFriend ToInterface()
        {
            Contracts.Friend frnd = new Contracts.Friend();
            frnd.Friend_Id = this.Friend_Id;
            frnd.Userto = this.Userto;
            frnd.Userfrom = this.Userfrom;

            return frnd;
        }
    }

    public class DeleteFrnd
    {
        [Required]
        [Display(Name = "User_from")]
        public String User_from { get; set; }

        [Required]
        [Display(Name = "User_to")]
        public String User_to { get; set; }
    }

    public class GetFriendsByUId
    {
        [Required]
        [Display(Name = "UserId")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "pagenumber")]
        public int pagenumber { get; set; }

        [Required]
        [Display(Name = "pagesize")]
        public int pagesize { get; set; }
    }

    public class Pagination
    {
        [Required]
        [Display(Name = "pagenumber")]
        public int pagenumber { get; set; }

        [Required]
        [Display(Name = "pagesize")]
        public int pagesize { get; set; }

        [Required]
        [Display(Name = "UserId")]
        public string UserId { get; set; }
    }
}
