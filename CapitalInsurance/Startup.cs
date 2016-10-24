using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CapitalInsurance.Startup))]
namespace CapitalInsurance
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
