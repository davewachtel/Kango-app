using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(CL.Services.Web.Startup))]

namespace CL.Services.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(SerializationConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            ConfigureAuth(app);
        }
    }
}