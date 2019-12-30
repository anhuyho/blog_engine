using System.Collections.Generic;
using BlogEngine.DataTransferObject;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace BlogEngine.IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
                //new IdentityResource
                //{
                //    Name = "rc.scope",
                //    UserClaims =
                //    {
                //        "rc.garndma"
                //    }
                //}
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource(Contanst.BlogAPI)
            };

        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var clients = new List<Client>();
            var endPoint = new Endpoint(configuration);
            var mvcClient = new Client
            {
                ClientId = "client_id_mvc",
                ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { endPoint.Mvc + "/signin-oidc" },
                PostLogoutRedirectUris = { endPoint.Mvc + "/Home/Index" },

                AllowedScopes = {
                       Contanst.BlogAPI,
                        IdentityServerConstants.StandardScopes.OpenId
                    },

                AllowOfflineAccess = true,
                RequireConsent = false,
            };
            clients.Add(mvcClient);

            var apiClient = new Client
            {
                ClientId = "client_id",
                ClientSecrets = { new Secret("client_secret".ToSha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                AllowedScopes = { Contanst.BlogAPI }
            };

            clients.Add(apiClient);
            return clients;
        }
            
    }
}
