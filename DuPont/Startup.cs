using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DuPont.Startup))]
namespace DuPont
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
