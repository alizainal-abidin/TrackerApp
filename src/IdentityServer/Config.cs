namespace IdentityServer
{
    using System.Collections.Generic;
    using IdentityModel;
    using IdentityServer4;
    using IdentityServer4.Models;

    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("trackerApi", "Tracker API", new List<string> { "email", "phone" }),              
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "tracking-api",
                    ClientName = "Tracker System API",
                    ClientSecrets =
                    {
                        new Secret("a-very-strong-key-here".ToSha256())
                    },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "trackerApi",
                    },
                    AllowOfflineAccess = true,
                    RequireClientSecret = true,
                }
            };
    }
}