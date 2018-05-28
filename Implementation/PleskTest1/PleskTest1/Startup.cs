using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PleskTest1.Startup))]
namespace PleskTest1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
