using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RAPSimple.Startup))]
namespace RAPSimple
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
