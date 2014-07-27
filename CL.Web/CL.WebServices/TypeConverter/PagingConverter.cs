using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace CL.Services.Web.TypeConverter
{
    public class PagingFilter
    {
        public int Size { get; set; }
        public int Page { get; set; }

        public PagingFilter() { }

        public PagingFilter(int pageNum, int pageSize)
        {
            this.Page = pageNum;
            this.Size = pageSize;
        }
    }
}