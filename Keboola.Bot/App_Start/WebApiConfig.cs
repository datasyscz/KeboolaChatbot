using System.Web.Http;
using Keboola.Bot.Job;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quartz;
using Quartz.Impl;

namespace Keboola.Bot
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /* //  config.Filters.Add(new AuthorizeAttribute());
               // Json settings
               config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
               config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                   new CamelCasePropertyNamesContractResolver();
               config.Formatters.JsonFormatter.SerializerSettings.Formatting = Formatting.Indented;
               JsonConvert.DefaultSettings = () => new JsonSerializerSettings
               {
                   ContractResolver = new CamelCasePropertyNamesContractResolver(),
                   Formatting = Formatting.Indented,
                   NullValueHandling = NullValueHandling.Ignore
               };

               // Web API routes
               config.MapHttpAttributeRoutes();

               config.Routes.MapHttpRoute(
                   "DefaultApi",
                   "api/{controller}/{id}",
                   new {id = RouteParameter.Optional}
               );


               //refresh tokens from keboola
               IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
             //  scheduler.Start();

               IJobDetail job = JobBuilder.Create<TokenShedulerJob>().Build();

               ITrigger trigger = TriggerBuilder.Create()
                   .WithDailyTimeIntervalSchedule
                   (s =>
                       s.WithIntervalInMinutes(20)
                           .OnEveryDay()
                           .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
                   )
                   .Build();

               scheduler.ScheduleJob(job, trigger);


               //Run on start
               TokenShedulerJob sheduler = new TokenShedulerJob();
               sheduler.Execute(null);*/
        }
    }
}