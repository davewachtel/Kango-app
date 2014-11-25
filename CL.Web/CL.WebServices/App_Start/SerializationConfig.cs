using Newtonsoft.Json;
using CL.Services.Web.Formatter;
using System.Web.Http;

namespace CL.Services.Web
{
    public class SerializationConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
            json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new LowercaseContractResolver();

            /*
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver(); //CamelCasePropertyNamesContractResolver
            config.Formatters.JsonFormatter.SerializerSettings = settings;
             */ 
        }
    }
}