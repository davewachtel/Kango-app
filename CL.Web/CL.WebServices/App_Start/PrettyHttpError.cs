using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;
using System.Web.ModelBinding;

namespace CL.Services.Web.App_Start
{
    public class PrettyHttpError
    {
        private System.Web.Http.ModelBinding.ModelStateDictionary modelState;

        public PrettyHttpError(System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            Message = "Your request is invalid.";
            Errors = new List<string>();

            foreach (var item in modelState)
            {
                foreach (var childItem in item.Value.Errors)
                {
                    Errors.Add(childItem.ErrorMessage);
                }

                if (Errors.Count == 0)
                {
                    continue;
                }
            }
        }
        
        public string Message { get; set; }

        public List<string> Errors { get; set; }
    }

}
