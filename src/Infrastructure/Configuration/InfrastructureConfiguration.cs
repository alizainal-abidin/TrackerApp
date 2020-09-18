namespace Infrastructure.Configuration
{
    using Application.Common.Interfaces;
    using Infrastructure.Persistence;
    using Infrastructure.Services;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<TrackerDbContext>(options => options.UseInMemoryDatabase("TrackerAppDb_Test"));
            }
            else
            {
                services.AddDbContext<TrackerDbContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("db"),
                        b => b.MigrationsAssembly(typeof(TrackerDbContext).Assembly.FullName)));
            }

            services.AddScoped<ITrackerDbContext>(provider => provider.GetService<TrackerDbContext>());

            services.AddTransient<IDateTimeService, DateTimeService>();

            services.AddAuthentication().AddIdentityServerJwt();

            return services;
        }
    }
}