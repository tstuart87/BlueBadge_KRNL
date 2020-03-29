﻿using Hangfire;
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

            LocationService service = new LocationService();
            RecurringJob.AddOrUpdate(() => service.SetGdus(), Cron.Daily(11, 30));
            //RecurringJob.AddOrUpdate(() => service.<new_method_goes_here>(), Cron.Daily(21, 47));    //Put new methods in here eventually.
        }
    }
}
