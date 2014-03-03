using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ParkBlog.Web.Startup))]
namespace ParkBlog.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
