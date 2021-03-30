using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SSWProject.Startup))]
namespace SSWProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
