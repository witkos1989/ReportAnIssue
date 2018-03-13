using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReportAnIssue.Startup))]
namespace ReportAnIssue
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
