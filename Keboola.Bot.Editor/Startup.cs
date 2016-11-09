using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Keboola.Bot.Editor.Startup))]
namespace Keboola.Bot.Editor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
