using Polly;
using Polly.Retry;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MS.Identity {
    public static class IWebHostExtension {
        public static IWebHost MigrateDbContext<TContext> (this IWebHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext {
            using (var scope = webHost.Services.CreateScope ()) {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<TContext>>();

                var context = services.GetService<TContext> ();

                try {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                    var retry = Policy.Handle<SqlException> ()
                        .WaitAndRetry (new TimeSpan[] {
                            TimeSpan.FromSeconds (3),
                                TimeSpan.FromSeconds (5),
                                TimeSpan.FromSeconds (8),
                        });

                    //if the sql server container is not created on run docker compose this
                    //migration can't fail for network related exception. The retry options for DbContext only 
                    //apply to transient exceptions
                    // Note that this is NOT applied when running some orchestrators (let the orchestrator to recreate the failing service)
                    retry.Execute (() => InvokeSeeder (seeder, context, services));

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
                } catch (Exception ex) {
                    logger.LogError (ex, "An error occurred while migrating the database used on context {DbContextName}", typeof (TContext).Name);
                }
            }

            return webHost;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context,services);
        }
    }
}