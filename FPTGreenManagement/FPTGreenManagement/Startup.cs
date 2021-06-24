using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FPTGreenManagement.Startup))]
namespace FPTGreenManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
