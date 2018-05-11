using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SynologyTest.Startup))]
namespace SynologyTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
