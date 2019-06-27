using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Identity.Data;
using Serilog;
using Serilog.Events;

namespace MS.Identity {
    public class Program {
        public static void Main (string[] args) {
            var configuration =GetConfiguration();
            var host = CreateWebHostBuilder (args).Build ();
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Debug()
                                .MinimumLevel.Override("Microsoft",LogEventLevel.Warning)
                                .Enrich.FromLogContext()
                                .WriteTo.File("logs/log.txt",rollOnFileSizeLimit:true,rollingInterval:RollingInterval.Day)
                                .CreateLogger();
            //授权服务器中生成的RefreshToken和AuthorizationCode默认是存储在内存中的，
            //因此如果服务重启这些数据就失效了，那么就需要实现IPersistedGrantStore接口对这些数据的存储
            host.MigrateDbContext<PersistedGrantDbContext> ((_, __) => { })
                .MigrateDbContext<ApplicationDbContext> ((context, services) => {
                    var env = services.GetService<IHostingEnvironment> ();
                    var logger = services.GetService<ILogger<ApplicationDbContextSeed>> ();
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
            .UseStartup<Startup> ()
            .UseSerilog();

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}