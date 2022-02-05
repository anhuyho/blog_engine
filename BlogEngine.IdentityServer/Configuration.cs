using System.Collections.Generic;
using BlogEngine.DataTransferObject;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogEngine.IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource(Contanst.BlogAPI)
            };

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client> { APIClientFactory(), MVCClientFactory(configuration) };
        }

        public static void AddInMemory(this IIdentityServerBuilder services, IConfiguration configuration)
        {
            services.AddInMemoryApiResources(GetApis())
                .AddInMemoryIdentityResources(GetIdentityResources())
                .AddInMemoryClients(GetClients(configuration))
                .AddDeveloperSigningCredential();
        }
        public static Client MVCClientFactory(IConfiguration configuration)
        {
            var endPoint = new Endpoint(configuration);
            return new Client
            {
                ClientId = "client_id_mvc",
                ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                //https://huyblog.azurewebsites.net/signin-oidc
                RedirectUris = { endPoint.Mvc + "/signin-oidc" },
                PostLogoutRedirectUris = { endPoint.Mvc + "/Home/Index" },

                AllowedScopes = {
                       Contanst.BlogAPI,
                        IdentityServerConstants.StandardScopes.OpenId
                    },

                AllowOfflineAccess = true,
                RequireConsent = false,
            };
        }
        public static Client APIClientFactory()
        {
            return new Client
            {
                ClientId = "client_id",
                ClientSecrets = { new Secret("client_secret".ToSha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                AllowedScopes = { Contanst.BlogAPI }
            };
        }
    }
}
