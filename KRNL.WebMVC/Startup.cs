using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KRNL.WebMVC.Startup))]
namespace KRNL.WebMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
