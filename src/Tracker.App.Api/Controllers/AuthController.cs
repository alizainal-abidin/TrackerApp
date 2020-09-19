namespace Tracker.App.Api.Controllers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using IdentityServer4.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Tracker.App.Api.Models;

    [Authorize]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IHttpClientFactory httpClientFactory;

        public AuthController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            this.configuration = configuration;
            this.httpClientFactory = httpClientFactory;
        }

        // NOTE: JUST FOR DEMO PURPOSE!
        // All auth logic ideally should be done in the IdentityServer side.
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var tokenResponse = await this.GetAccessToken(model);

            return this.Ok(new
            {
                access_token = tokenResponse.AccessToken,
                expires_in = tokenResponse.ExpiresIn,
                token_type = tokenResponse.TokenType,
                refresh_token = tokenResponse.RefreshToken,
            });
        }       

        private async Task<TokenResponse> GetAccessToken(LoginViewModel model)
        {
            var serverClient = this.httpClientFactory.CreateClient();
            var disco = await serverClient.GetDiscoveryDocumentAsync(this.configuration["Authority:Issuer"]);
            var tokenResponse = await serverClient.RequestPasswordTokenAsync(
                new PasswordTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = this.configuration["Authority:ClientId"],
                    ClientSecret = this.configuration["Authority:ClientSecret"],
                    Scope = this.configuration["Authority:Audience"],
                    GrantType = GrantType.ResourceOwnerPassword,
                    ClientCredentialStyle = ClientCredentialStyle.PostBody,
                    UserName = model.UserName,
                    Password = model.Password
                });
            return tokenResponse;
        }
    }
}