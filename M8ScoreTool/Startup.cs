using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(M8ScoreTool.Startup))]
namespace M8ScoreTool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
