namespace IdentityServer.Controllers
{
    using System.Threading.Tasks;
    using IdentityServer.Models.ViewModels;
    using IdentityServer4.Services;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    public class AuthController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IIdentityServerInteractionService identityServerInteractionService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AuthController(
            IConfiguration configuration,
            IIdentityServerInteractionService identityServerInteractionService,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            this.configuration = configuration;
            this.identityServerInteractionService = identityServerInteractionService;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return this.View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.userManager.FindByNameAsync(model.UserName);
            
            // sign-in.
            var results = await this.signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (results.Succeeded)
            {
                return this.Redirect(model.ReturnUrl);
            }

            return this.View(model);
        }        

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await this.signInManager.SignOutAsync();

            var logoutContext = await this.identityServerInteractionService.GetLogoutContextAsync(logoutId);

            return this.Redirect(logoutContext.PostLogoutRedirectUri);
        }
    }
}