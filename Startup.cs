using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(amigopet.Startup))]
namespace amigopet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
