using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using MS.Identity.Configuration;

namespace MS.Identity.Data
{
    public class ConfigurationDbContextSeed
    {
        internal async Task SeedAsync(ConfigurationDbContext context, IConfiguration configuration)
        {
            var clientUrls = new Dictionary<string, string>();

            clientUrls.Add("Mvc", configuration.GetValue<string>("MvcClient"));
            if (!context.Clients.Any())
            {
                foreach (var item in Config.GetClients(clientUrls))
                {
                    context.Clients.Add(item.ToEntity());
                }
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var item in Config.GetResources())
                {
                    context.IdentityResources.Add(item.ToEntity());
                }
            }

            if (!context.ApiResources.Any())
            {
                foreach (var api in Config.GetApis())
                {
                    context.ApiResources.Add(api.ToEntity());
                }
            }
            await context.SaveChangesAsync();

        }
    }
}