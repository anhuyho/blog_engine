using System.Collections.Generic;
using BlogEngine.DataTransferObject;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace BlogEngine.IdentityServer
{
    public static class Configuration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.garndma"
                    }
                }
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource(Contanst.BlogAPI)
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
                new Client {
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = { Contanst.WebEndPoint + "/signin-oidc" },
                    PostLogoutRedirectUris = {  Contanst.WebEndPoint + "/signout-callback-oidc" },

                    AllowedScopes = {
                       Contanst.BlogAPI,
                        IdentityServerConstants.StandardScopes.OpenId,
                        "rc.scope",
                    },

                    AllowOfflineAccess = true,
                    RequireConsent = false,
                }
            };
    }
}
