namespace Application.Tests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using Infrastructure.Persistence;
    using MediatR;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using NUnit.Framework;
    using Respawn;
    using Tracker.App.Api;

    [SetUpFixture]
    public class TestHelpers
    {
        private static IConfigurationRoot configuration;
        private static IServiceScopeFactory scopeFactory;
        private static Checkpoint checkpoint;
        private static string currentUserId;        

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetService<IMediator>();

            return await mediator.Send(request);
        }

        public static string RunAsDefaultUser()
        {
            currentUserId = "test@local.com";
            return "test@local.com";
        }        

        public static async Task ResetState()
        {
            await checkpoint.Reset(configuration.GetConnectionString("db"));
            currentUserId = null;
        }

        public static async Task<TEntity> FindAsync<TEntity>(params object[] keyValues)
            where TEntity : class
        {
            using var scope = scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<TrackerDbContext>();

            return await context.FindAsync<TEntity>(keyValues);
        }        

        public static async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = scopeFactory.CreateScope();

            var context = scope.ServiceProvider.GetService<TrackerDbContext>();

            context.Add(entity);

            await context.SaveChangesAsync();
        }

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            configuration = builder.Build();

            var startup = new Startup(configuration);

            var services = new ServiceCollection();

            services.AddSingleton(Mock.Of<IWebHostEnvironment>(w =>
                w.EnvironmentName == "Development" &&
                w.ApplicationName == "Tracker.App.Api"));

            services.AddLogging();

            startup.ConfigureServices(services);

            // Replace service registration for ICurrentUserService
            // Remove existing registration
            var currentUserServiceDescriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(ICurrentUserService));

            services.Remove(currentUserServiceDescriptor);

            // Register testing version
            services.AddTransient(provider =>
                Mock.Of<ICurrentUserService>(s => s.UserId == currentUserId));

            scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();

            checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };

            EnsureDatabase();
        }

        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
        }

        private static void EnsureDatabase()
        {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<TrackerDbContext>();

            context.Database.Migrate();
        }
    }
}