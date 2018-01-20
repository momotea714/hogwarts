using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Hogwarts.Startup))]
namespace Hogwarts
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
