namespace Tracker.App.Api.Configurations
{
    using System.Linq;
    using Autofac;
    using Microsoft.Extensions.Configuration;
    using Tracker.App.Api.Services;

    public class ApiModule : Module
    {
        private readonly IConfiguration configuration;

        public ApiModule(IConfiguration configuration) => this.configuration = configuration;

        protected override void Load(ContainerBuilder builder)
        {
            // Services
            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<CurrentUserService>()                   
                   .SingleInstance();

            base.Load(builder);
        }
    }
}