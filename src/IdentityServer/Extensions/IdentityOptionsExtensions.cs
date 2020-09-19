namespace IdentityServer.Extensions
{
    using Microsoft.AspNetCore.Identity;

    public static class IdentityOptionsExtensions
    {
        public static void SetLoosePasswordPolicy(this IdentityOptions options)
        {
            // loose enforced password policy so that I can use my super simple and awesome 'password'.
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        }
    }
}