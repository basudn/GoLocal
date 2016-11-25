using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GoLocal.Startup))]
namespace GoLocal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
