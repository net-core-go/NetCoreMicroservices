using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace MS.Identity.Configuration
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>{
                new ApiResource("Orders","Orders Api")
            };
        }

        /// <summary>
        /// 获取份资源OpenId、OpenId为系统自带配置：https://www.cnblogs.com/stulzq/p/8726002.html
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetResources()
        {
            var customProfile = new IdentityResource(
                name: "custom.profile",
                displayName: "Custom profile",
                claimTypes: new[] { "role"});
            return new List<IdentityResource>{
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                customProfile
            };
        }

        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    ClientUri = $"{clientsUrl["Mvc"]}",                             // public uri of the client
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = false,
                    RequireConsent = false,
                    AllowOfflineAccess = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    RedirectUris = new List<string>
                    {
                        $"{clientsUrl["Mvc"]}/signin-oidc"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        $"{clientsUrl["Mvc"]}/signout-callback-oidc"
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "custom.profile"
                    },
                    AccessTokenLifetime = 60*60*2, // 2 hours
                    IdentityTokenLifetime= 60*60*2 // 2 hours
                }
            };
        }

    }
}