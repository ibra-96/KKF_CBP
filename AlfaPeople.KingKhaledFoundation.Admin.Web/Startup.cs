using Owin;
using Microsoft.Owin;

[assembly: OwinStartupAttribute(typeof(AlfaPeople.KingKhaledFoundation.Admin.Web.Startup))]
namespace AlfaPeople.KingKhaledFoundation.Admin.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
