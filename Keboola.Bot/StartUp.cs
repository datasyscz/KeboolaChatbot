using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace Keboola.Bot
{
    public class Startup
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder app)
        {
            try
            {
                throw new Exception("It's Working");
            }
            catch (Exception ex)
            {
                logger.Error("Test");
            }
        }
    }
}