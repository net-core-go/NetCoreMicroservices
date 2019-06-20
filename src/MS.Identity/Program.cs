using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MS.Identity {
    public class Program {
        public static void Main (string[] args) {
            var host = CreateWebHostBuilder (args).Build ();
            host.MigrateDbContext<PersistedGrantDbContext> ((_, __) => { })
                .MigrateDbContext<ApplicationDbContext> ((context, services) => {
                    var env = services.GetService<IHostingEnvironment> ();
                    //var logger = services.GetService<ILogger<ApplicationDbContextSeed>> ();
                    var settings = services.GetService<IOptions<AppSettings>> ();

                    new ApplicationDbContextSeed ()
                        .SeedAsync (context, env, logger, settings)
                        .Wait ();
                })
                .MigrateDbContext<ConfigurationDbContext> ((context, services) => {
                    new ConfigurationDbContextSeed ()
                        .SeedAsync (context, configuration)
                        .Wait ();
                });
            host.Run ();

        }

        public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
            .UseStartup<Startup> ();
    }
}