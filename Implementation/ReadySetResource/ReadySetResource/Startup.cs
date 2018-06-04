using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReadySetResource.Startup))]
namespace ReadySetResource
{
    /// <summary>
    /// Creates an instance of the startup when called upon
    /// </summary>
    public partial class Startup
    {
        /// <summary>
        /// Configues the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }
        
    }
}
