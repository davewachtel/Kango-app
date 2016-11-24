using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CL.Services.Web.Models.User
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
   /*
    public class UserModel : Contracts.IUser
    {
        public String Id { get; set; }
        public String UserName { get; set; }

    }
    */

    public class UserModel : IUser
    {
       
        public string Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "Your must provide a PhoneNumber")]
        //[StringLength(15, ErrorMessage = "Not a valid Phone number", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
    }

    public class sysEmail
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    public class profie
    {
        [Required]
        [Display(Name = "UserId")]
        public string Id { get; set; }
    }

    public class updateProfile
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }

        
        [Display(Name = "Noti")]
        public string noti { get; set; }

    }
    public class CheckNumbers
    {
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "PhoneNumber")]
        public String[] PhoneNumber { get; set; }
    }
}