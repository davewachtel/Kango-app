using System;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Mvc;
using PushSharp.Apple;
using Newtonsoft.Json.Linq;
using CL.Services.Data;

[assembly: OwinStartup(typeof(CL.Services.Web.Startup))]

namespace CL.Services.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            ConfigureAuth(app);

            GlobalConfiguration.Configure(WebApiConfig.Register);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);   
            GlobalConfiguration.Configure(SerializationConfig.Register);
            string path = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/CertificatesLive.p12");
            
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
              path,"12345",true);

            PushNotifications.Apple = new ApnsServiceBroker(config);
           //PushNotifications.Apple.QueueNotification(new ApnsNotification
           //{
              //DeviceToken = "B770A67EB55213735B65A4AAB9A0D2CC86D7BCE280B33C61BA841C2E0955E28E",
              //Payload = JObject.Parse("{\"aps\":{\"alert\":\"You Just share a media\",\"sound\":\"default\"}}")
               //Payload = JObject.Parse(String.Format("{\"aps\":{\"alert\":\"{0}\",\"badge\":1}}", "myasdasdasd"))
            //});
         

            PushNotifications.Apple.OnNotificationSucceeded += (notification) => {
                Console.WriteLine("Apple Notification Sent!");
            };

            PushNotifications.Apple.OnNotificationFailed += (notification, aggregateEx) => {

                aggregateEx.Handle(ex => {

                    // See what kind of exception it was to further diagnose
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException)ex;

                        // Deal with the failed notification
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;

                        Console.WriteLine($"Apple Notification Failed: ID={apnsNotification.Identifier}, Code={statusCode}");

                    }
                    else
                    {
                        // Inner exception might hold more useful information like an ApnsConnectionException           
                        Console.WriteLine($"Apple Notification Failed for some unknown reason : {ex.InnerException}");
                    }

                    // Mark it as handled
                    return true;
                });
            };

            PushNotifications.Apple.Start();

        }
    }
}