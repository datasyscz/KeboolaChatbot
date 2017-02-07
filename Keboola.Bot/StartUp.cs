using System;
using System.Reflection;
using log4net;
using log4net.Config;
using Owin;

[assembly: XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace Keboola.Bot
{
    public class Startup
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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