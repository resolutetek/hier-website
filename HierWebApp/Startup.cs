using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HierWebApp.Startup))]
namespace HierWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
