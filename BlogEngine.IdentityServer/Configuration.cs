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
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.garndma"
                    }
                }
            };

        public static IEnumerable<ApiResource> GetApis()
        {
            var apiOne = new ApiResource(Contanst.BlogAPI);
            apiOne.UserClaims = new List<string>{ "Huy", "Boi", "Hai" };
            return new List<ApiResource> {
                apiOne
            };
        }
        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            var endpoint = new Endpoint(configuration);
            var mvcEndpoint = endpoint.Mvc;
            var clients = new List<Client>();
            var scopes = new List<string>();
            scopes.Add(Contanst.BlogAPI);
            scopes.Add(IdentityServerConstants.StandardScopes.OpenId);
            scopes.Add(IdentityServerConstants.StandardScopes.Profile);
            scopes.Add("rc.scope");
            scopes.Add(IdentityServerConstants.StandardScopes.Email);


            var mvcClient = new Client
            {
                ClientId = "client_id_mvc",
                ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { mvcEndpoint + "/signin-oidc" },
                PostLogoutRedirectUris = { mvcEndpoint + "/Home/Index" },

                AllowedScopes = scopes,

                AllowOfflineAccess = true,
                RequireConsent = false,

                AlwaysIncludeUserClaimsInIdToken = true,
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
