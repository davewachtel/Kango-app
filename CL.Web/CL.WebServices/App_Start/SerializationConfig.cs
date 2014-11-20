using Newtonsoft.Json;
using CL.Services.Web.Formatter;
using System.Web.Http;

namespace CL.Services.Web
{
    public class SerializationConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new LowercaseContractResolver(); //CamelCasePropertyNamesContractResolver
            config.Formatters.JsonFormatter.SerializerSettings = settings;
        }
    }
}