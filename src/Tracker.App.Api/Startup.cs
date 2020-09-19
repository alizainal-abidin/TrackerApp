namespace Tracker.App.Api
{
    using Application.Common.Interfaces;
    using Application.Configuration;
    using FluentValidation.AspNetCore;
    using global::Infrastructure.Configuration;
    using global::Infrastructure.Persistence;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Tracker.App.Api.Filters;
    using Tracker.App.Api.Services;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer("Bearer", config =>
                {
                    config.Authority = this.Configuration["Authority:Issuer"];
                    config.Audience = this.Configuration["Authority:Audience"];
                });
                        
            services.AddApplication();
            services.AddInfrastructure(this.Configuration);

            services.AddTransient<ICurrentUserService, CurrentUserService>();
            services.AddHttpContextAccessor();
            services.AddHealthChecks().AddDbContextCheck<TrackerDbContext>();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Tracker API Documentation", Version = "v1" });
            });

            services.AddControllersWithViews(options => options.Filters.Add(new CustomApiExceptionFilterAttribute()))
                    .AddFluentValidation();

            services.AddHttpClient();
            services.AddControllers();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        //public void ConfigureContainer(ContainerBuilder builder)
        //{
        //    builder.RegisterModule(new EfModule(this.Configuration.GetConnectionString("db")));
        //    builder.RegisterModule(new ApiModule(this.Configuration));
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tracker API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
