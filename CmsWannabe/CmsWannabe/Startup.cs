using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CmsWannabe.Startup))]
namespace CmsWannabe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
