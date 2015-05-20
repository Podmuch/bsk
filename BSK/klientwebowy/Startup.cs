using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(klientwebowy.Startup))]
namespace klientwebowy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
