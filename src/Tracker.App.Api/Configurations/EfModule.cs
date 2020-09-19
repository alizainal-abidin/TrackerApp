namespace Tracker.App.Api.Configurations
{
    using Autofac;
    using global::Infrastructure.Persistence;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Diagnostics;

    public class EfModule : Module
    {
        private readonly DbContextOptions<TrackerDbContext> options;

        public EfModule(string sqlServerConnectionString)
        {
            var builder = new DbContextOptionsBuilder<TrackerDbContext>();
            builder.UseSqlServer(sqlServerConnectionString, options => { options.EnableRetryOnFailure(); });
            builder.ConfigureWarnings(w => w.Throw(RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning));
            this.options = builder.Options;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TrackerDbContext>()
                .AsSelf()
                .WithParameter(new NamedParameter("options", this.options))
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}