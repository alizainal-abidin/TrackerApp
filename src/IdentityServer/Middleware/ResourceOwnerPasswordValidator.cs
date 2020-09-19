namespace IdentityServer.Middleware
{
    using System.Threading.Tasks;
    using IdentityServer4.Models;
    using IdentityServer4.Validation;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore.Identity;

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ResourceOwnerPasswordValidator(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = this.userManager.FindByNameAsync(context.UserName).GetAwaiter().GetResult();
            if (user != null)
            {
                // sign-in.
                var results = this.signInManager.PasswordSignInAsync(user, context.Password, false, false).GetAwaiter().GetResult();
                if (results.Succeeded)
                {
                    context.Result = new GrantValidationResult(user.Id, "password", null, "local", null);
                    return Task.FromResult(context.Result);
                }
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "The username and password do not match", null);
            return Task.FromResult(context.Result);
        }
    }
}