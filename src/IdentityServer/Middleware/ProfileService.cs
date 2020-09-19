namespace IdentityServer.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using IdentityModel;
    using IdentityServer4.Extensions;
    using IdentityServer4.Models;
    using IdentityServer4.Services;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore.Identity;

    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileService(UserManager<ApplicationUser> userManager) => this.userManager = userManager;

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var subjectId = context.Subject.GetSubjectId();
                var user = this.userManager.FindByNameAsync(subjectId).GetAwaiter().GetResult();

                var claims = new List<Claim>
                {
                    // add as many claims as you want.                    
                    // new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean).
                    new Claim(JwtClaimTypes.Subject, user.Email.ToString()),
                    new Claim(JwtClaimTypes.Email, user.Email),
                    new Claim(JwtClaimTypes.Id, user.Id),
                };

                context.IssuedClaims = claims;
                return Task.FromResult(0);
            }
            catch (Exception)
            {
                return Task.FromResult(0);
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }
    }
}