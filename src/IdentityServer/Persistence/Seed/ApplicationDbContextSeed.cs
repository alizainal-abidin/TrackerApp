namespace IdentityServer.Persistence.Seed
{
    using System.Linq;
    using System.Threading.Tasks;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore.Identity;

    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {            
            var defaultUser = new ApplicationUser 
            { 
                UserName = "manager@terkwaz.com", 
                Email = "manager@terkwaz.com",
                FullName = "Big Boss",
                PhoneNumber = "0621010189",                
            };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "password");
            }
        }
    }
}