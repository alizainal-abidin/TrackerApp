namespace Infrastructure.Configuration
{
    using Application.Common.Interfaces;
    using Infrastructure.Persistence;
    using Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Tracker.App.Infrastructure.Services;

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
                {
                    options.UseSqlServer(
                        configuration.GetConnectionString("db"),
                        options =>
                        {
                            options.EnableRetryOnFailure();
                            options.MigrationsAssembly(typeof(TrackerDbContext).Assembly.FullName);
                        });
                    options.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning));
                });
            }

            services.AddScoped<ITrackerDbContext>(provider => provider.GetService<TrackerDbContext>());
            
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddScoped<IDomainEventService, DomainEventService>();

            return services;
        }
    }
}