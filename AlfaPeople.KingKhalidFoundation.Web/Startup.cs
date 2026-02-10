using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AlfaPeople.KingKhalidFoundation.Web.Startup))]
namespace AlfaPeople.KingKhalidFoundation.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
          
        }
    }
}
