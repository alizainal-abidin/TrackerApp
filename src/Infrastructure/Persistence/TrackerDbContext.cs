namespace Infrastructure.Persistence
{
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Common.Interfaces;
    using AutoMapper.Configuration;
    using Domain.Common;
    using Domain.Entities;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public class TrackerDbContext : DbContext, ITrackerDbContext
    {
        private readonly DbContextOptions options;
        private readonly IConfiguration configuration;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTimeService dateTimeService;
        private readonly IOptions<OperationalStoreOptions> operationalStoreOptions;

        public TrackerDbContext(
            DbContextOptions options,
            IConfiguration configuration,
            ICurrentUserService currentUserService,
            IDateTimeService dateTimeService,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options)
        {
            this.options = options;
            this.configuration = configuration;
            this.currentUserService = currentUserService;
            this.dateTimeService = dateTimeService;
            this.operationalStoreOptions = operationalStoreOptions;
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectParticipant> ProjectParticipants { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in this.ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = this.currentUserService.UserId;
                        entry.Entity.CreatedDate = this.dateTimeService.Now;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = this.currentUserService.UserId;
                        entry.Entity.LastModified = this.dateTimeService.Now;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}