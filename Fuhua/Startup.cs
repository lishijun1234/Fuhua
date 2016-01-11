using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Fuhua.Startup))]
namespace Fuhua
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
