namespace IdentityServer
{
    using System;
    using System.Security.Cryptography.X509Certificates;
    using IdentityServer.Extensions;
    using IdentityServer.Middleware;
    using IdentityServer.Persistence;
    using IdentityServer4.Hosting;
    using IdentityServer4.AspNetIdentity;
    using IdentityServer4.Services;
    using IdentityServer4.Validation;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.HttpOverrides;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                // prevent error: "OpenId Connect ProtocolException: Message contains error: 'invalid_client'"
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("db"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.EnableRetryOnFailure();
                    }));

            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>()
                    .AddTransient<IProfileService, ProfileService>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SetLoosePasswordPolicy())
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .Services.AddTransient<IProfileService, ProfileService>();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "IS4.Cookie";
                config.LoginPath = "/Auth/Login";
                config.LogoutPath = "/Auth/Logout";
            });

            var builder = services.AddIdentityServer(options =>
            {

            })
                .AddAspNetIdentity<ApplicationUser>()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources());

            var dataProtectionBuilder = services.AddDataProtection().SetApplicationName(typeof(Startup).Assembly.FullName);
            if (this.HostingEnvironment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                var signingCertificatePfxBase64 = this.Configuration["Credentials:Base64SigningCertificate"];
                var pfxBytes = Convert.FromBase64String(signingCertificatePfxBase64);
                var x509Certificate2 = new X509Certificate2(pfxBytes);
                builder.AddSigningCredential(x509Certificate2);
            }

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (this.HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // prevent error: "OpenId Connect ProtocolException: Message contains error: 'invalid_client'"
            app.UseForwardedHeaders();

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            app.ConfigureCors();

            app.UseMiddleware<IdentityServerMiddleware>();
        }
    }
}