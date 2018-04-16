using Microsoft.Owin;
using Owin;
using Stripe;

[assembly: OwinStartupAttribute(typeof(ReadySetResource.Startup))]
namespace ReadySetResource
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }
        
    }
}
