using Hangfire;
using Hangfire.SqlServer;
using KRNL.Services;
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

            GlobalConfiguration.Configuration
                .UseSqlServerStorage("DefaultConnection");

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            //LocationService service = new LocationService();
            //RecurringJob.AddOrUpdate(() => service.SetGdusAndCumulativePrecipAndGrowthStages(), Cron.Daily(07, 01));
        }
    }
}
